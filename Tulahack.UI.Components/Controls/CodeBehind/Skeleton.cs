﻿using Avalonia;
using Avalonia.Controls;

namespace Tulahack.UI.Components.Controls.CodeBehind;

public class Skeleton : ContentControl
{
    public static readonly StyledProperty<bool> IsActiveProperty =
        AvaloniaProperty.Register<Skeleton, bool>(nameof(IsActive));
    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    public static readonly StyledProperty<bool> IsLoadingProperty =
        AvaloniaProperty.Register<Skeleton, bool>(nameof(IsLoading));

    public bool IsLoading
    {
        get => GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }
}