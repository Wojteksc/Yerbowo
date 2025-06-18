namespace Yerbowo.Application.Functions.Products.Command.RemoveProducts;

public class RemoveProductHandler(
	IProductRepository productRepository, 
	IStringLocalizer<SharedResource> localizer) : ICommandHandler<RemoveProductCommand>
{
    public async Task Handle(RemoveProductCommand request, CancellationToken cancellationToken)
	{
		var product = await productRepository.GetAsync(request.Id);

		if (product == null || product.IsRemoved)
		{
			throw new ProductNotFoundException(localizer);
		}

		await productRepository.RemoveAsync(product);
	}
}