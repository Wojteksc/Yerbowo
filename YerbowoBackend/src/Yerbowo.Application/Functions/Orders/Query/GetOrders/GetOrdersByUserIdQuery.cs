namespace Yerbowo.Application.Functions.Orders.Query.GetOrders;

public record GetOrdersByUserIdQuery(int UserId) : IQuery<IEnumerable<OrderDto>> { }