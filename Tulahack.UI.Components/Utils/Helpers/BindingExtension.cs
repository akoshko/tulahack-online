using Avalonia;
using Avalonia.Data;
using Tulahack.UI.Components.Reactive;

namespace Tulahack.UI.Components.Utils.Helpers;

public static class BindingExtension
{
    public static ResultDisposable TryBind(this AvaloniaObject obj, AvaloniaProperty property, IBinding? binding)
    {
        if (binding is null)
        {
            return new ResultDisposable(new EmptyDisposable(), false);
        }

        return new ResultDisposable(obj.Bind(property, binding), true);

    }
}