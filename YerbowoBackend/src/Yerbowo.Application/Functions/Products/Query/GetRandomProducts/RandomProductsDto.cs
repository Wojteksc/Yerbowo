namespace Yerbowo.Application.Functions.Products.Query.GetRandomProducts;

public record RandomProductsDto
{
    public IEnumerable<ProductCardDto> Bestsellers { get; init; }

    public IEnumerable<ProductCardDto> News { get; init; }

    public IEnumerable<ProductCardDto> Promotions { get; init; }
}