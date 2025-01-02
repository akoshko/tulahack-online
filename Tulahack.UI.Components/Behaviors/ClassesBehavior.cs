using Avalonia;
using Avalonia.Controls;
using Avalonia.Reactive;

namespace Tulahack.UI.Components.Behaviors;

public static class ClassesBehavior
{
    public static readonly char[] WHITESPACE_CHARS = { ' ', '\n', '\t', '\r' };

    #region TheClasses Attached Avalonia Property
    public static string GetTheClasses(AvaloniaObject obj) =>
        obj.GetValue(TheClassesProperty);

    public static void SetTheClasses(AvaloniaObject obj, string value) =>
        obj.SetValue(TheClassesProperty, value);

    public static readonly AttachedProperty<string> TheClassesProperty =
        AvaloniaProperty.RegisterAttached<object, StyledElement, string>
        (
            "TheClasses"
        );
    #endregion TheClasses Attached Avalonia Property


    #region InsertClasses Attached Avalonia Property
    public static string GetInsertClasses(AvaloniaObject obj) =>
        obj.GetValue(InsertClassesProperty);

    public static void SetInsertClasses(AvaloniaObject obj, string value) =>
        obj.SetValue(InsertClassesProperty, value);

    public static readonly AttachedProperty<string> InsertClassesProperty =
        AvaloniaProperty.RegisterAttached<object, Control, string>
        (
            "InsertClasses"
        );

    private static readonly AnonymousObserver<AvaloniaPropertyChangedEventArgs<string>> OnClassesChangedObserver = new (OnClassesChanged);
    private static readonly AnonymousObserver<AvaloniaPropertyChangedEventArgs<string>> OnInsertClassesChangedObserver = new (OnInsertClassesChanged);

    #endregion InsertClasses Attached Avalonia Property


    static ClassesBehavior()
    {
        TheClassesProperty.Changed.Subscribe(OnClassesChangedObserver);
        InsertClassesProperty.Changed.Subscribe(OnInsertClassesChangedObserver);
    }

    internal static string[] GetClasses(this string classesStr) =>
        classesStr.Split(WHITESPACE_CHARS, StringSplitOptions.RemoveEmptyEntries);

    private static void OnInsertClassesChanged(AvaloniaPropertyChangedEventArgs<string> change)
    {
        string oldClassesStr = change.OldValue.Value;

        StyledElement sender = change.Sender as StyledElement;

        if (oldClassesStr != null)
        {
            var oldClasses = oldClassesStr.GetClasses();
            sender.Classes.RemoveAll(oldClasses);
        }

        string newClassesStr = change.NewValue.Value;

        if (newClassesStr != null)
        {
            var newClasses = newClassesStr.GetClasses();
            sender.Classes.InsertRange(0, newClasses);
        }
    }

    private static void OnClassesChanged(AvaloniaPropertyChangedEventArgs<string> change)
    {
        StyledElement sender = change.Sender as StyledElement;

        string classesStr = change.NewValue.Value;

        if (classesStr != null)
        {
            var classes = classesStr.GetClasses();

            if (classes != null)
            {
                sender.Classes.Replace(classes);
                return;
            }
        }
    }
}