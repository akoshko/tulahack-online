using System.Threading.Tasks;
using Tulahack.UI.Storage;

namespace Tulahack.UI.ViewModels.Base;

public interface IPersistedViewModel<T> where T : IState
{
    public Task<T?> GetState();
    public void ApplyState(T state);
    Task InitFromStorage();
}
