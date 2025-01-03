using System.Web;
using CommunityToolkit.Mvvm.Input;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using Tulahack.UI.ViewModels.Base;

namespace Tulahack.Desktop.Auth;

// https://github.com/IdentityModel/IdentityModel.OidcClient.Samples/tree/main/WindowsConsoleSystemBrowser
public class AuthViewModel : ViewModelBase
{
    private const string PC_Authority = "https://keycloak.<you-name-it>/realms/tulahack";
    private const string PC_ClientId = "tulahack-client";
    private const string PC_Secret = "<TOP_SECRET>";

    private TokenResponse _tokenResponse = new();
    public Action InitApp { get; set; } = () => { };
    public AsyncRelayCommand Auth { get; set; }

    public AuthViewModel()
    {
        Auth = new AsyncRelayCommand(Run);
    }

    private async Task Run() => await Login();

    private async Task Login()
    {
        // create a redirect URI using an available port on the loopback address.
        // requires the OP to allow random ports on 127.0.0.1 - otherwise set a static port
        var browser = new SystemBrowser(30080, string.Empty);
        var redirectUri = $"http://127.0.0.1:{browser.Port}";

        var options = new OidcClientOptions
        {
            Authority = PC_Authority,
            ClientId = PC_ClientId,
            RedirectUri = redirectUri,
            ClientSecret = PC_Secret,
            Scope = "openid email profile",
            FilterClaims = false,
            Browser = browser,
            DisablePushedAuthorization = true
        };

        var oidcClient = new OidcClient(options);

        AuthorizeState authorizeState = await oidcClient.PrepareLoginAsync();
        BrowserResult browserResult = await browser.InvokeAsync(
            new BrowserOptions(authorizeState.StartUrl, string.Empty)
        );

        using var httpClient = new HttpClient();
        var code = HttpUtility.ParseQueryString($"{redirectUri}{browserResult.Response}").Get("code");

        using var request = new AuthorizationCodeTokenRequest();
        request.Address = $"{PC_Authority}/protocol/openid-connect/token";
        request.ClientId = PC_ClientId;
        request.ClientSecret = PC_Secret;
        request.Code = code ?? string.Empty;
        request.RedirectUri = "http://127.0.0.1:30080";
        request.CodeVerifier = authorizeState.CodeVerifier;

        _tokenResponse = await httpClient.RequestAuthorizationCodeTokenAsync(request);

        InitApp.Invoke();
        // To get access_token by refresh_token
        // var refreshResult = await _oidcClient.RefreshTokenAsync(response.RefreshToken);
    }

    public TokenResponse GetAccessToken() => _tokenResponse;
}
