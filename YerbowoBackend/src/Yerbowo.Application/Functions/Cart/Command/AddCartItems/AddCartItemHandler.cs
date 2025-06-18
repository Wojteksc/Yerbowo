namespace Yerbowo.Application.Functions.Cart.Command.AddCartItems;

public class AddCartItemHandler(
    IHttpContextAccessor httpContextAccessor,
    IProductRepository productRepository,
    IMapper mapper,
    IStringLocalizer<SharedResource> localizer) : ICommandHandler<AddCartItemCommand, CartDto>
{
    private readonly ISession _session = httpContextAccessor.HttpContext.Session;

    public async Task<CartDto> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
    {
        CartValidatorHelper.VerifyQuantity(request.Quantity, localizer);

        var products = CartSessionHelper.GetCartProducts(_session);
        var productDb = await productRepository.GetWithCategoryAsync(request.Id);
        var productDto = mapper.Map<CartProductItemDto>(productDb);
        var product = products.FirstOrDefault(x => x.Product.Id == productDb.Id);

        if (product is not null)
        {
            product.Quantity += request.Quantity;
            CartValidatorHelper.VerifyStock(productDb, product.Quantity, localizer);
        }
        else
        {
            CartValidatorHelper.VerifyStock(productDb, request.Quantity, localizer);
            products.Add(new CartItemDto
            {
                Product = productDto,
                Quantity = request.Quantity
            });
        }

        CartSessionHelper.SaveCartProducts(_session, products);

        return mapper.Map<CartDto>(products);
    }
}