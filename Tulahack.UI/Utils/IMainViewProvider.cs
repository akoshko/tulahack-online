using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Tulahack.UI.Views;

namespace Tulahack.UI.Utils;

public interface IMainViewProvider
{
    IStorageProvider? GetStorageProvider();
    AppView? GetRootView();
}

public class MainViewProvider : IMainViewProvider
{
    public AppView? GetRootView() =>
        Application.Current?.ApplicationLifetime switch
        {
            IClassicDesktopStyleApplicationLifetime desktop => desktop.MainWindow?.GetControl<AppView>("AppViewControl"),
            ISingleViewApplicationLifetime web => web.MainView as AppView,
            _ => null
        };

    public IStorageProvider? GetStorageProvider()
    {
        var topLevel = TopLevel.GetTopLevel(GetRootView());
        return topLevel?.StorageProvider;
    }
}