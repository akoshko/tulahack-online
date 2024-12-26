using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Tulahack.UI.Messaging;

public class SelectedPageContextChanged(SelectedPageChangedArgs args)
    : ValueChangedMessage<SelectedPageChangedArgs>(args);