using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Tulahack.Dtos;
using Tulahack.UI.Services;
using Tulahack.UI.ViewModels.Base;
using Tulahack.UI.ViewModels.Designer;

namespace Tulahack.UI.ViewModels.Pages.Public;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly IUserService _userService;
    private readonly ILogger<SettingsViewModel> _logger;

    [ObservableProperty]
    private ContestCaseComplexityDto _difficulty = ContestCaseComplexityDto.Easy;
    [ObservableProperty]
    private FormOfParticipationTypeDto _participation = FormOfParticipationTypeDto.Online;
    [ObservableProperty]
    private UserPreferencesDto? _preferences = new()
    {
        SelectedTheme = UserPreferredThemeDto.Default
    };

    [ObservableProperty] private ContestantDto? _contestant;
    [ObservableProperty] private ContestCaseComplexityDto? _complexity;

    // AvaloniaUI Designer hack
    public SettingsViewModel() : this(
        new DesignUserService(),
        DesignerMocks.LoggerMock<SettingsViewModel>().Object)
    { }

    public SettingsViewModel(
        IUserService userService,
        ILogger<SettingsViewModel> logger)
    {
        _logger = logger;
        _userService = userService;
    }

    [RelayCommand]
    public Task SaveChanges()
    {
        if (Preferences is null)
        {
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
        // await _userService.SaveUserPreferences(Preferences);
    }
}
