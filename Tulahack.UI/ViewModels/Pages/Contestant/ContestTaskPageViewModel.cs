﻿using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;
using Tulahack.Dtos;
using Tulahack.UI.Components.Controls.CodeBehind;
using Tulahack.UI.ViewModels.Base;

namespace Tulahack.UI.ViewModels.Pages.Contestant;

public partial class ContestTaskPageViewModel : ViewModelBase
{
    [ObservableProperty] private ContestCaseDto? _contestCase;

    [ObservableProperty]
    private HyperlinkContent _backHyperlink = new()
    {
        Alias = "Back", Url = "_blank"
    };

    [ObservableProperty]
    private HyperlinkContent _lofiHyperlink = new()
    {
        Alias = "Get some LoFi",
        Url = "https://www.youtube.com/watch?v=jfKfPfyJRdk"
    };

    [RequiresUnreferencedCode("See comment above base class for more details.")]
    protected override void OnActivated()
    {
        if (NavigationArgs?.Args is null)
        {
            return;
        }

        ContestCase = NavigationArgs.Args as ContestCaseDto;
    }
}
