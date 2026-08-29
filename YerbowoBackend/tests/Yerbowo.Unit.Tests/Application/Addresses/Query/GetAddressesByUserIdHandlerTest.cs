namespace Yerbowo.Unit.Tests.Application.Addresses.Query;

public class GetAddressesByUserIdHandlerTest
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock;
    private readonly GetAddressesByUserIdHandler _handler;

    Guid AddressId1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
    Guid AddressId2 = Guid.Parse("00000000-0000-0000-0000-000000000002");
    Guid UserId = Guid.Parse("10000000-0000-0000-0000-000000000001");

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
        var address1 = new Address(AddressId1, UserId, "aliasTest", "firstNameTest", "lastNameTest",
            "streetTest", "buildingNumberTest", "apartmentNumberTest", "placeTest",
            "postCodeTest", "phoneTest", "emailTest");

        var address2 = new Address(AddressId2, UserId, "aliasTest2", "firstNameTest2", "lastNameTest2",
            "streetTest2", "buildingNumberTest2", "apartmentNumberTest2", "placeTest2",
            "postCodeTest2", "phoneTest2", "emailTest2");

        var request = new GetAddressesByUserIdQuery(UserId);

        var expectedAddresses = new List<AddressDto>()
        {
            new AddressDto()
            {
                Id = AddressId1,
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
                Email = "emailTest",
            },
            new AddressDto()
            {
                Id = AddressId2,
                UserId = UserId,
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

        _addressRepositoryMock
            .Setup(x => x.GetAddresses(request.UserId))
            .ReturnsAsync(new List<Address>() { address1, address2 });

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().BeEquivalentTo(expectedAddresses);
    }
}