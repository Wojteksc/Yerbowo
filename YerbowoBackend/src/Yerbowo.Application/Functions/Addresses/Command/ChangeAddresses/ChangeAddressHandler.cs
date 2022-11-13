﻿namespace Yerbowo.Application.Functions.Addresses.Command.ChangeAddresses;

public class ChangeAddressHandler : IRequestHandler<ChangeAddressCommand>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IMapper _mapper;

    public ChangeAddressHandler(IAddressRepository addressRepository,
        IMapper mapper)
    {
        _addressRepository = addressRepository;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(ChangeAddressCommand request, CancellationToken cancellationToken)
    {
        var address = await _addressRepository.GetAsync(request.Id);

        if (address == null)
            throw new Exception("Nie znaleziono adresu");

        _mapper.Map(request, address);

        await _addressRepository.UpdateAsync(address);

        return Unit.Value;
    }
}