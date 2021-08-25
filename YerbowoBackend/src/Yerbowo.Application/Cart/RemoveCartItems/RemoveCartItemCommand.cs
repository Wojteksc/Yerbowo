using MediatR;
using System.Collections.Generic;

namespace Yerbowo.Application.Cart.RemoveCartItems
{
	public class RemoveCartItemCommand : IRequest<CartDto>
    {
		public int ProductId { get; }

		public RemoveCartItemCommand(int productId)
		{
			ProductId = productId;
		}

	}
}
