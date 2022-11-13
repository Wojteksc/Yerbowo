namespace Yerbowo.Application.Functions.Orders.Query.GetOrderDetails;

public class GetOrderDetailsByIdHandler : IRequestHandler<GetOrderDetailsByIdQuery, OrderDetailsDto>
{
	private readonly IOrderRepository _orderRepository;
	private readonly IMapper _mapper;

	public GetOrderDetailsByIdHandler(IOrderRepository orderRepository,
		IMapper mapper)
	{
		_orderRepository = orderRepository;
		_mapper = mapper;
	}

	public async Task<OrderDetailsDto> Handle(GetOrderDetailsByIdQuery request, CancellationToken cancellationToken)
	{
		var order = await _orderRepository.GetAsync(request.Id);

		return _mapper.Map<OrderDetailsDto>(order);
	}
}