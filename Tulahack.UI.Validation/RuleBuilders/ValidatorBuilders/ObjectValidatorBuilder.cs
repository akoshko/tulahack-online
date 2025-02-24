﻿using System;
using System.Collections.Generic;
using Tulahack.UI.Validation.ObjectValidator;
using Tulahack.UI.Validation.ValidatableObject;

namespace Tulahack.UI.Validation.RuleBuilders.ValidatorBuilders;

/// <inheritdoc cref="IObjectValidatorBuilder" />
internal class ObjectValidatorBuilder<TObject> : IObjectValidatorBuilder
    where TObject : IValidatableObject
{
    private readonly IReadOnlyList<IRuleBuilder<TObject>> _rulesBuilders;

    /// <summary>
    /// Create validation builder with specified adapters.
    /// </summary>
    public ObjectValidatorBuilder(IReadOnlyList<IRuleBuilder<TObject>> rulesBuilders)
    {
        _rulesBuilders = rulesBuilders;
    }

    /// <inheritdoc />
    public Type SupportedType => typeof(TObject);

    /// <inheritdoc />
    public IObjectValidator Build(IValidatableObject instance) =>
        instance switch
        {
            null => throw new ArgumentNullException(nameof(instance)),
            TObject i => new ObjectValidator<TObject>(i, _rulesBuilders),
            _ => throw new NotSupportedException(
                $"Cannot create validator for type {instance.GetType()}, supported only {typeof(TObject)}")
        };
}