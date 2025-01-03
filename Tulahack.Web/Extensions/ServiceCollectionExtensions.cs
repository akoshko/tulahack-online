using Microsoft.Extensions.DependencyInjection;
using Tulahack.UI.Storage;
using Tulahack.UI.Utils;

namespace Tulahack.Web.Extensions;

public class AuthContextProvider : AbstractAuthContextProvider
{
    private readonly string _accessToken;
    public override string GetAccessToken() => _accessToken;
    public override string GetGroup() => _accessToken.GetGroup();

    public AuthContextProvider(string accessToken)
    {
        _accessToken = accessToken;
    }
}

public static class ServiceCollectionExtensions
{
    public static void AddBrowserStorageServices(this IServiceCollection collection) =>
        collection.AddSingleton<IRuntimeStorageProvider<AppState>, Storage.BrowserSettingsStore<AppState>>();

    public static IAuthContextProvider AddBrowserAuthProvider(this IServiceCollection collection)
    {
        var extensions = new JsExportedMethods();

        extensions.LogMessage("Init AddTokenProvider");
        var token = extensions.GetToken();
        extensions.LogMessage($"Fetched token {token[..6]}...");

        var provider = new AuthContextProvider(token);
        _ = collection.AddSingleton<IAuthContextProvider, AuthContextProvider>(_ => provider);

        extensions.LogMessage("Done!");
        return provider;
    }
}