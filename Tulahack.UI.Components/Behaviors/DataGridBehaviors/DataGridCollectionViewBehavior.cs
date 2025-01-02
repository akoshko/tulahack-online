using System.Collections;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;

namespace Tulahack.UI.Components.Behaviors.DataGridBehaviors;

public static class DataGridCollectionViewBehavior
{
    #region ItemsSource Attached Avalonia Property

    public static IEnumerable GetItemsSource(DataGrid obj) =>
        obj.GetValue(ItemsSourceProperty);

    public static void SetItemsSource(DataGrid obj, IEnumerable value) =>
        obj.SetValue(ItemsSourceProperty, value);

    public static readonly AttachedProperty<IEnumerable> ItemsSourceProperty =
        AvaloniaProperty.RegisterAttached<DataGrid, DataGrid, IEnumerable>
        (
            "ItemsSource"
        );

    #endregion ItemsSource Attached Avalonia Property

    static DataGridCollectionViewBehavior()
    {
        _ = ItemsSourceProperty.Changed.Subscribe(OnItemsSourcePropertyChanged);
    }

    private static void OnItemsSourcePropertyChanged(AvaloniaPropertyChangedEventArgs<IEnumerable> args)
    {
        var dataGrid = (DataGrid)args.Sender;

        IEnumerable itemsSource = args.NewValue.Value;

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        dataGrid.ItemsSource = itemsSource == null ? null : new DataGridCollectionView(itemsSource);
    }
}
