namespace Yerbowo.Application.Functions.Addresses.Command.RemoveAddresses;

public class RemoveAddressHandler : IRequestHandler<RemoveAddressComand>
{
    private readonly IAddressRepository _addressRepository;

    public RemoveAddressHandler(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<Unit> Handle(RemoveAddressComand request, CancellationToken cancellationToken)
    {
        var address = await _addressRepository.GetAsync(request.Id);

        if (address == null || address.IsRemoved)
        {
            throw new Exception("Adres nie istnieje");
        }

        await _addressRepository.RemoveAsync(address);

        return Unit.Value;
    }
}
