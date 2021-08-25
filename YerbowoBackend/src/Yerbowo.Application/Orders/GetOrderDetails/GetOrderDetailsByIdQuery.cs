using MediatR;

namespace Yerbowo.Application.Orders.GetOrderDetails
{
	public class GetOrderDetailsByIdQuery : IRequest<OrderDetailsDto>
	{
		public int Id { get; }

		public GetOrderDetailsByIdQuery(int id)
		{
			Id = id;
		}

	}
}
