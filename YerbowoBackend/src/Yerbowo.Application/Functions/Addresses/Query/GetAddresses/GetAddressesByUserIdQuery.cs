namespace Yerbowo.Application.Functions.Addresses.Query.GetAddresses;

public record GetAddressesByUserIdQuery(Guid UserId) : IQuery<IEnumerable<AddressDto>> { }