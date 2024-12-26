namespace Tulahack.UI.ViewModels.Base;

public abstract partial class PageViewModelBase : ViewModelBase, IPageContext
{
    public bool CanGoBack { get; }
}