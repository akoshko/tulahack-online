﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Layout;
using Tulahack.UI.Components.Common;

namespace Tulahack.UI.Components.Controls.CodeBehind.Form;

[PseudoClasses(PC_FixedWidth)]
public class Form : ItemsControl
{
    // ReSharper disable once InconsistentNaming
    public const string PC_FixedWidth = ":fixed-width";

    public static readonly StyledProperty<GridLength> LabelWidthProperty = AvaloniaProperty.Register<Form, GridLength>(
        nameof(LabelWidth));

    /// <summary>
    /// Behavior:
    /// <para>Fixed Width: all labels are with fixed length. </para>
    /// <para>Star: all labels are aligned by max length. </para>
    /// <para>Auto: labels are not aligned. </para>
    /// </summary>
    public GridLength LabelWidth
    {
        get => GetValue(LabelWidthProperty);
        set => SetValue(LabelWidthProperty, value);
    }

    public static readonly StyledProperty<Position> LabelPositionProperty = AvaloniaProperty.Register<Form, Position>(
        nameof(LabelPosition), defaultValue: Position.Top);

    public Position LabelPosition
    {
        get => GetValue(LabelPositionProperty);
        set => SetValue(LabelPositionProperty, value);
    }

    public static readonly StyledProperty<HorizontalAlignment> LabelAlignmentProperty =
        AvaloniaProperty.Register<Form, HorizontalAlignment>(
            nameof(LabelAlignment), defaultValue: HorizontalAlignment.Left);

    public HorizontalAlignment LabelAlignment
    {
        get => GetValue(LabelAlignmentProperty);
        set => SetValue(LabelAlignmentProperty, value);
    }

    static Form()
    {
        _ = LabelWidthProperty.Changed.AddClassHandler<Form, GridLength>((x, args) => x.LabelWidthChanged(args));
    }

    private void LabelWidthChanged(AvaloniaPropertyChangedEventArgs<GridLength> args)
    {
        GridLength newValue = args.NewValue.Value;
        var isFixed = newValue.IsStar || newValue.IsAbsolute;
        PseudoClasses.Set(PC_FixedWidth, isFixed);
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        recycleKey = null;
        return item is not FormItem && item is not FormGroup;
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        if (item is not Control control)
        {
            return new FormItem();
        }

        return new FormItem()
        {
            Content = control,
            [!FormItem.LabelProperty] = control[!FormItem.LabelProperty],
            [!FormItem.IsRequiredProperty] = control[!FormItem.IsRequiredProperty],
            [!FormItem.NoLabelProperty] = control[!FormItem.NoLabelProperty],
        };
    }
}
