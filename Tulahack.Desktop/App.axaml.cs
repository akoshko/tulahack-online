using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Configuration;
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
            IdentityModel.Client.TokenResponse token = splashScreenViewModel.GetAccessToken();

            if (string.IsNullOrEmpty(token.AccessToken))
            {
                return;
            }

            var services = new ServiceCollection();
            IConfiguration configuration = services.AddConfiguration();
            var origin = configuration.GetSection("DesktopConfiguration:ApiUrl").Value ?? "http://localhost";
            // TODO: check for default origin
            services.AddDesktopTokenProvider(token);
            services.AddEssentials(origin);
            services.AddDesktopStorageServices();
            services.AddServices();
            services.AddViewModels();

            ServiceProvider provider = services.BuildServiceProvider();
            Ioc.Default.ConfigureServices(provider);

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
        var services = new ServiceCollection();
        IConfiguration configuration = services.AddConfiguration();
        var origin = configuration.GetSection("DesktopConfiguration:ApiUrl").Value ?? "http://localhost:8080";
        services.AddDesignProviders();
        services.AddEssentials(origin);
        services.AddServices();
        services.AddViewModels();

        ServiceProvider provider = services.BuildServiceProvider();
        Ioc.Default.ConfigureServices(provider);
    }
}
