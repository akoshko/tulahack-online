using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Tulahack.UI.Converters;

public class TeamCheckStatusConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is null ? "Ничего не найдено" : "Команда найдена!";

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value?.ToString();
}