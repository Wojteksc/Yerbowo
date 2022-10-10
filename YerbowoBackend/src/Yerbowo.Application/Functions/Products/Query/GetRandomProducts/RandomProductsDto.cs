namespace Yerbowo.Application.Functions.Products.Query.GetRandomProducts;

public class RandomProductsDto
{
    public IEnumerable<ProductCardDto> Bestsellers { get; protected set; }

    public IEnumerable<ProductCardDto> News { get; protected set; }

    public IEnumerable<ProductCardDto> Promotions { get; protected set; }
}