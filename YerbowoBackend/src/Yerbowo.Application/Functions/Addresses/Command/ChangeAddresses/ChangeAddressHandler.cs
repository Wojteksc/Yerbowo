namespace Yerbowo.Application.Functions.Addresses.Command.ChangeAddresses;

public class ChangeAddressHandler : IRequestHandler<ChangeAddressCommand>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public ChangeAddressHandler(
        IAddressRepository addressRepository,
        IMapper mapper,
        IStringLocalizer<SharedResource> localizer)
    {
        _addressRepository = addressRepository;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<Unit> Handle(ChangeAddressCommand request, CancellationToken cancellationToken)
    {
        var address = await _addressRepository.GetAsync(request.Id);

        if (address == null)
            throw new Exception(_localizer["ExceptionAddressNotFound"]);

        _mapper.Map(request, address);

        await _addressRepository.UpdateAsync(address);

        return Unit.Value;
    }
}