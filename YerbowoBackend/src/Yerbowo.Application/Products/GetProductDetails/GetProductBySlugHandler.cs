using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Yerbowo.Infrastructure.Data.Products;

namespace Yerbowo.Application.Products.GetProductDetails
{
	public class GetProductBySlugHandler : IRequestHandler<GetProductBySlugQuery, ProductDetailsDto>
	{
		private readonly IProductRepository _productRepository;
		private readonly IMapper _mapper;

		public GetProductBySlugHandler(IProductRepository productRepository,
			IMapper mapper)
		{
			_productRepository = productRepository;
			_mapper = mapper;
		}

		public async Task<ProductDetailsDto> Handle(GetProductBySlugQuery request, CancellationToken cancellationToken)
		{
			var product = await _productRepository.GetAsync(request.Slug, x => x
				.Include(p => p.Subcategory)
				.ThenInclude(s => s.Category));

			if (product == null)
				throw new Exception("Produkt nie istnieje");

			return _mapper.Map<ProductDetailsDto>(product);
		}
	}
}
