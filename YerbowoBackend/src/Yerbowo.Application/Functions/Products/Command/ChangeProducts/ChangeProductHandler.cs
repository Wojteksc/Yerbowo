namespace Yerbowo.Application.Functions.Products.Command.ChangeProducts;

public class ChangeProductHandler : IRequestHandler<ChangeProductCommand>
{
	private readonly IProductRepository _productRepository;
	private readonly IMapper _mapper;
	private readonly IStringLocalizer<SharedResource> _localizer;

    public ChangeProductHandler(IProductRepository productRepository,
        IMapper mapper,
        IStringLocalizer<SharedResource> localizer)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task Handle(ChangeProductCommand request, CancellationToken cancellationToken)
	{
		var productDb = await _productRepository.GetAsync(request.Id) 
			?? throw new ArgumentException(_localizer["ExceptionProductNotFound"]);
        
		if (request.State == ProductState.Promotion
			&& request.Price >= productDb.Price
			&& productDb.OldPrice != default)
			throw new Exception(_localizer["ExceptionNewPromotionalProductPriceMustBeLowerThanCurrentPrice"]);

		if(await _productRepository.ExistsAsync(productDb.Slug))
            throw new Exception(_localizer["ExceptionProductAlreadyExistsWithThatName"]);

		productDb.SetOldPrice(productDb.Price);
        _mapper.Map(request, productDb);

		await _productRepository.UpdateAsync(productDb);	
	}
}