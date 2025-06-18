namespace Yerbowo.Application.Functions.Addresses.Command.RemoveAddresses;

public class RemoveAddressHandler(
    IAddressRepository addressRepository, 
    IStringLocalizer<SharedResource> localizer) : ICommandHandler<RemoveAddressCommand>
{
    public async Task Handle(RemoveAddressCommand request, CancellationToken cancellationToken)
    {
        var address = await addressRepository.GetAsync(request.Id);

        if (address == null || address.IsRemoved)
        {
            throw new AddressNotFoundException(localizer);
        }

        await addressRepository.RemoveAsync(address);
    }
}