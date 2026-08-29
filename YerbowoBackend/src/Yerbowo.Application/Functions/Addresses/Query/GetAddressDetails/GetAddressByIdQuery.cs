namespace Yerbowo.Application.Functions.Addresses.Query.GetAddressDetails;

public record GetAddressByIdQuery(Guid Id) : IQuery<AddressDetailsDto> { }