using System;
using Tulahack.UI.Validation.ValidatorFactory;

namespace Tulahack.UI.Validation.Exceptions;

/// <summary>
/// Exception thrown when builder already registered in <see cref="IValidatorFactory" />.
/// </summary>
public class ObjectValidatorBuilderAlreadyRegisteredException : Exception
{
    /// <summary>
    /// Initialize a new instance of <see cref="ObjectValidatorBuilderAlreadyRegisteredException" /> class.
    /// </summary>
    public ObjectValidatorBuilderAlreadyRegisteredException(Type type) : base(
        $"Object validator builder already registered for type {type}")
    { }
}
