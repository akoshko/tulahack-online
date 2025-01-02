using Avalonia.Reactive;

namespace Tulahack.UI.Components.Utils.Helpers;

public static class ObservableExtension
{
    public static IDisposable Subscribe<T>(this IObservable<T> observable, Action<T> action) =>
        observable.Subscribe(new AnonymousObserver<T>(action));
}