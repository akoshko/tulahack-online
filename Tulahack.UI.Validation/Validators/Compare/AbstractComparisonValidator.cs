﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Tulahack.UI.Validation.Enums;
using Tulahack.UI.Validation.Models;
using Tulahack.UI.Validation.Resources.StringSources;
using Tulahack.UI.Validation.ValidatableObject;
using Tulahack.UI.Validation.Validators.Base;
using Tulahack.UI.Validation.Validators.Contexts;

namespace Tulahack.UI.Validation.Validators.Compare;

/// <summary>
/// Base validator which compare property value with specified value.
/// </summary>
/// <typeparam name="TObject">Type of validatable object.</typeparam>
/// <typeparam name="TProp">Type of validatable property.</typeparam>
/// <typeparam name="TParam">The type of comparison value.</typeparam>
public abstract class AbstractComparisonValidator<TObject, TProp, TParam> : BaseSyncPropertyValidator<TObject, TProp>
    where TObject : IValidatableObject
{
    private readonly IComparer _comparer;
    private readonly ValidatorParameter<TObject, TParam> _valueToCompare;

    /// <summary>
    /// Initialize a new instance of <see cref="AbstractComparisonValidator{TObject,TProp,TParam}" /> class.
    /// </summary>
    /// <param name="stringSource">The source for validation message.</param>
    /// <param name="valueToCompareExpression">Expression of value to compare.</param>
    /// <param name="comparer">The values comparer.</param>
    /// <param name="validationMessageType">Type of validation message.</param>
    protected AbstractComparisonValidator(
        IStringSource stringSource,
        Expression<Func<TObject, TParam>> valueToCompareExpression,
        IComparer? comparer,
        ValidationMessageType validationMessageType)
        : base(stringSource, validationMessageType, valueToCompareExpression)
    {
        _comparer = comparer ?? Comparer<TProp>.Default;
        _valueToCompare = new ValidatorParameter<TObject, TParam>(valueToCompareExpression);
    }

    /// <inheritdoc />
    protected sealed override bool IsValid(ValidationContext<TObject, TProp> context)
    {
        TProp? propertyValue = context.PropertyValue;

        if (propertyValue == null)
        {
            return true;
        }

        TParam paramValue = context.GetParamValue(_valueToCompare);
        var comparisonResult = _comparer.Compare(propertyValue, paramValue);

        if (IsValid(comparisonResult) == false)
        {
            context.RegisterMessageArgument("ValueToCompare", _valueToCompare, paramValue);
            return false;
        }

        return true;
    }

    /// <summary>
    /// Check is valid by comparison result.
    /// </summary>
    /// <param name="comparisonResult">Comparison result.</param>
    protected abstract bool IsValid(int comparisonResult);
}
