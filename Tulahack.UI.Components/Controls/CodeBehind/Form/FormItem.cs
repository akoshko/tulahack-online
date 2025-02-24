﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Layout;
using Avalonia.Reactive;
using Avalonia.VisualTree;
using Tulahack.UI.Components.Common;
using Tulahack.UI.Components.Utils.Helpers;

namespace Tulahack.UI.Components.Controls.CodeBehind.Form;

[PseudoClasses(PC_Horizontal, PC_NoLabel)]
public class FormItem : ContentControl
{
    // ReSharper disable once InconsistentNaming
    public const string PC_Horizontal = ":horizontal";
    // ReSharper disable once InconsistentNaming
    public const string PC_NoLabel = ":no-label";

    #region Attached Properties

    public static readonly AttachedProperty<object?> LabelProperty =
        AvaloniaProperty.RegisterAttached<FormItem, Control, object?>("Label");
    public static void SetLabel(Control obj, object? value) => obj.SetValue(LabelProperty, value);
    public static object? GetLabel(Control obj) => obj.GetValue(LabelProperty);


    public static readonly AttachedProperty<bool> IsRequiredProperty =
        AvaloniaProperty.RegisterAttached<FormItem, Control, bool>("IsRequired");
    public static void SetIsRequired(Control obj, bool value) => obj.SetValue(IsRequiredProperty, value);
    public static bool GetIsRequired(Control obj) => obj.GetValue(IsRequiredProperty);

    public static readonly AttachedProperty<bool> NoLabelProperty =
        AvaloniaProperty.RegisterAttached<FormItem, Control, bool>("NoLabel");

    public static void SetNoLabel(Control obj, bool value) => obj.SetValue(NoLabelProperty, value);
    public static bool GetNoLabel(Control obj) => obj.GetValue(NoLabelProperty);

    #endregion

    private readonly List<IDisposable> _formSubscriptions = new();

    public static readonly StyledProperty<double> LabelWidthProperty = AvaloniaProperty.Register<FormItem, double>(
        nameof(LabelWidth));

    public double LabelWidth
    {
        get => GetValue(LabelWidthProperty);
        set => SetValue(LabelWidthProperty, value);
    }

    public static readonly StyledProperty<HorizontalAlignment> LabelAlignmentProperty =
        AvaloniaProperty.Register<FormItem, HorizontalAlignment>(
            nameof(LabelAlignment));

    public HorizontalAlignment LabelAlignment
    {
        get => GetValue(LabelAlignmentProperty);
        set => SetValue(LabelAlignmentProperty, value);
    }

    static FormItem()
    {
        NoLabelProperty.AffectsPseudoClass<FormItem>(PC_NoLabel);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        Form? form = this.GetVisualAncestors().OfType<Form>().FirstOrDefault();

        if (form is null)
        {
            return;
        }

        _formSubscriptions.Clear();
        IDisposable labelSubscription = form
            .GetObservable(Form.LabelWidthProperty)
            .Subscribe(new AnonymousObserver<GridLength>(length =>
            {
                LabelWidth = length.IsAbsolute ? length.Value : double.NaN;
            }));
        IDisposable positionSubscription = form
            .GetObservable(Form.LabelPositionProperty)
            .Subscribe(new AnonymousObserver<Position>(position =>
            {
                PseudoClasses.Set(PC_Horizontal, position == Position.Left);
            }));
        IDisposable alignmentSubscription = form
            .GetObservable(Form.LabelAlignmentProperty)
            .Subscribe(new AnonymousObserver<HorizontalAlignment>(alignment => { LabelAlignment = alignment; }));
        _formSubscriptions.Add(labelSubscription);
        _formSubscriptions.Add(positionSubscription);
        _formSubscriptions.Add(alignmentSubscription);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        foreach (IDisposable subscription in _formSubscriptions)
        {
            subscription.Dispose();
        }
    }
}
