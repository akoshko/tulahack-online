using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Text.Json;
using Tulahack.UI.Storage;
using Tulahack.UI.Utils;

namespace Tulahack.Desktop.Storage;

public class DefaultSettingsStore<T> : IRuntimeStorageProvider<T>
{
    private static string Identifier { get; } = typeof(T).FullName?.Replace(".", string.Empty) ?? "default";

        
    /// <inheritdoc />
    public async Task SaveObject(T obj, string key)
    {
        try
        {
            // Get a new isolated store for this user, domain, and assembly.
            using var isoStore = IsolatedStorageFile.GetUserStoreForApplication();

            //  Create data stream.
            await using var isoStream = isoStore.OpenFile(Identifier + key, FileMode.CreateNew, FileAccess.Write);
            await JsonSerializer.SerializeAsync(isoStream, obj, typeof(T), TulahackJsonContext.Default);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }

    /// <inheritdoc />
    public async Task<T?> LoadObject(string key)
    {
        try
        {
            using var isoStore = IsolatedStorageFile.GetUserStoreForApplication();
            await using var isoStream = isoStore.OpenFile(Identifier + key, FileMode.Open, FileAccess.Read);
            var storedObj = (T?)await JsonSerializer.DeserializeAsync(isoStream, typeof(T), TulahackJsonContext.Default);
            return storedObj ?? default;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }

        return default;
    }

}