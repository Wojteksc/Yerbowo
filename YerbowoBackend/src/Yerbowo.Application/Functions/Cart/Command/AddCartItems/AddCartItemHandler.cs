namespace Yerbowo.Application.Functions.Cart.Command.AddCartItems;

public class AddCartItemHandler : IRequestHandler<AddCartItemCommand, CartDto>
{
    private readonly ISession _session;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public AddCartItemHandler(IHttpContextAccessor httpContextAccessor,
        IProductRepository productRepository,
        IMapper mapper,
        IStringLocalizer<SharedResource> localizer)
    {
        _session = httpContextAccessor.HttpContext.Session;
        _productRepository = productRepository;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<CartDto> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
    {
        CartValidatorHelper.VerifyQuantity(request.Quantity, _localizer);

        var products = CartSessionHelper.GetCartProducts(_session);
        var productDb = await _productRepository.GetWithCategoryAsync(request.Id);
        var productDto = _mapper.Map<CartProductItemDto>(productDb);
        var product = products.FirstOrDefault(x => x.Product.Id == productDb.Id);

        if (product is not null)
        {
            product.Quantity += request.Quantity;
            CartValidatorHelper.VerifyStock(productDb, product.Quantity, _localizer);
        }
        else
        {
            CartValidatorHelper.VerifyStock(productDb, request.Quantity, _localizer);
            products.Add(new CartItemDto
            {
                Product = productDto,
                Quantity = request.Quantity
            });
        }

        CartSessionHelper.SaveCartProducts(_session, products);

        return _mapper.Map<CartDto>(products);
    }
}