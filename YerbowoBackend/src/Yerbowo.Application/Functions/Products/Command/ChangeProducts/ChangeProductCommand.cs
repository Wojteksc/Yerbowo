namespace Yerbowo.Application.Functions.Products.Command.ChangeProducts;
public record ChangeProductCommand : ICommand, ICommandIdentity
{
    public Guid Id { get; init; }
    public Guid SubcategoryId { get; init; }
    public string Code { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public int Stock { get; init; }
    public ProductState State { get; init; }
    public string Image { get; init; }
}