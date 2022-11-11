namespace Yerbowo.Application.Functions.Cart.Command.RemoveCartItems;

public class RemoveCartItemHandler : IRequestHandler<RemoveCartItemCommand, CartDto>
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly ISession _session;
	private readonly IMapper _mapper;

	public RemoveCartItemHandler(IHttpContextAccessor httpContextAccessor,
		IMapper mapper)
	{
		_httpContextAccessor = httpContextAccessor;
		_session = _httpContextAccessor.HttpContext.Session;
		_mapper = mapper;
	}

	public Task<CartDto> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
	{
		var products = _session.GetObjectFromJson<List<CartItemDto>>(Consts.CartSessionKey);
		int index = products.FindIndex(x => x.Product.Id == request.ProductId);

		if (index != -1)
		{
			products.RemoveAt(index);
			
			_session.SetObjectAsJson(Consts.CartSessionKey, products);
		}

		return Task.FromResult(_mapper.Map<CartDto>(_session.GetObjectFromJson<List<CartItemDto>>(Consts.CartSessionKey)));
	}
}