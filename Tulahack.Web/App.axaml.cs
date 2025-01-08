using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Tulahack.UI.Extensions;
using Tulahack.UI.ViewModels;
using Tulahack.UI.Views;
using Tulahack.Web.Extensions;

namespace Tulahack.Web;

public class App : Application
{
    public override void Initialize() =>
        AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        var extensions = new UI.Utils.JsExportedMethods();

        _ = services.AddBrowserAuthProvider();
        services.AddEssentials(extensions.GetOrigin());
        services.AddBrowserStorageServices();
        services.AddServices();
        services.AddViewModels();

        ServiceProvider provider = services.BuildServiceProvider();
        Ioc.Default.ConfigureServices(provider);

        if (ApplicationLifetime is not ISingleViewApplicationLifetime application)
        {
            return;
        }

        var appWindow = new AppView();
        application.MainView = appWindow;
        AppViewModel mainWindowViewModel = Ioc.Default.GetRequiredService<AppViewModel>();
        application.MainView.DataContext = mainWindowViewModel;

        base.OnFrameworkInitializationCompleted();
    }
}
