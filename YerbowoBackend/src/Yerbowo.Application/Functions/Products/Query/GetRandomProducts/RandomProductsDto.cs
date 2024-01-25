namespace Yerbowo.Application.Functions.Products.Query.GetRandomProducts;

public class RandomProductsDto
{
    public IEnumerable<ProductCardDto> Bestsellers { get; set; }

    public IEnumerable<ProductCardDto> News { get; set; }

    public IEnumerable<ProductCardDto> Promotions { get; set; }
}