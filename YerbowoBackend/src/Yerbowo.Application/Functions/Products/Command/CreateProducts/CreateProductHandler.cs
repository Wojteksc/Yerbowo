namespace Yerbowo.Application.Functions.Products.Command.CreateProducts;

public class CreateProductHandler(
	IProductRepository productRepository,
    IMapper mapper,
    IStringLocalizer<SharedResource> localizer) : ICommandHandler<CreateProductCommand, int>
{
    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
	{
		if (await productRepository.ExistsAsync(request.Name.ToSlug()))
			throw new ProductNameIsAlreadyExistsException(localizer);

		var product = mapper.Map<Product>(request);

		await productRepository.AddAsync(product);

		return product.Id;
	}
}