using System.Threading.Tasks;

namespace Tulahack.UI.Storage;

public interface IRuntimeStorageProvider<T>
{
    Task SaveObject(T obj, string key);
    Task<T?> LoadObject(string key);
}