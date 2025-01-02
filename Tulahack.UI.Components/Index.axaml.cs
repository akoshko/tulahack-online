using System.Globalization;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Tulahack.UI.Components;

/// <summary>
/// Notice: Don't set Locale if your app is in InvariantGlobalization mode.
/// </summary>
public class TulahackTheme : Styles
{
    private static readonly Lazy<Dictionary<CultureInfo, string>> _localeToResource =
        new Lazy<Dictionary<CultureInfo, string>>(
            () => new Dictionary<CultureInfo, string>
            {
                { new CultureInfo("ru-RU"), "avares://Tulahack.UI.Components/Locale/ru-RU.axaml" },
                { new CultureInfo("en-US"), "avares://Tulahack.UI.Components/Locale/en-US.axaml" },
            });

    private const string PC_DefaultResource = "avares://Tulahack.UI.Components/Locale/ru-RU.axaml";

    private readonly IServiceProvider? sp;

    public TulahackTheme(IServiceProvider? provider = null)
    {
        sp = provider;
        AvaloniaXamlLoader.Load(provider, this);
    }

    private CultureInfo? _locale;

    public CultureInfo? Locale
    {
        get => _locale;
        set
        {
            try
            {
                _locale = value;
                var resource = TryGetLocaleResource(value);

                if (AvaloniaXamlLoader.Load(sp, new Uri(resource)) is not ResourceDictionary d)
                {
                    return;
                }

                foreach (KeyValuePair<object, object?> kv in d)
                {
                    Resources.Add(kv);
                }
            }
            catch
            {
                _locale = CultureInfo.InvariantCulture;
            }
        }
    }

    private static string TryGetLocaleResource(CultureInfo? locale)
    {
        if (Equals(locale, CultureInfo.InvariantCulture))
        {
            return PC_DefaultResource;
        }

        if (locale is null)
        {
            return _localeToResource.Value[new CultureInfo("ru-RU")];
        }

        if (_localeToResource.Value.TryGetValue(locale, out var resource))
        {
            return resource;
        }

        return _localeToResource.Value[new CultureInfo("ru-RU")];
    }
}
