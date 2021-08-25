using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Yerbowo.Domain.Products;
using Yerbowo.Infrastructure.Data.Products;

namespace Yerbowo.Application.Products.CreateProducts
{
	public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductCommand>
	{
		private readonly IProductRepository _productRepository;
		private readonly IMapper _mapper;

		public CreateProductHandler(IProductRepository productRepository,
			IMapper mapper)
		{
			_productRepository = productRepository;
			_mapper = mapper;
		}

		public async Task<CreateProductCommand> Handle(CreateProductCommand request, CancellationToken cancellationToken)
		{
			//TO DO Veryfication by slug or name
			//if (await _productRepository.ExistsAsync(request.Id))
			//	throw new Exception("Nie znaleziono produktu");

			var product = _mapper.Map<Product>(request);

			await _productRepository.AddAsync(product);

			return _mapper.Map <CreateProductCommand>(product);
		}
	}
}
