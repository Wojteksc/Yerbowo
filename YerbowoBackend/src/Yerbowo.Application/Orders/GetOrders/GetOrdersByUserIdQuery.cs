using MediatR;
using System.Collections.Generic;

namespace Yerbowo.Application.Orders.GetOrders
{
	public class GetOrdersByUserIdQuery : IRequest<IEnumerable<OrderDto>>
	{
		public int UserId { get; }

		public GetOrdersByUserIdQuery(int userId)
		{
			UserId = userId;
		}

	}
}
