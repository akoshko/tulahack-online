using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Reactive;
using Tulahack.UI.Components.Utils;

namespace Tulahack.UI.Components.Behaviors;

public static class CallAction
{
    #region TheEvent Attached Avalonia Property

    public static RoutedEvent GetTheEvent(AvaloniaObject obj) =>
        obj.GetValue(TheEventProperty);

    public static void SetTheEvent(AvaloniaObject obj, RoutedEvent value) =>
        obj.SetValue(TheEventProperty, value);

    public static readonly AttachedProperty<RoutedEvent> TheEventProperty =
        AvaloniaProperty.RegisterAttached<object, Control, RoutedEvent>
        (
            "TheEvent"
        );

    #endregion TheEvent Attached Avalonia Property


    #region TargetObject Attached Avalonia Property

    public static object GetTargetObject(AvaloniaObject obj) =>
        obj.GetValue(TargetObjectProperty);

    public static void SetTargetObject(AvaloniaObject obj, object value) =>
        obj.SetValue(TargetObjectProperty, value);

    public static readonly AttachedProperty<object> TargetObjectProperty =
        AvaloniaProperty.RegisterAttached<object, Control, object>
        (
            "TargetObject"
        );

    #endregion TargetObject Attached Avalonia Property


    #region MethodName Attached Avalonia Property

    public static string GetMethodName(AvaloniaObject obj) =>
        obj.GetValue(MethodNameProperty);

    public static void SetMethodName(AvaloniaObject obj, string value) =>
        obj.SetValue(MethodNameProperty, value);

    public static readonly AttachedProperty<string> MethodNameProperty =
        AvaloniaProperty.RegisterAttached<object, Control, string>
        (
            "MethodName"
        );

    #endregion MethodName Attached Avalonia Property

    private static void ResetEvent(AvaloniaPropertyChangedEventArgs<RoutedEvent> e)
    {
        var sender = e.Sender as Interactive;

        if (e.OldValue.HasValue)
        {
            RoutedEvent routedEvent = e.OldValue.Value;

            DisconnectEventHandling(sender, routedEvent);
        }

        if (e.NewValue.HasValue)
        {
            RoutedEvent routedEvent = e.NewValue.Value;

            ConnectEventHandling(sender, routedEvent);
        }
    }

    private static void DisconnectEventHandling(Interactive? sender, RoutedEvent routedEvent)
    {
        if (sender == null)
        {
            return;
        }

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (routedEvent != null)
        {
            sender.RemoveHandler(routedEvent, (EventHandler<RoutedEventArgs>)OnEvent);
        }
    }


    private static void ConnectEventHandling(Interactive? sender, RoutedEvent routedEvent)
    {
        if (sender == null)
        {
            return;
        }

        RoutingStrategies? routingStr = GetTheRoutingStrategy(sender);

        RoutingStrategies routingStrategies =
            routingStr ?? RoutingStrategies.Bubble | RoutingStrategies.Direct | RoutingStrategies.Tunnel;

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (routedEvent != null)
        {
            sender.AddHandler
            (
                routedEvent,
                (EventHandler<RoutedEventArgs>)OnEvent,
                routingStrategies);
        }
    }

    #region TheRoutingStrategy Attached Avalonia Property

    public static RoutingStrategies? GetTheRoutingStrategy(AvaloniaObject obj) =>
        obj.GetValue(TheRoutingStrategyProperty);

    public static void SetTheRoutingStrategy(AvaloniaObject obj, RoutingStrategies? value) =>
        obj.SetValue(TheRoutingStrategyProperty, value);

    public static readonly AttachedProperty<RoutingStrategies?> TheRoutingStrategyProperty =
        AvaloniaProperty.RegisterAttached<object, Control, RoutingStrategies?>
        (
            "TheRoutingStrategy",
            RoutingStrategies.Bubble
        );

    #endregion TheRoutingStrategy Attached Avalonia Property


    #region StaticType Attached Avalonia Property

    public static Type GetStaticType(AvaloniaObject obj) =>
        obj.GetValue(StaticTypeProperty);

