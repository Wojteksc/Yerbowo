namespace Yerbowo.Application.Functions.Cart.Command.AddCartItems;

public record AddCartItemCommand(int Id, int Quantity) : ICommand<CartDto> { }