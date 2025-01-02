using System;
using System.Collections;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Tulahack.UI.Converters;

public class CountEnumerableConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is ICollection enumerable ? enumerable.Count : 1;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is null ? default : (int)value;
}