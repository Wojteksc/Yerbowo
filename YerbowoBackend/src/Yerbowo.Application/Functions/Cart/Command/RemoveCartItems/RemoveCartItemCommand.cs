namespace Yerbowo.Application.Functions.Cart.Command.RemoveCartItems;

public record RemoveCartItemCommand(int ProductId) : ICommand<CartDto> { }