using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Tulahack.UI.Components.Utils.Helpers;

public static class AvaloniaPropertyExtension
{
    public static void SetValue<T>(
        this AvaloniaProperty<T> property,
        T value,
        params AvaloniaObject?[] objects)
    {
        foreach (AvaloniaObject? obj in objects)
        {
            _ = obj?.SetValue(property, value);
        }
    }

    public static void SetValue<T, TControl>(
        this AvaloniaProperty<T> property,
        T value,
        IEnumerable<TControl?> objects)
        where TControl : AvaloniaObject
    {
        foreach (TControl? obj in objects)
        {
            _ = obj?.SetValue(property, value);
        }
    }

    public static void AffectsPseudoClass<TControl>(
        this AvaloniaProperty<bool> property,
        string pseudoClass,
        RoutedEvent<RoutedEventArgs>? routedEvent = null)
        where TControl : Control =>
        property.Changed.AddClassHandler<TControl, bool>((control, args) =>
        {
            OnPropertyChanged(control, args, pseudoClass, routedEvent);
        });

    private static void OnPropertyChanged<TControl, TArgs>(
        TControl control,
        AvaloniaPropertyChangedEventArgs<bool> args,
        string pseudoClass,
        RoutedEvent<TArgs>? routedEvent)
        where TControl : Control
        where TArgs : RoutedEventArgs, new()
    {
        PseudolassesExtensions.Set(control.Classes, pseudoClass, args.NewValue.Value);

        if (routedEvent is not null)
        {
            control.RaiseEvent(new TArgs() { RoutedEvent = routedEvent });
        }
    }

    public static void AffectsPseudoClass<TControl, TArgs>(
        this AvaloniaProperty<bool> property,
        string pseudoClass,
        RoutedEvent<TArgs>? routedEvent = null)
        where TControl : Control
        where TArgs : RoutedEventArgs, new() =>
        property.Changed.AddClassHandler<TControl, bool>((control, args) =>
        {
            OnPropertyChanged(control, args, pseudoClass, routedEvent);
        });
}
