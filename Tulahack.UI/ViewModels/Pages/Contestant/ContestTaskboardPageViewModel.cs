using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Tulahack.Dtos;
using Tulahack.UI.Messaging;
using Tulahack.UI.Services;
using Tulahack.UI.ViewModels.Base;

namespace Tulahack.UI.ViewModels.Pages.Contestant;

public partial class ContestTaskboardPageViewModel : ViewModelBase
{
    [ObservableProperty] private List<ContestCaseDto> _records;
    [ObservableProperty] private ContestCaseDto _selectedItem;

    private readonly INavigationService _navigationService;
    private readonly ITaskboardService _taskboardService;



    // AvaloniaUI Designer hack
    public ContestTaskboardPageViewModel() : this(
        Ioc.Default.GetRequiredService<INavigationService>(),
        Ioc.Default.GetRequiredService<ITaskboardService>())
    {
    }

    public ContestTaskboardPageViewModel(
        INavigationService navigationService,
        ITaskboardService taskboardService)
    {
        _navigationService = navigationService;
        _taskboardService = taskboardService;
    }

    [RequiresUnreferencedCode("See comment above base class for more details.")]
    protected async override void OnActivated()
    {
        Records = await _taskboardService.GetContestCases();
        if (Records.Count != 0)
        {
            SelectedItem = Records.First();
        }
    }

    [RelayCommand]
    public void NavigateToTaskPage(ContestCaseDto dto) =>
        _navigationService.Navigate<ContestTaskPageViewModel>(new NavigationArgs { Sender = this, Args = dto });
}