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
		var productDb = await _productRepository.GetAsync(request.Id);

		if (productDb == null)
			throw new Exception("Nie znaleziono produktu");

		if (request.State == ProductState.Promotion
			&& request.Price >= productDb.Price
			&& productDb.OldPrice != default)
			throw new Exception("Nowe cena produktu objętego promocją musi być mniejsza od aktualnej ceny produktu.");

		if(await _productRepository.ExistsAsync(productDb.Slug))
            throw new Exception($"Produkt o nazwie {request.Name} już istnieje. Zmień nazwę.");

		productDb.SetOldPrice(productDb.Price);
        _mapper.Map(request, productDb);

		await _productRepository.UpdateAsync(productDb);	

		return Unit.Value;
	}
}