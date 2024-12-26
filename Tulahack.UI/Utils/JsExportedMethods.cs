using System.Runtime.InteropServices.JavaScript;

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
    
    public string GetOrigin() => GetLocationOrigin();
    public string GetToken() => GetJwtToken();
    public void LogMessage(string message) => Log(message);
    public void OpenUrl(string url) => OpenInNewTab(url);
}