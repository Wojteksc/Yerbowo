namespace Yerbowo.Application.Functions.Products.Query.GetProductDetails;

public class GetProductBySlugHandler(
	IProductRepository productRepository,
    IMapper mapper,
    IStringLocalizer<SharedResource> localizer) : IQueryHandler<GetProductBySlugQuery, ProductDetailsDto>
{
    public async Task<ProductDetailsDto> Handle(GetProductBySlugQuery request, CancellationToken cancellationToken)
	{
		var product = await productRepository.GetWithCategoryAsync(request.Slug) 
            ?? throw new ProductNotFoundException(localizer);
        
        return mapper.Map<ProductDetailsDto>(product);
	}
}