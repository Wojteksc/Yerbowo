namespace Yerbowo.Application.Functions.Cart.Query.GetCartItems;

public class GetCartItemsHandler : IRequestHandler<GetCartItemsQuery, CartDto>
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly ISession _session;
	private readonly IMapper _mapper;

	public GetCartItemsHandler(IHttpContextAccessor httpContextAccessor,
		IMapper mapper)
	{
		_httpContextAccessor = httpContextAccessor;
		_session = _httpContextAccessor.HttpContext.Session;
		_mapper = mapper;
	}

	public Task<CartDto> Handle(GetCartItemsQuery request, CancellationToken cancellationToken)
	{
		List<CartItemDto> cartItems;

		if (_session.GetString(Consts.CartSessionKey) == null)
		{
			cartItems = new List<CartItemDto>();
		}
		else
		{
			var value = _session.GetString(Consts.CartSessionKey);
			cartItems = JsonSerializer.Deserialize<List<CartItemDto>>(value);
		}

		return Task.FromResult(_mapper.Map<CartDto>(cartItems));
	}
}