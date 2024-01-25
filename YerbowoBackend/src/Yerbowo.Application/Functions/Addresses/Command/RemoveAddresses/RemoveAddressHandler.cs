namespace Yerbowo.Application.Functions.Addresses.Command.RemoveAddresses;

public class RemoveAddressHandler : IRequestHandler<RemoveAddressCommand>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public RemoveAddressHandler(IAddressRepository addressRepository, IStringLocalizer<SharedResource> localizer)
    {
        _addressRepository = addressRepository;
        _localizer = localizer;
    }

    public async Task Handle(RemoveAddressCommand request, CancellationToken cancellationToken)
    {
        var address = await _addressRepository.GetAsync(request.Id);

        if (address == null || address.IsRemoved)
        {
            throw new ArgumentException(_localizer["ExceptionAddressNotFound"]);
        }

        await _addressRepository.RemoveAsync(address);
    }
}