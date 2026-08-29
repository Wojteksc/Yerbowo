namespace Yerbowo.Application.Functions.Cart.Command.AddCartItems;

public record AddCartItemCommand(Guid Id, int Quantity) : ICommand<CartDto> { }