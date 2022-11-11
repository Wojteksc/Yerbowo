namespace Yerbowo.Unit.Tests.Application.Addresses.Query;

public class GetAddressesByUserIdHandlerTest
{
    [Fact]
    public async Task Should_ReturnAddresses_When_ExistInDatabase()
    {
        var address1 = new Address(1, "aliasTest", "firstNameTest", "lastNameTest",
            "streetTest", "buildingNumberTest", "apartmentNumberTest", "placeTest",
            "postCodeTest", "phoneTest", "emailTest");

        var address2 = new Address(1, "aliasTest2", "firstNameTest2", "lastNameTest2",
"streetTest2", "buildingNumberTest2", "apartmentNumberTest2", "placeTest2",
"postCodeTest2", "phoneTest2", "emailTest2");

        var address3 = new Address(2, "aliasTest2", "firstNameTest2", "lastNameTest2",
"streetTest2", "buildingNumberTest2", "apartmentNumberTest2", "placeTest2",
"postCodeTest2", "phoneTest2", "emailTest2");

        var addresses = new List<Address>() { address1, address2, address3 };

        var addressQuery = new GetAddressesByUserIdQuery(1);

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

        var mockAddressRepository = new Mock<IAddressRepository>();
        mockAddressRepository.Setup(x => x.GetAddresses(It.IsAny<int>()))
            .ReturnsAsync(new List<Address>() { address1, address2 });

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(x => x.Map<IEnumerable<AddressDto>>(It.IsAny<List<Address>>()))
            .Returns(expectedAddresses);

        var getAddressesByUserIdHandler = new GetAddressesByUserIdHandler(
            mockMapper.Object,
            mockAddressRepository.Object);

        var result = await getAddressesByUserIdHandler.Handle(addressQuery, It.IsAny<CancellationToken>());

        result.Should().BeEquivalentTo(expectedAddresses);
    }
}