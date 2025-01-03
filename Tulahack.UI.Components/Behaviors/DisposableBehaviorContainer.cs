using Tulahack.UI.Components.Utils;

namespace Tulahack.UI.Components.Behaviors;

internal class DisposableBehaviorContainer<T> : IDisposable, ISuspendable
{
    public IStatelessBehavior<T> TheBehavior { get; }
    public T TheObjectTheBehaviorIsAttachedTo { get; }

    public DisposableBehaviorContainer
    (
        IStatelessBehavior<T> behavior,
        T objectTheBehaviorIsAttachedTo
    )
    {
        TheBehavior = behavior;
        TheObjectTheBehaviorIsAttachedTo = objectTheBehaviorIsAttachedTo;

        TheBehavior.Attach(TheObjectTheBehaviorIsAttachedTo);
    }

    public void Suspend() =>
        TheBehavior.Detach(TheObjectTheBehaviorIsAttachedTo, false);

    public void Dispose() =>
        TheBehavior.Detach(TheObjectTheBehaviorIsAttachedTo);

    public void Reset(bool resetItems = true) =>
        TheBehavior.Reset(TheObjectTheBehaviorIsAttachedTo, resetItems);
}