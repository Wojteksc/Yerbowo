namespace Yerbowo.Application.Functions.Cart.Command.ChangeCartItems;

public class ChangeCartItemHandler(
	IHttpContextAccessor httpContextAccessor,
    IProductRepository productRepository,
    IMapper mapper,
    IStringLocalizer<SharedResource> localizer) : ICommandHandler<ChangeCartItemCommand, CartDto>
{
	private readonly ISession _session = httpContextAccessor.HttpContext.Session;

    public async Task<CartDto> Handle(ChangeCartItemCommand request, CancellationToken cancellationToken)
	{
		CartValidatorHelper.VerifyQuantity(request.Quantity, localizer);

		var products = CartSessionHelper.GetCartProducts(_session);
		var product = products.SingleOrDefault(x => x.Product.Id == request.Id) 
            ?? throw new ProductNotFoundException(localizer);
        
        var productDb = await productRepository.GetAsync(request.Id);

        CartValidatorHelper.VerifyStock(productDb, request.Quantity, localizer);
        product.Quantity = request.Quantity;
        CartSessionHelper.SaveCartProducts(_session, products);

		return mapper.Map<CartDto>(products);
	}
}