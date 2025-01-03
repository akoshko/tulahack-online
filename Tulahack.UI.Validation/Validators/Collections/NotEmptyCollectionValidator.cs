﻿using System.Collections.Generic;
using System.Linq;
using Tulahack.UI.Validation.Enums;
using Tulahack.UI.Validation.Helpers;
using Tulahack.UI.Validation.Resources.StringSources;
using Tulahack.UI.Validation.ValidatableObject;
using Tulahack.UI.Validation.Validators.Base;
using Tulahack.UI.Validation.Validators.Contexts;

namespace Tulahack.UI.Validation.Validators.Collections;

/// <summary>
/// Validator which check that collection contains at least one item.
/// </summary>
/// <typeparam name="TObject">The type of validatable object.</typeparam>
/// <typeparam name="TCollection">The type of collection.</typeparam>
/// <typeparam name="TItem">The type of element of collection.</typeparam>
public class NotEmptyCollectionValidator<TObject, TCollection, TItem> : BaseSyncPropertyValidator<TObject, TCollection>
    where TObject : IValidatableObject
    where TCollection : IEnumerable<TItem>
{
    /// <summary>
    /// Initialize a new instance of <see cref="NotEmptyCollectionValidator{TObject,TCollection,TItem}" /> class.
    /// </summary>
    /// <param name="validationMessageType">Type of validation message.</param>
    public NotEmptyCollectionValidator(ValidationMessageType validationMessageType)
        : base(new LanguageStringSource(ValidatorsNames.NotEmptyCollectionValidator), validationMessageType)
    {
    }


    /// <inheritdoc />
    protected override bool IsValid(ValidationContext<TObject, TCollection> context) =>
        context.PropertyValue?.Any() == true;
}