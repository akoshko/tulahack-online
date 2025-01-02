using Avalonia;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Tulahack.UI.Extensions;
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

        services.AddBrowserAuthProvider();
        services.AddEssentials(extensions.GetOrigin());
        services.AddBrowserStorageServices();
        services.AddServices();
        services.AddViewModels();

        var provider = services.BuildServiceProvider();
        Ioc.Default.ConfigureServices(provider);



        base.OnFrameworkInitializationCompleted();
    }
}