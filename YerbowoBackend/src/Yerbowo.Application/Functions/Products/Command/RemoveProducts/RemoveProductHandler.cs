namespace Yerbowo.Application.Functions.Products.Command.RemoveProducts;

public class RemoveProductHandler : IRequestHandler<RemoveProductCommand>
{
	private readonly IProductRepository _productRepository;
	private readonly IStringLocalizer<SharedResource> _localizer;

    public RemoveProductHandler(IProductRepository productRepository, IStringLocalizer<SharedResource> localizer)
    {
        _productRepository = productRepository;
        _localizer = localizer;
    }

    public async Task<Unit> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
	{
		var product = await _productRepository.GetAsync(request.Id);

		if (product == null || product.IsRemoved)
		{
			throw new Exception(_localizer["ExceptionProductDoesNotExist"]);
		}

		await _productRepository.RemoveAsync(product);

		return Unit.Value;
	}
}