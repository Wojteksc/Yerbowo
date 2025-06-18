namespace Yerbowo.Api.Controllers;

[ApiController]
[Route("api/cart")]
public class CartController(IRequestDispatcher dispatcher) : ApiControllerBase
{
    [HttpGet]
	public async Task<ActionResult<CartDto>> Get()
	{
		var cart = await dispatcher.ExecuteQuery(new GetCartItemsQuery());
		return Ok(cart);
	}

	[HttpGet("totalCartProducts")]
	public async Task<ActionResult<int>> GetTotalCartProducts()
	{
		int totalCartItems = await dispatcher.ExecuteQuery(new GetTotalCartItemsQuery());
		return Ok(totalCartItems);
	}

	[HttpPost]
	public async Task<ActionResult<CartDto>> Add(AddCartItemCommand command)
	{
		var cart = await dispatcher.ExecuteCommand(command);
		return Ok(cart);
	}

	[HttpPut("{id}")]
	[BadRequestFilter]
	public async Task<ActionResult<CartDto>> Put(int id, ChangeCartItemCommand command)
	{
		var cart = await dispatcher.ExecuteCommand(command);
		return Ok(cart);
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult<CartDto>> Delete(int id)
	{
		var cart = await dispatcher.ExecuteCommand(new RemoveCartItemCommand(id));
		return Ok(cart);
	}
}