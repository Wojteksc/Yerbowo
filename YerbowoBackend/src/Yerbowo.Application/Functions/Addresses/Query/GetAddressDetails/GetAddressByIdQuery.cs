namespace Yerbowo.Application.Functions.Addresses.Query.GetAddressDetails;

public record GetAddressByIdQuery(int Id) : IQuery<AddressDetailsDto> { }