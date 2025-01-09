using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Tulahack.UI.Constants;
using Tulahack.UI.Services;
using Tulahack.UI.Storage;
using Tulahack.UI.ToastNotifications;
using Tulahack.UI.Utils;
using Tulahack.UI.ViewModels;
using Tulahack.UI.ViewModels.Pages.Contestant;
using Tulahack.UI.ViewModels.Pages.Expert;
using Tulahack.UI.ViewModels.Pages.Moderator;
using Tulahack.UI.ViewModels.Pages.Public;
using Tulahack.UI.ViewModels.Pages.Superuser;

namespace Tulahack.UI.Extensions;

public class DesignAuthProvider : AbstractAuthContextProvider
{
    private readonly string _accessToken = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IkRoQ19MWWpuMGVHdDE2WE93VUNaT0hUMU9DMktIMzh3YVRfZjgyckRQeDgifQ.eyJleHAiOjE3Mjg1NjEzNTUsImlhdCI6MTcxODU2MTI5NSwianRpIjoiYzFhMjRkNmMtNGRiOS00NGUzLTk2NzUtOTJkZThjYWMwN2RiIiwiaXNzIjoiaHR0cHM6Ly9rZXljbG9hay5ldXJla2EtdGVhbS5ydS9yZWFsbXMvdHVsYWhhY2siLCJhdWQiOlsidHVsYWhhY2stY2xpZW50IiwiYWNjb3VudCJdLCJzdWIiOiI2NWRjMzJhYS0zODNhLTRmMDEtYWViMy02ZjE2ZjQyMzU3MjIiLCJ0eXAiOiJCZWFyZXIiLCJhenAiOiJ0dWxhaGFjay1jbGllbnQiLCJzZXNzaW9uX3N0YXRlIjoiMGQ4NWM0MjAtM2I5NC00YWY2LTljMTUtODU1ZTMwYTFhOTE4IiwiYWNyIjoiMSIsImFsbG93ZWQtb3JpZ2lucyI6WyJodHRwczovL3R1bGFoYWNrLmV1cmVrYS10ZWFtLnJ1IiwiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMS8qIiwiaHR0cDovL2xvY2FsaG9zdDo4ODg5IiwiaHR0cDovL2xvY2FsaG9zdDo1MTczLyoiLCJodHRwczovL29hdXRoMi5ldXJla2EtdGVhbS5ydSIsImh0dHA6Ly9sb2NhbGhvc3Q6NTAwMC8qIiwiaHR0cDovL2xvY2FsaG9zdDo0MjAwIiwiaHR0cHM6Ly9tb29kLmV1cmVrYS10ZWFtLnJ1LyoiXSwicmVhbG1fYWNjZXNzIjp7InJvbGVzIjpbIm9mZmxpbmVfYWNjZXNzIiwiZGVmYXVsdC1yb2xlcy10dWxhaGFjayIsInVtYV9hdXRob3JpemF0aW9uIl19LCJyZXNvdXJjZV9hY2Nlc3MiOnsiYWNjb3VudCI6eyJyb2xlcyI6WyJtYW5hZ2UtYWNjb3VudCIsIm1hbmFnZS1hY2NvdW50LWxpbmtzIiwidmlldy1wcm9maWxlIl19fSwic2NvcGUiOiJlbWFpbCBwcm9maWxlIiwic2lkIjoiMGQ4NWM0MjAtM2I5NC00YWY2LTljMTUtODU1ZTMwYTFhOTE4IiwiZW1haWxfdmVyaWZpZWQiOnRydWUsIm5hbWUiOiJFbGVuYSBBcmVmeWV2YSIsInByZWZlcnJlZF91c2VybmFtZSI6ImVsZW5hYXJlZnlldmEiLCJnaXZlbl9uYW1lIjoiRWxlbmEiLCJmYW1pbHlfbmFtZSI6IkFyZWZ5ZXZhIiwiZW1haWwiOiJzdXBlcnVzZXJAdHVsYWhhY2sucnUiLCJncm91cCI6WyIvUHVibGljIiwiL1N1cGVydXNlcnMiXX0.DkJyLla76WBURu4NuHghjBUSJsW-AePMhLOOyveba915Z8Q8q0hen76Zp1wFvVu3ckex1YI0hy15pvKeSNt0qvVBPqAVyS25ZpsP9cdQr1F0o_KxPMbTWO9HPrHrNoat1iUXrIDzT7RT5Snb_7kA4oA28Q0rfpBOmFLitK5IrKmerFd8yPBAhDfprz_SRJzukVA8Wru-qEQHKIiYTShyca5wP8hf0FXsQGgIMZlHY6Bjc9Q0lrE7ZXu6-ZxJA91AGBJgvNQwueyKCOo10VRuc5yuSHamp1R-bSfAwydHgmwyD-GYYNduOhYikOLuQiWqxU98n9H_DYf0h7pUMo63Pw";
    public override string GetAccessToken() => _accessToken;
    public override string GetGroup() => Groups.Superuser;
}

