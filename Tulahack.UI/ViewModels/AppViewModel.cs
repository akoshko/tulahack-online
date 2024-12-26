using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tulahack.UI.Services;
using Tulahack.UI.Storage;
using Tulahack.UI.ToastNotifications;
using Tulahack.UI.ViewModels.Base;
using Tulahack.UI.ViewModels.Designer;

namespace Tulahack.UI.ViewModels;

public partial class AppViewModel : PersistedViewModelBase<AppState>
{
    private readonly IRuntimeStorageProvider<AppState> _storage;
    private readonly INotificationsService _notificationsService;

    [ObservableProperty] private NavigationViewModel _navigationViewModel;
    [ObservableProperty] private TitleViewModel _titleViewModel;
    [ObservableProperty] private ContentViewModel _contentViewModel;

    [ObservableProperty] private bool _isPaneOpen;

    public INotificationMessageManager Manager { get; set; }

    [RelayCommand]
    private void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }

    public AppViewModel() : this(
        DesignerMocks.NavigationViewModelMock,
        DesignerMocks.TitleViewModelMock,
        DesignerMocks.ContentViewModelMock,
        new DesignStorageProvider<AppState>(),
        new NotificationsService(new NotificationMessageManager()))
    {
    }

    public AppViewModel(
        NavigationViewModel navigationViewModel,
        TitleViewModel titleViewModel,
        ContentViewModel contentViewModel,
        IRuntimeStorageProvider<AppState> storage,
        INotificationsService notificationsService) : base(storage)
    {
        _storage = storage;
        _notificationsService = notificationsService;

        _navigationViewModel = navigationViewModel;
        _titleViewModel = titleViewModel;
        _contentViewModel = contentViewModel;

        Manager = notificationsService.Manager;
    }
}