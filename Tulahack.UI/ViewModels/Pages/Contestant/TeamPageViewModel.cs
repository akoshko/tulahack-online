using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tulahack.Dtos;
using Tulahack.UI.Components.Controls.CodeBehind;
using Tulahack.UI.Services;
using Tulahack.UI.Utils;
using Tulahack.UI.ViewModels.Base;
using Tulahack.UI.ViewModels.Designer;

namespace Tulahack.UI.ViewModels.Pages.Contestant;

public partial class TeamPageViewModel : ViewModelBase
{
    private readonly ITeamService _teamService;
    private readonly IMainViewProvider _mainViewProvider;
    private readonly ILogger<TeamPageViewModel> _logger;

    [ObservableProperty]
    private HyperlinkContent _backHyperlink = new()
    {
        Alias = "Back", Url = "_blank"
    };

    [ObservableProperty] private TeamDto _team = new();

    // AvaloniaUI Designer hack
    public TeamPageViewModel() : this(
        new DesignTeamService(),
        new MainViewProvider(),
        DesignerMocks.LoggerMock<TeamPageViewModel>().Object)
    {
    }

    public TeamPageViewModel(
        ITeamService teamService,
        IMainViewProvider mainViewProvider,
        ILogger<TeamPageViewModel> logger)
    {
        _teamService = teamService;
        _mainViewProvider = mainViewProvider;
        _logger = logger;
    }

    [RelayCommand]
    public async Task UploadTeaserCommand()
    {
        _logger.LogInformation("TeamPageViewModel: Executing UploadTeaserCommand");
        IStorageProvider? sp = _mainViewProvider.GetStorageProvider();
        if (sp is null)
        {
            return;
        }

        IReadOnlyList<IStorageFile> result = await sp.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Open File",
            FileTypeFilter =
            [
                FilePickerFileTypes.All,
                FilePickerFileTypes.TextPlain
            ],
            AllowMultiple = false
        });

        if (result.Count != 0)
        {
            await _teamService.UploadTeaser(result[0]);
        }
    }

    [RequiresUnreferencedCode("See comment above base class for more details.")]
    protected override void OnActivated()
    {
        if (NavigationArgs?.Args is TeamDto dto)
        {
            Team = dto;
        }
    }
}