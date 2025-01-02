using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tulahack.API.Extensions;
using Tulahack.API.Services;
using Tulahack.Model;

namespace Tulahack.API.Controllers;

[ApiController]
[Authorize(Policy = "Public+")]
[Route("api/callback")]
[ProducesResponseType(typeof(Contestant), StatusCodes.Status302Found)]
[ProducesResponseType(typeof(Contestant), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(Contestant), StatusCodes.Status403Forbidden)]
public class CallbackController : ControllerBase
{
    private readonly ILogger<CallbackController> _logger;
    private readonly IAccountService _accountService;

    public CallbackController(
        ILogger<CallbackController> logger,
        IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PersonBase), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register()
    {
        Claim ssoUserClaim = HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
        _ = Guid.TryParse(ssoUserClaim.Value, out Guid userId);

        var jwt = await HttpContext.GetTokenAsync("access_token");
        if (string.IsNullOrEmpty(jwt))
        {
            return Forbid("Incoming JwT token is in invalid format");
        }

        PersonBase? user = await _accountService.GetAccount(userId) ?? await _accountService.CreateAccount(jwt);

        if (user.Role != jwt.GetRole())
        {
            _ = await _accountService.RefreshAccess(jwt);
        }

        return Redirect("https://tulahack.<you-name-it>/index.html");
    }
}
