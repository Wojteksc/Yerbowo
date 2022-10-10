using Yerbowo.Application.Functions.Addresses.Query.GetAddressDetails;
using Yerbowo.Domain.Addresses;
using Yerbowo.Infrastructure.Data.Addresses;

namespace Yerbowo.Application.Functions.Addresses.Command.CreateAddresses;

public class CreateAddressHandler : IRequestHandler<CreateAddressCommand, AddressDetailsDto>
{
    private readonly IMapper _mapper;
    private readonly IAddressRepository _addressRepository;

    public CreateAddressHandler(IMapper mapper,
        IAddressRepository addressRepository)
    {
        _mapper = mapper;
        _addressRepository = addressRepository;
    }

    public async Task<AddressDetailsDto> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var address = _mapper.Map<Address>(request);

        await _addressRepository.AddAsync(address);

        return _mapper.Map<AddressDetailsDto>(address);
    }
}
