namespace Yerbowo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMemoryCache _memoryCache;

    public HomeController(IMediator mediator,
        IMemoryCache memoryCache)
    {
        _mediator = mediator;
        _memoryCache = memoryCache;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var products = await _memoryCache.GetOrCreateAsync("HomeProducts", async x =>
        {
            return await _mediator.Send(new GetRandomProductsQuery());
        });

        return Ok(products);
    }
}