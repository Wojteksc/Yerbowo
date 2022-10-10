namespace Yerbowo.Application.Functions.Cart.Command.ChangeCartItems;

public class ChangeCartItemCommand : IRequest<CartDto>
{
	public int Id { get; }
	public int Quantity { get; }

	public ChangeCartItemCommand(int id, int quantity)
	{
		Id = id;
		Quantity = quantity;
	}
}