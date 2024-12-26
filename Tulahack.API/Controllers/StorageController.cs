using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tulahack.API.Extensions;
using Tulahack.API.Services;
using Tulahack.API.Utils;
using Tulahack.Model;

namespace Tulahack.API.Controllers;

[ApiController]
[UserIdActionFilter]
[Authorize(Policy = "Public+")]
[Route("api/[controller]")]
public class StorageController : ControllerBase
{
    private readonly ILogger<StorageController> _logger;
    private readonly IStorageService _storageService;

    public StorageController(
        ILogger<StorageController> logger,
        IStorageService storageService)
    {
        _logger = logger;
        _storageService = storageService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(StorageFile), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(Guid fileId)
    {
        var result = await _storageService.GetFile(HttpContext.User.GetUserId(), fileId, default);

        if (result is null)
            return NotFound("Cannot find file for provided FileId");

        return Ok(result);
    }

    [HttpGet("file")]
    public async Task<IActionResult> GetFile(Guid fileId)
    {
        var result = await _storageService.GetFile(HttpContext.User.GetUserId(), fileId, default);

        if (result is null)
            return NotFound("Cannot find file for provided FileId");

        var stream = new FileStream(result.Filepath, FileMode.Open, FileAccess.Read);
        return File(stream, MediaTypeNames.Application.Octet, result.Filename);
    }

    [HttpGet("{teamId:guid}/files")]
    [ProducesResponseType(typeof(IEnumerable<StorageFile>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFiles(Guid teamId)
    {
        var result = await _storageService.GetTeamFiles(teamId);

        if (result is null)
            return NotFound("Cannot find any file for provided User");

        return Ok(result);
    }

    [HttpGet("files")]
    [ProducesResponseType(typeof(IEnumerable<StorageFile>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFiles()
    {
        var result = await _storageService.GetFiles(HttpContext.User.GetUserId());

        if (result is null)
            return NotFound("Cannot find any file for provided User");

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(StorageFile), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(IFormFile files, [FromQuery] FilePurposeType purposeType)
    {
        var result = await _storageService.UploadFile(
            files,
            purpose: purposeType,
            userId: HttpContext.User.GetUserId(),
            default);
        return Ok(result);
    }

    [HttpPatch]
    [ProducesResponseType(typeof(StorageFile), StatusCodes.Status200OK)]
    public async Task<IActionResult> Patch(Guid fileId)
    {
        return Ok();
    }

    [HttpDelete]
    [ProducesResponseType(typeof(StorageFile), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete()
    {
        return Ok();
    }
}