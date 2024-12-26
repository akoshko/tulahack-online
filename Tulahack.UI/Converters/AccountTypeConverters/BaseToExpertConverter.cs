using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Tulahack.Dtos;

namespace Tulahack.UI.Converters.AccountTypeConverters;

public class BaseToExpertConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value as ExpertDto;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value?.ToString();
}