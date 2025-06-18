namespace Yerbowo.Application.Functions.Orders.Query.GetOrderDetails;

public record OrderItemDto
{
    public string ProductName { get; init; }
    public string ProductImage { get; init; }
    public string ProductCategorySlug { get; init; }
    public string ProductSubcategorySlug { get; init; }
    public string ProductSlug { get; init; }
    public int Quantity { get; init; }
    public decimal Price { get; init; }
    public decimal Sum { get; init; }
}