using System;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Tulahack.UI.Validation.Themes;

/// <summary>
/// Overriden templates for fluent theme of Avalonia.
/// </summary>
public class FluentTheme : Styles
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FluentTheme"/> class.
    /// </summary>
    /// <param name="sp">The parent's service provider.</param>
    public FluentTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}