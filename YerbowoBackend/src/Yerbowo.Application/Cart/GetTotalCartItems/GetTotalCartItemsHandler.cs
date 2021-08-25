using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Yerbowo.Application.Extensions;
using Yerbowo.Application.Settings;

namespace Yerbowo.Application.Cart.GetTotalCartItems
{
	public class GetTotalCartItemsHandler : IRequestHandler<GetTotalCartItemsQuery, int>
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ISession _session;
		private readonly IMapper _mapper;

		public GetTotalCartItemsHandler(IHttpContextAccessor httpContextAccessor,
			IMapper mapper)
		{
			_httpContextAccessor = httpContextAccessor;
			_session = _httpContextAccessor.HttpContext.Session;
			_mapper = mapper;
		}

		public Task<int> Handle(GetTotalCartItemsQuery request, CancellationToken cancellationToken)
		{
			var cartItems = _session.GetObjectFromJson<List<CartItemDto>>(Consts.CartSessionKey);
			int totalCartItems = cartItems != null ? cartItems.Sum(ci => ci.Quantity) : 0;
			return Task.FromResult(totalCartItems);
		}
	}
}
