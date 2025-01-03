using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tulahack.API.Extensions;
using Tulahack.API.Services;
using Tulahack.API.Utils;
using Tulahack.Dtos;

namespace Tulahack.API.Controllers;

[ApiController]
[UserIdActionFilter]
[Authorize(Policy = "Public+")]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(
        IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PersonBaseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        Model.PersonBase? user = await _accountService.GetAccount(HttpContext.User.GetUserId());
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpGet("contestant")]
    [ProducesResponseType(typeof(ContestantDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetContestant()
    {
        ContestantDto? user = await _accountService.GetContestantDetails(HttpContext.User.GetUserId());
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpGet("expert")]
    [ProducesResponseType(typeof(ExpertDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExpert()
    {
        ExpertDto? user = await _accountService.GetExpertDetails(HttpContext.User.GetUserId());
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpGet("moderator")]
    [ProducesResponseType(typeof(ModeratorDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetModerator()
    {
        ModeratorDto? user = await _accountService.GetModeratorDetails(HttpContext.User.GetUserId());
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPatch]
    [ProducesResponseType(typeof(PersonBaseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Patch([FromBody] PersonBaseDto dto)
    {
        PersonBaseDto? user = await _accountService.UpdateAccount(dto);
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPatch("contestant")]
    [ProducesResponseType(typeof(ContestantDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> PatchContestant([FromBody] ContestantDto dto)
    {
        ContestantDto? user = await _accountService.UpdateContestant(dto);
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPatch("expert")]
    [ProducesResponseType(typeof(ExpertDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> PatchExpert([FromBody] ExpertDto dto)
    {
        ExpertDto? user = await _accountService.UpdateExpert(dto);
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPatch("moderator")]
    [ProducesResponseType(typeof(ModeratorDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> PatchModerator([FromBody] ModeratorDto dto)
    {
        ModeratorDto? user = await _accountService.UpdateModerator(dto);
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PersonBaseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] PersonBaseDto dto)
    {
        PersonBaseDto? user = await _accountService.CreateAccount(dto);
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost("contestant")]
    [ProducesResponseType(typeof(ContestantDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateContestant([FromBody] ContestantDto dto)
    {
        ContestantDto? user = await _accountService.CreateContestant(dto);
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost("expert")]
    [ProducesResponseType(typeof(ExpertDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateExpert([FromBody] ExpertDto dto)
    {
        ExpertDto? user = await _accountService.CreateExpert(dto);
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost("moderator")]
    [ProducesResponseType(typeof(ModeratorDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateModerator([FromBody] ModeratorDto dto)
    {
        ModeratorDto? user = await _accountService.CreateModerator(dto);
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpDelete]
    [ProducesResponseType(typeof(PersonBaseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete()
    {
        Model.PersonBase? user = await _accountService.DeleteAccount(HttpContext.User.GetUserId());
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }
}