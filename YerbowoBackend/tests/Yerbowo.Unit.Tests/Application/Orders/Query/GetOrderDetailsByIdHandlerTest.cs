namespace Yerbowo.Unit.Tests.Application.Orders.Query;

public class GetOrderDetailsByIdHandlerTest
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly GetOrderDetailsByIdHandler _handler;

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
        var dateTimeNow = DateTime.Now;

        var orderItems = new List<OrderItem>()
        {
            new OrderItem(productId: 4, quantity: 4, price: 36.45m) { CreatedAt = dateTimeNow, UpdatedAt = dateTimeNow },
            new OrderItem(productId: 6, quantity: 1, price: 45m) { CreatedAt = dateTimeNow, UpdatedAt = dateTimeNow },
        };

        var order = new Order(1, 1, OrderStatus.Completed, 36.45m + 45m, "", orderItems);

        var request = new GetOrderDetailsByIdQuery(1);

        var expectedOrder = new OrderDetailsDto()
        {
            Id = 1,
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

        _orderRepositoryMock.Setup(x => x.GetAsync(1))
            .ReturnsAsync(order);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().BeEquivalentTo(expectedOrder, config =>
            config
            .Excluding(x => x.Id)
            .Excluding(x => x.Address));
    }
}