using Avalonia;

namespace Tulahack.Desktop;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        TaskScheduler.UnobservedTaskException += (sender, eventArgs) =>
        {
            // TODO: find a way how to show error notifications
            // var notificationsService = Ioc.Default.GetRequiredService<INotificationsService>();
            // notificationsService.ShowError(e.Message);
            
            Console.WriteLine(sender?.ToString() ?? "Critical async exception");
            Console.WriteLine(eventArgs.ToString());
        };
        
        try
        {
            // prepare and run your App here
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        catch (Exception e)
        {
            // here we can work with the exception, for example add it to our log file
            Console.WriteLine(e.Message);
            
            // TODO: find a way how to show error notifications
            // var notificationsService = Ioc.Default.GetRequiredService<INotificationsService>();
            // notificationsService.ShowError(e.Message);
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}