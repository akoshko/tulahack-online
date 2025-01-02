using Tulahack.UI.ToastNotifications.Controls;

namespace Tulahack.UI.ToastNotifications;

/// <summary>
/// The notification message factory.
/// </summary>
/// <seealso cref="INotificationMessageFactory" />
public class NotificationMessageFactory : INotificationMessageFactory
{
    /// <summary>
    /// Gets the message.
    /// </summary>
    /// <returns>
    /// Returns new instance of notification message.
    /// </returns>
    public INotificationMessage GetMessage() =>
        new NotificationMessage();

    /// <summary>
    /// Gets the button.
    /// </summary>
    /// <returns>
    /// Returns new instance of notification message button.
    /// </returns>
    public INotificationMessageButton GetButton() =>
        new NotificationMessageButton();
}