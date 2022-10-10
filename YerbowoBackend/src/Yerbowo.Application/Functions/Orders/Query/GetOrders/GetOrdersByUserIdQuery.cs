namespace Yerbowo.Application.Functions.Orders.Query.GetOrders;

public class GetOrdersByUserIdQuery : IRequest<IEnumerable<OrderDto>>
{
	public int UserId { get; }

	public GetOrdersByUserIdQuery(int userId)
	{
		UserId = userId;
	}
}