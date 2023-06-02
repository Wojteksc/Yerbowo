namespace Yerbowo.Application.Functions.Products.Command.CreateProducts;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
	private readonly IProductRepository _productRepository;
	private readonly IMapper _mapper;

	public CreateProductHandler(IProductRepository productRepository,
		IMapper mapper)
	{
		_productRepository = productRepository;
		_mapper = mapper;
	}

	public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
	{
		if (await _productRepository.ExistsAsync(request.Name.ToSlug()))
			throw new Exception($"Produkt o nazwie {request.Name} już istnieje. Zmień nazwę.");

		var product = _mapper.Map<Product>(request);

		await _productRepository.AddAsync(product);

		return _mapper.Map<ProductDto>(product);
	}
}