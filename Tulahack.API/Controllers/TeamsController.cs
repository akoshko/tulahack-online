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
public class TeamsController : ControllerBase
{
    private readonly ILogger<TeamsController> _logger;
    private readonly ITeamsService _teamsService;

    public TeamsController(
        ILogger<TeamsController> logger,
        ITeamsService teamsService)
    {
        _logger = logger;
        _teamsService = teamsService;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(TeamDto), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> GetTeam()
    {
        var ssoUserClaim = HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
        Guid.TryParse(ssoUserClaim.Value, out var userId);

        var team = await _teamsService.GetTeamByUserId(userId);

        if (team is null) return NotFound();

        return Ok(team);
    }
    
    [HttpGet("{teamId:guid}")]
    [ProducesResponseType(typeof(TeamDto), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> GetTeam(Guid teamId)
    {
        var ssoUserClaim = HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
        Guid.TryParse(ssoUserClaim.Value, out var userId);

        var team = await _teamsService.GetTeam(teamId);

        if (team is null) return NotFound();

        return Ok(team);
    }
    
    [HttpPost("join")]
    [ProducesResponseType(typeof(TeamDto), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> JoinTeam([FromBody] ContestApplicationDto application)
    {
        var ssoUserClaim = HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
        Guid.TryParse(ssoUserClaim.Value, out var userId);

        var team = await _teamsService.JoinTeam(userId, application);

        if (team is null) return NotFound();

        return Ok(team);
    }
    
    [HttpPost("create")]
    [ProducesResponseType(typeof(TeamDto), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> CreateTeam([FromBody] ContestApplicationDto application)
    {
        var ssoUserClaim = HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
        Guid.TryParse(ssoUserClaim.Value, out var userId);

        var team = await _teamsService.CreateTeam(userId, application);

        if (team is null) return NotFound();

        return Ok(team);
    }
}