public class DesignStorageProvider<T> : IRuntimeStorageProvider<T>
{
    public Task SaveObject(T obj, string key) => Task.CompletedTask;

    public Task<T?> LoadObject(string key) => Task.FromResult<T?>((T)new object());
}

public class ApiAuthHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
    {
        IAuthContextProvider? tokenProvider = Ioc.Default.GetService<IAuthContextProvider>();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenProvider?.GetAccessToken());
        return base.SendAsync(request, cancellationToken);
    }
}

[RequiresUnreferencedCode("DTO types should be specified in TulahackJsonContext as [JsonSerializable(typeof(T))]")]
[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2072:UnrecognizedReflectionPattern",
    Justification = "Implementation detail of Activator that linker intrinsically recognizes")]
public static class ServiceCollectionExtensions
{
    public static void AddEssentials(this IServiceCollection collection, string origin)
    {
        _ = collection.AddLogging();

        _ = collection.AddTransient<ApiAuthHandler>();
        _ = collection.AddHttpClient().ConfigureHttpClientDefaults(builder =>
        {
            _ = builder.ConfigureHttpClient(client => { client.BaseAddress = new Uri($"{origin}/api/"); })
                .AddHttpMessageHandler(provider => provider.GetService<ApiAuthHandler>() ?? new ApiAuthHandler());
        });
        _ = collection.AddTransient<IHttpService, HttpService>();

        _ = collection.AddSingleton<JsonSerializerOptions>(_ => new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter()
            },
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            PropertyNameCaseInsensitive = true,
            TypeInfoResolver = TulahackJsonContext.Default
        });

        _ = collection.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);
    }

    public static void AddDesignProviders(this IServiceCollection collection)
    {
        _ = collection.AddSingleton<IRuntimeStorageProvider<AppState>, DesignStorageProvider<AppState>>();
        _ = collection.AddSingleton<IAuthContextProvider, DesignAuthProvider>(_ => new DesignAuthProvider());
    }

    public static void AddServices(this IServiceCollection collection)
    {
        // Generic services
        _ = collection.AddSingleton<IMainViewProvider, MainViewProvider>();

        // Utility services
        _ = collection.AddSingleton<INotificationMessageManager, NotificationMessageManager>();
        _ = collection.AddSingleton<INotificationsService, NotificationsService>();
        _ = collection.AddSingleton<INavigationService, NavigationService>();

        // Data services
        _ = collection.AddSingleton<IUserService, UserService>();
        _ = collection.AddSingleton<IDashboardService, DashboardService>();
        _ = collection.AddSingleton<ITaskboardService, TaskboardService>();
        _ = collection.AddSingleton<ITeamService, TeamService>();
        _ = collection.AddSingleton<IApplicationService, ApplicationService>();
    }

    public static void AddViewModels(this IServiceCollection collection)
    {
        // Base
        _ = collection.AddSingleton<AppViewModel>();
        _ = collection.AddSingleton<NavigationViewModel>();
        _ = collection.AddSingleton<ContentViewModel>();
        _ = collection.AddSingleton<TitleViewModel>();

        // Public
        _ = collection.AddTransient<DashboardViewModel>();
        _ = collection.AddTransient<SettingsViewModel>();
        _ = collection.AddTransient<ProfilePageViewModel>();
        _ = collection.AddTransient<ApplicationFormViewModel>();

        // Contestant
        _ = collection.AddTransient<TeamPageViewModel>();
        _ = collection.AddTransient<ContestTaskPageViewModel>();
        _ = collection.AddTransient<ContestTaskboardPageViewModel>();
        _ = collection.AddTransient<ContestSchedulePageViewModel>();
        _ = collection.AddTransient<ContestEventPageViewModel>();
        _ = collection.AddTransient<ContestEventSchedulePageViewModel>();

        // Expert
        _ = collection.AddTransient<AssessmentPageViewModel>();
        _ = collection.AddTransient<RatingPageViewModel>();

        // Moderator
        _ = collection.AddTransient<ScoreboardPageViewModel>();
        _ = collection.AddTransient<ParticipatorsPageViewModel>();
        _ = collection.AddTransient<TeamsPageViewModel>();
        _ = collection.AddTransient<ExpertsPageViewModel>();

        // Superuser
        _ = collection.AddTransient<ModeratorsPageViewModel>();
        _ = collection.AddTransient<HackathonOverviewPageViewModel>();
        _ = collection.AddTransient<HackathonSettingsPageViewModel>();
    }
}