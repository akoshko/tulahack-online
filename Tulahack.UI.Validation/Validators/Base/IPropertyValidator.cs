﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tulahack.UI.Validation.Models;
using Tulahack.UI.Validation.Resources.StringSources;
using Tulahack.UI.Validation.ValidatableObject;
using Tulahack.UI.Validation.Validators.Conditions;
using Tulahack.UI.Validation.Validators.Contexts;
using Tulahack.UI.Validation.Validators.Throttle;

namespace Tulahack.UI.Validation.Validators.Base;

/// <summary>
/// Validator which check property value.
/// </summary>
/// <typeparam name="TObject">Type of validatable object.</typeparam>
public interface IPropertyValidator<TObject>
    where TObject : IValidatableObject
{
    /// <summary>
    /// Validator executing asynchronously.
    /// </summary>
    /// <remarks>
    /// If <see langword="true" />, then <see cref="ValidatePropertyAsync" /> method will be called.
    /// Otherwise will be called <see cref="ValidateProperty" />.
    /// </remarks>
    bool IsAsync { get; }

    /// <remarks>
    /// Properties which can affect on state of validatable property.
    /// </remarks>
    IReadOnlyList<string> RelatedProperties { get; }


    /// <summary>
    /// Get validation messages of property.
    /// </summary>
    /// <param name="contextFactory">Factory which allows create validation context for property.</param>
    /// <returns>List of validation messages.</returns>
    IReadOnlyList<ValidationMessage> ValidateProperty(ValidationContextFactory<TObject> contextFactory);

    /// <summary>
    /// Get validation messages of property.
    /// </summary>
    /// <param name="contextFactory">Factory which allows create validation context for property.</param>
    /// <param name="cancellationToken">Token for cancelling validation.</param>
    /// <returns>List of validation messages.</returns>
    Task<IReadOnlyList<ValidationMessage>> ValidatePropertyAsync(ValidationContextFactory<TObject> contextFactory, CancellationToken cancellationToken);

    #region Settings

    /// <summary>
    /// Change the string source for validatable messages.
    /// </summary>
    /// <param name="stringSource">New string source.</param>
    void SetStringSource(IStringSource stringSource);

    /// <summary>
    /// Validate property only if <paramref name="condition" /> is <see langword="true" />.
    /// Property always valid if condition is <see langword="false" />.
    /// </summary>
    /// <param name="condition">Condition.</param>
    void ValidateWhen(IValidationCondition<TObject> condition);

    /// <summary>
    /// Allows to setup delay before property validation execution.
    /// If property changes value while this delay, previous value won't be validated.
    /// </summary>
    void Throttle(IPropertiesThrottle propertiesThrottle);

    #endregion
}