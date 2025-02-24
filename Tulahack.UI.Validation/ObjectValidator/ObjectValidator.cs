﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ReactiveValidation;
using Tulahack.UI.Validation.Enums;
using Tulahack.UI.Validation.Exceptions;
using Tulahack.UI.Validation.Helpers;
using Tulahack.UI.Validation.Helpers.Nito.AsyncEx;
using Tulahack.UI.Validation.Models;
using Tulahack.UI.Validation.ObjectObserver;
using Tulahack.UI.Validation.Options;
using Tulahack.UI.Validation.Resources.StringSources;
using Tulahack.UI.Validation.RuleBuilders;
using Tulahack.UI.Validation.ValidatableObject;
using Tulahack.UI.Validation.ValidatorFactory;
using Tulahack.UI.Validation.Validators.Base;
using Tulahack.UI.Validation.Validators.Contexts;

namespace Tulahack.UI.Validation.ObjectValidator;

/// <inheritdoc cref="IObjectValidator" />
internal class ObjectValidator<TObject> : BaseNotifyPropertyChanged, IObjectValidator
    where TObject : IValidatableObject
{
    /// <summary>
    /// Instance of validatable object.
    /// </summary>
    private readonly TObject _instance;

    private readonly Dictionary<string, IStringSource?> _displayNamesSources;

    private readonly ObjectObserver<TObject> _observer;
    private readonly Dictionary<string, ValidatableProperty<TObject>> _validatableProperties;

    private readonly object _lock = new();
    private readonly AsyncManualResetEvent _asyncValidationWaiter = new(true);


    private bool _isValid;
    private bool _hasWarnings;
    private IReadOnlyList<ValidationMessage> _validationMessages = Array.Empty<ValidationMessage>();

    private bool _isDisposed;

    /// <remarks>
    /// This is very important thing. <see cref="LanguageManager.CultureChanged" /> uses a weak reference to delegates.
    /// If there is no reference to delegate, target will be collected by GC.
    /// ObjectValidator keep this reference until it will be collected.
    /// After this WeakReference will be collected too.
    /// </remarks>
    /// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly EventHandler<CultureChangedEventArgs>? _cultureChangedEventHandler;

    private readonly ConcurrentDictionary<string, PropertyChangedStopwatch> _propertyChangedStopwatches = new();

    /// <summary>
    /// Create new instance of object validator.
    /// </summary>
    /// <param name="instance">Instance of validatable object.</param>
    /// <param name="ruleBuilders">List of rules builders.</param>
    public ObjectValidator(TObject instance, IReadOnlyList<IRuleBuilder<TObject>> ruleBuilders)
    {
        _instance = instance;

        _displayNamesSources = GetDisplayNames();
        _validatableProperties = GetValidatableProperties(ruleBuilders);

        _observer = new ObjectObserver<TObject>(_instance, GetPropertySettings(ruleBuilders));
        _observer.PropertyChanged += OnPropertyChanged;

        if (ValidationOptions.LanguageManager.TrackCultureChanged)
        {
            _cultureChangedEventHandler = OnCultureChanged;
            ValidationOptions.LanguageManager.CultureChanged += _cultureChangedEventHandler;
        }

        IsValid = true;
        HasWarnings = false;
    }

    #region IObjectValidator

    /// <inheritdoc />
    public bool IsValid
    {
        get => _isValid;
        private set => SetAndRaiseIfChanged(ref _isValid, value);
    }

    /// <inheritdoc />
    public bool HasWarnings
    {
        get => _hasWarnings;
        private set => SetAndRaiseIfChanged(ref _hasWarnings, value);
    }

    /// <inheritdoc />
    public bool IsAsyncValidating
    {
        get => !_asyncValidationWaiter.IsSet;
        private set
        {
            var isValueChanged = _asyncValidationWaiter.IsSet == value;

            if (!isValueChanged)
            {
                return;
            }

            if (value)
            {
                _asyncValidationWaiter.Reset();
            }
            else
            {
                _asyncValidationWaiter.Set();
            }

            OnPropertyChanged();
        }
    }

    /// <inheritdoc />
    public IReadOnlyList<ValidationMessage> ValidationMessages
    {
        get => _validationMessages;
        private set => SetAndRaiseIfChanged(ref _validationMessages, value);
    }


    /// <inheritdoc />
    public IReadOnlyList<ValidationMessage> GetMessages(string propertyName)
    {
        if (string.IsNullOrEmpty(propertyName))
        {
            return Array.Empty<ValidationMessage>();
        }

        lock (_lock)
        {
            if (!_validatableProperties.TryGetValue(propertyName, out ValidatableProperty<TObject>? property))
            {
                return [];
            }

            return property.ValidationMessages;
        }
    }

    /// <inheritdoc />
    public void Revalidate()
    {
        // If force revalidate is called, then throttle should be ignored.
        // Remove all stopwatches for this.
        _propertyChangedStopwatches.Clear();

        RevalidateInternal();
    }

    /// <inheritdoc />
    public Task WaitValidatingCompletedAsync(CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            // Only return task, it's prohibited to wait here (because of deadlocks).
            return _asyncValidationWaiter.WaitAsync(cancellationToken);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Dispose object validator.
    /// </summary>
    private void Dispose(bool isDisposing)
    {
        if (!isDisposing || _isDisposed)
        {
            return;
        }

        lock (_lock)
        {
            if (_isDisposed)
            {
                return;
            }

            _observer.Dispose();

            if (ValidationOptions.LanguageManager.TrackCultureChanged)
            {
                ValidationOptions.LanguageManager.CultureChanged -= _cultureChangedEventHandler;
            }

            // Cancel all async tasks.
            // We don't need to wait them, because they won't affect on anything.
            IEnumerable<CancellationTokenSource?> asyncTokenSources = _validatableProperties
                .Values
                .SelectMany(p => p.AsyncValidatorCancellationTokenSources.Values);

            foreach (CancellationTokenSource? tokenSource in asyncTokenSources)
            {
                tokenSource?.Cancel();
            }

            ValidationMessages = Array.Empty<ValidationMessage>();
            IsValid = true;
            HasWarnings = false;

            foreach (var validatableProperty in _validatableProperties.Keys)
            {
                _instance.OnPropertyMessagesChanged(validatableProperty);
            }

            _isDisposed = true;
        }
    }

    #endregion

    /// <summary>
    /// Handle property changed event.
    /// </summary>
    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        Debug.Assert(args.PropertyName != null, "args.PropertyName != null");
        _ = _propertyChangedStopwatches.AddOrUpdate(
            args.PropertyName,
            _ => new PropertyChangedStopwatch(),
            (_, stopwatch) => stopwatch.Restart());

        RevalidateInternal(args.PropertyName);
    }

    /// <summary>
    /// Revalidate properties.
    /// </summary>
    /// <param name="propertyName">
    /// Name of changed property.
    /// <see langword="null" /> if all properties should be revalidated.
    /// </param>
    private void RevalidateInternal(string? propertyName = null)
    {
        lock (_lock)
        {
            var aggregatedValidationContext =
                new AggregatedValidationContext<TObject>(_instance, _displayNamesSources, _propertyChangedStopwatches);
            var changedProperties = new List<string>();
            var propertiesAsyncValidators = new Dictionary<string, List<IPropertyValidator<TObject>>>();

            foreach (KeyValuePair<string, ValidatableProperty<TObject>> validatableProperty in _validatableProperties)
            {
                ValidatableProperty<TObject> info = validatableProperty.Value;
                var isTarget = string.IsNullOrEmpty(propertyName) || info.PropertyName == propertyName;

                if (!isTarget && !info.RelatedProperties.Contains(propertyName!))
                {
                    continue;
                }

                var isMessagesChanged = false;
                var shouldIgnoreAllFollowing = false;
                var shouldRevalidateAllFollowing = isTarget;
                var shouldAsyncValidate = false;
                var asyncValidators = new List<IPropertyValidator<TObject>>();

                foreach (IPropertyValidator<TObject> propertyValidator in info.Validators)
                {
                    // After first async validator - all others will be validated in ValidateInternalAsync.
                    shouldAsyncValidate |= propertyValidator.IsAsync;

                    // Check if all following validators should be ignored and have empty message.
                    if (shouldIgnoreAllFollowing)
                    {
                        // Cancel async validation if it's running.
                        if (shouldAsyncValidate)
                        {
                            info.AsyncValidatorCancellationTokenSources[propertyValidator]?.Cancel();
                        }

                        if (info.ValidatorsValidationMessages[propertyValidator].Any())
                        {
                            info.ValidatorsValidationMessages[propertyValidator] = Array.Empty<ValidationMessage>();
                            isMessagesChanged = true;
                        }

                        continue;
                    }

                    // If changed property doesn't affect at property validator then we don't need to revalidate it.
                    var isRelated = propertyValidator.RelatedProperties.Contains(propertyName);

                    if (!shouldRevalidateAllFollowing && !isRelated)
                    {
                        shouldIgnoreAllFollowing |=
                            info.ValidatorsValidationMessages[propertyValidator].Any() &&
                            info.PropertyCascadeMode == CascadeMode.Stop;
                        continue;
                    }

                    if (shouldAsyncValidate)
                    {
                        // Reset messages, because we don't need them during async validation.
                        // Also if this validator had messages and PropertyCascadeMode == CascadeMode.Stop then we need to revalidate all others validators.
                        // Because they were skipped because of this validator.
                        if (info.ValidatorsValidationMessages[propertyValidator].Any())
                        {
                            info.ValidatorsValidationMessages[propertyValidator] = Array.Empty<ValidationMessage>();
                            isMessagesChanged = true;
                            shouldRevalidateAllFollowing |= info.PropertyCascadeMode == CascadeMode.Stop;
                        }

                        // Add this validator to list only if should be revalidated.
                        if (shouldRevalidateAllFollowing || isRelated)
                        {
                            // Cancel previous execution (if it is running) and create new token source for future execution.
                            info.AsyncValidatorCancellationTokenSources[propertyValidator]?.Cancel();
                            info.AsyncValidatorCancellationTokenSources[propertyValidator] =
                                new CancellationTokenSource();

                            asyncValidators.Add(propertyValidator);
                        }

                        continue;
                    }

                    // Validate sync validators here.
                    ValidationContextFactory<TObject> contextProvider = aggregatedValidationContext.CreateContextFactory(info.PropertyName);
                    IReadOnlyList<ValidationMessage> messages = ValidatePropertyValidator(propertyValidator, contextProvider);
                    shouldIgnoreAllFollowing |= messages.Any() && info.PropertyCascadeMode == CascadeMode.Stop;

                    if (messages.SequenceEqual(info.ValidatorsValidationMessages[propertyValidator]))
                    {
                        continue;
                    }

                    info.ValidatorsValidationMessages[propertyValidator] = messages;
                    isMessagesChanged = true;
                    shouldRevalidateAllFollowing |=
                        messages.Count == 0 && info.PropertyCascadeMode == CascadeMode.Stop;
                }

                if (isMessagesChanged)
                {
                    changedProperties.Add(info.PropertyName);
                }

                if (asyncValidators.Any())
                {
                    propertiesAsyncValidators.Add(info.PropertyName, asyncValidators);
                }
            }

            NotifyChangedProperties(changedProperties);
            ValidateInternalAsync(aggregatedValidationContext, propertiesAsyncValidators);
        }
    }

    /// <summary>
    /// Validate using async validators.
    /// </summary>
    private async void ValidateInternalAsync(
        AggregatedValidationContext<TObject> aggregatedValidationContext,
        Dictionary<string, List<IPropertyValidator<TObject>>> propertiesAsyncValidators)
    {
        if (propertiesAsyncValidators.Count == 0)
        {
            return;
        }

        IsAsyncValidating = true;

        try
        {
            var tasks = new List<Task>();

            foreach (KeyValuePair<string, List<IPropertyValidator<TObject>>> propertyValidators in propertiesAsyncValidators)
            {
                tasks.Add(ValidatePropertyAsync(aggregatedValidationContext, propertyValidators.Key,
                    propertyValidators.Value));
            }

            await Task.WhenAll(tasks)
                .ConfigureAwait(false);
        }
        finally
        {
            lock (_lock)
            {
                IsAsyncValidating = _validatableProperties
                    .Values
                    .SelectMany(vp => vp.AsyncValidatorCancellationTokenSources.Values)
                    .Any(s => s?.IsCancellationRequested == false);
            }
        }
    }

    /// <summary>
    /// Validate property with async validators.
    /// </summary>
    private async Task ValidatePropertyAsync(
        AggregatedValidationContext<TObject> aggregatedValidationContext,
        string propertyName,
        IReadOnlyList<IPropertyValidator<TObject>> propertyValidators)
    {
        ValidatableProperty<TObject> info = _validatableProperties[propertyName];
        var tokenSources = propertyValidators
            .ToDictionary(pv => pv, pv => info.AsyncValidatorCancellationTokenSources[pv]);

        foreach (IPropertyValidator<TObject> propertyValidator in propertyValidators)
        {
            // Skip this validator because it should be revalidated with new property value.
            CancellationTokenSource? tokenSource = tokenSources[propertyValidator];

            if (tokenSource!.IsCancellationRequested)
            {
                continue;
            }

            ValidationContextFactory<TObject> contextProvider = aggregatedValidationContext.CreateContextFactory(info.PropertyName);
            IReadOnlyList<ValidationMessage> messages = await ValidatePropertyValidatorAsync(propertyValidator, contextProvider, tokenSource.Token)
                .ConfigureAwait(false);

            if (tokenSource.IsCancellationRequested)
            {
                continue;
            }

            lock (_lock)
            {
                if (tokenSource.IsCancellationRequested)
                {
                    continue;
                }

                tokenSource.CancelAsync();

                // Inside RevalidateInternal we have reset messages.
                // So we need only check if there are something new.
                if (messages.Any())
                {
                    info.ValidatorsValidationMessages[propertyValidator] = messages;
                    NotifyChangedProperties(new[] { propertyName });

                    // There are first messages, so we don't need to revalidate following validators.
                    if (info.PropertyCascadeMode == CascadeMode.Stop)
                    {
                        // But we should cancel all tokens for them.
                        IEnumerable<IPropertyValidator<TObject>> followingValidators = propertyValidators
                            .SkipWhile(pv => pv != propertyValidator)
                            .Skip(1);

                        foreach (IPropertyValidator<TObject>? followingValidator in followingValidators)
                        {
                            CancellationTokenSource? followingTokenSource = info.AsyncValidatorCancellationTokenSources[followingValidator];
                            followingTokenSource!.CancelAsync();
                        }

                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Validate property by specified validator.
    /// </summary>
    /// <param name="propertyValidator">Property validator.</param>
    /// <param name="contextFactory">Context factory.</param>
    private static IReadOnlyList<ValidationMessage> ValidatePropertyValidator(
        IPropertyValidator<TObject> propertyValidator,
        ValidationContextFactory<TObject> contextFactory)
    {
        try
        {
            IReadOnlyList<ValidationMessage> messages = propertyValidator.ValidateProperty(contextFactory);

            if (messages == null)
            {
                throw new NullReferenceException("Invalid return value of PropertyValidator");
            }

            return messages;
        }
        catch (Exception e)
        {
            return new[]
            {
                new ValidationMessage(new ExceptionSource(e)),
            };
        }
    }

    /// <summary>
    /// Validate property by specified async validator.
    /// </summary>
    /// <param name="propertyValidator">Property validator.</param>
    /// <param name="contextFactory">Context factory.</param>
    /// <param name="cancellationToken">Token for cancelling validation.</param>
    private async static Task<IReadOnlyList<ValidationMessage>> ValidatePropertyValidatorAsync(
        IPropertyValidator<TObject> propertyValidator,
        ValidationContextFactory<TObject> contextFactory,
        CancellationToken cancellationToken)
    {
        try
        {
            IReadOnlyList<ValidationMessage> messages = propertyValidator.IsAsync
                ? await propertyValidator.ValidatePropertyAsync(contextFactory, cancellationToken)
                // ReSharper disable once MethodHasAsyncOverloadWithCancellation
                // It's not an async validator, so we should use sync method.
                : propertyValidator.ValidateProperty(contextFactory);

            if (messages == null)
            {
                throw new NullReferenceException("Invalid return value of PropertyValidator");
            }

            return messages;
        }
        // If cancellation requested - result won't be used.
        catch (Exception) when (cancellationToken.IsCancellationRequested)
        {
            return Array.Empty<ValidationMessage>();
        }
        catch (Exception e)
        {
            return new[]
            {
                new ValidationMessage(new ExceptionSource(e)),
            };
        }
    }

    /// <summary>
    /// Update state and notify if properties changed.
    /// </summary>
    /// <param name="changedProperties">List of changed properties.</param>
    private void NotifyChangedProperties(IReadOnlyList<string> changedProperties)
    {
        if (changedProperties.Count == 0)
        {
            return;
        }

        ValidationMessages = _validatableProperties
            .Values
            .SelectMany(vp => vp.ValidationMessages)
            .ToList();
        IsValid = ValidationMessages.IsValid();
        HasWarnings = ValidationMessages.HasWarnings();

        foreach (var changedProperty in changedProperties)
        {
            _instance.OnPropertyMessagesChanged(changedProperty);
        }
    }

    /// <summary>
    /// Handle application culture changed event.
    /// </summary>
    private void OnCultureChanged(object? sender, CultureChangedEventArgs e)
    {
        foreach (ValidationMessage validationMessage in ValidationMessages)
        {
            validationMessage.UpdateMessage();
        }
    }

    /// <summary>
    /// Create list of display name for all properties of instance.
    /// </summary>
    private static Dictionary<string, IStringSource?> GetDisplayNames()
    {
        const BindingFlags bindingAttributes = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        var displayNamesSources = new Dictionary<string, IStringSource?>();
        PropertyInfo[] properties = typeof(TObject).GetProperties(bindingAttributes);

        foreach (PropertyInfo property in properties)
        {
            displayNamesSources.Add(property.Name,
                ValidationOptions.DisplayNameResolver.GetPropertyNameSource(property));
        }

        return displayNamesSources;
    }

    /// <summary>
    /// Get information of validatable properties.
    /// </summary>
    /// <param name="ruleBuilders">Rule builders.</param>
    private Dictionary<string, ValidatableProperty<TObject>> GetValidatableProperties(
        IReadOnlyList<IRuleBuilder<TObject>> ruleBuilders)
    {
        var propertyValidators = new Dictionary<string, List<IPropertyValidator<TObject>>>();
        var propertyCascadeModes = new Dictionary<string, CascadeMode>();

        foreach (IRuleBuilder<TObject> ruleBuilder in ruleBuilders)
        {
            IReadOnlyList<IPropertyValidator<TObject>> validators = ruleBuilder.GetValidators();

            foreach (var validatableProperty in ruleBuilder.ValidatableProperties)
            {
                if (!propertyValidators.TryGetValue(validatableProperty, out List<IPropertyValidator<TObject>>? value))
                {
                    value = new List<IPropertyValidator<TObject>>();
                    propertyValidators[validatableProperty] = value;
                }

                value.AddRange(validators);

                if (ruleBuilder.PropertyCascadeMode != null)
                {
                    propertyCascadeModes[validatableProperty] = ruleBuilder.PropertyCascadeMode.Value;
                }
                else if (!propertyCascadeModes.ContainsKey(validatableProperty))
                {
                    propertyCascadeModes[validatableProperty] = ValidationOptions.PropertyCascadeMode;
                }
            }
        }

        return propertyValidators.ToDictionary(
            pv => pv.Key,
            pv => new ValidatableProperty<TObject>(pv.Key, _displayNamesSources[pv.Key], propertyCascadeModes[pv.Key],
                pv.Value));
    }

    /// <summary>
    /// Get information of validatable properties settings.
    /// </summary>
    /// <param name="ruleBuilders">Rule builders.</param>
    private static Dictionary<string, ObservingPropertySettings> GetPropertySettings(
        IReadOnlyList<IRuleBuilder<TObject>> ruleBuilders)
    {
        var propertySettings = new Dictionary<string, ObservingPropertySettings>();

        foreach (IRuleBuilder<TObject> ruleBuilder in ruleBuilders)
        {
            ObservingPropertySettings settings = ruleBuilder.ObservingPropertiesSettings;

            if (settings.IsDefaultSettings)
            {
                continue;
            }

            foreach (var validatableProperty in ruleBuilder.ValidatableProperties)
            {
                if (!propertySettings.TryGetValue(validatableProperty, out ObservingPropertySettings? previousSettings))
                {
                    previousSettings = new ObservingPropertySettings();
                    propertySettings[validatableProperty] = previousSettings;
                }

                if (settings.PropertyValueFactoryMethod != null && previousSettings.PropertyValueFactoryMethod != null)
                {
                    throw new MethodAlreadyCalledException(
                        $"Validator factory method used twice for {typeof(TObject)}.{validatableProperty}");
                }

                if (settings.CollectionItemFactoryMethod != null &&
                    previousSettings.CollectionItemFactoryMethod != null)
                {
                    throw new MethodAlreadyCalledException(
                        $"Validator item factory method used twice for {typeof(TObject)}.{validatableProperty}");
                }

                previousSettings.TrackValueChanged |= settings.TrackValueChanged;
                previousSettings.TrackValueErrorsChanged |= settings.TrackValueErrorsChanged;
                previousSettings.TrackCollectionChanged |= settings.TrackCollectionChanged;
                previousSettings.TrackCollectionItemChanged |= settings.TrackCollectionItemChanged;
                previousSettings.TrackCollectionItemErrorsChanged |= settings.TrackCollectionItemErrorsChanged;
                previousSettings.PropertyValueFactoryMethod ??= settings.PropertyValueFactoryMethod;
                previousSettings.CollectionItemFactoryMethod ??= settings.CollectionItemFactoryMethod;
            }
        }

        return propertySettings;
    }
}
