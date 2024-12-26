using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Tulahack.Dtos;

namespace Tulahack.UI.Converters;

public class TaskComplexityBrushConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null) return Brushes.Transparent;
        var complexity = (ContestCaseComplexityDto)value;
        return complexity switch
        {
            ContestCaseComplexityDto.Beginner => Brushes.DarkGreen,
            ContestCaseComplexityDto.Easy => Brushes.CornflowerBlue,
            ContestCaseComplexityDto.Normal => Brushes.Goldenrod,
            ContestCaseComplexityDto.Hard => Brushes.IndianRed,
            ContestCaseComplexityDto.Insane => Brushes.DarkRed,
            ContestCaseComplexityDto.Unknown => Brushes.Transparent,
            _ => Brushes.Transparent
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value?.ToString() ?? "Transparent";
}