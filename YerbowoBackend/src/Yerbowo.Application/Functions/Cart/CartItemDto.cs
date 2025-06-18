namespace Yerbowo.Application.Functions.Cart;

public record CartItemDto
{
	public CartProductItemDto Product { get; init; }
	public int Quantity { get; set; }
}