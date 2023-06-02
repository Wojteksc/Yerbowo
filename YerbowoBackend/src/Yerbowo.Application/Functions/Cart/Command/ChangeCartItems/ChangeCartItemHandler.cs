namespace Yerbowo.Application.Functions.Cart.Command.ChangeCartItems;

public class ChangeCartItemHandler : IRequestHandler<ChangeCartItemCommand, CartDto>
{
	private readonly ISession _session;
	private readonly IProductRepository _productRepository;
	private readonly IMapper _mapper;

	public ChangeCartItemHandler(IHttpContextAccessor httpContextAccessor,
		IProductRepository productRepository,
		IMapper mapper)
	{
		_session = httpContextAccessor.HttpContext.Session;
		_productRepository = productRepository;
		_mapper = mapper;
	}

	public async Task<CartDto> Handle(ChangeCartItemCommand request, CancellationToken cancellationToken)
	{
		CartHelper.VerifyQuantity(request.Quantity);

		var products = CartHelper.GetCartProducts(_session);
		var product = products.FirstOrDefault(x => x.Product.Id == request.Id);

		if (product is null)
		{
			throw new Exception("Nie znaleziono produktu.");
		}

        var productDb = await _productRepository.GetAsync(request.Id);

        CartHelper.VerifyStock(productDb, request.Quantity);
        product.Quantity = request.Quantity;
        CartHelper.SaveCartProducts(_session, products);

		return _mapper.Map<CartDto>(products);
	}
}