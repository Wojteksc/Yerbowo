using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Yerbowo.Application.Orders.GetOrderDetails;
using Yerbowo.Application.Orders.GetOrders;

namespace Yerbowo.Api.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/orders")]
    [ApiController]
    public class OrdersController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(int userId, int orderId)
        {
            if (userId != UserId)
                return Unauthorized();

            var order = await _mediator.Send(new GetOrderDetailsByIdQuery(orderId));

            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders(int userId)
        {
            if (userId != UserId)
                return Unauthorized();

            var orders = await _mediator.Send(new GetOrdersByUserIdQuery(userId));

            return Ok(orders);
        }
    }
}