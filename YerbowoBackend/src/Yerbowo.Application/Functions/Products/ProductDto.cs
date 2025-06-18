namespace Yerbowo.Application.Functions.Products;

public record ProductDto
{
    public int Id { get; init; }
    public string Code { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string Slug { get; init; }
    public decimal Price { get; init; }
    public int Stock { get; init; }
    public ProductState State { get; init; }
    public string Image { get; init; }
    public DateTime CreatedAt { get; init; }
}