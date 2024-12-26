using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Tulahack.UI.Constants;
using Tulahack.UI.Messaging;
using Tulahack.UI.Utils;
using Tulahack.UI.ViewModels;
using Tulahack.UI.ViewModels.Pages.Public;
using Tulahack.UI.ViewModels.Pages.Contestant;
using Tulahack.UI.ViewModels.Pages.Expert;
using Tulahack.UI.ViewModels.Pages.Moderator;
using Tulahack.UI.ViewModels.Pages.Superuser;

namespace Tulahack.UI.Services;

public interface INavigationService
{
    IEnumerable<PageContextModel> Pages { get; }
    IEnumerable<PageContextModel> GetTitleMenu();
    IEnumerable<PageContextModel> GetApplicationMenu();
    bool CanGoBack { get; }
    void GoBack();
    void Navigate<T>() where T : IPageContext;
    void Navigate<T>(NavigationArgs args) where T : IPageContext;
}

public class NavigationService : INavigationService
{
    private readonly IMessenger _messenger;
    private readonly IAuthContextProvider _authContextProvider;
    private readonly ILogger<INavigationService> _logger;
    
    private readonly IEnumerable<PageContextModel> _pages = new List<PageContextModel>()
    {
        new(NavigationKeys.Settings, IconKeys.SettingsRegular, typeof(SettingsViewModel)),
        new(NavigationKeys.Profile, IconKeys.PersonRegular, typeof(ProfilePageViewModel)),
        new(NavigationKeys.Dashboard, IconKeys.HomeRegular, typeof(DashboardViewModel)),
        new(NavigationKeys.ApplicationForm, IconKeys.RocketRegular, typeof(ApplicationFormViewModel)),
        new(NavigationKeys.Team, IconKeys.PeopleRegular, typeof(TeamPageViewModel)),
        new(NavigationKeys.ContestTask, IconKeys.TasksAppRegular, typeof(ContestTaskPageViewModel)),
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
    
    private readonly IEnumerable<PageContextModel> _appTitleButtons = new List<PageContextModel>()
    {
        new(NavigationKeys.Settings, IconKeys.SettingsRegular, typeof(SettingsViewModel)),
        new(NavigationKeys.Profile, IconKeys.PersonRegular, typeof(ProfilePageViewModel)),
    };

    private readonly IEnumerable<PageContextModel> _publicPages = new List<PageContextModel>()
    {
        new(NavigationKeys.Dashboard, IconKeys.HomeRegular, typeof(DashboardViewModel)),
    };

    private readonly IEnumerable<PageContextModel> _contestantPages = new List<PageContextModel>()
    {
        new(NavigationKeys.Team, IconKeys.PeopleRegular, typeof(TeamPageViewModel)),
        // TODO: is it needed here?
        // new(NavigationKeys.ContestTask, IconKeys.TasksAppRegular, typeof(ContestTaskPageViewModel)),
        new(NavigationKeys.ContestTaskboard, IconKeys.TaskListRegular, typeof(ContestTaskboardPageViewModel)),
        new(NavigationKeys.ContestSchedule, IconKeys.CalendarClockRegular, typeof(ContestSchedulePageViewModel)),
        new(NavigationKeys.ContestEvent, IconKeys.VideoClipRegular, typeof(ContestEventPageViewModel)),
        new(NavigationKeys.ContestEventSchedule, IconKeys.AlertRegular, typeof(ContestEventSchedulePageViewModel))
    };

    private readonly IEnumerable<PageContextModel> _expertPages = new List<PageContextModel>()
    {
        new(NavigationKeys.Assessment, IconKeys.ScalesRegular, typeof(AssessmentPageViewModel)),
        new(NavigationKeys.Rating, IconKeys.DataHistogramRegular, typeof(RatingPageViewModel))
    };

    private readonly IEnumerable<PageContextModel> _moderatorPages = new List<PageContextModel>()
    {
        new(NavigationKeys.Scoreboard, IconKeys.RewardRegular, typeof(ScoreboardPageViewModel)),
        new(NavigationKeys.Participators, IconKeys.PeopleTeamRegular, typeof(ParticipatorsPageViewModel)),
        new(NavigationKeys.Teams, IconKeys.PeopleAudienceRegular, typeof(TeamsPageViewModel)),
        new(NavigationKeys.Experts, IconKeys.HatGraduationRegular, typeof(ExpertsPageViewModel)),
    };

    private readonly IEnumerable<PageContextModel> _superuserPages = new List<PageContextModel>()
    {
        new(NavigationKeys.Moderators, IconKeys.GuestRegular, typeof(ModeratorsPageViewModel)),
        new(NavigationKeys.HackathonOverview, IconKeys.HeartPulseRegular, typeof(HackathonOverviewPageViewModel)),
        new(NavigationKeys.HackathonSettings, IconKeys.OptionsRegular, typeof(HackathonSettingsPageViewModel)),
    };

    public NavigationService(
        IAuthContextProvider authContextProvider,
        IMessenger messenger,
        ILogger<INavigationService> logger)
    {
        _authContextProvider = authContextProvider;
        _messenger = messenger;
        _logger = logger;
    }

    public IEnumerable<PageContextModel> Pages => _pages;

    public IEnumerable<PageContextModel> GetTitleMenu() => _appTitleButtons;

    public IEnumerable<PageContextModel> GetApplicationMenu()
    {
        var role = _authContextProvider.GetGroup();
        _logger.LogWarning($"Claim role: {role}");

        if (role == Groups.Public)
            return _publicPages;

        if (role == Groups.Contestant)
            return _publicPages
                .Concat(_contestantPages);

        if (role == Groups.Expert)
            return _publicPages
                .Concat(_contestantPages)
                .Concat(_expertPages);

        if (role == Groups.Moderator)
            return _publicPages
                .Concat(_contestantPages)
                .Concat(_expertPages)
                .Concat(_moderatorPages);

        if (role == Groups.Superuser)
            return _publicPages
                .Concat(_contestantPages)
                .Concat(_expertPages)
                .Concat(_moderatorPages)
                .Concat(_superuserPages);

        return _publicPages;
    }

    public bool CanGoBack => true;

    public void GoBack()
    {
    }

    public void Navigate<T>() where T : IPageContext =>
        _messenger.Send(new SelectedPageContextChanged(new SelectedPageChangedArgs { ContextType = typeof(T) }));

    public void Navigate<T>(NavigationArgs args) where T : IPageContext =>
        _messenger.Send(new SelectedPageContextChanged(new SelectedPageChangedArgs { ContextType = typeof(T), NavigationArgs = args}));
}