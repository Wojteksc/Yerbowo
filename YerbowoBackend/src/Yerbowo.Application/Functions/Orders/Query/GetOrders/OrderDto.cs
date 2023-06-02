namespace Yerbowo.Application.Functions.Orders.Query.GetOrders;

public class OrderDto
{
    public int Id { get; set; }
    public List<OrderProductImageDto> ProductImages { get; set; }
    public string Date { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; }
}