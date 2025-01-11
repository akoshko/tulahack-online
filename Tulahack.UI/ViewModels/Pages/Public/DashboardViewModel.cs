using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Tulahack.Dtos;
using Tulahack.UI.Extensions;
using Tulahack.UI.Services;
using Tulahack.UI.ViewModels.Base;

namespace Tulahack.UI.ViewModels.Pages.Public;

public partial class DashboardViewModel : ViewModelBase
{
    private readonly IDashboardService _dashboardService;
    [ObservableProperty] private DashboardDto _dashboardDto;
    [ObservableProperty] private List<TimelineItemViewModel> _timelineSource;

    // AvaloniaUI Designer hack
    public DashboardViewModel() : this(Ioc.Default.GetRequiredService<IDashboardService>()) { }

    public DashboardViewModel(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [RequiresUnreferencedCode("See comment above base class for more details.")]
    protected override void OnActivated() =>
        Observable
            .Start(Init)
            .Subscribe()
            .Dispose();

    private async Task Init()
    {
        DashboardDto = await _dashboardService.GetDashboardOverview();
        TimelineSource = DashboardDto.Timeline.Items
            .Select(item => new TimelineItemViewModel
            {
                Description = item.Message,
                TimeFormat = "dd.MM.yyy",
                Time = item.Start,
                Header = item.Label,
                ItemType = item.ItemType.ConvertToControlType()
            })
            .ToList();
    }
}
