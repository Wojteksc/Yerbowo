namespace Yerbowo.Application.Functions.Products.Query.GetPagedProducts;

public class PagedProductCardDto
{
	public int PageNumber { get; set; }
	public int PageSize { get; set; }
	public int TotalPages { get; set; }
	public int TotalCount { get; set; }
	public List<ProductCardDto> Products { get; set; }
}