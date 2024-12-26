using Tulahack.UI.Validation.RuleBuilders.ValidatorBuilders;

namespace Tulahack.UI.Validation.ValidatorFactory
{
    /// <summary>
    /// Allows get prepared builder for object validation creating.
    /// </summary>
    public interface IObjectValidatorBuilderCreator
    {
        /// <summary>
        /// Create prepared builder.
        /// </summary>
        IObjectValidatorBuilder Create();
    }
}
