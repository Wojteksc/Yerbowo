using Yerbowo.Application.Products.GetProductDetails;

namespace Yerbowo.Application.Cart
{
	public class CartItemDto
	{
		public CartProductItemDto Product { get; set; }
		public int Quantity { get; set; }
	}
}
