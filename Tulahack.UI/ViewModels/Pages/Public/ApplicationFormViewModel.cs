﻿using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Tulahack.Dtos;
using Tulahack.UI.Constants;
using Tulahack.UI.Services;
using Tulahack.UI.ViewModels.Base;

namespace Tulahack.UI.ViewModels.Pages.Public;

public partial class ApplicationFormViewModel : ViewModelBase
{
    [ObservableProperty] private bool _createNewTeamOption;
    [ObservableProperty] private bool _joinExistingTeamOption;

    [ObservableProperty] private bool? _teamCheckInProgress;
    [ObservableProperty] private bool? _teamCheckResult;
    [ObservableProperty] private string _teamCheckResultIcon = string.Empty;

    [ObservableProperty] private TeamDto? _existingTeam;
    [ObservableProperty] private ContestApplicationDto _contestsApplication;

    private readonly ITeamService _teamService;
    private readonly IApplicationService _applicationService;
    private readonly INotificationsService _notificationsService;

    public ApplicationFormViewModel() : this(
        Ioc.Default.GetRequiredService<ITeamService>(),
        Ioc.Default.GetRequiredService<IApplicationService>(),
        Ioc.Default.GetRequiredService<INotificationsService>())
    {
    }

    public ApplicationFormViewModel(
        ITeamService teamService,
        IApplicationService applicationService,
        INotificationsService notificationsService)
    {
        _teamService = teamService;
        _applicationService = applicationService;
        _notificationsService = notificationsService;

        ContestsApplication = new ContestApplicationDto
        {
            TeamName = "",
            Section = 1,
            AboutTeam = "Some team of 5 students",
            FormOfParticipation = FormOfParticipationTypeDto.Online,
            ParticipatorsCount = 3,
            JoinExistingTeam = false,
            ExistingTeamId = null,
            TeamLeaderId = Guid.NewGuid()
        };
    }

    [RelayCommand]
    public async Task CheckTeam()
    {
        try
        {
            TeamCheckInProgress = true;

#pragma warning disable IDE0045
            if (ContestsApplication.ExistingTeamId.HasValue)
#pragma warning restore IDE0045
            {
                ExistingTeam = await _teamService.GetTeamById(ContestsApplication.ExistingTeamId.Value);
            }
            else
            {
                ExistingTeam = null;
            }

            TeamCheckInProgress = false;
        }
        catch (Exception)
        {
            ExistingTeam = null;
            TeamCheckInProgress = false;
        }

        TeamCheckResult = ExistingTeam is not null;
        TeamCheckResultIcon = TeamCheckResult is true ? IconKeys.CheckmarkCircleRegular : IconKeys.ErrorCircleRegular;
    }

    [RelayCommand]
    public async Task SubmitApplication()
    {
        ContestsApplication.Validate();
        if (ContestsApplication.HasErrors)
        {
            _notificationsService.ShowWarning("Form validation failed! Check your form and try again.");
            return;
        }

        await _applicationService.SubmitApplicationAsync(ContestsApplication);
    }
}