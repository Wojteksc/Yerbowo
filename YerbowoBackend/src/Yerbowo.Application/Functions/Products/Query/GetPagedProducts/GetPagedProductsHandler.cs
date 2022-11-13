﻿namespace Yerbowo.Application.Functions.Products.Query.GetPagedProducts;

public class GetPagedProductsHandler : IRequestHandler<PageProductQuery, PagedProductCardDto>
{
	private readonly IProductRepository _productRepository;
	private readonly IMapper _mapper;

	public GetPagedProductsHandler(IProductRepository productRepository,
		IMapper mapper)
	{
		_productRepository = productRepository;
		_mapper = mapper;
	}

	public async Task<PagedProductCardDto> Handle(PageProductQuery request, CancellationToken cancellationToken)
	{
		var products = await _productRepository.BrowseAsync(request.PageNumber, request.PageSize, request.Category, request.Subcategory);

		return _mapper.Map<PagedProductCardDto>(products);
	}
}