namespace Yerbowo.Application.Functions.Products;

public class ProductDto
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public ProductState State { get; set; }
    public string Image { get; set; }
    public DateTime CreatedAt { get; set; }
}