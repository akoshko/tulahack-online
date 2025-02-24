using Avalonia;

namespace Tulahack.UI.ToastNotifications;

/// <summary>
/// The notification message manager.
/// </summary>
/// <seealso cref="INotificationMessageManager" />
public class NotificationMessageManager : AvaloniaObject, INotificationMessageManager
{
    private readonly List<INotificationMessage> _queuedMessages = new();

    /// <summary>
    /// Occurs when new notification message is queued.
    /// </summary>
    public event NotificationMessageManagerEventHandler OnMessageQueued;

    /// <summary>
    /// Occurs when notification message is dismissed.
    /// </summary>
    public event NotificationMessageManagerEventHandler OnMessageDismissed;

    /// <summary>
    /// Gets or sets the factory.
    /// </summary>
    /// <value>
    /// The factory.
    /// </value>
    public INotificationMessageFactory Factory { get; set; } = new NotificationMessageFactory();


    /// <summary>
    /// Queues the specified message.
    /// This will ignore the <c>null</c> message or already queued notification message.
    /// </summary>
    /// <param name="message">The message.</param>
    public void Queue(INotificationMessage? message)
    {
        if (message == null || _queuedMessages.Contains(message))
        {
            return;
        }

        _queuedMessages.Add(message);

        TriggerMessageQueued(message);
    }

    /// <summary>
    /// Triggers the message queued event.
    /// </summary>
    /// <param name="message">The message.</param>
    private void TriggerMessageQueued(INotificationMessage message) =>
        OnMessageQueued.Invoke(this, new NotificationMessageManagerEventArgs(message));

    /// <summary>
    /// Dismisses the specified message.
    /// This will ignore the <c>null</c> or not queued notification message.
    /// </summary>
    /// <param name="message">The message.</param>
    public void Dismiss(INotificationMessage? message)
    {
        if (message == null || !_queuedMessages.Contains(message))
        {
            return;
        }

        _ = _queuedMessages.Remove(message);

        if (message is INotificationAnimation animatableMessage)
        {
            // var animation = animatableMessage.AnimationOut;
            if (
                animatableMessage is { Animates: true })
            {
                animatableMessage.AnimatableElement.DismissAnimation = true;

                _ = Task.Delay(500).ContinueWith(_ => TriggerMessageDismissed(message),
                    OperatingSystem.IsBrowser()
                        ? TaskScheduler.Current
                        : TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                TriggerMessageDismissed(message);
            }
        }
        else
        {
            TriggerMessageDismissed(message);
        }
    }

    /// <summary>
    /// Triggers the message dismissed event.
    /// </summary>
    /// <param name="message">The message.</param>
    private void TriggerMessageDismissed(INotificationMessage message) =>
        OnMessageDismissed.Invoke(this, new NotificationMessageManagerEventArgs(message));
}
