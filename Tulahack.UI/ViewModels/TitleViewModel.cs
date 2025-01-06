using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Tulahack.Dtos;
using Tulahack.UI.Constants;
using Tulahack.UI.Services;
using Tulahack.UI.Utils;
using Tulahack.UI.ViewModels.Pages.Public;

namespace Tulahack.UI.ViewModels;

public partial class TitleViewModel : Base.ViewModelBase
{
    [ObservableProperty] private string _title = "Default title";
    [ObservableProperty] private PageContextModel _currentPage;
    [ObservableProperty] private PersonBaseDto _account;
    private readonly IAuthContextProvider _provider;
    private readonly IUserService _userService;
    private readonly INavigationService _navigationService;

    public TitleViewModel() : this(
        Ioc.Default.GetRequiredService<IUserService>(),
        Ioc.Default.GetRequiredService<IAuthContextProvider>(),
        Ioc.Default.GetRequiredService<INavigationService>())
    {
        CurrentPage = new(
            "Designer Mode header",
            IconKeys.HomeRegular,
            typeof(DashboardViewModel)
        );
        Title = CurrentPage.Label;
    }

    public TitleViewModel(
        IUserService userService,
        IAuthContextProvider provider,
        INavigationService navigationService)
    {
        _navigationService = navigationService;
        _provider = provider;
        _userService = userService;

        Account = _provider.GetDefaultAccount();
    }

    [RelayCommand]
    public void NavigateToProfilePage() =>
        _navigationService.Navigate<ProfilePageViewModel>();

    [RelayCommand]
    public void NavigateToSettingsPage() =>
        _navigationService.Navigate<SettingsViewModel>();

    [RelayCommand]
    public void NavigateToApplicationForm() =>
        _navigationService.Navigate<ApplicationFormViewModel>();

    [RequiresUnreferencedCode("See comment above base class for more details.")]
    protected async override void OnActivated()
    {
        switch (_provider.GetRole())
        {
            case ContestRoleDto.Contestant:
                Account = await _userService.GetAccountAs<ContestantDto>(nameof(ContestantDto)) ?? _provider.GetDefaultAccount();
                break;
            case ContestRoleDto.Expert:
                Account = await _userService.GetAccountAs<ExpertDto>(nameof(ExpertDto)) ?? _provider.GetDefaultAccount();
                break;
            case ContestRoleDto.Moderator:
                Account = await _userService.GetAccountAs<ModeratorDto>(nameof(ModeratorDto)) ?? _provider.GetDefaultAccount();
                break;
            case ContestRoleDto.Visitor:
            case ContestRoleDto.Superuser:
            default:
                break;
        }
    }
}
