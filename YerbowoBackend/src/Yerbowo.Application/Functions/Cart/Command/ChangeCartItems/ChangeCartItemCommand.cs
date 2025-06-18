namespace Yerbowo.Application.Functions.Cart.Command.ChangeCartItems;

public record ChangeCartItemCommand(int Id, int Quantity) : ICommand<CartDto>, ICommandIdentity { }