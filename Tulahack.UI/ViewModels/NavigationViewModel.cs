using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Tulahack.UI.Messaging;
using Tulahack.UI.Services;
using Tulahack.UI.ViewModels.Base;
using Tulahack.UI.ViewModels.Designer;
using Tulahack.UI.ViewModels.Pages.Public;

namespace Tulahack.UI.ViewModels;

public partial class NavigationViewModel : ViewModelBase
{
    private readonly TitleViewModel _titleViewModel;
    private readonly ContentViewModel _contentViewModel;
    private readonly ILogger<NavigationViewModel> _logger;
    
    [ObservableProperty] private ObservableCollection<PageContextModel> _navigationItems;
    [ObservableProperty] private PageContextModel? _selectedListItem;

    partial void OnSelectedListItemChanged(PageContextModel? value)
    {
        if (value is null) return;
        
        NavigateToPage(value);
    }
    
    public NavigationViewModel() : this(
        DesignerMocks.ContentViewModelMock,
        DesignerMocks.TitleViewModelMock,
        new DesignNavigationService(),
        WeakReferenceMessenger.Default,
        DesignerMocks.LoggerMock<NavigationViewModel>().Object) {}
    
    public NavigationViewModel(
        ContentViewModel contentViewModel,
        TitleViewModel titleViewModel,
        INavigationService navigationService,
        IMessenger messenger,
        ILogger<NavigationViewModel> logger)
    {
        _contentViewModel = contentViewModel;
        _titleViewModel = titleViewModel;
        _logger = logger;
        
        // navigationService and messenger are registered as singletons
        messenger.Register<SelectedPageContextChanged>(this, (r, m) =>
        {
            var page = navigationService.Pages.First(item => item.ViewModelType == m.Value.ContextType);
            if (m.Value.NavigationArgs is null)
                NavigateToPage(page);
            
            NavigateToPage(page, m.Value.NavigationArgs);
        });
        
        var menu = navigationService.GetApplicationMenu();
        NavigationItems = new ObservableCollection<PageContextModel>(menu);
        
        SelectedListItem = NavigationItems.First(item => item.ViewModelType == typeof(DashboardViewModel));
    }

    private void NavigateToPage(PageContextModel? context, NavigationArgs? args = null)
    {
        if (context is null)
        {
            _logger.LogWarning($"Cannot perform navigation: {context?.Label ?? "Unknown"} page context is null!");
            return;
        }
            
        _logger.LogWarning($"Navigating to {context.Label} page");
        var pageContext = Ioc.Default.GetService(context.ViewModelType) as ViewModelBase;
        if (pageContext is null)     
        {
            _logger.LogWarning($"Cannot perform navigation: not found {context.Label} page ViewModel in DI container!");
            return;
        }
        
        _contentViewModel.CurrentPageContext = pageContext;
        _titleViewModel.CurrentPage = context;
        
        if (args is not null)
            pageContext.SetArgs(args);
        
        pageContext.Activate();
    }
}