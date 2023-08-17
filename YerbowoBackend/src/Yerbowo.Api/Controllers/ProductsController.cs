namespace Yerbowo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ApiControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public ProductsController(
        IMediator mediator,
        IStringLocalizer<SharedResource> localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Get(string slug)
    {
        var product = await _mediator.Send(new GetProductBySlugQuery(slug));

        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery]PageProductQuery query)
    {
        var product = await _mediator.Send(query);

        Response.AddPagination(product.PageNumber, product.PageSize, product.TotalCount, product.TotalPages);

        return Ok(product.Products);
    }


    [HttpPost]
    [Authorize(Policy = "HasAdminRole")]
    [Route("~/api/users/{userId}/products")]
    public async Task<IActionResult> Create(int userId, CreateProductCommand command)
    {
        if (userId != UserId)
            return Unauthorized(_localizer["ResponseUnathorized"]);

        var product = await _mediator.Send(command);

        return CreatedAtRoute(nameof(Get), new { id = product.Id }, product);
    }


    [HttpPut("{id}")]
    [Authorize(Policy = "HasAdminRole")]
    public async Task<IActionResult> Update(int id, ChangeProductCommand command)
    {
        if (id != command.Id)
            return BadRequest(_localizer["ResponseBadRequest"]);

        await _mediator.Send(command);

        return NoContent();
    }


    [HttpDelete("{id}")]
    [Authorize(Policy = "HasAdminRole")]
    public async Task<IActionResult> Remove(int id)
    {
        await _mediator.Send(new RemoveProductCommand(id));

        return NoContent();
    }
}