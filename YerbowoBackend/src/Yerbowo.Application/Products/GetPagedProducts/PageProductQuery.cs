using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Yerbowo.Application.Products.GetPagedProducts
{
	public class PageProductQuery : IRequest<PagedProductCardDto>
	{
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 20;

		[Required]
		public string Category { get; set; }
		[Required]
		public string Subcategory { get; set; }
	}
}
