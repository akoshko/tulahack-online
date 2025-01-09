using System;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace Tulahack.UI.Utils;

public partial class JsExportedMethods
{
    [JSImport("window.location.origin", "main.js")]
    private static partial string GetLocationOrigin();

    [JSImport("auth.token", "main.js")]
    private static partial string GetJwtToken();

    [JSImport("globalThis.console.log")]
    private static partial void Log([JSMarshalAs<JSType.String>] string message);

    [JSImport("openInNewTab", "main.js")]
    private static partial void OpenInNewTab([JSMarshalAs<JSType.String>] string url);

    [JSImport("getAsync", "main.js")]
    [return: JSMarshalAs<JSType.Promise<JSType.Any>>()]
    private static partial Task<object> GetAsync([JSMarshalAs<JSType.String>] string url);

    [JSImport("postAsync", "main.js")]
    [return: JSMarshalAs<JSType.Promise<JSType.Any>>()]
    private static partial Task<object> PostAsync([JSMarshalAs<JSType.String>] string url,
        [JSMarshalAs<JSType.Array<JSType.Number>>]
        byte[] data);

    [JSImport("putAsync", "main.js")]
    [return: JSMarshalAs<JSType.Promise<JSType.Any>>()]
    private static partial Task<object> PutAsync([JSMarshalAs<JSType.String>] string url,
        [JSMarshalAs<JSType.Array<JSType.Number>>]
        byte[] data);

    [JSImport("patchAsync", "main.js")]
    [return: JSMarshalAs<JSType.Promise<JSType.Any>>()]
    private static partial Task<object> PatchAsync([JSMarshalAs<JSType.String>] string url,
        [JSMarshalAs<JSType.Array<JSType.Number>>]
        byte[] data);

    [JSImport("deleteAsync", "main.js")]
    [return: JSMarshalAs<JSType.Promise<JSType.Any>>()]
    private static partial Task<object> DeleteAsync([JSMarshalAs<JSType.String>] string url);

    public string GetOrigin() => GetLocationOrigin();
    public string GetToken() => GetJwtToken();
    public void LogMessage(string message) => Log(message);
    public void OpenUrl(string url) => OpenInNewTab(url);
    public void OpenUrl(Uri url) => OpenInNewTab(url.ToString());
    public static Task<object> GetAsync(Uri url) => GetAsync(url.ToString());

    public static Task<object> PostAsync(Uri url, string data) =>
        PostAsync(url.ToString(), Encoding.UTF8.GetBytes(data));

    public static Task<object> PutAsync(Uri url, string data) =>
        PutAsync(url.ToString(), Encoding.UTF8.GetBytes(data));

    public static Task<object> PatchAsync(Uri url, string data) =>
        PatchAsync(url.ToString(), Encoding.UTF8.GetBytes(data));

    public static Task<object> DeleteAsync(Uri url) =>
        DeleteAsync(url.ToString());
}