    public static void SetStaticType(AvaloniaObject obj, Type value) =>
        obj.SetValue(StaticTypeProperty, value);

    public static readonly AttachedProperty<Type> StaticTypeProperty =
        AvaloniaProperty.RegisterAttached<object, Control, Type>
        (
            "StaticType"
        );

    #endregion StaticType Attached Avalonia Property


    private static void OnEvent(object? sender, RoutedEventArgs e)
    {
        if (sender is not Interactive avaloniaObject)
        {
            return;
        }

        var methodName = avaloniaObject.GetValue(MethodNameProperty);

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (methodName == null)
        {
            return;
        }

        // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
        var targetObject = avaloniaObject.GetValue(TargetObjectProperty) ?? avaloniaObject.DataContext;
        if (targetObject == null)
        {
            return;
        }

        Type staticType = GetStaticType(avaloniaObject);
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        var isStatic = staticType != null;

        IEnumerable<object> args = [];

        if (isStatic)
        {
            args = args.Union([targetObject]);
        }

        args = GetHasArg(avaloniaObject) ? args.Union(new[] { GetArg1(avaloniaObject) }) : args.Union(GetArgs(avaloniaObject));

        if (isStatic)
        {
            targetObject = staticType;
        }

        _ = targetObject?.CallMethodExtras(methodName, false, isStatic, args.ToArray());
    }


    #region HasArg Attached Avalonia Property

    public static bool GetHasArg(AvaloniaObject obj) =>
        obj.GetValue(HasArgProperty);

    public static void SetHasArg(AvaloniaObject obj, bool value) =>
        obj.SetValue(HasArgProperty, value);

    public static readonly AttachedProperty<bool> HasArgProperty =
        AvaloniaProperty.RegisterAttached<object, Control, bool>
        (
            "HasArg"
        );

    #endregion HasArg Attached Avalonia Property


    #region Arg1 Attached Avalonia Property

    public static object GetArg1(AvaloniaObject obj) =>
        obj.GetValue(Arg1Property);

    public static void SetArg1(AvaloniaObject obj, object value) =>
        obj.SetValue(Arg1Property, value);

    public static readonly AttachedProperty<object> Arg1Property =
        AvaloniaProperty.RegisterAttached<object, Control, object>
        (
            "Arg1"
        );

    #endregion Arg1 Attached Avalonia Property


    #region Args Attached Avalonia Property

    public static List<object> GetArgs(AvaloniaObject obj) =>
        obj.GetValue(ArgsProperty);

    public static void SetArgs(AvaloniaObject obj, List<object> value) =>
        obj.SetValue(ArgsProperty, value);

    public static readonly AttachedProperty<List<object>> ArgsProperty =
        AvaloniaProperty.RegisterAttached<object, Control, List<object>>
        (
            "Args"
        );

    #endregion Args Attached Avalonia Property

    private static IDisposable? _eventSubscription;
    private static readonly AnonymousObserver<AvaloniaPropertyChangedEventArgs<RoutedEvent>> ResetEventObserver =
        new(ResetEvent);
    private static readonly AnonymousObserver<AvaloniaPropertyChangedEventArgs<RoutingStrategies?>>
        ResetRoutingStrategyObserver = new(ResetRoutingStrategy);

    private static void Init()
    {
        _eventSubscription?.Dispose();
        _eventSubscription = TheEventProperty.Changed.Subscribe(ResetEventObserver);
    }

    private static void ResetRoutingStrategy(AvaloniaPropertyChangedEventArgs<RoutingStrategies?> e)
    {
        if (e.Sender is not Interactive sender)
        {
            return;
        }

        RoutedEvent routedEvent = GetTheEvent(sender);
        DisconnectEventHandling(sender, routedEvent);

        ConnectEventHandling(sender, routedEvent);
    }

    static CallAction()
    {
        Init();
        _ = TheRoutingStrategyProperty.Changed.Subscribe(ResetRoutingStrategyObserver);
    }
}
