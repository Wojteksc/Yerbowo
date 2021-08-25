using System.Collections.Generic;

namespace Yerbowo.Application.Cart
{
	public class CartDto
	{
		public List<CartItemDto> Items { get; set; }
		public decimal Sum { get; set; }
		public int TotalItems { get; set; } 
	}
}
