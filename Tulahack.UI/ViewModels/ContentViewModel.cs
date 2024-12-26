using CommunityToolkit.Mvvm.ComponentModel;

namespace Tulahack.UI.ViewModels;

public partial class ContentViewModel : Base.ViewModelBase
{
    [ObservableProperty] private IPageContext? _currentPageContext;
}