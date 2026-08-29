namespace Yerbowo.Application.Functions.Orders.Query.GetOrderDetails;

public record GetOrderDetailsByIdQuery(Guid Id) : IQuery<OrderDetailsDto> { }