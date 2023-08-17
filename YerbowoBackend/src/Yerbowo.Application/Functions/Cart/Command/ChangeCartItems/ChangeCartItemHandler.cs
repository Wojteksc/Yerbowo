namespace Yerbowo.Application.Functions.Cart.Command.ChangeCartItems;

public class ChangeCartItemHandler : IRequestHandler<ChangeCartItemCommand, CartDto>
{
	private readonly ISession _session;
	private readonly IProductRepository _productRepository;
	private readonly IMapper _mapper;
	private readonly IStringLocalizer<SharedResource> _localizer;

    public ChangeCartItemHandler(IHttpContextAccessor httpContextAccessor,
        IProductRepository productRepository,
        IMapper mapper,
        IStringLocalizer<SharedResource> localizer)
    {
        _session = httpContextAccessor.HttpContext.Session;
        _productRepository = productRepository;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<CartDto> Handle(ChangeCartItemCommand request, CancellationToken cancellationToken)
	{
		CartValidatorHelper.VerifyQuantity(request.Quantity, _localizer);

		var products = CartSessionHelper.GetCartProducts(_session);
		var product = products.FirstOrDefault(x => x.Product.Id == request.Id);

		if (product is null)
		{
			throw new Exception(_localizer["ExceptionProductNotFound"]);
		}

        var productDb = await _productRepository.GetAsync(request.Id);

        CartValidatorHelper.VerifyStock(productDb, request.Quantity, _localizer);
        product.Quantity = request.Quantity;
        CartSessionHelper.SaveCartProducts(_session, products);

		return _mapper.Map<CartDto>(products);
	}
}