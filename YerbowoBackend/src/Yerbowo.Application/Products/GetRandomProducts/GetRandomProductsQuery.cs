using MediatR;

namespace Yerbowo.Application.Products.GetRandomProducts
{
	public class GetRandomProductsQuery : IRequest<RandomProductsDto>
	{
	}
}
