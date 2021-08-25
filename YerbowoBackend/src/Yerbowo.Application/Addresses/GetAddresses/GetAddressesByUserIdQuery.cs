using MediatR;
using System.Collections.Generic;

namespace Yerbowo.Application.Addresses.GetAddresses
{
	public class GetAddressesByUserIdQuery : IRequest<IEnumerable<AddressDto>>
	{
		public int UserId { get; }

		public GetAddressesByUserIdQuery(int userId)
		{
			UserId = userId;
		}
	}
}
