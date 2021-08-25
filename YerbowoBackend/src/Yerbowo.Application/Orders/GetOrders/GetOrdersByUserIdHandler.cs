using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Yerbowo.Infrastructure.Data.Orders;

namespace Yerbowo.Application.Orders.GetOrders
{
	internal class GetOrdersByUserIdHandler : IRequestHandler<GetOrdersByUserIdQuery, IEnumerable<OrderDto>>
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IMapper _mapper;

		public GetOrdersByUserIdHandler(IOrderRepository orderRepository,
			IMapper mapper)
		{
			_orderRepository = orderRepository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<OrderDto>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
		{
			var orders = await _orderRepository.GetByUserAsync(request.UserId);

			return _mapper.Map<IEnumerable<OrderDto>>(orders);
		}
	}
}
