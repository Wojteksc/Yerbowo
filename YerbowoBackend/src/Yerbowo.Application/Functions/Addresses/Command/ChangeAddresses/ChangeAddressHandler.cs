namespace Yerbowo.Application.Functions.Addresses.Command.ChangeAddresses;

public class ChangeAddressHandler(
    IAddressRepository addressRepository,
    IMapper mapper,
    IStringLocalizer<SharedResource> localizer) : ICommandHandler<ChangeAddressCommand>
{
    public async Task Handle(ChangeAddressCommand request, CancellationToken cancellationToken)
    {
        var address = await addressRepository.GetAsync(request.Id) 
            ?? throw new AddressNotFoundException(localizer);
        
        mapper.Map(request, address);

        await addressRepository.UpdateAsync(address);
    }
}