namespace Yerbowo.Application.Functions.Orders.Query.GetOrderDetails;

public class GetOrderDetailsByIdQuery : IRequest<OrderDetailsDto>
{
	public int Id { get; }

	public GetOrderDetailsByIdQuery(int id)
	{
		Id = id;
	}

}