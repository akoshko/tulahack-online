using System.Web;
using CommunityToolkit.Mvvm.Input;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using Microsoft.Extensions.Configuration;
using Tulahack.UI.ViewModels.Base;

namespace Tulahack.Desktop.Auth;

// https://github.com/IdentityModel/IdentityModel.OidcClient.Samples/tree/main/WindowsConsoleSystemBrowser
public class AuthViewModel : ViewModelBase
{
    private string? Authority { get; set; }
    private string? AuthorityClientId { get; set; }
    private string? AuthoritySecret { get; set; }

    private TokenResponse _tokenResponse = new();
    public Action InitApp { get; set; } = () => { };
    public AsyncRelayCommand Auth { get; set; }

    public AuthViewModel() : this(null) { }
    public AuthViewModel(IConfiguration? configuration)
    {
        if (configuration is null)
        {
            Auth = new AsyncRelayCommand(Run);
            return;
        }

        Authority = configuration["DesktopConfiguration:Authority"];
        AuthorityClientId = configuration["DesktopConfiguration:AuthorityClientId"];
        AuthoritySecret = configuration["DesktopConfiguration:AuthoritySecret"];

        if (Authority is null || AuthorityClientId is null || AuthoritySecret is null)
        {
            throw new ApplicationException("Authority or ClientId or Secret is null! Configure app settings from appsettings.json");
        }

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
            Authority = Authority,
            ClientId = AuthorityClientId,
            RedirectUri = redirectUri,
            ClientSecret = AuthoritySecret,
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
        request.Address = $"{Authority}/protocol/openid-connect/token";
        request.ClientId = AuthorityClientId!;
        request.ClientSecret = AuthoritySecret;
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
