namespace Yerbowo.Unit.Tests.Application.Orders.Query;

public class GetOrderDetailsByIdHandlerTest
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly GetOrderDetailsByIdHandler _handler;

    Guid OrderItemId1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
    Guid OrderItemId2 = Guid.Parse("00000000-0000-0000-0000-000000000002");
    Guid OrderId = Guid.Parse("10000000-0000-0000-0000-000000000001");
    Guid UserId = Guid.Parse("20000000-0000-0000-0000-000000000001");
    Guid ProductId1 = Guid.Parse("30000000-0000-0000-0000-000000000001");
    Guid ProductId2 = Guid.Parse("30000000-0000-0000-0000-000000000002");
    Guid AddressId = Guid.Parse("40000000-0000-0000-0000-000000000001");

    public GetOrderDetailsByIdHandlerTest()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();

        _handler = new GetOrderDetailsByIdHandler(
            _orderRepositoryMock.Object,
            AutoMapperConfig.Initialize());
    }

    [Fact]
    public async Task Should_GetOderDetailsCorrectly()
    {
        var dateTimeNow = DateTime.UtcNow;

        var orderItems = new List<OrderItem>()
        {
            new OrderItem(OrderItemId1, ProductId1, 4, 36.45m) { CreatedAt = dateTimeNow, UpdatedAt = dateTimeNow },
            new OrderItem(OrderItemId2, ProductId2, 1, 45m) { CreatedAt = dateTimeNow, UpdatedAt = dateTimeNow },
        };

        var order = new Order(OrderId, UserId, AddressId, OrderStatus.Completed, 36.45m + 45m, "", orderItems);

        var request = new GetOrderDetailsByIdQuery(OrderId);

        var expectedOrder = new OrderDetailsDto()
        {
            Id = OrderId,
            Address = new AddressDto(),
            OrderItems = new List<OrderItemDto>
            {
                new OrderItemDto
                {
                    Quantity = 4,
                    Price = 36.45m,
                    Sum = 145.80m
                },
                new OrderItemDto
                {
                    Quantity = 1,
                    Price = 45,
                    Sum = 45
                }
            },
            TotalCost = 36.45m + 45m
        };

        _orderRepositoryMock
            .Setup(x => x.GetAsync(OrderId))
            .ReturnsAsync(order);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().BeEquivalentTo(expectedOrder, config =>
            config
            .Excluding(x => x.Id)
            .Excluding(x => x.Address));
    }
}