

namespace Yerbowo.Api.Controllers;

[ApiController]
[Route("api/cart")]
public class CartController : ApiControllerBase
{
	private readonly IMediator _mediator;

	public CartController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	public async Task<IActionResult> Get()
	{
		var cart = await _mediator.Send(new GetCartItemsQuery());
		return Ok(cart);
	}

	[HttpGet("totalCartProducts")]
	public async Task<IActionResult> GetTotalCartProducts()
	{
		int totalCartItems = await _mediator.Send(new GetTotalCartItemsQuery());
		return Ok(totalCartItems);
	}

	[HttpPost]
	public async Task<IActionResult> Add(AddCartItemCommand command)
	{
		var cart = await _mediator.Send(command);
		return Ok(cart);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Put(int id, ChangeCartItemCommand command)
	{
		if (id != command.Id)
			return BadRequest();

		var cart = await _mediator.Send(command);
		return Ok(cart);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		var cart = await _mediator.Send(new RemoveCartItemCommand(id));
		return Ok(cart);
	}
}