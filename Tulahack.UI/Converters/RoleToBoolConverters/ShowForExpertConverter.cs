using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Tulahack.Dtos;

namespace Tulahack.UI.Converters.RoleToBoolConverters;

public class ShowForExpertConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is ContestRoleDto.Expert;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value?.ToString();
}