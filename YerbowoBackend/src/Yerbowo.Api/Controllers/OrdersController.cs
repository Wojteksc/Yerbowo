namespace Yerbowo.Api.Controllers;

[Authorize]
[Route("api/users/{userId}/orders")]
[ApiController]
public class OrdersController(IRequestDispatcher dispatcher) : ApiControllerBase
{
    [HttpGet("{orderId}")]
    [UnathorizedFilter]
    public async Task<ActionResult<OrderDetailsDto>> GetOrder(int userId, int orderId)
    {
        var order = await dispatcher.ExecuteQuery(new GetOrderDetailsByIdQuery(orderId));

        return Ok(order);
    }

    [HttpGet]
    [UnathorizedFilter]
    public async Task<ActionResult<OrderDto>> GetOrders(int userId)
    {
        var orders = await dispatcher.ExecuteQuery(new GetOrdersByUserIdQuery(userId));

        return Ok(orders);
    }
}