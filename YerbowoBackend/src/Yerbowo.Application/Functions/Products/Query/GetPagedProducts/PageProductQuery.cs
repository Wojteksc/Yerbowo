namespace Yerbowo.Application.Functions.Products.Query.GetPagedProducts;

public record PageProductQuery : IQuery<PagedProductCardDto>
{
	public int PageNumber { get; init; } = 1;
	public int PageSize { get; init; } = 20;
	[Required]
	public string Category { get; init; }
	[Required]
	public string Subcategory { get; init; }
}