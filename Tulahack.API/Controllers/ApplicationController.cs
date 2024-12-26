using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tulahack.API.Services;
using Tulahack.API.Utils;
using Tulahack.Dtos;
using Tulahack.Model;

namespace Tulahack.API.Controllers;

[ApiController]
[UserIdActionFilter]
[Authorize(Policy = "Public+")]
[Route("api/[controller]")]
[ProducesResponseType(typeof(Contestant), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(Contestant), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(Contestant), StatusCodes.Status404NotFound)]
public class ApplicationController : ControllerBase
{
    private readonly ILogger<ApplicationController> _logger;
    private readonly IApplicationService _applicationService;

    public ApplicationController(
        ILogger<ApplicationController> logger,
        IApplicationService applicationService)
    {
        _logger = logger;
        _applicationService = applicationService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ContestApplicationDto), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> SubmitApplication([FromBody] ContestApplicationDto application)
    {
        var ssoUserClaim = HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
        Guid.TryParse(ssoUserClaim.Value, out var userId);

        var result = await _applicationService.SubmitApplication(userId, application);

        if (result is null) return NotFound();

        return Ok(result);
    }

    [HttpPatch("{applicationId:guid}/accept")]
    [ProducesResponseType(typeof(ContestApplicationDto), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> AcceptApplication([FromRoute] Guid applicationId, [FromBody] string justification)
    {
        var ssoUserClaim = HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
        Guid.TryParse(ssoUserClaim.Value, out var userId);

        var result = await _applicationService.AcceptApplication(userId, applicationId, justification);

        if (result is null) return NotFound();

        return Ok(result);
    }

    [HttpPatch("{applicationId:guid}/decline")]
    [ProducesResponseType(typeof(ContestApplicationDto), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> DeclineApplication([FromRoute] Guid applicationId, [FromBody] string justification)
    {
        var ssoUserClaim = HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
        Guid.TryParse(ssoUserClaim.Value, out var userId);

        var result = await _applicationService.DeclineApplication(userId, applicationId, justification);

        if (result is null) return NotFound();

        return Ok(result);
    }
}