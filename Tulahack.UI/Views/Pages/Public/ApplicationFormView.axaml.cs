using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Tulahack.Dtos;

namespace Tulahack.UI.Views.Pages.Public;

public class ApplicationFormConsentConverter : IMultiValueConverter
{
    public object? Convert(IList<object?>? values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values?.Count != 2)
        {
            throw new NotSupportedException();
        }

        if (!values.All(x => x is bool or UnsetValueType or null or ContestApplicationDto))
        {
            throw new NotSupportedException();
        }

        if (values[0] is not bool consent ||
            values[1] is not ContestApplicationDto dto)
        {
            return BindingOperations.DoNothing;
        }

        dto.Validate();
        return consent && !dto.HasErrors;
    }
}

public partial class ApplicationFormView : UserControl
{
    public ApplicationFormView()
    {
        InitializeComponent();
    }
}