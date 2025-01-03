using System;
using System.Linq.Expressions;
using Tulahack.UI.Validation.RuleBuilders;
using Tulahack.UI.Validation.ValidatableObject;
using Tulahack.UI.Validation.Validators.Conditions;

namespace Tulahack.UI.Validation.Extensions;

/// <summary>
/// Extensions method of <see cref="IRuleBuilder{TObject,TProp,TBuilder}.When" /> and <see cref="IRuleBuilderOption{TObject,TProp}.AllWhen" />.
/// </summary>
public static class RuleBuilderConditionExtensions
{
    #region When

    /// <summary>
    /// Last validator will check only if the condition is <see langword="true" />.
    /// </summary>
    public static IRuleBuilder<TObject, TProp, TNext> When<TNext, TObject, TProp>(
        this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
        Func<bool> condition)
        where TObject : IValidatableObject
        where TNext : IRuleBuilder<TObject, TProp, TNext> =>
        ruleBuilder.When(WrapCondition<TObject>(condition));

    /// <summary>
    /// Last validator will check only if the condition is <see langword="true" />.
    /// </summary>
    public static IRuleBuilder<TObject, TProp, TNext> When<TNext, TObject, TProp>(
        this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
        Expression<Func<TObject, bool>> conditionProperty)
        where TObject : IValidatableObject
        where TNext : IRuleBuilder<TObject, TProp, TNext> =>
        ruleBuilder.When(WrapCondition(conditionProperty));

    /// <summary>
    /// Last validator will check only if the condition is <see langword="true" />.
    /// </summary>
    public static IRuleBuilder<TObject, TProp, TNext> When<TNext, TObject, TProp, TParam>(
        this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
        Expression<Func<TObject, TParam>> property,
        Func<TParam, bool> condition)
        where TObject : IValidatableObject
        where TNext : IRuleBuilder<TObject, TProp, TNext> =>
        ruleBuilder.When(WrapCondition(condition, property));

    /// <summary>
    /// Last validator will check only if the condition is <see langword="true" />.
    /// </summary>
    public static IRuleBuilder<TObject, TProp, TNext> When<TNext, TObject, TProp, TParam1, TParam2>(
        this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
        Expression<Func<TObject, TParam1>> property1,
        Expression<Func<TObject, TParam2>> property2,
        Func<TParam1, TParam2, bool> condition)
        where TObject : IValidatableObject
        where TNext : IRuleBuilder<TObject, TProp, TNext> =>
        ruleBuilder.When(WrapCondition(condition, property1, property2));

    /// <summary>
    /// Last validator will check only if the condition is <see langword="true" />.
    /// </summary>
    public static IRuleBuilder<TObject, TProp, TNext> When<TNext, TObject, TProp, TParam1, TParam2, TParam3>(
        this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
        Expression<Func<TObject, TParam1>> property1,
        Expression<Func<TObject, TParam2>> property2,
        Expression<Func<TObject, TParam3>> property3,
        Func<TParam1, TParam2, TParam3, bool> condition)
        where TObject : IValidatableObject
        where TNext : IRuleBuilder<TObject, TProp, TNext> =>
        ruleBuilder.When(WrapCondition(condition, property1, property2, property3));

    #endregion

    #region AllWhen

    /// <summary>
    /// The validation of the rule will occur only if the condition is <see langword="true" />.
    /// </summary>
    public static IRuleBuilderOption<TObject, TProp> AllWhen<TNext, TObject, TProp>(
        this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
        Func<bool> condition)
        where TObject : IValidatableObject
        where TNext : IRuleBuilder<TObject, TProp, TNext> =>
        ruleBuilder.AllWhen(WrapCondition<TObject>(condition));

    /// <summary>
    /// The validation of the rule will occur only if the property value is <see langword="true" />.
    /// </summary>
    public static IRuleBuilderOption<TObject, TProp> AllWhen<TNext, TObject, TProp>(
        this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
        Expression<Func<TObject, bool>> conditionProperty)
        where TObject : IValidatableObject
        where TNext : IRuleBuilder<TObject, TProp, TNext> =>
        ruleBuilder.AllWhen(WrapCondition(conditionProperty));

