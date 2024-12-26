using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Tulahack.Model;

namespace Tulahack.API.Controllers;

[ApiController]
[Authorize(Policy = "Public+")]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public class GeoController : ControllerBase
{
    private readonly ILogger<GeoController> _logger;

    public GeoController(ILogger<GeoController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet("GetProvincesList")]
    [ProducesResponseType(typeof(List<Province>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProvincesList()
    {
        JsonSerializer serializer = new JsonSerializer();
        using var stream = new StreamReader("Static/provinces.json");
        await using JsonReader reader = new JsonTextReader(stream);
        var provinces = serializer.Deserialize<List<Province>>(reader);
        return Ok(provinces);
    }
    
    [HttpGet("GetProvinceById/{id}")]
    [ProducesResponseType(typeof(Province), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProvinceById([FromRoute]int id)
    {
        JsonSerializer serializer = new JsonSerializer();
        using var stream = new StreamReader("Static/provinces.json");
        await using JsonReader reader = new JsonTextReader(stream);
        var provinces = serializer.Deserialize<List<Province>>(reader);
        var province = provinces?.FirstOrDefault(item => item.Id == id);
        return Ok(province);
    }
}