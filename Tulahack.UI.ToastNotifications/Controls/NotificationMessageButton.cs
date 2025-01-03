using Avalonia.Controls;

namespace Tulahack.UI.ToastNotifications.Controls;

/// <summary>
/// The notification message button.
/// </summary>
/// <seealso cref="Button" />
/// <seealso cref="INotificationMessageButton" />
public class NotificationMessageButton : Button, INotificationMessageButton
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationMessageButton"/> class.
    /// </summary>
    public NotificationMessageButton() : this(string.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationMessageButton"/> class.
    /// </summary>
    /// <param name="content">The content.</param>
    public NotificationMessageButton(object content)
    {
        Content = content;
    }

    /// <summary>
    /// Called when a <see ref="T:System.Windows.Controls.Button" /> is clicked.
    /// </summary>
    protected override void OnClick()
    {
        base.OnClick();
        Callback(this);
    }

    /// <summary>
    /// Gets or sets the callback.
    /// </summary>
    /// <value>
    /// The callback.
    /// </value>
    public Action<INotificationMessageButton> Callback { get; set; }
}