namespace Tulahack.UI.Components.Utils
{
    public interface IStatelessBehavior<in T>
    {
        void Attach(T obj, bool setItems = true);

        void Detach(T obj, bool unsetItems = true);
    }

    public interface ICollectionItemBehavior<in T>
    {
        protected void OnItemAdded(T item);
        protected void OnItemRemoved(T item);
    }

    public interface ICollectionStatelessBehavior<in T> : IStatelessBehavior<IEnumerable<T>>, ICollectionItemBehavior<T>
    {

    }

    public static class StatelessBehaviorUtils
    {
        public static void Reset<T>(this IStatelessBehavior<T> behavior, T obj, bool resetItems = true)
        {
            behavior.Detach(obj, resetItems);
            behavior.Attach(obj, resetItems);
        }
    }
}
