namespace Yerbowo.Unit.Tests.Application.Addresses.Command;

public class CreateAddressHandlerTest
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock;
    private readonly CreateAddressHandler _handler;

    public CreateAddressHandlerTest()
    {
        _addressRepositoryMock = new Mock<IAddressRepository>();

        _handler = new CreateAddressHandler(
            AutoMapperConfig.Initialize(),
            _addressRepositoryMock.Object);
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

        var expectedResult = new AddressDetailsDto
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

        var addresses = new List<Address>();

        _addressRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Address>()))
            .Callback<Address>(a => addresses.Add(a));

        var result = await _handler.Handle(command, CancellationToken.None);

        _addressRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Address>()), Times.Once());
        result.Should().BeEquivalentTo(expectedResult);
        addresses.Should().AllBeEquivalentTo(expectedInsertedAddress);
    }
}