namespace Yerbowo.Application.Functions.Cart;

public record CartDto
{
	public List<CartItemDto> Items { get; init; }
	public decimal Sum { get; init; }
	public int TotalItems { get; init; } 
}