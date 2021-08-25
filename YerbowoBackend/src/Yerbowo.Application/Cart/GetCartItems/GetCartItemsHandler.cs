using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Yerbowo.Application.Settings;

namespace Yerbowo.Application.Cart.GetCartItems
{
	public class GetCartItemsHandler : IRequestHandler<GetCartItemsQuery, CartDto>
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ISession _session;
		private readonly IMapper _mapper;

		public GetCartItemsHandler(IHttpContextAccessor httpContextAccessor,
			IMapper mapper)
		{
			_httpContextAccessor = httpContextAccessor;
			_session = _httpContextAccessor.HttpContext.Session;
			_mapper = mapper;
		}

		public Task<CartDto> Handle(GetCartItemsQuery request, CancellationToken cancellationToken)
		{
			List<CartItemDto> cartItems;

			if (_session.GetString(Consts.CartSessionKey) == null)
			{
				cartItems = new List<CartItemDto>();
			}
			else
			{
				var value = _session.GetString(Consts.CartSessionKey);
				cartItems = JsonConvert.DeserializeObject<List<CartItemDto>>(value);
			}

			return Task.FromResult(_mapper.Map<CartDto>(cartItems));
		}
	}
}
