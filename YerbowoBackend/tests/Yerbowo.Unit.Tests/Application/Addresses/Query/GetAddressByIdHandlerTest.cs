namespace Yerbowo.Unit.Tests.Application.Addresses.Query;

public class GetAddressByIdHandlerTest
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock;
    private readonly GetAddressByIdHandler _handler;

    Guid AddressId = Guid.Parse("00000000-0000-0000-0000-000000000001");
    Guid UserId = Guid.Parse("10000000-0000-0000-0000-000000000002");

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
        var address = new Address(AddressId, UserId, "aliasTest", "firstNameTest", "lastNameTest",
            "streetTest", "buildingNumberTest", "apartmentNumberTest", "placeTest",
            "postCodeTest", "phoneTest", "emailTest");

        var addressQuery = new GetAddressByIdQuery(AddressId);

        var addressDetailsDto = new AddressDetailsDto()
        {
            Id = AddressId,
            UserId = UserId,
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

        _addressRepositoryMock
            .Setup(x => x.GetAsync(addressQuery.Id))
            .ReturnsAsync(address);

        var result = await _handler.Handle(addressQuery, CancellationToken.None);

        result.Should().BeEquivalentTo(addressDetailsDto);
    }
}