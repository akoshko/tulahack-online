using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
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
            var authViewModel = new AuthViewModel();
            var splashScreen = new AuthView { DataContext = authViewModel };
            authViewModel.InitApp = () => InitApp(authViewModel, splashScreen, application);
            application.MainWindow = splashScreen;
            splashScreen.Show();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private async void InitApp(
        AuthViewModel splashScreenViewModel,
        AuthView splashScreen,
        IClassicDesktopStyleApplicationLifetime application)
    {
        try
        {
            var token = splashScreenViewModel.GetAccessToken();
            if (string.IsNullOrEmpty(token.AccessToken))
            {
                return;
            }

            var services = new ServiceCollection();
            var configuration = services.AddConfiguration();
            var origin = configuration.GetSection("DesktopConfiguration:ApiUrl").Value ?? "http://localhost";
            // TODO: check for default origin
            services.AddDesktopTokenProvider(token);
            services.AddEssentials(origin);
            services.AddDesktopStorageServices();
            services.AddServices();
            services.AddViewModels();

            var provider = services.BuildServiceProvider();
            Ioc.Default.ConfigureServices(provider);

            var appWindow = new AppViewWindow();
            application.MainWindow = appWindow;
            var mainWindowViewModel = Ioc.Default.GetRequiredService<AppViewModel>();
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
        var services = new ServiceCollection();
        var configuration = services.AddConfiguration();
        var origin = configuration.GetSection("DesktopConfiguration:ApiUrl").Value ?? "http://localhost:8080";
        services.AddDesignProviders();
        services.AddEssentials(origin);
        services.AddServices();
        services.AddViewModels();

        var provider = services.BuildServiceProvider();
        Ioc.Default.ConfigureServices(provider);
    }
}