using System;
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
using Tulahack.UI.ViewModels.Designer;
using Tulahack.UI.ViewModels.Pages.Contestant;
using Tulahack.UI.ViewModels.Pages.Expert;
using Tulahack.UI.ViewModels.Pages.Moderator;
using Tulahack.UI.ViewModels.Pages.Public;
using Tulahack.UI.ViewModels.Pages.Superuser;

namespace Tulahack.UI.Extensions
{
    public class DesignAuthProvider : AbstractAuthContextProvider
    {
        private readonly string _accessToken = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IkRoQ19MWWpuMGVHdDE2WE93VUNaT0hUMU9DMktIMzh3YVRfZjgyckRQeDgifQ.eyJleHAiOjE3Mjg1NjEzNTUsImlhdCI6MTcxODU2MTI5NSwianRpIjoiYzFhMjRkNmMtNGRiOS00NGUzLTk2NzUtOTJkZThjYWMwN2RiIiwiaXNzIjoiaHR0cHM6Ly9rZXljbG9hay5ldXJla2EtdGVhbS5ydS9yZWFsbXMvdHVsYWhhY2siLCJhdWQiOlsidHVsYWhhY2stY2xpZW50IiwiYWNjb3VudCJdLCJzdWIiOiI2NWRjMzJhYS0zODNhLTRmMDEtYWViMy02ZjE2ZjQyMzU3MjIiLCJ0eXAiOiJCZWFyZXIiLCJhenAiOiJ0dWxhaGFjay1jbGllbnQiLCJzZXNzaW9uX3N0YXRlIjoiMGQ4NWM0MjAtM2I5NC00YWY2LTljMTUtODU1ZTMwYTFhOTE4IiwiYWNyIjoiMSIsImFsbG93ZWQtb3JpZ2lucyI6WyJodHRwczovL3R1bGFoYWNrLmV1cmVrYS10ZWFtLnJ1IiwiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMS8qIiwiaHR0cDovL2xvY2FsaG9zdDo4ODg5IiwiaHR0cDovL2xvY2FsaG9zdDo1MTczLyoiLCJodHRwczovL29hdXRoMi5ldXJla2EtdGVhbS5ydSIsImh0dHA6Ly9sb2NhbGhvc3Q6NTAwMC8qIiwiaHR0cDovL2xvY2FsaG9zdDo0MjAwIiwiaHR0cHM6Ly9tb29kLmV1cmVrYS10ZWFtLnJ1LyoiXSwicmVhbG1fYWNjZXNzIjp7InJvbGVzIjpbIm9mZmxpbmVfYWNjZXNzIiwiZGVmYXVsdC1yb2xlcy10dWxhaGFjayIsInVtYV9hdXRob3JpemF0aW9uIl19LCJyZXNvdXJjZV9hY2Nlc3MiOnsiYWNjb3VudCI6eyJyb2xlcyI6WyJtYW5hZ2UtYWNjb3VudCIsIm1hbmFnZS1hY2NvdW50LWxpbmtzIiwidmlldy1wcm9maWxlIl19fSwic2NvcGUiOiJlbWFpbCBwcm9maWxlIiwic2lkIjoiMGQ4NWM0MjAtM2I5NC00YWY2LTljMTUtODU1ZTMwYTFhOTE4IiwiZW1haWxfdmVyaWZpZWQiOnRydWUsIm5hbWUiOiJFbGVuYSBBcmVmeWV2YSIsInByZWZlcnJlZF91c2VybmFtZSI6ImVsZW5hYXJlZnlldmEiLCJnaXZlbl9uYW1lIjoiRWxlbmEiLCJmYW1pbHlfbmFtZSI6IkFyZWZ5ZXZhIiwiZW1haWwiOiJzdXBlcnVzZXJAdHVsYWhhY2sucnUiLCJncm91cCI6WyIvUHVibGljIiwiL1N1cGVydXNlcnMiXX0.DkJyLla76WBURu4NuHghjBUSJsW-AePMhLOOyveba915Z8Q8q0hen76Zp1wFvVu3ckex1YI0hy15pvKeSNt0qvVBPqAVyS25ZpsP9cdQr1F0o_KxPMbTWO9HPrHrNoat1iUXrIDzT7RT5Snb_7kA4oA28Q0rfpBOmFLitK5IrKmerFd8yPBAhDfprz_SRJzukVA8Wru-qEQHKIiYTShyca5wP8hf0FXsQGgIMZlHY6Bjc9Q0lrE7ZXu6-ZxJA91AGBJgvNQwueyKCOo10VRuc5yuSHamp1R-bSfAwydHgmwyD-GYYNduOhYikOLuQiWqxU98n9H_DYf0h7pUMo63Pw";
        public override string GetAccessToken() => _accessToken;
        public override string GetGroup() => Groups.Superuser;
    }
    
