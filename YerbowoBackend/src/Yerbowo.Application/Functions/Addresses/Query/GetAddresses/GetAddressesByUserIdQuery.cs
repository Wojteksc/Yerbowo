namespace Yerbowo.Application.Functions.Addresses.Query.GetAddresses;

public class GetAddressesByUserIdQuery : IRequest<IEnumerable<AddressDto>>
{
	public int UserId { get; }

	public GetAddressesByUserIdQuery(int userId)
	{
		UserId = userId;
	}
}