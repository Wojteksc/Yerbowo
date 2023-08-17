namespace Yerbowo.Application.Functions.Cart.Utils;

public static class CartSessionHelper
{
    public static List<CartItemDto> GetCartProducts(ISession session)
    {
        return session.GetObjectFromJson<List<CartItemDto>>(Consts.CartSessionKey) ?? new List<CartItemDto>();
    }

    public static void SaveCartProducts(ISession session, List<CartItemDto> products)
    {
        session.SetString(Consts.CartSessionKey, JsonSerializer.Serialize(products));
    }
}