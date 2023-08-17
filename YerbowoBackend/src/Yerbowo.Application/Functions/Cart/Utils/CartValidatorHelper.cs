namespace Yerbowo.Application.Functions.Cart.Utils;

public static class CartValidatorHelper
{
    public static void VerifyStock(Product product, int cartItemQuantity, IStringLocalizer<SharedResource> localizer)
	{
		if (cartItemQuantity > product.Stock)
			throw new Exception(localizer["ExceptionStockExceeded"]);
	}

	public static void VerifyQuantity(int quantity, IStringLocalizer<SharedResource> localizer)
	{
		if (quantity < 1)
		{
			throw new Exception(localizer["ExceptionIncorrectQuantity"]);
		}
	}
}