namespace Yerbowo.Application.Functions.Cart.Query.GetTotalCartItems;

public class GetTotalCartItemsHandler(IHttpContextAccessor httpContextAccessor) 
	: IQueryHandler<GetTotalCartItemsQuery, int>
{
	private readonly ISession _session = httpContextAccessor.HttpContext.Session;

    public async Task<int> Handle(GetTotalCartItemsQuery request, CancellationToken cancellationToken)
	{
		var cartItems = CartSessionHelper.GetCartProducts(_session);
		return await Task.FromResult(cartItems.Sum(ci => ci.Quantity));
	}
}