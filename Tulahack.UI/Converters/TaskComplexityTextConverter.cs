using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Tulahack.Dtos;

namespace Tulahack.UI.Converters;

public class TaskComplexityTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return Brushes.Transparent;
        }

        var complexity = (ContestCaseComplexityDto)value;
        return complexity switch
        {
            ContestCaseComplexityDto.Beginner => "Beginner",
            ContestCaseComplexityDto.Easy => "Easy",
            ContestCaseComplexityDto.Normal => "Normal",
            ContestCaseComplexityDto.Hard => "Hard",
            ContestCaseComplexityDto.Insane => "Insane",
            ContestCaseComplexityDto.Unknown => "Unknown",
            _ => "Undefined"
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => BindingOperations.DoNothing;
}