using Tulahack.UI.Components.Utils;

namespace Tulahack.UI.Components.Behaviors;

public interface ISuspendableDisposable : ISuspendable, IDisposable { }

// used to dispose of behaviors
public class BehaviorsDisposable<T> : ISuspendableDisposable
{
    readonly List<DisposableBehaviorContainer<T>> _disposableBehaviors = new();

    public T TheObjectTheBehaviorsAreAttachedTo()
    {
        if (_disposableBehaviors.LastOrDefault() is null)
        {
            return default;
        }

        return _disposableBehaviors.LastOrDefault()!.TheObjectTheBehaviorIsAttachedTo;
    }

    internal BehaviorsDisposable
    (
        DisposableBehaviorContainer<T> disposableBehaviorToAdd,
        BehaviorsDisposable<T>? previousBehavior = null
    )
    {
        if (previousBehavior != null)
        {
            _disposableBehaviors.AddRange(previousBehavior._disposableBehaviors);
        }

        _disposableBehaviors.Add(disposableBehaviorToAdd);
    }

    public void Reset(bool resetItems = true)
    {
        foreach (DisposableBehaviorContainer<T> behaviorContainer in _disposableBehaviors)
        {
            behaviorContainer.Reset(resetItems);
        }
    }

    public void Suspend()
    {
        foreach (DisposableBehaviorContainer<T> behaviorContainer in _disposableBehaviors)
        {
            behaviorContainer.Suspend();
        }
    }

    public void Dispose()
    {
        foreach (DisposableBehaviorContainer<T> behaviorContainer in _disposableBehaviors)
        {
            behaviorContainer.Dispose();
        }
    }
}
