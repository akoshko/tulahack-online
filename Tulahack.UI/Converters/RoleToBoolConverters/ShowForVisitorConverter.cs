using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Tulahack.Dtos;

namespace Tulahack.UI.Converters.RoleToBoolConverters;

public class ShowForVisitorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is ContestRoleDto.Visitor;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value?.ToString();
}