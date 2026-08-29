namespace Yerbowo.Application.Functions.Orders.Query.GetOrders;

public record OrderDto
{
    public Guid Id { get; init; }
    public List<OrderProductImageDto> ProductImages { get; init; }
    public string Date { get; init; }
    public decimal Total { get; init; }
    public string Status { get; init; }
}