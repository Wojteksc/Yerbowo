namespace Yerbowo.Application.Functions.Products.Command.RemoveProducts;

public class RemoveProductHandler : IRequestHandler<RemoveProductCommand>
{
	private readonly IProductRepository _productRepository;

	public RemoveProductHandler(IProductRepository productRepository)
	{
		_productRepository = productRepository;
	}

	public async Task<Unit> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
	{
		var product = await _productRepository.GetAsync(request.Id);

		if (product == null || product.IsRemoved)
		{
			throw new Exception("Produkt nie istnieje");
		}

		await _productRepository.RemoveAsync(product);

		return Unit.Value;
	}
}