using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Tulahack.UI.Converters;

public class TeamCheckResultToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is true ? Brushes.SeaGreen : Brushes.IndianRed;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value?.ToString() ?? "Transparent";
}