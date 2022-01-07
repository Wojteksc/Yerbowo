using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Yerbowo.Application.Addresses.RemoveAddresses;
using Yerbowo.Domain.Addresses;
using Yerbowo.Infrastructure.Data.Addresses;

namespace Yerbowo.Unit.Tests.Application.Addresses.RemoveAddress
{
    public class RemoveAddressHandlerTest
    {
        [Fact]
        public async Task Should_RemoveProductCorrectly()
        {
            var address = new Address(1, "aliasTest", "firstNameTest", "lastNameTest",
                "streetTest", "buildingNumberTest", "apartmentNumberTest", "placeTest",
                "postCodeTest", "phoneTest", "emailTest");

            var mockAddressRepository = new Mock<IAddressRepository>();
            mockAddressRepository.Setup(x => x.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(address);
            mockAddressRepository.Setup(x => x.RemoveAsync(address));

            var removeAddressHandler = new RemoveAddressHandler(mockAddressRepository.Object);
            await removeAddressHandler.Handle(new RemoveAddressComand(It.IsAny<int>()), It.IsAny<CancellationToken>());

            mockAddressRepository.Verify(x => x.RemoveAsync(address), Times.Once());
        }

        [Fact]
        public async Task Should_ThrowException_WhenProductIsNull()
        {
            var mockAddressRepository = new Mock<IAddressRepository>();
            mockAddressRepository.Setup(x => x.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Address>(null));

            var removeAddressHandler = new RemoveAddressHandler(mockAddressRepository.Object);

            Func<Task> act = () => removeAddressHandler.Handle(new RemoveAddressComand(It.IsAny<int>()), It.IsAny<CancellationToken>());
            var exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Adres nie istnieje", exception.Message);
        }

        [Fact]
        public async Task Should_ThrowException_WhenProductIsRemoved()
        {
            var address = new Address(1, "aliasTest", "firstNameTest", "lastNameTest",
    "streetTest", "buildingNumberTest", "apartmentNumberTest", "placeTest",
    "postCodeTest", "phoneTest", "emailTest");
            address.IsRemoved = true;

            var mockAddressRepository = new Mock<IAddressRepository>();
            mockAddressRepository.Setup(x => x.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(address);

            var removeAddressHandler = new RemoveAddressHandler(mockAddressRepository.Object);

            Func<Task> act = () => removeAddressHandler.Handle(new RemoveAddressComand(It.IsAny<int>()), It.IsAny<CancellationToken>());
            var exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Adres nie istnieje", exception.Message);
        }
    }
}