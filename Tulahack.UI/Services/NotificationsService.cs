using System;
using Tulahack.UI.ToastNotifications;

namespace Tulahack.UI.Services;

public interface INotificationsService
{
    public INotificationMessageManager Manager { get; }
    public INotificationMessage ShowSuccess(string? message = default);
    public INotificationMessage ShowInformation(string message);
    public INotificationMessage ShowWarning(string message);
    public INotificationMessage ShowError(string message);
}

public class NotificationsService(INotificationMessageManager manager) : INotificationsService
{
    public INotificationMessageManager Manager { get; } = manager;

    public INotificationMessage ShowSuccess(string? message = default)
        => Manager
            .CreateMessage()
            .Accent("#5cb85c")
            .Background("#333")
            .Animates(true)
            .HasBadge("Success")
            .HasMessage(string.Join(" ", ["Yay! Well done!", message]))
            .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
            .Queue();

    public INotificationMessage ShowInformation(string message)
        => Manager
            .CreateMessage()
            .Accent("#1751C3")
            .Animates(true)
            .Background("#333")
            .HasBadge("Info")
            .HasMessage(message)
            .Dismiss().WithButton("Got it!", _ => { })
            .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
            .Queue();

    public INotificationMessage ShowWarning(string message)
        => Manager
            .CreateMessage()
            .Accent("#E0A030")
            .Background("#333")
            .HasBadge("Warn")
            .HasHeader("Attention!")
            .Animates(true)
            .HasMessage(message)
            .Dismiss().WithButton("Ok", _ => { })
            .Dismiss().WithButton("Close", _ => { })
            .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
            .Queue();

    public INotificationMessage ShowError(string message)
        => Manager
            .CreateMessage()
            .Accent("#F15B19")
            .Background("#F15B19")
            .Animates(true)
            .HasMessage($"Critical error happened! Details: {message}")
            .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
            .Queue();
}