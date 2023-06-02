namespace Yerbowo.Application.Functions.Cart.Query.GetCartItems;

public class GetCartItemsHandler : IRequestHandler<GetCartItemsQuery, CartDto>
{
    private readonly ISession _session;
    private readonly IMapper _mapper;

    public GetCartItemsHandler(IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _session= httpContextAccessor.HttpContext.Session;
        _mapper = mapper;
    }

    public async Task<CartDto> Handle(GetCartItemsQuery request, CancellationToken cancellationToken)
    {
        var cartItems = CartHelper.GetCartProducts(_session);
        return await Task.FromResult(_mapper.Map<CartDto>(cartItems));
    }
}