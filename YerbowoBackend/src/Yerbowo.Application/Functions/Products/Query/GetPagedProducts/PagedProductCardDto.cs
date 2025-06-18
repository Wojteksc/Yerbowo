namespace Yerbowo.Application.Functions.Products.Query.GetPagedProducts;

public record PagedProductCardDto
{
	public int PageNumber { get; init; }
	public int PageSize { get; init; }
	public int TotalPages { get; init; }
	public int TotalCount { get; init; }
	public List<ProductCardDto> Products { get; init; }
}