namespace Yerbowo.Application.Functions.Cart.Command.RemoveCartItems;

public class RemoveCartItemHandler : IRequestHandler<RemoveCartItemCommand, CartDto>
{
    private readonly ISession _session;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public RemoveCartItemHandler(IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IStringLocalizer<SharedResource> localizer)
    {
        _session = httpContextAccessor.HttpContext.Session;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<CartDto> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        var products = CartSessionHelper.GetCartProducts(_session);
        var product = products.FirstOrDefault(x => x.Product.Id == request.ProductId);

        if (product is null)
        {
            throw new Exception(_localizer["ExceptionProductNotFound"]);
        }

        products.Remove(product);
        _session.SetObjectAsJson(Consts.CartSessionKey, products);

        return await Task.FromResult(_mapper.Map<CartDto>(products));
    }
}