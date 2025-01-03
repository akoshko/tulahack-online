using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Tulahack.UI.Validation.ValidatableObject;
using Tulahack.UI.Validation.Validators.Contexts;

namespace Tulahack.UI.Validation.Validators.Conditions;

/// <inheritdoc />
public class FuncValidationCondition<TObject> : IValidationCondition<TObject>
    where TObject : IValidatableObject
{
    private readonly Func<TObject, bool> _conditionFunc;

    /// <summary>
    /// Validation will be executed only if the condition is <see langword="true" />.
    /// </summary>
    /// <param name="conditionFunc">Condition.</param>
    /// <param name="relatedProperties">Properties which can affect on state of validatable condition.</param>
    public FuncValidationCondition(Func<TObject, bool> conditionFunc, params LambdaExpression[] relatedProperties)
    {
        _conditionFunc = conditionFunc;
        RelatedProperties = relatedProperties;
    }


    /// <inheritdoc />
    public IReadOnlyList<LambdaExpression> RelatedProperties { get; }


    /// <inheritdoc />
    public bool ShouldIgnoreValidation(ValidationContextFactory<TObject> validationContextFactory)
    {
        ValidationContextCache validationCache = validationContextFactory.ValidationContextCache;
        if (validationCache.TryGetValue(this, out var shouldIgnoreObject))
        {
            return (bool)shouldIgnoreObject!;
        }

        var shouldIgnore = !_conditionFunc.Invoke(validationContextFactory.ValidatableObject);
        validationCache.SetValue(this, shouldIgnore);
        return shouldIgnore;
    }
}