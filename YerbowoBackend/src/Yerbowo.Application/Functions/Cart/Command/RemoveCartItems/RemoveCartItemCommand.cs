namespace Yerbowo.Application.Functions.Cart.Command.RemoveCartItems;

public class RemoveCartItemCommand : IRequest<CartDto>
{
	public int ProductId { get; }

	public RemoveCartItemCommand(int productId)
	{
		ProductId = productId;
	}
}