namespace Yerbowo.Application.Functions.Products;

public record ProductCardDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Slug { get; init; }
    public string CategorySlug { get; init; }
    public string SubcategorySlug { get; init; }
    public decimal Price { get; init; }
    public decimal OldPrice { get; init; }
    public ProductState State { get; init; }
    public string Image { get; init; }
    public DateTime CreatedAt { get; init; }
}