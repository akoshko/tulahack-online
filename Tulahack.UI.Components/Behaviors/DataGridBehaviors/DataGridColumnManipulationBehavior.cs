﻿using Avalonia;
using Avalonia.Controls;
using Tulahack.UI.Components.Utils;

namespace Tulahack.UI.Components.Behaviors.DataGridBehaviors;

public static class DataGridColumnManipulationBehavior
{
    #region IsOn Attached Avalonia Property
    public static bool GetIsOn(DataGrid obj) =>
        obj.GetValue(IsOnProperty);

    public static void SetIsOn(DataGrid obj, bool value) =>
        obj.SetValue(IsOnProperty, value);

    public static readonly AttachedProperty<bool> IsOnProperty =
        AvaloniaProperty.RegisterAttached<DataGrid, DataGrid, bool>
        (
            "IsOn"
        );
    #endregion IsOn Attached Avalonia Property


    #region ColumnManipulationBehavior Attached Avalonia Property
    public static BehaviorsDisposable<IEnumerable<DataGridColumn>> GetColumnManipulationBehavior(DataGrid obj) =>
        obj.GetValue(ColumnManipulationBehaviorProperty);

    public static void SetColumnManipulationBehavior(DataGrid obj, BehaviorsDisposable<IEnumerable<DataGridColumn>> value) =>
        obj.SetValue(ColumnManipulationBehaviorProperty, value);

    public static readonly AttachedProperty<BehaviorsDisposable<IEnumerable<DataGridColumn>>> ColumnManipulationBehaviorProperty =
        AvaloniaProperty.RegisterAttached<DataGrid, DataGrid, BehaviorsDisposable<IEnumerable<DataGridColumn>>>
        (
            "ColumnManipulationBehavior"
        );
    #endregion ColumnManipulationBehavior Attached Avalonia Property


    static DataGridColumnManipulationBehavior()
    {
        IsOnProperty.Changed.Subscribe(OnIsOnChanged);
    }

    private static void OnIsOnChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        DataGrid dataGrid = (DataGrid)args.Sender;

        if (args.NewValue.Value)
        {
            SetColumnManipulationBehavior(dataGrid, dataGrid.Columns.AddBehavior(OnColumnAdded));
        }
    }

    private static void OnColumnAdded(DataGridColumn col)
    {
        DataGridColumnHeader header = col.GetPropValue<DataGridColumnHeader>("HeaderCell", true);

        SetColumn(header, col);
    }

    #region Column Attached Avalonia Property
    public static DataGridColumn GetColumn(DataGridColumnHeader obj) =>
        obj.GetValue(ColumnProperty);

    public static void SetColumn(DataGridColumnHeader obj, DataGridColumn value) =>
        obj.SetValue(ColumnProperty, value);

    public static readonly AttachedProperty<DataGridColumn> ColumnProperty =
        AvaloniaProperty.RegisterAttached<DataGridColumnHeader, DataGridColumnHeader, DataGridColumn>
        (
            "Column"
        );
    #endregion Column Attached Avalonia Property


    #region CanRemoveColumn Attached Avalonia Property
    public static bool GetCanRemoveColumn(DataGridColumn column) =>
        column.GetValue(CanRemoveColumnProperty);

    public static void SetCanRemoveColumn(DataGridColumn column, bool value) =>
        column.SetValue(CanRemoveColumnProperty, value);

    public static readonly AttachedProperty<bool> CanRemoveColumnProperty =
        AvaloniaProperty.RegisterAttached<DataGridColumn, DataGridColumn, bool>
        (
            "CanRemoveColumn",
            true
        );
    #endregion CanRemoveColumn Attached Avalonia Property


    public static void RemoveColumn(this DataGridColumn column) =>
        column.IsVisible = false;
}