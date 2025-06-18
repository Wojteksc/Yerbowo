namespace Yerbowo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController(
    IRequestDispatcher dispatcher,
    IMemoryCache memoryCache) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<RandomProductsDto>> Get()
    {
        var products = await memoryCache.GetOrCreateAsync("HomeProducts", async x =>
        {
            return await dispatcher.ExecuteQuery(new GetRandomProductsQuery());
        });

        return Ok(products);
    }
}