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
    [HttpGet("GetProvincesList")]
    [ProducesResponseType(typeof(List<Province>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProvincesList()
    {
        var serializer = new JsonSerializer();
        using var stream = new StreamReader("Static/provinces.json");
        await using JsonReader reader = new JsonTextReader(stream);
        List<Province>? provinces = serializer.Deserialize<List<Province>>(reader);
        return Ok(provinces);
    }

    [HttpGet("GetProvinceById/{id}")]
    [ProducesResponseType(typeof(Province), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProvinceById([FromRoute] int id)
    {
        var serializer = new JsonSerializer();
        using var stream = new StreamReader("Static/provinces.json");
        await using JsonReader reader = new JsonTextReader(stream);
        List<Province>? provinces = serializer.Deserialize<List<Province>>(reader);
        Province? province = provinces?.FirstOrDefault(item => item.Id == id);
        return Ok(province);
    }
}
