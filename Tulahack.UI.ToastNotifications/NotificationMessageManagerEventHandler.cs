namespace Tulahack.UI.ToastNotifications;

/// <summary>
/// The notification message manager event handler.
/// </summary>
/// <param name="sender">The sender.</param>
/// <param name="args">The <see cref="NotificationMessageManagerEventArgs"/> instance containing the event data.</param>
#pragma warning disable CA1003
public delegate void NotificationMessageManagerEventHandler(
#pragma warning restore CA1003
    object sender,
    NotificationMessageManagerEventArgs args);
