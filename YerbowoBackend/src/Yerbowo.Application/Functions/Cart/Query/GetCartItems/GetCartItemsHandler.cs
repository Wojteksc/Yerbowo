namespace Yerbowo.Application.Functions.Cart.Query.GetCartItems;

public class GetCartItemsHandler(
    IHttpContextAccessor httpContextAccessor, 
    IMapper mapper) : IQueryHandler<GetCartItemsQuery, CartDto>
{
    private readonly ISession _session = httpContextAccessor.HttpContext.Session;

    public async Task<CartDto> Handle(GetCartItemsQuery request, CancellationToken cancellationToken)
    {
        var cartItems = CartSessionHelper.GetCartProducts(_session);
        return await Task.FromResult(mapper.Map<CartDto>(cartItems));
    }
}