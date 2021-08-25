using MediatR;
using System.Collections.Generic;

namespace Yerbowo.Application.Cart.ChangeCartItems
{
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
}
