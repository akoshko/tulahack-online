using System.ComponentModel;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace Tulahack.UI.Components.Controls.CodeBehind.EnumSelector;

public class EnumItemTuple
{
    public string? DisplayName { get; set; }
    public object? Value { get; set; }
}

public class EnumSelector : TemplatedControl
{
    public static readonly StyledProperty<Type?> EnumTypeProperty = AvaloniaProperty.Register<EnumSelector, Type?>(
        nameof(EnumType), validate: OnTypeValidate);

    public Type? EnumType
    {
        get => GetValue(EnumTypeProperty);
        set => SetValue(EnumTypeProperty, value);
    }

    private static bool OnTypeValidate(Type? arg)
    {
        if (arg is null)
        {
            return true;
        }

        return arg.IsEnum;
    }

    public static readonly StyledProperty<object?> ValueProperty = AvaloniaProperty.Register<EnumSelector, object?>(
        nameof(EnumValue), defaultBindingMode: BindingMode.TwoWay, coerce: OnValueCoerce);

    private static object? OnValueCoerce(AvaloniaObject o, object? value)
    {
        if (o is not EnumSelector selector)
        {
            return null;
        }

        if (value is null)
        {
            return null;
        }

        if (value.GetType() != selector.EnumType)
        {
            return null;
        }

        EnumItemTuple? first = selector.Values?.FirstOrDefault(a => Equals(a.Value, value));

        if (first is null)
        {
            return null;
        }

        return value;
    }

    public object? EnumValue
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    private EnumItemTuple? _selectedValue;

    public static readonly DirectProperty<EnumSelector, EnumItemTuple?> SelectedValueProperty =
        AvaloniaProperty.RegisterDirect<EnumSelector, EnumItemTuple?>(
            nameof(SelectedValue), o => o.SelectedValue, (o, v) => o.SelectedValue = v);

    public EnumItemTuple? SelectedValue
    {
        get => _selectedValue;
        private set => SetAndRaise(SelectedValueProperty, ref _selectedValue, value);
    }

    public static readonly DirectProperty<EnumSelector, IList<EnumItemTuple>?> ValuesProperty =
        AvaloniaProperty.RegisterDirect<EnumSelector, IList<EnumItemTuple>?>(
            nameof(Values), o => o.Values);

    private IList<EnumItemTuple>? _values;
    internal IList<EnumItemTuple>? Values
    {
        get => _values;
        private set => SetAndRaise(ValuesProperty, ref _values, value);
    }

    public static readonly StyledProperty<bool> DisplayDescriptionProperty =
        AvaloniaProperty.Register<EnumSelector, bool>(
            nameof(DisplayDescription));

    public bool DisplayDescription
    {
        get => GetValue(DisplayDescriptionProperty);
        set => SetValue(DisplayDescriptionProperty, value);
    }

    static EnumSelector()
    {
        _ = EnumTypeProperty.Changed.AddClassHandler<EnumSelector, Type?>((o, e) => o.OnTypeChanged(e));
        _ = SelectedValueProperty.Changed.AddClassHandler<EnumSelector, EnumItemTuple?>((o, e) =>
            o.OnSelectedValueChanged(e));
        _ = ValueProperty.Changed.AddClassHandler<EnumSelector, object?>((o, e) => o.OnValueChanged(e));
    }

    private void OnValueChanged(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        if (_updateFromComboBox)
        {
            return;
        }

        var newValue = args.NewValue.Value;

        if (newValue is null)
        {
            SetCurrentValue(SelectedValueProperty, null);
        }
        else
        {
            if (newValue.GetType() != EnumType)
            {
                SetCurrentValue(SelectedValueProperty, null);
            }

            EnumItemTuple? tuple = Values?.FirstOrDefault(x => Equals(x.Value, newValue));
            SetCurrentValue(SelectedValueProperty, tuple);
        }
    }

    private bool _updateFromComboBox;

    private void OnSelectedValueChanged(AvaloniaPropertyChangedEventArgs<EnumItemTuple?> args)
    {
        _updateFromComboBox = true;
        EnumItemTuple? newValue = args.NewValue.Value;
        SetCurrentValue(ValueProperty, newValue?.Value);
        _updateFromComboBox = false;
    }

    private void OnTypeChanged(AvaloniaPropertyChangedEventArgs<Type?> args)
    {
        Values?.Clear();
        Type? newType = args.GetNewValue<Type?>();

        if (newType is null || !newType.IsEnum)
        {
            return;
        }

        Values = GenerateItemTuple();
    }

    private List<EnumItemTuple> GenerateItemTuple()
    {
        if (EnumType is null)
        {
            return new List<EnumItemTuple>();
        }

        Array values = Enum.GetValues(EnumType);
        List<EnumItemTuple> list = new();

        foreach (var value in values)
        {
            if (value.GetType() == EnumType)
            {
                var displayName = value.ToString();
                if (displayName is null)
                {
                    continue;
                }

                System.Reflection.FieldInfo? field = EnumType.GetField(displayName);
                var description = field?.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();

                if (description is not null)
                {
                    displayName = ((DescriptionAttribute)description).Description;
                }

                list.Add(new EnumItemTuple()
                {
                    DisplayName = displayName,
                    Value = value
                });
            }
        }

        return list;
    }
}
