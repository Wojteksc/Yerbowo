namespace Yerbowo.Application.Functions.Addresses.Query.GetAddressDetails;

public class GetAddressByIdHandler(
    IMapper mapper, 
    IAddressRepository addressRepository) : IQueryHandler<GetAddressByIdQuery, AddressDetailsDto>
{
    public async Task<AddressDetailsDto> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
    {
        var address = await addressRepository.GetAsync(request.Id);

        return mapper.Map<AddressDetailsDto>(address);
    }
}