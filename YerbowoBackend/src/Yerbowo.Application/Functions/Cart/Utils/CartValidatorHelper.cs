namespace Yerbowo.Application.Functions.Cart.Utils;

public static class CartValidatorHelper
{
    public static void VerifyStock(Product product, int cartItemQuantity, IStringLocalizer<SharedResource> localizer)
	{
		if (cartItemQuantity > product.Stock)
			throw new CartStockExceeededException(localizer);
	}

	public static void VerifyQuantity(int quantity, IStringLocalizer<SharedResource> localizer)
	{
		if (quantity < 1)
			throw new CartStockIsIncorrectException(localizer);
	}
}