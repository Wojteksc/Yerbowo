namespace Yerbowo.Application.Functions.Cart.Utils;

public static class CartHelper
{
	public static List<CartItemDto> GetCartProducts(ISession session)
	{
		return session.GetObjectFromJson<List<CartItemDto>>(Consts.CartSessionKey) ?? new List<CartItemDto>();
	}

    public static void SaveCartProducts(ISession session, List<CartItemDto> products)
	{
        session.SetString(Consts.CartSessionKey, JsonSerializer.Serialize(products));
    }

    public static void VerifyStock(Product product, int cartItemQuantity)
	{
		if (cartItemQuantity > product.Stock)
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