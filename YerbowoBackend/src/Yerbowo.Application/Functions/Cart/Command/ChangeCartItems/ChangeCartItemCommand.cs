namespace Yerbowo.Application.Functions.Cart.Command.ChangeCartItems;

public record ChangeCartItemCommand(Guid Id, int Quantity) : ICommand<CartDto>, ICommandIdentity { }