using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tulahack.Desktop.Storage;
using Tulahack.UI.Constants;
using Tulahack.UI.Storage;
using Tulahack.UI.Utils;

namespace Tulahack.Desktop.Extensions;

public class AuthContextProvider : AbstractAuthContextProvider
{
    private readonly TokenResponse _tokenResponse;
    public override string? GetAccessToken() => _tokenResponse.AccessToken;
    public override string GetGroup() => _tokenResponse.AccessToken?.GetGroup() ?? Groups.Public;

    public AuthContextProvider(TokenResponse response)
    {
        _tokenResponse = response;
    }
}

public static class ServiceCollectionExtensions
{
    public static void AddDesktopStorageServices(this IServiceCollection collection) =>
        collection.AddSingleton<IRuntimeStorageProvider<AppState>, DefaultSettingsStore<AppState>>();

    public static void AddDesktopTokenProvider(this IServiceCollection collection, TokenResponse token) =>
        collection.AddSingleton<IAuthContextProvider, AuthContextProvider>(_ => new AuthContextProvider(token));

    public static IConfiguration AddConfiguration(this IServiceCollection collection)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        _ = collection.AddSingleton<IConfiguration>(_ => configuration);
        return configuration;
    }
}