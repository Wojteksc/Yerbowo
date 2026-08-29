namespace Yerbowo.Unit.Tests.Application.Orders.Query;

public class GetOrdersByUserIdHandlerTest
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly GetOrdersByUserIdHandler _handler;

    Guid OrderItemId1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
    Guid OrderItemId2 = Guid.Parse("00000000-0000-0000-0000-000000000002");
    Guid OrderId = Guid.Parse("10000000-0000-0000-0000-000000000001");
    Guid UserId = Guid.Parse("20000000-0000-0000-0000-000000000001");
    Guid ProductId1 = Guid.Parse("30000000-0000-0000-0000-000000000001");
    Guid ProductId2 = Guid.Parse("30000000-0000-0000-0000-000000000002");
    Guid AddressId = Guid.Parse("40000000-0000-0000-0000-000000000001");

    public GetOrdersByUserIdHandlerTest()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();

        _handler = new GetOrdersByUserIdHandler(
            _orderRepositoryMock.Object,
            AutoMapperConfig.Initialize());
    }

    [Fact]
    public async Task Should_GetOdersByUserIdCorrectly()
    {
        var dateTimeNow = DateTime.UtcNow;

        var orderItems = new List<OrderItem>()
        {
            new OrderItem(OrderItemId1, ProductId1, 4, 36.45m) { CreatedAt = dateTimeNow, UpdatedAt = dateTimeNow },
            new OrderItem(OrderItemId2, ProductId2, 1, 45m) { CreatedAt = dateTimeNow, UpdatedAt = dateTimeNow },
        };

        var order = new Order(OrderId, UserId, AddressId, OrderStatus.Completed, 36.45m + 45m, "", orderItems);

        var request = new GetOrdersByUserIdQuery(UserId);

        var expectedOrders = new List<OrderDto>()
        {
            new OrderDto
            {
                Id = OrderId,
                Status = "Skompletowane",
                Date = null,
                Total = 36.45m + 45m,
                ProductImages = new List<OrderProductImageDto> 
                {
                    new OrderProductImageDto { Quantity = 4 },
                    new OrderProductImageDto { Quantity = 1 }
                }
            }
        };

        _orderRepositoryMock
            .Setup(x => x.GetByUserAsync(UserId))
            .ReturnsAsync(new List<Order> { order });

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().BeEquivalentTo(expectedOrders, config =>
            config
            .Excluding(x => x.Id));
    }
}