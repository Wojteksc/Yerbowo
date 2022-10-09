namespace Yerbowo.Application.Functions.Products.Query.GetProductDetails;

public class GetProductBySlugQuery : IRequest<ProductDetailsDto>
{
	public string Slug { get; }

	public GetProductBySlugQuery(string slug)
	{
		Slug = slug;
	}
}