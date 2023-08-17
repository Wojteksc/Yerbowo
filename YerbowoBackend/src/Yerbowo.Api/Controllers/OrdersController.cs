namespace Yerbowo.Api.Controllers;

[Authorize]
[Route("api/users/{userId}/orders")]
[ApiController]
public class OrdersController : ApiControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public OrdersController(
        IMediator mediator,
        IStringLocalizer<SharedResource> localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrder(int userId, int orderId)
    {
        if (userId != UserId)
            return Unauthorized(_localizer["ResponseUnathorized"]);

        var order = await _mediator.Send(new GetOrderDetailsByIdQuery(orderId));

        return Ok(order);
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders(int userId)
    {
        if (userId != UserId)
            return Unauthorized(_localizer["ResponseUnathorized"]);

        var orders = await _mediator.Send(new GetOrdersByUserIdQuery(userId));

        return Ok(orders);
    }
}