namespace Yerbowo.Application.Functions.Addresses.Query.GetAddresses;

public record GetAddressesByUserIdQuery(int UserId) : IQuery<IEnumerable<AddressDto>> { }