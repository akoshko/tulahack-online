﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Threading.Tasks;
using Tulahack.UI.Storage;
using Tulahack.UI.Utils;

namespace Tulahack.Web.Storage;

public partial class BrowserSettingsStore<T> : IRuntimeStorageProvider<T>
{
    [JSImport("globalThis.localStorage.setItem")]
    private static partial void SetItem(string key, string value);

    [JSImport("globalThis.localStorage.getItem")]
    private static partial string GetItem(string key);

    private static string Identifier { get; } = typeof(T).FullName?.Replace(".", string.Empty) ?? "default";

    /// <inheritdoc />
    public Task SaveObject(T obj, string key)
    {
        var serializedObjJson = JsonSerializer.Serialize(obj, typeof(T), TulahackJsonContext.Default);

        SetItem(Identifier + key, serializedObjJson);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<T?> LoadObject(string key)
    {
        try
        {
            var t = GetItem(Identifier + key);

            if (string.IsNullOrEmpty(t))
            {
                return Task.FromResult<T?>(default);
            }

            var x = (T?)JsonSerializer.Deserialize(t, typeof(T), TulahackJsonContext.Default);
            return Task.FromResult(x ?? default);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }

        return Task.FromResult<T?>(default);
    }
}
