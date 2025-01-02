using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tulahack.UI.Constants;
using Tulahack.UI.Services;
using Tulahack.UI.ViewModels.Designer;
using Tulahack.UI.ViewModels.Pages.Public;

namespace Tulahack.UI.ViewModels;

public partial class TitleViewModel : Base.ViewModelBase
{
    [ObservableProperty] private string _title = "Default title";
    [ObservableProperty] private PageContextModel _currentPage;
    private readonly INavigationService _navigationService;

    public TitleViewModel() : this(new DesignNavigationService())
    {
        CurrentPage = new(
            "Designer Mode header",
            IconKeys.HomeRegular,
            typeof(DashboardViewModel)
        );
        Title = CurrentPage.Label;
    }

    public TitleViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
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
}