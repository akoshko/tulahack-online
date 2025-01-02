using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tulahack.UI.Services;
using Tulahack.UI.ViewModels;

namespace Tulahack.Web;

internal sealed partial class Program
{
#pragma warning disable IDE0060
    private static Task Main(string[] args)
#pragma warning restore IDE0060
    {
        _ = Trace.Listeners.Add(new ConsoleTraceListener());

        TaskScheduler.UnobservedTaskException += (sender, eventArgs) =>
        {
            ILogger<AppViewModel> logger = Ioc.Default.GetRequiredService<ILogger<AppViewModel>>();
            logger.LogCritical("Critical async exception: {Message}", eventArgs.ToString());

            INotificationsService notificationsService = Ioc.Default.GetRequiredService<INotificationsService>();
            _ = notificationsService.ShowError(eventArgs.Exception.Message);
        };
        return BuildAvaloniaApp().StartBrowserAppAsync("out");
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder
            .Configure<App>()
            .WithInterFont();
}
