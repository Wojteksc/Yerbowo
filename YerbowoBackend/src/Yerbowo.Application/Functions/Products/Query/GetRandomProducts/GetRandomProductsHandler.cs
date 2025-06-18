namespace Yerbowo.Application.Functions.Products.Query.GetRandomProducts;

public class GetRandomProductsHandler(
    IProductRepository productRepository,
    IMapper mapper) : IQueryHandler<GetRandomProductsQuery, RandomProductsDto>
{
    public async Task<RandomProductsDto> Handle(GetRandomProductsQuery request, CancellationToken cancellationToken)
    {
        const int ProductsTotal = 30, ProductsPerPage = 4;

        var products = await productRepository.BrowseRandomAsync(ProductsTotal);
        var productsCardDto = mapper.Map<List<ProductCardDto>>(products);

        var bestsellers = GetBestsellers(productsCardDto, ProductsPerPage);
        var news = GetNews(productsCardDto, ProductsPerPage);
        var promotions = GetPromotions(productsCardDto, ProductsPerPage);

        return new RandomProductsDto
        {
            Bestsellers = bestsellers,
            News = news,
            Promotions = promotions
        };
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