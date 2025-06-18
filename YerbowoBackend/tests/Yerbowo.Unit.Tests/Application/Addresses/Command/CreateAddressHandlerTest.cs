namespace Yerbowo.Unit.Tests.Application.Addresses.Command;

public class CreateAddressHandlerTest
{
    private readonly Mock<IAddressRepository> _addressRepository;

    private readonly CreateAddressHandler _handler;

    const int AddressId = 1000;

    public CreateAddressHandlerTest()
    {
        _addressRepository = new();

        _handler = new CreateAddressHandler(
            AutoMapperConfig.Initialize(),
            _addressRepository.Object);
    }

    [Fact]
    public async Task Should_CreateAddressCorrectly()
    {
        var command = new CreateAddressCommand()
        {
            UserId = 1,
            Alias = "aliastTest",
            FirstName = "firstName",
            LastName = "LastName",
            Street = "Street",
            BuildingNumber = "15A",
            ApartmentNumber = "3",
            Place = "Place",
            PostCode = "00-000",
            Phone = "000-000-000",
            Email = "test@test.com",
            Nip = "1156301130",
            Company = "Company_1"
        };

        var expectedInsertedAddress = new Address(
            1,
            "aliastTest",
            "firstName",
            "LastName",
            "Street",
            "15A",
            "3",
            "Place",
            "00-000",
            "000-000-000",
            "test@test.com",
            "1156301130",
            "Company_1");

        typeof(Address).GetProperty(nameof(Address.Id)).SetValue(expectedInsertedAddress, AddressId, null);

        var addresses = new List<Address>();

        _addressRepository
            .Setup(x => x.AddAsync(It.IsAny<Address>()))
            .Callback<Address>(a => 
            {
                typeof(Address).GetProperty(nameof(Address.Id)).SetValue(a, AddressId, null);
                addresses.Add(a); 
            });

        int addressId = await _handler.Handle(command, CancellationToken.None);

        addresses.Should().AllBeEquivalentTo(expectedInsertedAddress);
        addressId.Should().Be(AddressId);
    }
}