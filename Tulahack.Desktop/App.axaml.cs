using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.EventLog;
using Tulahack.Desktop.Auth;
using Tulahack.Desktop.Extensions;
using Tulahack.UI.Extensions;
using Tulahack.UI.ViewModels;
using Tulahack.UI.Views;

namespace Tulahack.Desktop;

public class App : Application
{
    public override void Initialize() =>
        AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (Design.IsDesignMode)
        {
            InitDesignApp();
            base.OnFrameworkInitializationCompleted();
            return;
        }

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime application)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var authViewModel = new AuthViewModel(configuration);

            var splashScreen = new AuthView { DataContext = authViewModel };
            authViewModel.InitApp = () => InitApp(authViewModel, splashScreen, application);
            application.MainWindow = splashScreen;
            splashScreen.Show();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void InitApp(
        AuthViewModel splashScreenViewModel,
        AuthView splashScreen,
        IClassicDesktopStyleApplicationLifetime application)
    {
        try
        {
            IHost host = Host
                .CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    IdentityModel.Client.TokenResponse token = splashScreenViewModel.GetAccessToken();

                    if (string.IsNullOrEmpty(token.AccessToken))
                    {
                        return;
                    }

                    IConfiguration configuration = services.AddConfiguration();
                    var origin = configuration.GetSection("DesktopConfiguration:ApiUrl").Value ?? "http://localhost";
                    // TODO: check for default origin
                    services.AddDesktopTokenProvider(token);
                    services.AddEssentials(origin);
                    services.AddDesktopStorageServices();
                    services.AddServices();
                    services.AddViewModels();
                })
                .ConfigureLogging(loggingBuilder =>
                {
                    // remove loggers incompatible with UWP
                    {
                        var eventLoggers = loggingBuilder.Services
                            .Where(l => l.ImplementationType == typeof(EventLogLoggerProvider))
                            .ToList();

                        foreach (ServiceDescriptor el in eventLoggers)
                        {
                            _ = loggingBuilder.Services.Remove(el);
                        }
                    }
                })
                .UseEnvironment(Environments.Production)
                .Build();

            Ioc.Default.ConfigureServices(host.Services);

            var appWindow = new AppViewWindow();
            application.MainWindow = appWindow;
            AppViewModel mainWindowViewModel = Ioc.Default.GetRequiredService<AppViewModel>();
            application.MainWindow.DataContext = mainWindowViewModel;

            appWindow.Show();
            splashScreen.Close();
        }
        catch (TaskCanceledException)
        {
            splashScreen.Close();
        }
    }

    private void InitDesignApp()
    {
        IHost host = Host
            .CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                IConfiguration configuration = services.AddConfiguration();
                var origin = configuration.GetSection("DesktopConfiguration:ApiUrl").Value ?? "http://localhost:8080";
                services.AddDesignProviders();
                services.AddEssentials(origin);
                services.AddServices();
                services.AddViewModels();
            })
            .ConfigureLogging(loggingBuilder =>
            {
                {
                    var eventLoggers = loggingBuilder.Services
                        .Where(l => l.ImplementationType == typeof(EventLogLoggerProvider))
                        .ToList();

                    foreach (ServiceDescriptor el in eventLoggers)
                    {
                        _ = loggingBuilder.Services.Remove(el);
                    }
                }
            })
            .UseEnvironment(Environments.Development)
            .Build();

        Ioc.Default.ConfigureServices(host.Services);
    }
}
