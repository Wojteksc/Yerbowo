namespace Yerbowo.Application.Functions.Addresses.Command.CreateAddresses;

public class CreateAddressHandler(
    IMapper mapper,
    IAddressRepository addressRepository) : ICommandHandler<CreateAddressCommand, int>
{
    public async Task<int> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var address = mapper.Map<Address>(request);

        await addressRepository.AddAsync(address);

        return address.Id;
    }
}