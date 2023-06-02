namespace Yerbowo.Unit.Tests.Application.Addresses.Query;

public class GetAddressByIdHandlerTest
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock;
    private readonly GetAddressByIdHandler _handler;

    public GetAddressByIdHandlerTest()
    {
        _addressRepositoryMock = new Mock<IAddressRepository>();

        _handler = new GetAddressByIdHandler(
            AutoMapperConfig.Initialize(),
            _addressRepositoryMock.Object);
    }

    [Fact]
    public async Task Should_ReturnAddressCorrectly()
    {
        var address = new Address(1, "aliasTest", "firstNameTest", "lastNameTest",
            "streetTest", "buildingNumberTest", "apartmentNumberTest", "placeTest",
            "postCodeTest", "phoneTest", "emailTest");

        var addressQuery = new GetAddressByIdQuery(1);

        var addressDetailsDto = new AddressDetailsDto()
        {
            UserId = 1,
            Alias = "aliasTest",
            FirstName = "firstNameTest",
            LastName = "lastNameTest",
            Street = "streetTest",
            BuildingNumber = "buildingNumberTest",
            ApartmentNumber = "apartmentNumberTest",
            Place = "placeTest",
            PostCode = "postCodeTest",
            Phone = "phoneTest",
            Email = "emailTest"
        };

        _addressRepositoryMock.Setup(x => x.GetAsync(addressQuery.Id))
            .ReturnsAsync(address);

        var result = await _handler.Handle(addressQuery, CancellationToken.None);

        result.Should().BeEquivalentTo(addressDetailsDto);
    }
}