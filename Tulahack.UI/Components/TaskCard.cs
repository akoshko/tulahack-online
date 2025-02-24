﻿using Avalonia;
using Avalonia.Controls.Primitives;

namespace Tulahack.UI.Components;

public class TaskCard : TemplatedControl
{
    /// <summary>
    /// Defines the <see cref="ThumbnailUrl"/> property.
    /// </summary>
    public static readonly StyledProperty<string> ThumbnailUrlProperty =
        AvaloniaProperty.Register<TaskCard, string>(nameof(ThumbnailUrl), defaultValue: string.Empty);

    /// <summary>
    /// Tile header text
    /// </summary>
    public string ThumbnailUrl
    {
        get => GetValue(ThumbnailUrlProperty);
        set => SetValue(ThumbnailUrlProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="CardTitle"/> property.
    /// </summary>
    public static readonly StyledProperty<string> CardTitleProperty =
        AvaloniaProperty.Register<TaskCard, string>(nameof(CardTitle), defaultValue: string.Empty);

    /// <summary>
    /// Tile header text
    /// </summary>
    public string CardTitle
    {
        get => GetValue(CardTitleProperty);
        set => SetValue(CardTitleProperty, value);
    }

    /// <summary>
    /// Defines the <see cref="CardContent"/> property.
    /// </summary>
    public static readonly StyledProperty<string> CardContentProperty =
        AvaloniaProperty.Register<TaskCard, string>(nameof(CardContent), defaultValue: string.Empty);

    /// <summary>
    /// Tile header text
    /// </summary>
    public string CardContent
    {
        get => GetValue(CardContentProperty);
        set => SetValue(CardContentProperty, value);
    }
}
