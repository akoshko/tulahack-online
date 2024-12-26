using Avalonia;
using Avalonia.Controls.Primitives;

namespace Tulahack.UI.Components;

public class DashboardTile : TemplatedControl
{
    /// <summary>
    /// Defines the <see cref="TileHeader"/> property.
    /// </summary>
    public static readonly StyledProperty<string> TileHeaderProperty =
        AvaloniaProperty.Register<DashboardTile, string>(nameof(TileHeader), defaultValue: string.Empty);

    /// <summary>
    /// Tile header text
    /// </summary>
    public string TileHeader
    {
        get => GetValue(TileHeaderProperty);
        set => SetValue(TileHeaderProperty, value);
    }
    
    /// <summary>
    /// Defines the <see cref="TileContent"/> property.
    /// </summary>
    public static readonly StyledProperty<string> TileContentProperty =
        AvaloniaProperty.Register<DashboardTile, string>(nameof(TileContent), defaultValue: string.Empty);

    /// <summary>
    /// Tile header text
    /// </summary>
    public string TileContent
    {
        get => GetValue(TileContentProperty);
        set => SetValue(TileContentProperty, value);
    }
}