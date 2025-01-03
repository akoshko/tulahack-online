using System.Collections.ObjectModel;

namespace Tulahack.UI.Components.Reactive;

internal class ReadonlyDisposableCollection(IList<IDisposable?> list) : ReadOnlyCollection<IDisposable?>(list), IDisposable
{
    private readonly IList<IDisposable?> _list = list;

    public void Dispose()
    {
        foreach (IDisposable? item in _list)
        {
            item?.Dispose();
        }
    }
}