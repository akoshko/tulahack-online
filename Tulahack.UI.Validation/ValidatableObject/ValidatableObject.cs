﻿using System;
using System.Collections;
using System.ComponentModel;
using Tulahack.UI.Validation.Models;
using Tulahack.UI.Validation.ObjectValidator;

namespace Tulahack.UI.Validation.ValidatableObject;

/// <inheritdoc cref="IValidatableObject" />
public class ValidatableObject : BaseNotifyPropertyChanged, IValidatableObject
{
    private IObjectValidator? _objectValidator;

    /// <inheritdoc />
    public ValidatableObject() { }


    /// <inheritdoc />
    public IObjectValidator? Validator
    {
        get => _objectValidator;
        set
        {
            _objectValidator?.Dispose();
            _objectValidator = value;
            _objectValidator?.Revalidate();
        }
    }

    /// <inheritdoc />
    public virtual void OnPropertyMessagesChanged(string propertyName) =>
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));


    #region INotifyDataErrorInfo

    /// <inheritdoc />
#pragma warning disable CA1033
    bool INotifyDataErrorInfo.HasErrors => Validator?.IsValid == false || Validator?.HasWarnings == true;
#pragma warning restore CA1033

    /// <inheritdoc />
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    /// <inheritdoc />
    public IEnumerable GetErrors(string? propertyName)
    {
        if (Validator == null)
        {
            return Array.Empty<ValidationMessage>();
        }

        return string.IsNullOrEmpty(propertyName)
            ? Validator.ValidationMessages
            : Validator.GetMessages(propertyName);
    }

    #endregion
}
