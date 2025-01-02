using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using Tulahack.UI.ViewModels;

namespace Tulahack.UI.Utils;

public class ContentViewSelector : IDataTemplate
{
    [Content]
    public Dictionary<string, IDataTemplate> AvailableTemplates { get; } = new ();

    public Control? Build(object? param)
    {
        if (param is not PageContextModel tab)
        {
            throw new ArgumentException("Selected page type doesn't match any available ContentView.axaml data template");
        }

        var key = tab.Label;
        
        if (key is null)
        {
            throw new ArgumentNullException(nameof(param));
        }

        return AvailableTemplates[key].Build(param);
    }

    public bool Match(object? data)
    {
        if (data is not PageContextModel tab)
        {
            return false;
        }

        var key = tab.Label;
        return AvailableTemplates.ContainsKey(key);
    }
}