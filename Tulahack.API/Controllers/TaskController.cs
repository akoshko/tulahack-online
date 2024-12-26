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
public class TaskController : ControllerBase
{
    private readonly ILogger<TaskController> _logger;
    private readonly ITaskService _taskService;

    public TaskController(
        ILogger<TaskController> logger,
        ITaskService taskService)
    {
        _logger = logger;
        _taskService = taskService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PersonBaseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(typeof(PersonBaseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] PersonBaseDto dto)
    {
        return Ok();
    }
    
    [HttpPatch]
    [ProducesResponseType(typeof(PersonBaseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Patch([FromBody] PersonBaseDto dto)
    {
        return Ok();
    }

    [HttpDelete]
    [ProducesResponseType(typeof(PersonBaseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete()
    {
        return Ok();
    }
}