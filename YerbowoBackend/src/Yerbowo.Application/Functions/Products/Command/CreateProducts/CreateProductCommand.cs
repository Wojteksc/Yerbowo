namespace Yerbowo.Application.Functions.Products.Command.CreateProducts;

public class CreateProductCommand : IRequest<ProductDto>
{
    public int SubcategoryId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public ProductState State { get; set; }
    public string Image { get; set; }
}