namespace Yerbowo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IRequestDispatcher dispatcher) : ApiControllerBase
{
    [HttpGet("{slug}")]
    public async Task<ActionResult<ProductDetailsDto>> Get(string slug)
    {
        var product = await dispatcher.ExecuteQuery(new GetProductBySlugQuery(slug));

        return Ok(product);
    }

    [HttpGet]
    public async Task<ActionResult<PagedProductCardDto>> GetProducts([FromQuery]PageProductQuery query)
    {
        var product = await dispatcher.ExecuteQuery(query);

        Response.AddPagination(product.PageNumber, product.PageSize, product.TotalCount, product.TotalPages);

        return Ok(product.Products);
    }

    [HttpPost]
    [Authorize(Policy = "HasAdminRole")]
    [Route("~/api/users/{userId}/products")]
    [UnathorizedFilter]
    public async Task<ActionResult<Guid>> Create(Guid userId, CreateProductCommand command)
    {
        Guid productId = await dispatcher.ExecuteCommand(command);
        return CreatedAtRoute(nameof(Get), new { id = productId }, productId);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "HasAdminRole")]
    [BadRequestFilter]
    public async Task<ActionResult> Update(Guid id, ChangeProductCommand command)
    {
        await dispatcher.ExecuteCommand(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "HasAdminRole")]
    public async Task<ActionResult> Remove(Guid id)
    {
        await dispatcher.ExecuteCommand(new RemoveProductCommand(id));

        return NoContent();
    }
}