using Avalonia;
using Avalonia.Controls;

namespace Tulahack.UI.Components.Controls.CodeBehind;

public class HyperlinkContent
{
    /// <summary>
    /// Url what would be displayed if Alias is not set (as hyperlink) ,or it would be used as hyperlink for Alias
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Alias what would be clickable if set,else raw url would be displayed (also clickable)
    /// </summary>
    public string Alias
    {
        get;
        set;
    }
}

public class Hyperlink : Button
{
    public static readonly DirectProperty<Hyperlink, string> UrlProperty
        = AvaloniaProperty.RegisterDirect<Hyperlink, string>(nameof(Url), o => o.Url, (o, v) => o.Url = v);

    private string _url;

    public static readonly DirectProperty<Hyperlink, string> AliasProperty
        = AvaloniaProperty.RegisterDirect<Hyperlink, string>(nameof(Alias), o => o.Alias, (o, v) => o.Alias = v);

    private string _alias;

    public string Url
    {
        get => _url;
        set
        {
            SetAndRaise(UrlProperty, ref _url, value);
            var textBlock = new TextBlock
            {
                Text = _url
            };
            if (string.IsNullOrEmpty(_alias))
            {
                Content =  textBlock;
            }
            if (!string.IsNullOrEmpty(_url))
            {
                Classes.Add("hyperlink");
            }
        }
    }


    public string Alias
    {
        get => string.IsNullOrEmpty(_alias) ? _url : _alias;
        set
        {
            SetAndRaise(UrlProperty, ref _alias, value);
            var textBlock = new TextBlock
            {
                Text = string.IsNullOrEmpty(_alias) ? _url : _alias
            };

            Content = textBlock;
        }
    }
}