namespace Yerbowo.Application.Functions.Products.Query.GetProductDetails;

public class GetProductBySlugHandler : IRequestHandler<GetProductBySlugQuery, ProductDetailsDto>
{
	private readonly IProductRepository _productRepository;
	private readonly IMapper _mapper;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public GetProductBySlugHandler(IProductRepository productRepository,
		IMapper mapper,
        IStringLocalizer<SharedResource> localizer)
	{
		_productRepository = productRepository;
		_mapper = mapper;
        _localizer = localizer;
    }

	public async Task<ProductDetailsDto> Handle(GetProductBySlugQuery request, CancellationToken cancellationToken)
	{
		var product = await _productRepository.GetWithCategoryAsync(request.Slug);

		if (product == null)
			throw new ArgumentException(_localizer["ExceptionProductNotFound"]);

		return _mapper.Map<ProductDetailsDto>(product);
	}
}