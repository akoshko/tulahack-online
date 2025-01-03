﻿using System;
using System.Diagnostics.CodeAnalysis;
using Tulahack.UI.Validation.Exceptions;
using Tulahack.UI.Validation.ObjectValidator;
using Tulahack.UI.Validation.ValidatableObject;

namespace Tulahack.UI.Validation.ValidatorFactory;

/// <summary>
/// Factory which allow create <see cref="IObjectValidator" /> for specified object.
/// </summary>
public interface IValidatorFactory
{
    /// <summary>
    /// Create validator for object.
    /// </summary>
    /// <param name="instance">Instance of validatable object.</param>
    /// <returns>Specified object validator.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="instance" /> is <see langword="null" />.</exception>
    /// <exception cref="ObjectValidatorBuilderNotFoundException">If not found validation rule builder for <paramref name="instance"/> or it base classes.</exception>
    IObjectValidator GetValidator(IValidatableObject instance);

    /// <summary>
    /// Try create validator for object.
    /// </summary>
    /// <param name="instance">Instance of validatable object.</param>
    /// <param name="objectValidator">Specified object validator.</param>
    /// <returns>
    /// <see langword="true" /> if exists validation rules for instance.
    /// <see langword="false" /> otherwise.
    /// </returns>
    /// <exception cref="ArgumentNullException">If <paramref name="instance"/> is <see langword="null" />.</exception>
    bool TryGetValidator<TObject>(IValidatableObject instance,
        [NotNullWhen(true)] out IObjectValidator? objectValidator);
}