    /// <summary>
    /// The validation of the rule will occur only if the condition is <see langword="true" />.
    /// </summary>
    public static IRuleBuilderOption<TObject, TProp> AllWhen<TNext, TObject, TProp, TParam>(
        this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
        Expression<Func<TObject, TParam>> property, Func<TParam, bool> condition)
        where TObject : IValidatableObject
        where TNext : IRuleBuilder<TObject, TProp, TNext> =>
        ruleBuilder.AllWhen(WrapCondition(condition, property));

    /// <summary>
    /// The validation of the rule will occur only if the condition is <see langword="true" />.
    /// </summary>
    public static IRuleBuilderOption<TObject, TProp> AllWhen<TNext, TObject, TProp, TParam1, TParam2>(
        this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
        Expression<Func<TObject, TParam1>> property1,
        Expression<Func<TObject, TParam2>> property2,
        Func<TParam1, TParam2, bool> condition)
        where TObject : IValidatableObject
        where TNext : IRuleBuilder<TObject, TProp, TNext> =>
        ruleBuilder.AllWhen(WrapCondition(condition, property1, property2));

    /// <summary>
    /// The validation of the rule will occur only if the condition is <see langword="true" />.
    /// </summary>
    public static IRuleBuilderOption<TObject, TProp> AllWhen<TNext, TObject, TProp, TParam1, TParam2, TParam3>(
        this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
        Expression<Func<TObject, TParam1>> property1,
        Expression<Func<TObject, TParam2>> property2,
        Expression<Func<TObject, TParam3>> property3,
        Func<TParam1, TParam2, TParam3, bool> condition)
        where TObject : IValidatableObject
        where TNext : IRuleBuilder<TObject, TProp, TNext> =>
        ruleBuilder.AllWhen(WrapCondition(condition, property1, property2, property3));

    #endregion

    private static FuncValidationCondition<TObject> WrapCondition<TObject>(Func<bool> condition)
        where TObject : IValidatableObject =>
        new(_ => condition());

    private static FuncValidationCondition<TObject> WrapCondition<TObject>(
        Expression<Func<TObject, bool>> conditionProperty) where TObject : IValidatableObject =>
        new(conditionProperty.Compile(), conditionProperty);

    private static FuncValidationCondition<TObject> WrapCondition<TObject, TParam>(
        Func<TParam, bool> condition,
        Expression<Func<TObject, TParam>> property)
        where TObject : IValidatableObject
    {
        Func<TObject, TParam> paramFunc = property.Compile();

        return new FuncValidationCondition<TObject>(instance =>
        {
            TParam param = paramFunc.Invoke(instance);
            return condition.Invoke(param);
        }, property);
    }

    private static FuncValidationCondition<TObject> WrapCondition<TObject, TParam1, TParam2>(
        Func<TParam1, TParam2, bool> condition,
        Expression<Func<TObject, TParam1>> property1,
        Expression<Func<TObject, TParam2>> property2)
        where TObject : IValidatableObject
    {
        Func<TObject, TParam1> param1Func = property1.Compile();
        Func<TObject, TParam2> param2Func = property2.Compile();

        return new FuncValidationCondition<TObject>(instance =>
        {
            TParam1 param1 = param1Func.Invoke(instance);
            TParam2 param2 = param2Func.Invoke(instance);

            return condition.Invoke(param1, param2);
        }, property1, property2);
    }

    private static FuncValidationCondition<TObject> WrapCondition<TObject, TParam1, TParam2, TParam3>(
        Func<TParam1, TParam2, TParam3, bool> condition,
        Expression<Func<TObject, TParam1>> property1,
        Expression<Func<TObject, TParam2>> property2,
        Expression<Func<TObject, TParam3>> property3)
        where TObject : IValidatableObject
    {
        Func<TObject, TParam1> param1Func = property1.Compile();
        Func<TObject, TParam2> param2Func = property2.Compile();
        Func<TObject, TParam3> param3Func = property3.Compile();

        return new FuncValidationCondition<TObject>(instance =>
        {
            TParam1 param1 = param1Func.Invoke(instance);
            TParam2 param2 = param2Func.Invoke(instance);
            TParam3 param3 = param3Func.Invoke(instance);

            return condition.Invoke(param1, param2, param3);
        }, property1, property2, property3);
    }
}