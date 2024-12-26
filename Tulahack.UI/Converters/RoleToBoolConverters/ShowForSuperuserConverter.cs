using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Tulahack.Dtos;

namespace Tulahack.UI.Converters.RoleToBoolConverters;

public class ShowForSuperuserConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is ContestRoleDto.Superuser;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value?.ToString();
}