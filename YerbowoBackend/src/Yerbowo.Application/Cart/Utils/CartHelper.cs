using System;
using System.Threading.Tasks;
using Yerbowo.Application.Cart.ChangeCartItems;
using Yerbowo.Infrastructure.Data.Products;

namespace Yerbowo.Application.Cart.Utils
{
	public static class CartHelper
    {
		public static async Task VerifyStock(IProductRepository productRepository, int cartItemId, int cartItemQuantity)
		{
			var productDb = await productRepository.GetAsync(cartItemId);

			if (cartItemQuantity > productDb.Stock)
				throw new Exception("Przekroczono zapas");
		}

		public static void VerifyQuantity(int quantity)
		{
			if (quantity < 1)
			{
				throw new Exception("Niepraidłowa ilość");
			}
		}
	}
}
