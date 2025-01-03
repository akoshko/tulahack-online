using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Tulahack.UI.Services;
using Tulahack.UI.Storage;
using Tulahack.UI.ToastNotifications;
using Tulahack.UI.ViewModels.Base;

namespace Tulahack.UI.ViewModels;

public partial class AppViewModel : PersistedViewModelBase<AppState>
{
    [ObservableProperty] private NavigationViewModel _navigationViewModel;
    [ObservableProperty] private TitleViewModel _titleViewModel;
    [ObservableProperty] private ContentViewModel _contentViewModel;

    [ObservableProperty] private bool _isPaneOpen;

    public INotificationMessageManager Manager { get; set; }

    [RelayCommand]
    private void TriggerPane() =>
        IsPaneOpen = !IsPaneOpen;

    // AvaloniaUI designer hack
    public AppViewModel() : this(
        Ioc.Default.GetRequiredService<NavigationViewModel>(),
        Ioc.Default.GetRequiredService<TitleViewModel>(),
        Ioc.Default.GetRequiredService<ContentViewModel>(),
        Ioc.Default.GetRequiredService<IRuntimeStorageProvider<AppState>>(),
        Ioc.Default.GetRequiredService<INotificationsService>())
    {
    }

    public AppViewModel(
        NavigationViewModel navigationViewModel,
        TitleViewModel titleViewModel,
        ContentViewModel contentViewModel,
        IRuntimeStorageProvider<AppState> storage,
        INotificationsService notificationsService) : base(storage)
    {
        _navigationViewModel = navigationViewModel;
        _titleViewModel = titleViewModel;
        _contentViewModel = contentViewModel;

        Manager = notificationsService.Manager;
    }
}