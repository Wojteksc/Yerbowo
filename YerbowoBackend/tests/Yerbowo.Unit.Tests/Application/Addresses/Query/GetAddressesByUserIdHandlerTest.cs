namespace Yerbowo.Unit.Tests.Application.Addresses.Query;

public class GetAddressesByUserIdHandlerTest
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock;
    private readonly GetAddressesByUserIdHandler _handler;

    public GetAddressesByUserIdHandlerTest()
    {
        _addressRepositoryMock = new Mock<IAddressRepository>();

        _handler = new GetAddressesByUserIdHandler(
            AutoMapperConfig.Initialize(),
            _addressRepositoryMock.Object);
    }

    [Fact]
    public async Task Should_ReturnCorrectAddresses()
    {
        var address1 = new Address(1, "aliasTest", "firstNameTest", "lastNameTest",
            "streetTest", "buildingNumberTest", "apartmentNumberTest", "placeTest",
            "postCodeTest", "phoneTest", "emailTest");

        var address2 = new Address(1, "aliasTest2", "firstNameTest2", "lastNameTest2",
            "streetTest2", "buildingNumberTest2", "apartmentNumberTest2", "placeTest2",
            "postCodeTest2", "phoneTest2", "emailTest2");

        var request = new GetAddressesByUserIdQuery(1);

        var expectedAddresses = new List<AddressDto>()
        {
            new AddressDto()
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
                Email = "emailTest",
            },
            new AddressDto()
            {
                UserId = 1,
                Alias = "aliasTest2",
                FirstName = "firstNameTest2",
                LastName = "lastNameTest2",
                Street = "streetTest2",
                BuildingNumber = "buildingNumberTest2",
                ApartmentNumber = "apartmentNumberTest2",
                Place = "placeTest2",
                PostCode = "postCodeTest2",
                Phone = "phoneTest2",
                Email = "emailTest2"
            }
        };

        _addressRepositoryMock.Setup(x => x.GetAddresses(request.UserId))
            .ReturnsAsync(new List<Address>() { address1, address2 });

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().BeEquivalentTo(expectedAddresses);
    }
}