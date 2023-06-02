namespace Yerbowo.Unit.Tests.Application.Orders.Query;

public class GetOrdersByUserIdHandlerTest
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly GetOrdersByUserIdHandler _handler;

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
        var dateTimeNow = DateTime.Now;

        var orderItems = new List<OrderItem>()
        {
            new OrderItem(productId: 4, quantity: 4, price: 36.45m) { CreatedAt = dateTimeNow, UpdatedAt = dateTimeNow },
            new OrderItem(productId: 6, quantity: 1, price: 45m) { CreatedAt = dateTimeNow, UpdatedAt = dateTimeNow },
        };

        var order = new Order(1, 1, OrderStatus.Completed, 36.45m + 45m, "", orderItems);

        var request = new GetOrdersByUserIdQuery(1);

        var expectedOrders = new List<OrderDto>()
        {
            new OrderDto()
            {
                Status = "Skompletowane",
                Date = DateTime.MinValue.ToString(),
                Total = 36.45m + 45m,
                ProductImages = new List<OrderProductImageDto> 
                {
                    new OrderProductImageDto { Quantity = 4 },
                    new OrderProductImageDto { Quantity = 1 }
                }
            }
        };

        _orderRepositoryMock.Setup(x => x.GetByUserAsync(1))
            .ReturnsAsync(new List<Order> { order });

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().BeEquivalentTo(expectedOrders, config =>
            config
            .Excluding(x => x.Id));
    }
}