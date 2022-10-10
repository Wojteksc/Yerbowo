using Yerbowo.Application.Functions.Addresses.Query.GetAddressDetails;
using Yerbowo.Domain.Addresses;
using Yerbowo.Infrastructure.Data.Addresses;

namespace Yerbowo.Unit.Tests.Application.Addresses.Query;

public class GetAddressByIdHandlerTest
{
    [Fact]
    public async Task Should_ReturnAddress_When_ExistsInDatabase()
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

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(x => x.Map<AddressDetailsDto>(address))
            .Returns(addressDetailsDto);

        var mockAddressRepository = new Mock<IAddressRepository>();
        mockAddressRepository.Setup(x => x.GetAsync(addressQuery.Id))
            .ReturnsAsync(address);

        var getAddressByIdHandler = new GetAddressByIdHandler(
            mockMapper.Object,
            mockAddressRepository.Object);

        var result = await getAddressByIdHandler.Handle(addressQuery, It.IsAny<CancellationToken>());

        result.Should().Be(addressDetailsDto);
    }
}