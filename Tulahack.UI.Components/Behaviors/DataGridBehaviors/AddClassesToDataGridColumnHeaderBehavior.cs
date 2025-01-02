using Avalonia;
using Avalonia.Controls;
using Tulahack.UI.Components.Utils;

namespace Tulahack.UI.Components.Behaviors.DataGridBehaviors;

public static class AddClassesToDataGridColumnHeaderBehavior
{
    #region TheClassesToAdd Attached Avalonia Property

    public static string GetTheClassesToAdd(DataGrid obj) =>
        obj.GetValue(TheClassesToAddProperty);

    public static void SetTheClassesToAdd(DataGrid obj, string value) =>
        obj.SetValue(TheClassesToAddProperty, value);

    public static readonly AttachedProperty<string> TheClassesToAddProperty =
        AvaloniaProperty.RegisterAttached<DataGrid, DataGrid, string>
        (
            "TheClassesToAdd"
        );

    #endregion TheClassesToAdd Attached Avalonia Property

    static AddClassesToDataGridColumnHeaderBehavior()
    {
        _ = TheClassesToAddProperty.Changed.Subscribe(OnClassesToAddChanged);
    }


    #region TheBehavior Attached Avalonia Property

    private static BehaviorsDisposable<IEnumerable<DataGridColumn>> GetTheBehavior(DataGrid obj) =>
        obj.GetValue(TheBehaviorProperty);

    private static void SetTheBehavior(DataGrid obj, BehaviorsDisposable<IEnumerable<DataGridColumn>> value) =>
        obj.SetValue(TheBehaviorProperty, value);

    private static readonly AttachedProperty<BehaviorsDisposable<IEnumerable<DataGridColumn>>> TheBehaviorProperty =
        AvaloniaProperty.RegisterAttached<DataGrid, DataGrid, BehaviorsDisposable<IEnumerable<DataGridColumn>>>
        (
            "TheBehavior"
        );

    #endregion TheBehavior Attached Avalonia Property


    private static void OnClassesToAddChanged(AvaloniaPropertyChangedEventArgs<string> args)
    {
        var dataGrid = (DataGrid)args.Sender;

        using BehaviorsDisposable<IEnumerable<DataGridColumn>> behavior = dataGrid.Columns.AddBehavior(OnColumnAdded);
        SetTheBehavior(dataGrid, behavior);
    }

    private static void OnColumnAdded(DataGridColumn col)
    {
        DataGrid dataGrid = col.GetPropValue<DataGrid>("OwningGrid", true);
        DataGridColumnHeader header = col.GetPropValue<DataGridColumnHeader>("HeaderCell", true);

        var classesToAdd = GetTheClassesToAdd(dataGrid);

        if (!string.IsNullOrEmpty(classesToAdd))
        {
            header.Classes.AddRange(classesToAdd.GetClasses());
        }
    }
}
