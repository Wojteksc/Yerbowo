namespace Yerbowo.Application.Functions.Products.Command.CreateProducts;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
	private readonly IProductRepository _productRepository;
	private readonly IMapper _mapper;
	private readonly IStringLocalizer<SharedResource> _localizer;

    public CreateProductHandler(IProductRepository productRepository,
        IMapper mapper,
        IStringLocalizer<SharedResource> localizer)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
	{
		if (await _productRepository.ExistsAsync(request.Name.ToSlug()))
			throw new Exception(_localizer["ExceptionProductAlreadyExistsWithThatName"]);

		var product = _mapper.Map<Product>(request);

		await _productRepository.AddAsync(product);

		return _mapper.Map<ProductDto>(product);
	}
}