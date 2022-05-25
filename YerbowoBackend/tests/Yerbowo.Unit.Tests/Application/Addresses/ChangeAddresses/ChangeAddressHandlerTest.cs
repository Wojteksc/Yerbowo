using Yerbowo.Application.Addresses.ChangeAddresses;
using Yerbowo.Domain.Addresses;
using Yerbowo.Infrastructure.Data.Addresses;

namespace Yerbowo.Unit.Tests.Application.Addresses.ChangeAddresses;

public class ChangeAddressHandlerTest
{
    [Fact]
    public async Task Should_ChangeAddress_When_CommandHasCorrectData()
    {

        var address = new Address(1,
            "aliastTest",
            "firstName",
            "LastName",
            "Street",
            "15A",
            "3",
            "Place",
            "00-000",
            "000-000-000",
            "test@test.com");

        var command = new ChangeAddressCommand()
        {
           Id = 1,
           Alias = "aliastTest",
           FirstName = "firstName",
           LastName = "LastName",
           Street = "Street2",
           BuildingNumber = "2",
           ApartmentNumber = "30",
           Place = "Place2",
           PostCode = "23-423",
           Phone = "000-000-000",
           Email = "test@test.com"
        };

        var mockAddressRepository = new Mock<IAddressRepository>();
        mockAddressRepository.Setup(x => x.GetAsync(It.IsAny<int>()))
            .ReturnsAsync(address);

        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(x => x.Map(command, address))
            .Returns(address);

        mockAddressRepository.Setup(x => x.UpdateAsync(address));

        var changeAddresHandler = new ChangeAddressHandler(
            mockAddressRepository.Object,
            mockMapper.Object);

        await changeAddresHandler.Handle(command, It.IsAny<CancellationToken>());

        mockAddressRepository.Verify(x => x.UpdateAsync(address), Times.Once());
    }

    [Fact]
    public async Task Should_ThrowException_When_AddressDoesNotExist()
    {
        var mockAddressRepository = new Mock<IAddressRepository>();
        mockAddressRepository.Setup(x => x.GetAsync(It.IsAny<int>()))
            .Returns(Task.FromResult<Address>(null));

        var mockMapper = new Mock<IMapper>();

        var changeAddresHandler = new ChangeAddressHandler(
            mockAddressRepository.Object,
            mockMapper.Object);

        Func<Task> act = () => changeAddresHandler.Handle(new ChangeAddressCommand(), It.IsAny<CancellationToken>());
        var exception = await Assert.ThrowsAsync<Exception>(act);
        Assert.Equal("Nie znaleziono adresu", exception.Message);
    }
}