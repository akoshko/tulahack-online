using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tulahack.API.Services;
using Tulahack.Model;

namespace Tulahack.API.Controllers;

[ApiController]
[Authorize(Policy = "Expert+")]
[Route("api/[controller]")]
[ProducesResponseType(typeof(Contestant), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(Contestant), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(Contestant), StatusCodes.Status404NotFound)]
public class AssessmentController : ControllerBase
{
    private readonly ILogger<AssessmentController> _logger;
    private readonly IAccountService _accountService;

    public AssessmentController(
        ILogger<AssessmentController> logger,
        IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PersonBase), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUser()
    {
        var ssoUserClaim = HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
        Guid.TryParse(ssoUserClaim.Value, out var userId);

        var user = await _accountService.GetAccount(userId);

        if (user is null) return NotFound();

        return Ok(user);
    }
}