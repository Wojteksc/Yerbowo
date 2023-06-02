namespace Yerbowo.Application.Functions.Cart.Command.AddCartItems;

public class AddCartItemHandler : IRequestHandler<AddCartItemCommand, CartDto>
{
    private readonly ISession _session;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public AddCartItemHandler(IHttpContextAccessor httpContextAccessor,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _session = httpContextAccessor.HttpContext.Session;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<CartDto> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
    {
        CartHelper.VerifyQuantity(request.Quantity);

        var products = CartHelper.GetCartProducts(_session);
        var productDb = await GetProduct(request.Id);
        var productDto = _mapper.Map<CartProductItemDto>(productDb);
        var product = products.FirstOrDefault(x => x.Product.Id == productDb.Id);

        if (product is not null)
        {
            product.Quantity += request.Quantity;
            CartHelper.VerifyStock(productDb, product.Quantity);
        }
        else
        {
            CartHelper.VerifyStock(productDb, request.Quantity);
            products.Add(new CartItemDto
            {
                Product = productDto,
                Quantity = request.Quantity
            });
        }

        CartHelper.SaveCartProducts(_session, products);

        return _mapper.Map<CartDto>(products);
    }

    private async Task<Product> GetProduct(int productId)
    {
        return await _productRepository.GetAsync(productId, x => x
            .Include(p => p.Subcategory.Category));
    }
}