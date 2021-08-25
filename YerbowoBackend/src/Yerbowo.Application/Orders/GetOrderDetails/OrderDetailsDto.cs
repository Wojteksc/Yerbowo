using System.Collections.Generic;
using Yerbowo.Application.Addresses.GetAddresses;

namespace Yerbowo.Application.Orders.GetOrderDetails
{
    public class OrderDetailsDto
    {
        public int Id { get; set; }
        public decimal TotalCost { get; set; }
        public AddressDto Address { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
