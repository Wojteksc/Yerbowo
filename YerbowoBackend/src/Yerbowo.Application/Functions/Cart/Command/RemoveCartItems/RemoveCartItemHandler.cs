namespace Yerbowo.Application.Functions.Cart.Command.RemoveCartItems;

public class RemoveCartItemHandler(
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper,
    IStringLocalizer<SharedResource> localizer) : ICommandHandler<RemoveCartItemCommand, CartDto>
{
    private readonly ISession _session = httpContextAccessor.HttpContext.Session;

    public async Task<CartDto> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        var products = CartSessionHelper.GetCartProducts(_session);
        var product = products.SingleOrDefault(x => x.Product.Id == request.ProductId) 
            ?? throw new ProductNotFoundException(localizer);
        
        products.Remove(product);
        _session.SetObjectAsJson(SessionKeys.CartSession, products);

        return await Task.FromResult(mapper.Map<CartDto>(products));
    }
}