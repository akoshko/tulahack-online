using Avalonia.Interactivity;
using Tulahack.UI.Components.Reactive;

namespace Tulahack.UI.Components.Utils.Helpers;

public static class RoutedEventExtension
{
    public static void AddHandler<TArgs>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        params Interactive?[] controls)
        where TArgs : RoutedEventArgs
    {
        foreach (Interactive? t in controls)
        {
            t?.AddHandler(routedEvent, handler);
        }
    }

    public static void AddHandler<TArgs, TControl>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        params TControl?[] controls)
        where TControl : Interactive
        where TArgs : RoutedEventArgs
    {
        foreach (TControl? t in controls)
        {
            t?.AddHandler(routedEvent, handler);
        }
    }

    public static void AddHandler<TArgs>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        RoutingStrategies strategies = RoutingStrategies.Bubble | RoutingStrategies.Direct,
        bool handledEventsToo = false,
        params Interactive?[] controls)
        where TArgs : RoutedEventArgs
    {
        foreach (Interactive? t in controls)
        {
            t?.AddHandler(routedEvent, handler, strategies, handledEventsToo);
        }
    }

    public static void AddHandler<TArgs, TControl>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        RoutingStrategies strategies = RoutingStrategies.Bubble | RoutingStrategies.Direct,
        bool handledEventsToo = false,
        params TControl?[] controls)
        where TArgs : RoutedEventArgs
        where TControl : Interactive
    {
        foreach (TControl? t in controls)
        {
            t?.AddHandler(routedEvent, handler, strategies, handledEventsToo);
        }
    }

    public static void AddHandler<TArgs, TControl>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        IEnumerable<TControl?> controls,
        RoutingStrategies strategies = RoutingStrategies.Bubble | RoutingStrategies.Direct,
        bool handledEventsToo = false)
        where TArgs : RoutedEventArgs
        where TControl : Interactive
    {
        foreach (TControl? t in controls)
        {
            t?.AddHandler(routedEvent, handler, strategies, handledEventsToo);
        }
    }

    public static void RemoveHandler<TArgs>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        params Interactive?[] controls)
        where TArgs : RoutedEventArgs
    {
        foreach (Interactive? t in controls)
        {
            t?.RemoveHandler(routedEvent, handler);
        }
    }

    public static void RemoveHandler<TArgs, TControl>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        params TControl?[] controls)
        where TArgs : RoutedEventArgs
        where TControl : Interactive
    {
        foreach (TControl? t in controls)
        {
            t?.RemoveHandler(routedEvent, handler);
        }
    }

    public static void RemoveHandler<TArgs, TControl>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        IEnumerable<TControl?> controls)
        where TArgs : RoutedEventArgs
        where TControl : Interactive
    {
        foreach (TControl? t in controls)
        {
            t?.RemoveHandler(routedEvent, handler);
        }
    }

    public static IDisposable AddDisposableHandler<TArgs>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        params Interactive?[] controls)
        where TArgs : RoutedEventArgs
    {
        var list = new List<IDisposable?>(controls.Length);

        foreach (Interactive? t in controls)
        {
            IDisposable? disposable = t?.AddDisposableHandler(routedEvent, handler);

            if (disposable != null)
            {
                list.Add(disposable);
            }
        }

        var result = new ReadonlyDisposableCollection(list);
        return result;
    }

    public static IDisposable AddDisposableHandler<TArgs, TControl>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        params TControl?[] controls)
        where TArgs : RoutedEventArgs
        where TControl : Interactive
    {
        var list = new List<IDisposable?>(controls.Length);

        foreach (TControl? t in controls)
        {
            IDisposable? disposable = t?.AddDisposableHandler(routedEvent, handler);

            if (disposable != null)
            {
                list.Add(disposable);
            }
        }

        var result = new ReadonlyDisposableCollection(list);
        return result;
    }

    public static IDisposable AddDisposableHandler<TArgs>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        RoutingStrategies strategies = RoutingStrategies.Bubble | RoutingStrategies.Direct,
        bool handledEventsToo = false,
        params Interactive?[] controls)
        where TArgs : RoutedEventArgs
    {
        var list = new List<IDisposable?>(controls.Length);

        foreach (Interactive? t in controls)
        {
            IDisposable? disposable = t?.AddDisposableHandler(routedEvent, handler, strategies, handledEventsToo);

            if (disposable != null)
            {
                list.Add(disposable);
            }
        }

        var result = new ReadonlyDisposableCollection(list);
        return result;
    }

    public static IDisposable AddDisposableHandler<TArgs, TControl>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        RoutingStrategies strategies = RoutingStrategies.Bubble | RoutingStrategies.Direct,
        bool handledEventsToo = false,
        params TControl?[] controls)
        where TArgs : RoutedEventArgs
        where TControl : Interactive
    {
        var list = new List<IDisposable?>(controls.Length);

        foreach (TControl? t in controls)
        {
            IDisposable? disposable = t?.AddDisposableHandler(routedEvent, handler, strategies, handledEventsToo);

            if (disposable != null)
            {
                list.Add(disposable);
            }
        }

        var result = new ReadonlyDisposableCollection(list);
        return result;
    }

    public static IDisposable AddDisposableHandler<TArgs, TControl>(
        this RoutedEvent<TArgs> routedEvent,
        EventHandler<TArgs> handler,
        IEnumerable<TControl> controls,
        RoutingStrategies strategies = RoutingStrategies.Bubble | RoutingStrategies.Direct,
        bool handledEventsToo = false)
        where TArgs : RoutedEventArgs
        where TControl : Interactive
    {
        // list is not initialized with controls.Count() to avoid multiple enumeration
        var list = new List<IDisposable?>();

        foreach (TControl t in controls)
        {
            // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
            IDisposable? disposable = t?.AddDisposableHandler(routedEvent, handler, strategies, handledEventsToo);

            if (disposable != null)
            {
                list.Add(disposable);
            }
        }

        var result = new ReadonlyDisposableCollection(list);
        return result;
    }
}
