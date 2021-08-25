using MediatR;

namespace Yerbowo.Application.Products.GetProductDetails
{
	public class GetProductBySlugQuery : IRequest<ProductDetailsDto>
	{
		public string Slug { get; }

		public GetProductBySlugQuery(string slug)
		{
			Slug = slug;
		}
	}
}
