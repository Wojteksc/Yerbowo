namespace Yerbowo.Application.Functions.Products.Command.ChangeProducts;
public record ChangeProductCommand : ICommand, ICommandIdentity
{
    public int Id { get; init; }
    public int SubcategoryId { get; init; }
    public string Code { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public int Stock { get; protected init; }
    public ProductState State { get; init; }
    public string Image { get; init; }
}