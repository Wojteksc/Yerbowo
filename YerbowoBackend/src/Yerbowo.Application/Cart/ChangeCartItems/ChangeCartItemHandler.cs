using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Yerbowo.Application.Cart.Utils;
using Yerbowo.Application.Extensions;
using Yerbowo.Application.Settings;
using Yerbowo.Infrastructure.Data.Products;

namespace Yerbowo.Application.Cart.ChangeCartItems
{
	public class ChangeCartItemHandler : IRequestHandler<ChangeCartItemCommand, CartDto>
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ISession _session;
		private readonly IProductRepository _productRepository;
		private readonly IMapper _mapper;

		public ChangeCartItemHandler(IHttpContextAccessor httpContextAccessor,
			IProductRepository productRepository,
			IMapper mapper)
		{
			_httpContextAccessor = httpContextAccessor;
			_session = _httpContextAccessor.HttpContext.Session;
			_productRepository = productRepository;
			_mapper = mapper;
		}

		public async Task<CartDto> Handle(ChangeCartItemCommand request, CancellationToken cancellationToken)
		{
			CartHelper.VerifyQuantity(request.Quantity);

			var products = _session.GetObjectFromJson<List<CartItemDto>>(Consts.CartSessionKey);

			var productIndex = products.FindIndex(x => x.Product.Id == request.Id);

			if (productIndex != -1)
			{
				await CartHelper.VerifyStock(_productRepository, request.Id, request.Quantity);

				products[productIndex].Quantity = request.Quantity;
				_session.SetString(Consts.CartSessionKey, JsonConvert.SerializeObject(products));
			}
			else
			{
				throw new Exception("Nie znaleziono produktu.");
			}


			return _mapper.Map<CartDto>(_session.GetObjectFromJson<List<CartItemDto>>(Consts.CartSessionKey));
		}
	}
}
