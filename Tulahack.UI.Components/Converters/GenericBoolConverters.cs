using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Tulahack.UI.Components.Converters;

public static class ObjUtils
{
    public static bool ObjEquals(this object? obj1, object? obj2)
    {
        if (obj1 == obj2)
        {
            return true;
        }

        if ((obj1 != null) && (obj1.Equals(obj2)))
        {
            return true;
        }

        return false;
    }
}

public class GenericBoolConverters<T> : IValueConverter
{
    public T TrueValue { get; set; }

    public T FalseValue { get; set; }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b)
        {
            return b ? TrueValue : FalseValue;
        }

        return FalseValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (TrueValue.ObjEquals(value))
        {
            return true;
        }

        return false;
    }
}

public class BoolToBrushConverters : GenericBoolConverters<IBrush> { }

public class BoolToDoubleConverters : GenericBoolConverters<double> { }
