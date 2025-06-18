namespace Yerbowo.Application.Functions.Addresses.Query.GetAddresses;

public class GetAddressesByUserIdHandler(
    IMapper mapper,
    IAddressRepository addressRepository) : IQueryHandler<GetAddressesByUserIdQuery, IEnumerable<AddressDto>>
{
    public async Task<IEnumerable<AddressDto>> Handle(GetAddressesByUserIdQuery request, CancellationToken cancellationToken)
    {
        var addresses = await addressRepository.GetAddresses(request.UserId);

        return mapper.Map<IEnumerable<AddressDto>>(addresses);
    }
}