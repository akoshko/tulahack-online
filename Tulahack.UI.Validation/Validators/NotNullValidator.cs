﻿using Tulahack.UI.Validation.Enums;
using Tulahack.UI.Validation.Helpers;
using Tulahack.UI.Validation.Resources.StringSources;
using Tulahack.UI.Validation.ValidatableObject;
using Tulahack.UI.Validation.Validators.Base;
using Tulahack.UI.Validation.Validators.Contexts;

namespace Tulahack.UI.Validation.Validators;

/// <summary>
/// Validator which check property value is not <see langword="null" />.
/// </summary>
/// <typeparam name="TObject">The type of validatable object.</typeparam>
/// <typeparam name="TProp">The type of validatable property.</typeparam>
public class NotNullValidator<TObject, TProp> : BaseSyncPropertyValidator<TObject, TProp>
    where TObject : IValidatableObject
{
    /// <summary>
    /// Initialize a new instance of <see cref="NotNullValidator{TObject,TProp}" /> class.
    /// </summary>
    /// <param name="validationMessageType">The type validatable message.</param>
    public NotNullValidator(ValidationMessageType validationMessageType)
        : base(new LanguageStringSource(ValidatorsNames.NotNullValidator), validationMessageType)
    { }


    /// <inheritdoc />
    protected override bool IsValid(ValidationContext<TObject, TProp> context) =>
        context.PropertyValue != null;
}