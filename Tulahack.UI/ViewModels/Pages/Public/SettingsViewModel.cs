using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tulahack.Dtos;
using Tulahack.UI.ViewModels.Base;

namespace Tulahack.UI.ViewModels.Pages.Public;

public partial class SettingsViewModel : ViewModelBase
{
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
