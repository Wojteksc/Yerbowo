namespace Yerbowo.Application.Functions.Orders.Query.GetOrders;

public record OrderProductImageDto
{
    public int Quantity { get; init; }
    public string Name { get; init; }
}