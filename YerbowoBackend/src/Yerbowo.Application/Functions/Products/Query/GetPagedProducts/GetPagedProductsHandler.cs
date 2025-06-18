namespace Yerbowo.Application.Functions.Products.Query.GetPagedProducts;

public class GetPagedProductsHandler(
	IProductRepository productRepository,
    IMapper mapper) : IQueryHandler<PageProductQuery, PagedProductCardDto>
{
    public async Task<PagedProductCardDto> Handle(PageProductQuery request, CancellationToken cancellationToken)
	{
		var products = await productRepository.BrowseAsync(request.PageNumber, request.PageSize, request.Category, request.Subcategory);

		return mapper.Map<PagedProductCardDto>(products);
	}
}