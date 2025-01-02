using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Tulahack.UI.Validation.Exceptions;
using Tulahack.UI.Validation.ObjectValidator;
using Tulahack.UI.Validation.RuleBuilders.ValidatorBuilders;
using Tulahack.UI.Validation.ValidatableObject;

namespace Tulahack.UI.Validation.ValidatorFactory;

/// <inheritdoc />
internal class DefaultValidatorFactory : IValidatorFactory
{
    private readonly Dictionary<Type, IObjectValidatorBuilder> _validatorsBuilder;

    /// <summary>
    /// Create instance of <see cref="DefaultValidatorFactory" /> class.
    /// </summary>
    public DefaultValidatorFactory()
    {
        _validatorsBuilder = new Dictionary<Type, IObjectValidatorBuilder>();
    }

    /// <summary>
    /// Registration of new object validator builder using its creator.
    /// </summary>
    /// <param name="creator">Creator of object validator builder.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public void Register(IObjectValidatorBuilderCreator creator)
    {
        ArgumentNullException.ThrowIfNull(creator);

        IObjectValidatorBuilder builder = creator.Create();

        if (builder == null)
        {
            throw new ArgumentNullException(nameof(creator));
        }

        if (_validatorsBuilder.ContainsKey(builder.SupportedType))
        {
            throw new ObjectValidatorBuilderAlreadyRegisteredException(builder.SupportedType);
        }

        _validatorsBuilder.Add(builder.SupportedType, builder);
    }

    /// <summary>
    /// Registration of new object validator builders using its creators which searching in specified assembly.
    /// </summary>
    /// <param name="assembly">Assembly, which contains creators.</param>
    /// <param name="factoryMethod">
    /// Method, which allows get creator by its type.
    /// This can be DI method.
    /// </param>
    public void Register(Assembly assembly, Func<Type, IObjectValidatorBuilderCreator>? factoryMethod = null)
    {
        IEnumerable<Type> creatorTypes = assembly
            .GetTypes()
            .Where(p => typeof(IObjectValidatorBuilderCreator).IsAssignableFrom(p));

        foreach (Type creatorType in creatorTypes)
        {
            IObjectValidatorBuilderCreator creator = factoryMethod != null
                ? factoryMethod.Invoke(creatorType)
                : (IObjectValidatorBuilderCreator)Activator.CreateInstance(creatorType)!;

            Register(creator);
        }
    }

    /// <inheritdoc />
    public IObjectValidator GetValidator(IValidatableObject instance)
    {
        ArgumentNullException.ThrowIfNull(instance);

        if (!TryGetValidatorBuilder(instance.GetType(), out var builder))
        {
            throw new ObjectValidatorBuilderNotFoundException(instance.GetType());
        }

        return builder.Build(instance);
    }

    /// <inheritdoc />
    public bool TryGetValidator<TObject>(IValidatableObject instance,
        [NotNullWhen(true)] out IObjectValidator? objectValidator)
    {
        ArgumentNullException.ThrowIfNull(instance);

        objectValidator = null;

        if (!TryGetValidatorBuilder(instance.GetType(), out var builder))
        {
            return false;
        }

        objectValidator = builder.Build(instance);
        return true;
    }

    /// <summary>
    /// Try get validator builder for specified type.
    /// If cannot find - try find validator builder for base classes.
    /// </summary>
    /// <param name="validatableObjectType">Type of validatable object.</param>
    /// <param name="builder">Found builder.</param>
    /// <returns>
    /// <see langword="true" /> if builder founded for type or base types.
    /// <see langword="false" /> otherwise.
    /// </returns>
    private bool TryGetValidatorBuilder(Type validatableObjectType,
        [NotNullWhen(true)] out IObjectValidatorBuilder? builder)
    {
        builder = null;

        while (true)
        {
            if (_validatorsBuilder.TryGetValue(validatableObjectType, out builder))
            {
                return true;
            }

            if (validatableObjectType.BaseType == null)
            {
                return false;
            }

            validatableObjectType = validatableObjectType.BaseType;
        }
    }
}
