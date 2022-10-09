using Yerbowo.Application.Functions.Addresses.Query.GetAddresses;

namespace Yerbowo.Application.Functions.Orders.Query.GetOrderDetails;

public class OrderDetailsDto
{
    public int Id { get; set; }
    public decimal TotalCost { get; set; }
    public AddressDto Address { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
}