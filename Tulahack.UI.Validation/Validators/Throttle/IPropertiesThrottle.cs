using System.Threading;
using System.Threading.Tasks;
using Tulahack.UI.Validation.ValidatableObject;
using Tulahack.UI.Validation.Validators.Contexts;

namespace Tulahack.UI.Validation.Validators.Throttle;

/// <summary>
/// Allows to setup delay before property validation execution.
/// If property changes value while this delay, previous value won't be validated.
/// </summary>
public interface IPropertiesThrottle
{
    /// <summary>
    /// Execute delay after property value changed.
    /// </summary>
    public Task DelayAsync<TObject>(ValidationContextFactory<TObject> validationContextFactory, CancellationToken cancellationToken)
        where TObject : IValidatableObject;
}