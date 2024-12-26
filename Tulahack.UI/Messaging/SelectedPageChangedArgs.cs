using System;

namespace Tulahack.UI.Messaging;

public class SelectedPageChangedArgs
{
    public required Type ContextType { get; set; }
    public NavigationArgs? NavigationArgs { get; set; } = null;
}