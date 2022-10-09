namespace Yerbowo.Application.Functions.Products.Command.RemoveProducts;

public class RemoveProductCommand : IRequest
{
	public int Id { get; }

	public RemoveProductCommand(int id)
	{
		Id = id;
	}
}