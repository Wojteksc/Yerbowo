using MediatR;
using System.Collections.Generic;

namespace Yerbowo.Application.Cart.AddCartItems
{
	public class AddCartItemCommand : IRequest<CartDto>
    {
		public int Id { get; }
		public int Quantity { get; }

		public AddCartItemCommand(int id, int quantity)
		{
			Id = id;
			Quantity = quantity;
		}
	}
}
