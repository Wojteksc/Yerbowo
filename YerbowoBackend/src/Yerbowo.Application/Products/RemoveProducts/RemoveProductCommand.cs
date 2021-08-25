using MediatR;

namespace Yerbowo.Application.Products.DeleteProducts
{
	public class RemoveProductCommand : IRequest
	{
		public int Id { get; }

		public RemoveProductCommand(int id)
		{
			Id = id;
		}

	}
}
