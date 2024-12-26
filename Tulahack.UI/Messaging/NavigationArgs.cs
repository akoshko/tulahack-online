using Tulahack.UI.ViewModels;

namespace Tulahack.UI.Messaging;

public class NavigationArgs
{
    public required IPageContext Sender { get; set; }
    public object? Args { get; set; }
}