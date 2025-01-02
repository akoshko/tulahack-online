using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Tulahack.Dtos;
using Tulahack.UI.Extensions;
using Tulahack.UI.Services;
using Tulahack.UI.ViewModels.Base;
using Tulahack.UI.ViewModels.Designer;

namespace Tulahack.UI.ViewModels.Pages.Public;

public partial class DashboardViewModel : ViewModelBase
{
    private readonly IDashboardService _dashboardService;
    [ObservableProperty] private DashboardDto _dashboardDto;
    [ObservableProperty] private List<TimelineItemViewModel> _timelineSource;

    // AvaloniaUI Designer hack
    public DashboardViewModel() : this(new DesignDashboardService()) {}
    public DashboardViewModel(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [RequiresUnreferencedCode("See comment above base class for more details.")]
    protected async override void OnActivated()
    {
        DashboardDto = await _dashboardService.GetDashboardOverview();
        TimelineSource = DashboardDto.Timeline.Items
            .Select(item => new TimelineItemViewModel
            {
                Description = item.Message,
                TimeFormat="dd.MM.yyy",
                Time = item.Start,
                Header = item.Label,
                ItemType = item.ItemType.ConvertToControlType()

            })
            .ToList();
    }
}