using System.Threading.Tasks;
using Tulahack.UI.Storage;

namespace Tulahack.UI.ViewModels.Base;

public abstract partial class PersistedViewModelBase<T>(IRuntimeStorageProvider<T> storageProvider)
    : ViewModelBase, IPersistedViewModel<T> where T: class, IState, new()
{
    private IRuntimeStorageProvider<T> StorageProvider { get; set; } = storageProvider;
    private T? ViewModelState { get; set; }

    public virtual Task<T?> GetState() =>
        StorageProvider.LoadObject(nameof(T));

    public virtual void ApplyState(T state) =>
        ViewModelState = state;

    public async virtual Task InitFromStorage()
    {
        var state = await StorageProvider.LoadObject(nameof(T)) ?? new T();
        ViewModelState = state;
    }
}