namespace Yerbowo.Application.Functions.Cart.Command.RemoveCartItems;

public class RemoveCartItemHandler : IRequestHandler<RemoveCartItemCommand, CartDto>
{
    private readonly ISession _session;
    private readonly IMapper _mapper;

    public RemoveCartItemHandler(IHttpContextAccessor httpContextAccessor,
        IMapper mapper)
    {
        _session = httpContextAccessor.HttpContext.Session;
        _mapper = mapper;
    }

    public async Task<CartDto> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        var products = CartHelper.GetCartProducts(_session);
        var product = products.FirstOrDefault(x => x.Product.Id == request.ProductId);

        if (product is null)
        {
            throw new Exception("Nie znaleziono produktu.");
        }

        products.Remove(product);
        _session.SetObjectAsJson(Consts.CartSessionKey, products);

        return await Task.FromResult(_mapper.Map<CartDto>(products));
    }
}