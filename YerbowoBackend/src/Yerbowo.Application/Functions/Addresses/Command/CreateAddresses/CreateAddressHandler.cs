namespace Yerbowo.Application.Functions.Addresses.Command.CreateAddresses;

public class CreateAddressHandler(
    IAddressRepository addressRepository,
    IIdGenerator idGenerator) : ICommandHandler<CreateAddressCommand, Guid>
{
    public async Task<Guid> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var address = new Address(
            idGenerator.Generate(),
            request.UserId,
            request.Alias,
            request.FirstName,
            request.LastName,
            request.Street,
            request.BuildingNumber,
            request.ApartmentNumber,
            request.Place,
            request.PostCode,
            request.Phone,
            request.Email,
            request.Nip,
            request.Company
        );

        await addressRepository.AddAsync(address);

        return address.Id;
    }
}