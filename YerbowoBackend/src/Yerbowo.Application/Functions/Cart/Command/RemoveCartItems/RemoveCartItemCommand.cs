namespace Yerbowo.Application.Functions.Cart.Command.RemoveCartItems;

public record RemoveCartItemCommand(Guid ProductId) : ICommand<CartDto> { }