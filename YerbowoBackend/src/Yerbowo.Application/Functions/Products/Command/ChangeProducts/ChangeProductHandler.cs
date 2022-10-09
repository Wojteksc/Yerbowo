using Yerbowo.Infrastructure.Data.Products;

namespace Yerbowo.Application.Functions.Products.Command.ChangeProducts;

public class ChangeProductHandler : IRequestHandler<ChangeProductCommand>
{
	private readonly IProductRepository _productRepository;
	private readonly IMapper _mapper;

	public ChangeProductHandler(IProductRepository productRepository,
		IMapper mapper)
	{
		_productRepository = productRepository;
		_mapper = mapper;
	}

	public async Task<Unit> Handle(ChangeProductCommand request, CancellationToken cancellationToken)
	{
		var product = await _productRepository.GetAsync(request.Id);

		if (product == null)
			throw new Exception("Nie znaleziono produktu");

		_mapper.Map(request, product);

		await _productRepository.UpdateAsync(product);

		return Unit.Value;
	}
}