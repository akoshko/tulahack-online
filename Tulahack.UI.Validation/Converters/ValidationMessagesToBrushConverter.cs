using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using Tulahack.UI.Validation.Enums;
using Tulahack.UI.Validation.Models;

namespace Tulahack.UI.Validation.Converters;

/// <summary>
/// Converter from list of validation messages to brush.
/// </summary>
internal class ValidationMessagesToBrushConverter : IMultiValueConverter
{
    /// <inheritdoc />
    /// <remarks>
    /// values[0] - ValidationMessageType.
    /// values[1] - Error brush.
    /// values[2] - Warning brush.
    /// </remarks>
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count < 3)
        {
            return null;
        }

        var validationMessages = values[0] as IEnumerable<object>;
        // ReSharper disable once PossibleMultipleEnumeration
        if (validationMessages?.Any() != true)
        {
            return null;
        }

        // ReSharper disable once PossibleMultipleEnumeration
        var isAllMessagesAreWarnings = validationMessages
            .All(vm => vm is ValidationMessage { ValidationMessageType: ValidationMessageType.Warning or ValidationMessageType.SimpleWarning });
        var errorBrush = values[1];
        var warningBrush = values[2];
        return isAllMessagesAreWarnings
            ? warningBrush
            : errorBrush;
    }
}