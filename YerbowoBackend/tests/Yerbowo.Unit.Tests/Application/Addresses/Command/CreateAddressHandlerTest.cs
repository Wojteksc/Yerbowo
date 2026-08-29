namespace Yerbowo.Unit.Tests.Application.Addresses.Command;

public class CreateAddressHandlerTest
{
    private readonly Mock<IAddressRepository> _addressRepository = new();
    private readonly Mock<IIdGenerator> _idGenerator = new();

    private readonly CreateAddressHandler _handler;

    Guid AddressId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public CreateAddressHandlerTest()
    {
        _handler = new CreateAddressHandler(
            _addressRepository.Object,
            _idGenerator.Object);
    }

    [Fact]
    public async Task Should_CreateAddressCorrectly()
    {
        var command = new CreateAddressCommand()
        {
            UserId = Guid.Parse("12345678-1234-1234-1234-123456789012"),
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
            AddressId,
            Guid.Parse("12345678-1234-1234-1234-123456789012"),
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

        Address insertedAddress = null;

        _addressRepository
            .Setup(x => x.AddAsync(It.IsAny<Address>()))
            .Callback<Address>(a => 
            {
                insertedAddress = a;
            });

        _idGenerator.Setup(x => x.Generate()).Returns(AddressId);

        Guid addressId = await _handler.Handle(command, CancellationToken.None);

        insertedAddress.Should().BeEquivalentTo(expectedInsertedAddress);
        addressId.Should().Be(expectedInsertedAddress.Id);
    }
}