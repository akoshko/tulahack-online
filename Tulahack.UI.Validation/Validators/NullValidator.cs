using Tulahack.UI.Validation.Enums;
using Tulahack.UI.Validation.Helpers;
using Tulahack.UI.Validation.Resources.StringSources;
using Tulahack.UI.Validation.ValidatableObject;
using Tulahack.UI.Validation.Validators.Base;
using Tulahack.UI.Validation.Validators.Contexts;

namespace Tulahack.UI.Validation.Validators
{
    /// <summary>
    /// Validator which check property value is <see langword="null" />.
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TProp">The type of validatable property.</typeparam>
    public class NullValidator<TObject, TProp> : BaseSyncPropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
    {
        /// <summary>
        /// Initialize a new instance of <see cref="NullValidator{TObject,TProp}" /> class.
        /// </summary>
        /// <param name="validationMessageType">The type validatable message.</param>
        public NullValidator(ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.NullValidator), validationMessageType)
        { }


        /// <inheritdoc />
        protected override bool IsValid(ValidationContext<TObject, TProp> context)
        {
            return context.PropertyValue == null;
        }
    }
}
