namespace Yerbowo.Application.Functions.Addresses.Query.GetAddressDetails;

public class GetAddressByIdHandler : IRequestHandler<GetAddressByIdQuery, AddressDetailsDto>
{
    private readonly IMapper _mapper;
    private readonly IAddressRepository _addressRepository;

    public GetAddressByIdHandler(IMapper mapper, IAddressRepository addressRepository)
    {
        _mapper = mapper;
        _addressRepository = addressRepository;
    }

    public async Task<AddressDetailsDto> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
    {
        var address = await _addressRepository.GetAsync(request.Id);

        return _mapper.Map<AddressDetailsDto>(address);
    }
}