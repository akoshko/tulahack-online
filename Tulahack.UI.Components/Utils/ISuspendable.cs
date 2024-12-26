namespace Tulahack.UI.Components.Utils
{
    public interface ISuspendable
    {
        void Suspend();

        void Reset(bool resetItems = true);
    }
}
