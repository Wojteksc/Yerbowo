namespace Yerbowo.Application.Functions.Orders.Query.GetOrders;

public record GetOrdersByUserIdQuery(Guid UserId) : IQuery<IEnumerable<OrderDto>> { }