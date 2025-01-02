using Tulahack.UI.Validation.Enums;
using Tulahack.UI.Validation.Helpers;
using Tulahack.UI.Validation.Resources.StringSources;
using Tulahack.UI.Validation.ValidatableObject;
using Tulahack.UI.Validation.Validators.Base;
using Tulahack.UI.Validation.Validators.Contexts;

namespace Tulahack.UI.Validation.Validators.Strings;

/// <summary>
/// Validator which check property value is not null or empty string.
/// </summary>
/// <typeparam name="TObject">Type of validatable object.</typeparam>
public class NotEmptyStringValidator<TObject> : BaseSyncPropertyValidator<TObject, string>
    where TObject : IValidatableObject
{
    /// <summary>
    /// Initialize a new instance of <see cref="NotEmptyStringValidator{TObject}" /> class.
    /// </summary>
    /// <param name="validationMessageType">Type of validation message.</param>
    public NotEmptyStringValidator(ValidationMessageType validationMessageType)
        : base(new LanguageStringSource(ValidatorsNames.NotEmptyStringValidator), validationMessageType)
    {}


    /// <inheritdoc />
    protected override bool IsValid(ValidationContext<TObject, string> context) =>
        string.IsNullOrEmpty(context.PropertyValue) == false;
}