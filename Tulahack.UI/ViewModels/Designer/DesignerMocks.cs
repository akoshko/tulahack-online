using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Moq;
using Tulahack.Dtos;
using Tulahack.UI.Constants;
using Tulahack.UI.Messaging;
using Tulahack.UI.Services;
using Tulahack.UI.Storage;
using Tulahack.UI.ViewModels.Pages.Contestant;
using Tulahack.UI.ViewModels.Pages.Expert;
using Tulahack.UI.ViewModels.Pages.Moderator;
using Tulahack.UI.ViewModels.Pages.Public;
using Tulahack.UI.ViewModels.Pages.Superuser;

namespace Tulahack.UI.ViewModels.Designer;

public class DesignStorageProvider<T> : IRuntimeStorageProvider<T>
{
    public Task SaveObject(T obj, string key) => Task.CompletedTask;

    public Task<T?> LoadObject(string key) => Task.FromResult<T?>((T)new object());
}

public class DesignNavigationService : INavigationService
{
    public IEnumerable<PageContextModel> Pages { get; init; } = new List<PageContextModel>()
    {
        new(NavigationKeys.Settings, IconKeys.SettingsRegular, typeof(SettingsViewModel)),
        new(NavigationKeys.Profile, IconKeys.PersonRegular, typeof(ProfilePageViewModel)),
        new(NavigationKeys.ApplicationForm, IconKeys.RocketRegular, typeof(ApplicationFormViewModel)),
        new(NavigationKeys.ContestTask, IconKeys.TasksAppRegular, typeof(ContestTaskPageViewModel)),

        new(NavigationKeys.Dashboard, IconKeys.HomeRegular, typeof(DashboardViewModel)),
        new(NavigationKeys.Team, IconKeys.PeopleRegular, typeof(TeamPageViewModel)),
        new(NavigationKeys.ContestTaskboard, IconKeys.TaskListRegular, typeof(ContestTaskboardPageViewModel)),
        new(NavigationKeys.ContestSchedule, IconKeys.CalendarClockRegular, typeof(ContestSchedulePageViewModel)),
        new(NavigationKeys.ContestEvent, IconKeys.VideoClipRegular, typeof(ContestEventPageViewModel)),
        new(NavigationKeys.ContestEventSchedule, IconKeys.AlertRegular, typeof(ContestEventSchedulePageViewModel)),
        new(NavigationKeys.Assessment, IconKeys.ScalesRegular, typeof(AssessmentPageViewModel)),
        new(NavigationKeys.Rating, IconKeys.DataHistogramRegular, typeof(RatingPageViewModel)),
        new(NavigationKeys.Scoreboard, IconKeys.RewardRegular, typeof(ScoreboardPageViewModel)),
        new(NavigationKeys.Participators, IconKeys.PeopleTeamRegular, typeof(ParticipatorsPageViewModel)),
        new(NavigationKeys.Teams, IconKeys.PeopleAudienceRegular, typeof(TeamsPageViewModel)),
        new(NavigationKeys.Experts, IconKeys.HatGraduationRegular, typeof(ExpertsPageViewModel)),
        new(NavigationKeys.Moderators, IconKeys.GuestRegular, typeof(ModeratorsPageViewModel)),
        new(NavigationKeys.HackathonOverview, IconKeys.HeartPulseRegular, typeof(HackathonOverviewPageViewModel)),
        new(NavigationKeys.HackathonSettings, IconKeys.OptionsRegular, typeof(HackathonSettingsPageViewModel))
    };

    public IEnumerable<PageContextModel> GetTitleMenu() =>
        new List<PageContextModel>()
        {
            new(NavigationKeys.Settings, IconKeys.SettingsRegular, typeof(SettingsViewModel)),
            new(NavigationKeys.Profile, IconKeys.PersonRegular, typeof(ProfilePageViewModel))
        };

    public IEnumerable<PageContextModel> GetApplicationMenu() => Pages.Skip(3);

    public void GoBack() { }

    public void Navigate<T>() where T : IPageContext =>
        WeakReferenceMessenger.Default
            .Send(new SelectedPageContextChanged(
                new SelectedPageChangedArgs
                {
                    ContextType = typeof(T)
                }));

    public void Navigate<T>(NavigationArgs args) where T : IPageContext =>
        WeakReferenceMessenger.Default
            .Send(new SelectedPageContextChanged(new SelectedPageChangedArgs
            {
                ContextType = typeof(T),
                NavigationArgs = args
            }));
}

public class DesignDashboardService : IDashboardService
{
    public Task<DashboardDto> GetDashboardOverview() =>
        Task.FromResult(new DashboardDto());
}

public class DesignTaskboardService : ITaskboardService
{
    public Task<List<ContestCaseDto>> GetContestCases() =>
        Task.FromResult(new List<ContestCaseDto>());
}

public class DesignUserService : IUserService
{
    public Task<T?> GetAccountAs<T>(string entityName) where T : class =>
        Task.FromResult(new ContestantDto { Firstname = "Mock", Lastname = "Mock", Id = Guid.NewGuid() } as T);

    public Task<ContestantDto?> GetAccount() => Task.FromResult(new ContestantDto
    {
        Firstname = "Mock", Lastname = "Mock", Id = Guid.NewGuid()
    })!;

    public Task SaveUserPreferences(UserPreferencesDto preferences) => Task.FromResult(true);
}

public class DesignApplicationService : IApplicationService
{
    public Task SubmitApplicationAsync(
        ContestApplicationDto dto,
        CancellationToken cancellationToken = default) => Task.FromResult(true);

    public Task ApproveApplicationAsync(
        ContestApplicationDto dto,
        CancellationToken cancellationToken = default) => Task.FromResult(true);

    public Task DeclineApplicationAsync(
        ContestApplicationDto dto,
        CancellationToken cancellationToken = default) => Task.FromResult(true);
}

public class DesignTeamService : ITeamService
{
    public Task<TeamDto?> GetTeam() => Task.FromResult(new TeamDto { Name = "Mock Team", Id = Guid.NewGuid() })!;

    public Task<TeamDto?> GetTeamById(Guid teamId) =>
        Task.FromResult(new TeamDto { Name = "Mock Team", Id = Guid.NewGuid() })!;

    public Task UploadTeaser(IStorageFile file) => Task.FromResult(true);
    public Task<List<StorageFileDto>> GetStorageFiles() => Task.FromResult(new List<StorageFileDto>());
    public Task<List<StorageFileDto>> GetStorageFiles(Guid teamId) => Task.FromResult(new List<StorageFileDto>());
    public Task JoinTeam(ContestApplicationDto application) => Task.FromResult(true);
    public Task CreateTeam(ContestApplicationDto application) => Task.FromResult(true);
}

public static class DesignerMocks
{
    public static TitleViewModel TitleViewModelMock = new();
    public static ContentViewModel ContentViewModelMock = new();
    public static NavigationViewModel NavigationViewModelMock = new();

    public static Mock<ILogger<T>> LoggerMock<T>() => new();
}
