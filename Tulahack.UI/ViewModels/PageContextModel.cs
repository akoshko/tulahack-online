using System;
using System.Collections.ObjectModel;
using Tulahack.UI.Messaging;
using Tulahack.UI.ViewModels.Base;

namespace Tulahack.UI.ViewModels;

public interface IPageContext
{
    public void Activate();
    public void Deactivate();
    public void SetArgs(NavigationArgs? args);
    public NavigationArgs? NavigationArgs { get; set; }
}

public class PageContextModel(string label, string icon, Type viewModelType)
{
    public string Label { get; } = label;
    public string IconKey { get; } = icon;
    public Type ViewModelType { get; } = viewModelType;
    public ObservableCollection<PageContextModel> NestedItems { get; } = new();
    public ViewModelBase? Context { get; set; }
}