using Cats.Application.UseCases.GetBreeds;
using Cats.Application.UseCases.GetImages;
using Microsoft.AspNetCore.Mvc;

namespace cats.api.server.Controllers;

[ApiController]
[Route("api/cats")]
public class CatsController(GetBreedsHandler getBreeds, GetImagesHandler getImages) : ControllerBase
{
    [HttpGet("breeds")]
    public async Task<IActionResult> Breeds(CancellationToken ct)
        => Ok(await getBreeds.Handle(new GetBreedsQuery(), ct));

    [HttpGet("images")]
    public async Task<IActionResult> Images([FromQuery] string breedId, [FromQuery] int limit = 10, CancellationToken ct = default)
    {
        try
        {
            var result = await getImages.Handle(new GetImagesQuery(breedId, limit), ct);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
