namespace Yerbowo.Application.Functions.Products.Query.GetRandomProducts;

public class GetRandomProductsHandler : IRequestHandler<GetRandomProductsQuery, RandomProductsDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetRandomProductsHandler(IProductRepository productRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<RandomProductsDto> Handle(GetRandomProductsQuery request, CancellationToken cancellationToken)
    {
        const int PRODUCTS_TOTAL = 30, PRODUCTS_PER_PAGE = 4;

        var products = await _productRepository.BrowseRandomAsync(PRODUCTS_TOTAL);
        var productsCardDto = _mapper.Map<List<ProductCardDto>>(products);

        var bestsellers = GetBestsellers(productsCardDto, PRODUCTS_PER_PAGE);
        var news = GetNews(productsCardDto, PRODUCTS_PER_PAGE);
        var promotions = GetPromotions(productsCardDto, PRODUCTS_PER_PAGE);

        var allTypesProducts = new { bestsellers, news, promotions };

        return _mapper.Map<RandomProductsDto>(allTypesProducts);
    }

    private IEnumerable<ProductCardDto> GetBestsellers(List<ProductCardDto> products, int amount)
    {
        return products
            .Where(p => p.State == ProductState.Bestseller)
            .Take(amount)
            .OrderBy(p => Guid.NewGuid())
            .AsEnumerable();
    }

    private IEnumerable<ProductCardDto> GetNews(List<ProductCardDto> products, int amount)
    {
        return products
            .Where(p => p.State == ProductState.New)
            .Take(amount)
            .OrderByDescending(p => p.CreatedAt)
            .AsEnumerable();
    }

    private IEnumerable<ProductCardDto> GetPromotions(List<ProductCardDto> products, int amount)
    {
        return products
            .Where(p => p.Price != p.OldPrice)
            .Take(amount)
            .OrderBy(p => Guid.NewGuid())
            .AsEnumerable();
    }
}