namespace Yerbowo.Application.Functions.Orders.Query.GetOrderDetails;

public class GetOrderDetailsByIdHandler(
	IOrderRepository orderRepository,
    IMapper mapper) : IQueryHandler<GetOrderDetailsByIdQuery, OrderDetailsDto>
{
    public async Task<OrderDetailsDto> Handle(GetOrderDetailsByIdQuery request, CancellationToken cancellationToken)
	{
		var order = await orderRepository.GetAsync(request.Id);

		return mapper.Map<OrderDetailsDto>(order);
	}
}