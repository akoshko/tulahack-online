using Tulahack.UI.Validation.ValidatableObject;

namespace Tulahack.UI.Validation.RuleBuilders
{
    /// <inheritdoc cref="IValidationBuilder{TObject}" />
    public class ValidationBuilder<TObject> : ValidationRuleBuilder<TObject>
        where TObject : IValidatableObject
    {
    }
}
