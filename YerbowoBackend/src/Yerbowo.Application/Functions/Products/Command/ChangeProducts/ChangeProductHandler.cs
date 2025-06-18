namespace Yerbowo.Application.Functions.Products.Command.ChangeProducts;

public class ChangeProductHandler(
	IProductRepository productRepository,
    IMapper mapper,
    IStringLocalizer<SharedResource> localizer) : ICommandHandler<ChangeProductCommand>
{
    public async Task Handle(ChangeProductCommand request, CancellationToken cancellationToken)
	{
		var productDb = await productRepository.GetAsync(request.Id) 
			?? throw new ProductNotFoundException(localizer);
        
		if (request.State == ProductState.Promotion
			&& request.Price >= productDb.Price
			&& productDb.OldPrice != default)
			throw new PromotionalProductPriceMustBeLowerException(localizer);

		if(await productRepository.ExistsAsync(productDb.Slug))
            throw new ProductNameIsAlreadyExistsException(localizer);

		productDb.SetOldPrice(productDb.Price);
        mapper.Map(request, productDb);

		await productRepository.UpdateAsync(productDb);	
	}
}