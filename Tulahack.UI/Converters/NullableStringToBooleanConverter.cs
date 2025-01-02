using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Tulahack.UI.Converters;

public class IntToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value?.ToString() ?? string.Empty;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is null ? default : (int)value;
}