using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Tulahack.UI.Validation.Enums;
using Tulahack.UI.Validation.Models;
using Tulahack.UI.Validation.Resources.StringSources;
using Tulahack.UI.Validation.ValidatableObject;
using Tulahack.UI.Validation.Validators.Contexts;

namespace Tulahack.UI.Validation.Validators.Base;

/// <summary>
/// Base class of async validators for property value.
/// </summary>
/// <typeparam name="TObject">Type of validatable object.</typeparam>
/// <typeparam name="TProp">Type of validatable property.</typeparam>
public abstract class BaseAsyncPropertyValidator<TObject, TProp> : BasePropertyValidator<TObject, TProp>
    where TObject : IValidatableObject
{
    /// <summary>
    /// Create new async validator for property value.
    /// </summary>
    /// <param name="stringSource">Source for validation message.</param>
    /// <param name="validationMessageType">Type of validation message.</param>
    /// <param name="relatedProperties">Properties which can affect on state of validatable property.</param>
    protected BaseAsyncPropertyValidator(
        IStringSource stringSource,
        ValidationMessageType validationMessageType,
        params LambdaExpression[] relatedProperties
    ) : base(stringSource, validationMessageType, relatedProperties)
    {
    }

    /// <inheritdoc />
    public sealed override bool IsAsync => true;

    /// <inheritdoc />
    public sealed override IReadOnlyList<ValidationMessage> ValidateProperty(ValidationContextFactory<TObject> contextFactory) =>
        throw new NotSupportedException();

    /// <inheritdoc />
    public async sealed override Task<IReadOnlyList<ValidationMessage>> ValidatePropertyAsync(ValidationContextFactory<TObject> contextFactory, CancellationToken cancellationToken)
    {
        if (CheckIgnoreValidation(contextFactory))
        {
            return Array.Empty<ValidationMessage>();
        }

        if (HasThrottle)
        {
            await ThrottleAsync(contextFactory, cancellationToken).ConfigureAwait(false);
        }

        ValidationContext<TObject, TProp> context = contextFactory.CreateContext<TProp>();
        if (await IsValidAsync(context, cancellationToken).ConfigureAwait(false))
        {
            return Array.Empty<ValidationMessage>();
        }

        return GetValidationMessages(context);
    }

    /// <summary>
    /// Check async if property is valid.
    /// </summary>
    /// <param name="context">Validation context.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// <see langword="true" />, if property are valid.
    /// <see langword="false" /> otherwise.
    /// </returns>
    protected abstract Task<bool> IsValidAsync(ValidationContext<TObject, TProp> context,
        CancellationToken cancellationToken);
}