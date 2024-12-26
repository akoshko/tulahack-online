using System.Diagnostics;
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
    private static Task Main(string[] args)
    {
        Trace.Listeners.Add(new ConsoleTraceListener());

        TaskScheduler.UnobservedTaskException += (sender, eventArgs) =>
        {
            var logger = Ioc.Default.GetRequiredService<ILogger<AppViewModel>>();
            logger.LogCritical(sender?.ToString() ?? "Critical async exception");
            logger.LogCritical(eventArgs.ToString());

            var notificationsService = Ioc.Default.GetRequiredService<INotificationsService>();
            notificationsService.ShowError(eventArgs.Exception.Message);
        };
        return BuildAvaloniaApp().StartBrowserAppAsync("out");
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder
            .Configure<App>()
            .WithInterFont();
}