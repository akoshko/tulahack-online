using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;
using Tulahack.Dtos;
using Tulahack.UI.Extensions;
using Tulahack.UI.Services;
using Tulahack.UI.Utils;
using Tulahack.UI.ViewModels.Base;
using Tulahack.UI.ViewModels.Designer;

namespace Tulahack.UI.ViewModels.Pages.Public;

public partial class ProfilePageViewModel : ViewModelBase
{


    [ObservableProperty] private PersonBaseDto _account;
    private readonly IAuthContextProvider _provider;
    private readonly IUserService _userService;

    // AvaloniaUI Designer hack
    public ProfilePageViewModel() : this(new DesignUserService(), new DesignAuthProvider())
    {
    }

    public ProfilePageViewModel(
        IUserService userService,
        IAuthContextProvider provider)
    {
        _userService = userService;
        _provider = provider;

        Account = _provider.GetDefaultAccount();
    }

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