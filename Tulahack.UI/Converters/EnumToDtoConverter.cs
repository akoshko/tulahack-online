﻿using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Tulahack.UI.Converters;

public class EnumToDtoConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value?.ToString();
}