namespace Yerbowo.Application.Functions.Cart.Utils;

public static class CartSessionHelper
{
    public static List<CartItemDto> GetCartProducts(ISession session) 
        => session.GetObjectFromJson<List<CartItemDto>>(SessionKeys.CartSession) ?? [];

    public static void SaveCartProducts(ISession session, List<CartItemDto> products) 
        => session.SetString(SessionKeys.CartSession, JsonSerializer.Serialize(products));
}