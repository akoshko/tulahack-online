using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tulahack.UI.Services;
using Tulahack.UI.ViewModels;

namespace Tulahack.Web;

[RequiresUnreferencedCode("DTO types should be specified in TulahackJsonContext as [JsonSerializable(typeof(T))]")]
[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2072:UnrecognizedReflectionPattern",
    Justification = "Implementation detail of Activator that linker intrinsically recognizes")]
// ReSharper disable once PartialTypeWithSinglePart
internal sealed partial class Program
{
#pragma warning disable IDE0060
    // ReSharper disable once UnusedParameter.Local
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
