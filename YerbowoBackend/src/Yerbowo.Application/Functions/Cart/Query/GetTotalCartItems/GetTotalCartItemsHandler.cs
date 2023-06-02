namespace Yerbowo.Application.Functions.Cart.Query.GetTotalCartItems;

public class GetTotalCartItemsHandler : IRequestHandler<GetTotalCartItemsQuery, int>
{
	private readonly ISession _session;

	public GetTotalCartItemsHandler(IHttpContextAccessor httpContextAccessor)
	{
		_session = httpContextAccessor.HttpContext.Session;
	}

	public async Task<int> Handle(GetTotalCartItemsQuery request, CancellationToken cancellationToken)
	{
		var cartItems = CartHelper.GetCartProducts(_session);
		return await Task.FromResult(cartItems.Sum(ci => ci.Quantity));
	}
}