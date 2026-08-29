namespace Yerbowo.Application.Functions.Products.Command.CreateProducts;

public class CreateProductHandler(
	IProductRepository productRepository,
    IStringLocalizer<SharedResource> localizer,
	IIdGenerator idGenerator) : ICommandHandler<CreateProductCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
	{
		if (await productRepository.ExistsAsync(request.Name.ToSlug()))
			throw new ProductNameIsAlreadyExistsException(localizer);

		var product = new Product(
			idGenerator.Generate(),
			request.SubcategoryId,
			request.Code,
			request.Name,
			request.Description,
			request.Price,
			0,
			request.Stock,
			request.State,
			request.Image);

		await productRepository.AddAsync(product);

		return product.Id;
	}
}