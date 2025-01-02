using Tulahack.UI.Components.Utils;

namespace Tulahack.UI.Components.Behaviors;

public class DoForEachItemCollectionBehavior<TCollItem> :
    IForEachItemCollectionBehavior<TCollItem>
{
    Action<TCollItem>? UnsetItemDelegate { get; }
    Action<TCollItem>? SetItemDelegate { get; }

    public void OnItemAdded(TCollItem item) =>
        SetItemDelegate?.Invoke(item);

    public void OnItemRemoved(TCollItem item) =>
        UnsetItemDelegate?.Invoke(item);

    public DoForEachItemCollectionBehavior
    (
        Action<TCollItem> onAdd,
        Action<TCollItem>? onRemove = null)
    {
        SetItemDelegate = onAdd;
        UnsetItemDelegate = onRemove;
    }

    public void Attach(IEnumerable<TCollItem> obj, bool setItems = true) =>
        throw new NotImplementedException();

    public void Detach(IEnumerable<TCollItem> obj, bool unsetItems = true) =>
        throw new NotImplementedException();
}

public static class DoForEachBehaviorUtils
{
    private static BehaviorsDisposable<IEnumerable<TCollItem>> AddBehaviorImpl<TCollItem>
    (
        this IEnumerable<TCollItem>? collection,
        IStatelessBehavior<IEnumerable<TCollItem>> behavior,
        BehaviorsDisposable<IEnumerable<TCollItem>>? previousBehavior = null)
    {
        if (collection == null)
        {
            return null;
        }

        var behaviorContainer =
            new DisposableBehaviorContainer<IEnumerable<TCollItem>>(behavior, collection);

        var behaviorsDisposable =
            new BehaviorsDisposable<IEnumerable<TCollItem>>(behaviorContainer, previousBehavior);

        return behaviorsDisposable;
    }

    public static BehaviorsDisposable<IEnumerable<TCollItem>> AddBehavior<TCollItem>
    (
        this IEnumerable<TCollItem> collection,
        IStatelessBehavior<IEnumerable<TCollItem>> behavior
    ) =>
        AddBehaviorImpl(collection, behavior);

    public static BehaviorsDisposable<IEnumerable<TCollItem>> AddBehavior<TCollItem>
    (
        this BehaviorsDisposable<IEnumerable<TCollItem>> previousBehavior,
        IStatelessBehavior<IEnumerable<TCollItem>> behavior
    )
    {
        Func<IEnumerable<TCollItem>> collection = previousBehavior.TheObjectTheBehaviorsAreAttachedTo;
        return AddBehaviorImpl(collection, behavior, previousBehavior);
    }

    private static BehaviorsDisposable<IEnumerable<TCollItem>> AddBehaviorImpl<TCollItem>(
        Func<IEnumerable<TCollItem>>? func,
        IStatelessBehavior<IEnumerable<TCollItem>> behavior,
        BehaviorsDisposable<IEnumerable<TCollItem>> previousBehavior)
    {
        if (func == null)
        {
            return null;
        }

        IEnumerable<TCollItem> collection = func.Invoke();
        var behaviorContainer = new DisposableBehaviorContainer<IEnumerable<TCollItem>>(behavior, collection);

        var behaviorsDisposable = new BehaviorsDisposable<IEnumerable<TCollItem>>(behaviorContainer, previousBehavior);

        return behaviorsDisposable;
    }

    private static BehaviorsDisposable<IEnumerable<TCollItem>> AddBehaviorImpl<TCollItem>
    (
        this IEnumerable<TCollItem>? collection,
        DoForEachItemCollectionBehavior<TCollItem> behavior,
        BehaviorsDisposable<IEnumerable<TCollItem>>? previousBehavior = null
    )
    {
        if (collection == null)
        {
            return null;
        }

        var behaviorContainer = new DisposableBehaviorContainer<IEnumerable<TCollItem>>(behavior, collection);

        var behaviorsDisposable = new BehaviorsDisposable<IEnumerable<TCollItem>>(behaviorContainer, previousBehavior);

        return behaviorsDisposable;
    }

    private static BehaviorsDisposable<IEnumerable<TCollItem>> AddBehaviorImpl<TCollItem>
    (
        this IEnumerable<TCollItem> collection,
        Action<TCollItem> onAdd,
        Action<TCollItem>? onRemove = null,
        BehaviorsDisposable<IEnumerable<TCollItem>>? previousBehavior = null
    )
    {
        var behavior =
            new DoForEachItemCollectionBehavior<TCollItem>(onAdd, onRemove);

        return collection?.AddBehaviorImpl(behavior, previousBehavior);
    }

    public static BehaviorsDisposable<IEnumerable<TCollItem>> AddBehavior<TCollItem>
    (
        this IEnumerable<TCollItem> collection,
        DoForEachItemCollectionBehavior<TCollItem> behavior
    ) =>
        collection.AddBehaviorImpl(behavior);

    public static BehaviorsDisposable<IEnumerable<TCollItem>> AddBehavior<TCollItem>
    (
        this IEnumerable<TCollItem> collection,
        Action<TCollItem> onAdd,
        Action<TCollItem>? onRemove = null
    ) =>
        collection.AddBehaviorImpl<TCollItem>(onAdd, onRemove);

    public static BehaviorsDisposable<IEnumerable<TCollItem>> AddBehavior<TCollItem>
    (
        this BehaviorsDisposable<IEnumerable<TCollItem>> previousBehaviors,
        Action<TCollItem> onAdd,
        Action<TCollItem>? onRemove = null
    )
    {
        IEnumerable<TCollItem> collection = previousBehaviors.TheObjectTheBehaviorsAreAttachedTo();

        return collection.AddBehaviorImpl<TCollItem>(onAdd, onRemove, previousBehaviors);
    }
}