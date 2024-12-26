using System;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Tulahack.UI.Validation.Themes;

/// <summary>
/// Overriden templates for simple theme of Avalonia.
/// </summary>
public class SimpleTheme : Styles
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleTheme"/> class.
    /// </summary>
    /// <param name="sp">The parent's service provider.</param>
    public SimpleTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}