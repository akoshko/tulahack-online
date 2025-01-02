using System.Collections;
using System.Collections.Specialized;
using Tulahack.UI.Components.Utils;

namespace Tulahack.UI.Components.Behaviors;

public interface IForEachItemCollectionBehavior<TCollItem> :
    ICollectionStatelessBehavior<TCollItem>
{
    private void SetItems(IEnumerable? items)
    {
        if (items == null)
        {
            return;
        }

        foreach (TCollItem item in items)
        {
            if (item is { } behaviorItem)
            {
                OnItemAdded(behaviorItem);
            }
        }
    }

    private void UnsetItems(IEnumerable? items)
    {
        if (items == null)
        {
            return;
        }

        foreach (TCollItem? item in items)
        {
            if (item is { } behaviorItem)
            {
                OnItemRemoved(behaviorItem);
            }
        }
    }


    private void Collection_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            UnsetItems(e.OldItems);
        }

        if (e.NewItems != null)
        {
            SetItems(e.NewItems);
        }
    }

    void DetachImpl(IEnumerable<TCollItem>? collection, bool unsetItems = true)
    {
        if (collection == null)
        {
            return;
        }

        if (collection is INotifyCollectionChanged notifiableCollection)
        {
            notifiableCollection.CollectionChanged -= Collection_CollectionChanged;
        }

        if (unsetItems)
        {
            UnsetItems(collection.ToList());
        }
    }

    public new void Detach(IEnumerable<TCollItem> collection, bool unsetItems) =>
        DetachImpl(collection, unsetItems);

    void AttachImpl(IEnumerable<TCollItem>? collection, bool setItems = true)
    {
        if (collection == null)
        {
            return;
        }

        if (setItems)
        {
            SetItems(collection);
        }


        if (collection is INotifyCollectionChanged notifiableCollection)
        {
            notifiableCollection.CollectionChanged += Collection_CollectionChanged;
        }
    }

    public new void Attach(IEnumerable<TCollItem> collection, bool setItems) =>
        AttachImpl(collection, setItems);
}