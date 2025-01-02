using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Tulahack.UI.Validation.Enums;
using Tulahack.UI.Validation.Helpers;
using Tulahack.UI.Validation.ObjectValidator;
using Tulahack.UI.Validation.RuleBuilders.ValidatorBuilders;
using Tulahack.UI.Validation.ValidatableObject;
using Tulahack.UI.Validation.ValidatorFactory;
using Tulahack.UI.Validation.Validators.PropertyValueTransformers;

namespace Tulahack.UI.Validation.RuleBuilders;

/// <summary>
/// Defines a validation rules for an object.
/// Derive from this class to create validation rules for object.
/// </summary>
/// <typeparam name="TObject">Type of validatable object.</typeparam>
public abstract class ValidationRuleBuilder<TObject> : IValidationBuilder<TObject>, IObjectValidatorBuilderCreator
    where TObject : IValidatableObject
{
    private readonly List<IRuleBuilder<TObject>> _rulesBuilders;

    /// <summary>
    /// Create instance of <see cref="ValidationBuilder{TObject}" /> class.
    /// </summary>
    protected ValidationRuleBuilder()
    {
        _rulesBuilders = new List<IRuleBuilder<TObject>>();
    }

    /// <inheritdoc />
    public CascadeMode? PropertyCascadeMode { get; set; }

    /// <inheritdoc />
    public IObjectValidator Build(TObject instance) =>
        CreateBuilder().Build(instance);

    /// <inheritdoc />
    public IObjectValidatorBuilder Create() =>
        CreateBuilder();


    /// <inheritdoc />
    public ISinglePropertyRuleBuilderInitial<TObject, TProp> RuleFor<TProp>(Expression<Func<TObject, TProp>> property)
    {
        var propertyName = GetPropertyNameForValidator(property);
        var ruleBuilder = new SinglePropertyRuleBuilder<TObject, TProp>(propertyName);

        _rulesBuilders.Add(ruleBuilder);

        return ruleBuilder;
    }

    /// <inheritdoc />
    public ISinglePropertyRuleBuilderInitial<TObject, object?> RuleFor(string propertyName)
    {
        var ruleBuilder = new SinglePropertyRuleBuilder<TObject, object?>(propertyName);
        _rulesBuilders.Add(ruleBuilder);

        return ruleBuilder;
    }


    /// <inheritdoc />
    public ISinglePropertyRuleBuilderInitial<TObject, TPropConverter> Transform<TProp, TPropConverter>(
        Expression<Func<TObject, TProp>> property,
        IValueTransformer<TObject, TPropConverter> valueTransformer)
    {
        var propertyName = GetPropertyNameForValidator(property);
        var ruleBuilder = new SinglePropertyRuleBuilder<TObject, TPropConverter>(propertyName, valueTransformer);

        _rulesBuilders.Add(ruleBuilder);

        return ruleBuilder;
    }


    /// <inheritdoc />
    public IPropertiesRuleBuilderInitial<TObject> RuleFor(params Expression<Func<TObject, object?>>[] properties)
    {
        var propertiesNames = properties.Select(GetPropertyNameForValidator).ToArray();

        return RuleFor(propertiesNames);
    }

    /// <inheritdoc />
    public IPropertiesRuleBuilderInitial<TObject> RuleFor(params string[] propertiesNames)
    {
        var ruleBuilder = new PropertiesRuleBuilder<TObject>(propertiesNames);
        _rulesBuilders.Add(ruleBuilder);

        return ruleBuilder;
    }


    /// <inheritdoc />
    public ICollectionRuleBuilderInitial<TObject, TCollection, TItem> RuleForCollection<TCollection, TItem>(
        Expression<Func<TObject, TCollection>> collection)
        where TCollection : IEnumerable<TItem>
    {
        var propertyName = GetPropertyNameForValidator(collection);
        var ruleBuilder = new CollectionPropertyRuleBuilder<TObject, TCollection, TItem>(propertyName);

        _rulesBuilders.Add(ruleBuilder);

        return ruleBuilder;
    }


    /// <summary>
    /// Create prepared builder.
    /// </summary>
    private ObjectValidatorBuilder<TObject> CreateBuilder()
    {
        if (PropertyCascadeMode != null)
        {
            foreach (IRuleBuilder<TObject> ruleBuilder in _rulesBuilders)
            {
                ruleBuilder.PropertyCascadeMode ??= PropertyCascadeMode;
            }
        }

        return new ObjectValidatorBuilder<TObject>(_rulesBuilders);
    }

    /// <summary>
    /// Get property name by expression for validator. Throws ArgumentException, if couldn't validate this property.
    /// </summary>
    /// <typeparam name="TProp">The type of element of collection.</typeparam>
    /// <param name="property">Expression which contains property.</param>
    /// <returns>Property name.</returns>
    private static string GetPropertyNameForValidator<TProp>(Expression<Func<TObject, TProp>> property)
    {
        System.Reflection.PropertyInfo propertyInfo = ReactiveValidationHelper.GetPropertyInfo(typeof(TObject), property);
        return propertyInfo.Name;
    }
}
