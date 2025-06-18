namespace Yerbowo.Application.Functions.Orders.Query.GetOrderDetails;

public record GetOrderDetailsByIdQuery(int Id) : IQuery<OrderDetailsDto> { }