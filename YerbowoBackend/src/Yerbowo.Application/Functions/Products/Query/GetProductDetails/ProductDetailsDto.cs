namespace Yerbowo.Application.Functions.Products.Query.GetProductDetails;

public record ProductDetailsDto 
{
    public int Id { get; init; }
    public string Code { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public decimal OldPrice { get; init; }
    public int Stock { get; init; }
    public ProductState State { get; init; }
    public string Image { get; init; }
    public string Category { get; init; }
    public string Subcategory { get; init; }
}