    public class ApiAuthHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var tokenProvider = Ioc.Default.GetService<IAuthContextProvider>();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenProvider?.GetAccessToken());
            return base.SendAsync(request, cancellationToken);
        }
    }

    public static partial class ServiceCollectionExtensions
    {
        public static void AddEssentials(this IServiceCollection collection, string origin)
        {
            collection.AddLogging();

            collection.AddTransient<ApiAuthHandler>();
            collection.AddHttpClient().ConfigureHttpClientDefaults(builder =>
            {
                builder.ConfigureHttpClient(client => { client.BaseAddress = new Uri($"{origin}/api/"); })
                    .AddHttpMessageHandler(provider => provider.GetService<ApiAuthHandler>() ?? new ApiAuthHandler());
            });

            collection.AddSingleton<JsonSerializerOptions>(_ => new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter()
                },
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                PropertyNameCaseInsensitive = true,
                TypeInfoResolver = TulahackJsonContext.Default
            });

            collection.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);
        }

        public static void AddDesignProviders(this IServiceCollection collection)
        {
            collection.AddSingleton<IRuntimeStorageProvider<AppState>, DesignStorageProvider<AppState>>();
            collection.AddSingleton<IAuthContextProvider, DesignAuthProvider>(_ => new DesignAuthProvider());
        }
        
        public static void AddServices(this IServiceCollection collection)
        {
            // Generic services
            collection.AddSingleton<IMainViewProvider, MainViewProvider>();
            
            // Utility services
            collection.AddSingleton<INotificationMessageManager, NotificationMessageManager>();
            collection.AddSingleton<INotificationsService, NotificationsService>();
            collection.AddSingleton<INavigationService, NavigationService>();
            
            // Data services
            collection.AddSingleton<IUserService, UserService>();
            collection.AddSingleton<IDashboardService, DashboardService>();
            collection.AddSingleton<ITaskboardService, TaskboardService>();
            collection.AddSingleton<ITeamService, TeamService>();
            collection.AddSingleton<IApplicationService, ApplicationService>();
        }

        public static void AddViewModels(this IServiceCollection collection)
        {
            // Base
            collection.AddSingleton<AppViewModel>();
            collection.AddSingleton<NavigationViewModel>();
            collection.AddSingleton<ContentViewModel>();
            collection.AddSingleton<TitleViewModel>();

            // Public
            collection.AddTransient<DashboardViewModel>();
            collection.AddTransient<SettingsViewModel>();
            collection.AddTransient<ProfilePageViewModel>();
            collection.AddTransient<ApplicationFormViewModel>();

            // Contestant
            collection.AddTransient<TeamPageViewModel>();
            collection.AddTransient<ContestTaskPageViewModel>();
            collection.AddTransient<ContestTaskboardPageViewModel>();
            collection.AddTransient<ContestSchedulePageViewModel>();
            collection.AddTransient<ContestEventPageViewModel>();
            collection.AddTransient<ContestEventSchedulePageViewModel>();

            // Expert
            collection.AddTransient<AssessmentPageViewModel>();
            collection.AddTransient<RatingPageViewModel>();

            // Moderator
            collection.AddTransient<ScoreboardPageViewModel>();
            collection.AddTransient<ParticipatorsPageViewModel>();
            collection.AddTransient<TeamsPageViewModel>();
            collection.AddTransient<ExpertsPageViewModel>();

            // Superuser
            collection.AddTransient<ModeratorsPageViewModel>();
            collection.AddTransient<HackathonOverviewPageViewModel>();
            collection.AddTransient<HackathonSettingsPageViewModel>();
        }
    }
}