namespace Yerbowo.Application.Functions.Orders.Query.GetOrders;

public class GetOrdersByUserIdHandler(
	IOrderRepository orderRepository,
    IMapper mapper) : IQueryHandler<GetOrdersByUserIdQuery, IEnumerable<OrderDto>>
{
    public async Task<IEnumerable<OrderDto>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
	{
		var orders = await orderRepository.GetByUserAsync(request.UserId);

		return mapper.Map<IEnumerable<OrderDto>>(orders);
	}
}