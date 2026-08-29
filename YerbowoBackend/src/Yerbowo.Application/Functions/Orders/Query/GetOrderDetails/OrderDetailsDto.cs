namespace Yerbowo.Application.Functions.Orders.Query.GetOrderDetails;

public record OrderDetailsDto
{
    public Guid Id { get; init; }
    public decimal TotalCost { get; init; }
    public AddressDto Address { get; init; }
    public List<OrderItemDto> OrderItems { get; init; }
}