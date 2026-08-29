namespace Yerbowo.Application.Functions.Products.Command.CreateProducts;

public record CreateProductCommand : ICommand<Guid>
{
    public Guid SubcategoryId { get; init; }
    public string Code { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public int Stock { get; init; }
    public ProductState State { get; init; }
    public string Image { get; init; }
}