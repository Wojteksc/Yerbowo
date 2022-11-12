namespace Yerbowo.Application.Functions.Cart.Command.AddCartItems;
using Yerbowo.Application.Functions.Cart.Utils;

public class AddCartItemHandler : IRequestHandler<AddCartItemCommand, CartDto>
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	private ISession _session;
	private readonly IProductRepository _productRepository;
	private readonly IMapper _mapper;

	public AddCartItemHandler(IHttpContextAccessor httpContextAccessor,
		IProductRepository productRepository,
		IMapper mapper)
	{
		_httpContextAccessor = httpContextAccessor;
		_session = _httpContextAccessor.HttpContext.Session;
		_productRepository = productRepository;
		_mapper = mapper;
	}

	public async Task<CartDto> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
	{
		CartHelper.VerifyQuantity(request.Quantity);

		var productDb = await _productRepository.GetAsync(request.Id, x => x
			.Include(p => p.Subcategory)
			.ThenInclude(s => s.Category));

		var productDto = _mapper.Map<CartProductItemDto>(productDb);
		var products = _session.GetObjectFromJson<List<CartItemDto>>(Consts.CartSessionKey);

		if (products == null)
		{
			products = new List<CartItemDto>();
			await AddNewProductToCart(products, productDto, request);
		}
		else
		{
			int productIndex = products.FindIndex(x => x.Product.Id == productDb.Id);

			if (productIndex != -1)
			{
				int productQuantity = products[productIndex].Quantity + request.Quantity;
				await CartHelper.VerifyStock(_productRepository, request.Id, productQuantity);

				products[productIndex].Quantity = productQuantity;
				_session.SetString(Consts.CartSessionKey, JsonSerializer.Serialize(products));
			}
			else
			{
				await AddNewProductToCart(products, productDto, request);
			}
		}

		return _mapper.Map<CartDto>(_session.GetObjectFromJson<List<CartItemDto>>(Consts.CartSessionKey));
	}

	private async Task AddNewProductToCart(List<CartItemDto> cart, CartProductItemDto productDetailsDto, AddCartItemCommand request)
	{
		await CartHelper.VerifyStock(_productRepository, request.Id, request.Quantity);

		cart.Add(new CartItemDto()
		{
			Product = productDetailsDto,
			Quantity = request.Quantity
		});

		_session.SetString(Consts.CartSessionKey, JsonSerializer.Serialize(cart));
	}
}