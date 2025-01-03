using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Tulahack.UI.Components.Utils;

namespace Tulahack.UI.Components.Behaviors.DataGridBehaviors;

public static class DataGridFilteringBehavior
{
    #region ColumnFilterText Attached Avalonia Property

    public static string GetColumnFilterText(DataGridColumnHeader obj) =>
        obj.GetValue(ColumnFilterTextProperty);

    public static void SetColumnFilterText(DataGridColumnHeader obj, string value) =>
        obj.SetValue(ColumnFilterTextProperty, value);

    public static readonly AttachedProperty<string> ColumnFilterTextProperty =
        AvaloniaProperty.RegisterAttached<DataGridColumnHeader, DataGridColumnHeader, string>
        (
            "ColumnFilterText"
        );

    #endregion ColumnFilterText Attached Avalonia Property


    #region FilterPropName Attached Avalonia Property

    public static string GetFilterPropName(DataGridColumn obj) =>
        obj.GetValue(FilterPropNameProperty);

    public static void SetFilterPropName(DataGridColumn obj, string value) =>
        obj.SetValue(FilterPropNameProperty, value);

    public static readonly AttachedProperty<string> FilterPropNameProperty =
        AvaloniaProperty.RegisterAttached<DataGridColumn, DataGridColumn, string>
        (
            "FilterPropName"
        );

    #endregion FilterPropName Attached Avalonia Property


    #region ColumnPropGetter Attached Avalonia Property

    public static Func<object, object>? GetColumnPropGetter(DataGridColumn obj) =>
        obj.GetValue(ColumnPropGetterProperty);

    public static void SetColumnPropGetter(DataGridColumn obj, Func<object, object>? value) =>
        obj.SetValue(ColumnPropGetterProperty, value);

    public static readonly AttachedProperty<Func<object, object>?> ColumnPropGetterProperty =
        AvaloniaProperty.RegisterAttached<DataGridColumn, DataGridColumn, Func<object, object>?>
        (
            "ColumnPropGetter"
        );

    #endregion ColumnPropGetter Attached Avalonia Property


    #region RowDataType Attached Avalonia Property

    public static Type GetRowDataType(DataGrid obj) =>
        obj.GetValue(RowDataTypeProperty);

    public static void SetRowDataType(DataGrid obj, Type value) =>
        obj.SetValue(RowDataTypeProperty, value);

    public static readonly AttachedProperty<Type> RowDataTypeProperty =
        AvaloniaProperty.RegisterAttached<DataGrid, DataGrid, Type>
        (
            "RowDataType"
        );

    #endregion RowDataType Attached Avalonia Property


    #region HasFilters Attached Avalonia Property

    public static bool GetHasFilters(DataGrid obj) =>
        obj.GetValue(HasFiltersProperty);

    public static void SetHasFilters(DataGrid obj, bool value) =>
        obj.SetValue(HasFiltersProperty, value);

    public static readonly AttachedProperty<bool> HasFiltersProperty =
        AvaloniaProperty.RegisterAttached<DataGrid, DataGrid, bool>
        (
            "HasFilters",
            true
        );

    #endregion HasFilters Attached Avalonia Property


    static DataGridFilteringBehavior()
    {
        _ = ColumnFilterTextProperty.Changed.Subscribe(OnColumnFilterTextChanged);
        _ = FilterPropNameProperty.Changed.Subscribe(OnFilterPropNameChanged);
        _ = RowDataTypeProperty.Changed.Subscribe(OnRowDataTypeChanged);
    }

    private static void OnRowDataTypeChanged(AvaloniaPropertyChangedEventArgs<Type> obj)
    {
        var dataGrid = (DataGrid)obj.Sender;
        dataGrid.Columns.CollectionChanged += Columns_CollectionChanged;
        SetColumnPropGettersFromPropNames(dataGrid);
    }

    private static void Columns_CollectionChanged(object? sender,
        System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (DataGridColumn col in e.NewItems.Cast<DataGridColumn>())
            {
                SetColumnPropGetterFromPropName(col, GetFilterPropName(col));
            }
        }
    }

    private static void SetColumnPropGettersFromPropNames(DataGrid dataGrid)
    {
        foreach (DataGridColumn? col in dataGrid.Columns)
        {
            SetColumnPropGetterFromPropName(col, GetFilterPropName(col));
        }
    }

    private static void OnFilterPropNameChanged(AvaloniaPropertyChangedEventArgs<string> obj)
    {
        var col = (DataGridColumn)obj.Sender;

        var propName = obj.NewValue.Value;

        SetColumnPropGetterFromPropName(col, propName);
    }

    public static void SetColumnPropGetterFromPropName(DataGridColumn col, string? propName)
    {
        if (propName == null)
        {
            SetColumnPropGetter(col, null);
        }
        else
        {
            DataGrid dataGrid = col.GetPropValue<DataGrid>("OwningGrid", true);

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (dataGrid == null)
            {
                return;
            }

            Type rowType = GetRowDataType(dataGrid);

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (rowType == null)
            {
                return;
            }

            Func<object, object> propGetter = rowType.GetUntypedCsPropertyGetterByObjType(propName);

            SetColumnPropGetter(col, propGetter);
        }
    }

    private static void OnColumnFilterTextChanged(AvaloniaPropertyChangedEventArgs<string> args)
    {
        var header = (DataGridColumnHeader)args.Sender;

        var dataGrid = (DataGrid)header.GetPropValue("OwningGrid", true);

        BuildFilter(dataGrid);
    }


    public static void BuildFilter(DataGrid dataGrid)
    {
        var collectionView = (DataGridCollectionView)dataGrid.ItemsSource;

        var colFilters = new List<Func<object, bool>>();

        foreach (DataGridColumn column in dataGrid.Columns)
        {
            Func<object, object> columnPropGetter = GetColumnPropGetter(column);

            if (columnPropGetter == null)
            {
                continue;
            }

            DataGridColumnHeader columnHeader =
                column.GetPropValue<DataGridColumnHeader>("HeaderCell", true);

            var filterVal = GetColumnFilterText(columnHeader);

            if (string.IsNullOrEmpty(filterVal))
            {
                continue;
            }

            Func<object, bool> colFilter =
                obj => columnPropGetter(obj).ToString()?.Contains(filterVal, StringComparison.OrdinalIgnoreCase) ??
                       false;

            colFilters.Add(colFilter);
        }

        collectionView.Filter = colFilters.Count == 0 ? null : obj => colFilters.All(f => f(obj));
    }

    #region DataGridFilterTextBoxClasses Attached Avalonia Property

    public static string GetDataGridFilterTextBoxClasses(DataGrid obj) =>
        obj.GetValue(DataGridFilterTextBoxClassesProperty);

    public static void SetDataGridFilterTextBoxClasses(DataGrid obj, string value) =>
        obj.SetValue(DataGridFilterTextBoxClassesProperty, value);

    public static readonly AttachedProperty<string> DataGridFilterTextBoxClassesProperty =
        AvaloniaProperty.RegisterAttached<DataGrid, DataGrid, string>
        (
            "DataGridFilterTextBoxClasses"
        );

    #endregion DataGridFilterTextBoxClasses Attached Avalonia Property
}
