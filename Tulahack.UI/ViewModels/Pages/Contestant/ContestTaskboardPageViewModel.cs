using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
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
    [ObservableProperty] private FlatTreeDataGridSource<ContestCaseDto> _source;
    [ObservableProperty] private ContestCaseDto _selectedItem;

    private readonly INavigationService _navigationService;
    private readonly ITaskboardService _taskboardService;


    // AvaloniaUI Designer hack
    public ContestTaskboardPageViewModel() : this(
        Ioc.Default.GetRequiredService<INavigationService>(),
        Ioc.Default.GetRequiredService<ITaskboardService>())
    { }

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
        List<ContestCaseDto> records = await _taskboardService.GetContestCases();

        if (records.Count != 0)
        {
            SelectedItem = records.First();
        }

        Source = new FlatTreeDataGridSource<ContestCaseDto>(records)
        {
            Columns =
            {
                new TextColumn<ContestCaseDto, int>("#", x => x.Number),
                new TextColumn<ContestCaseDto, string>("Название", x => x.Title),
                new TextColumn<ContestCaseDto, string>("Компания", x => x.Company.Name),
                new TemplateColumn<ContestCaseDto>("Критерии", "AcceptanceCriteriaCell"),
                new TemplateColumn<ContestCaseDto>("Стек технологий", "TechStackCell"),
                new TemplateColumn<ContestCaseDto>("Сложность", "ComplexityCell"),
                new TemplateColumn<ContestCaseDto>("Кнопки", "ButtonsCell"),
            },
        };
    }

    [RelayCommand]
    public void NavigateToTaskPage(ContestCaseDto dto) =>
        _navigationService.Navigate<ContestTaskPageViewModel>(new NavigationArgs { Sender = this, Args = dto });
}
