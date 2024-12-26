using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using Tulahack.Dtos;

namespace Tulahack.UI.Converters;

public class FilterCasesByCompanyIdConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not ICollection)
            return new List<ContestCaseDto>();

        if (value is not List<ContestCaseDto> cases)
            return new List<ContestCaseDto>();

        if (parameter is null)
            return new List<ContestCaseDto>();
        
        var companyId = Guid.Parse(parameter.ToString()!);

        var filtered = cases
            .Where(item => item.Company.Id == companyId)
            .ToList();

        return string.Join(", ", filtered.Select(item => item.Title).ToArray());
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is null ? default : (int)value;
}