using AutoMapper;
using FluentAssertions;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Yerbowo.Application.Addresses.CreateAddresses;
using Yerbowo.Domain.Addresses;
using Yerbowo.Infrastructure.Data.Addresses;

namespace Yerbowo.Unit.Tests.Application.Addresses.CreateAddresses
{
    public class CreateAddressHandlerTest
    {
        [Fact]
        public async Task Create_Address_Should_Add_Correctly()
        {
            var address = new Address(
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
            "test@test.com");

            var command = new CreateAddressCommand()
            {
                Alias = "aliastTest",
                FirstName = "firstName",
                Lastname = "LastName",
                Street = "Street2",
                BuildingNumber = "15A",
                ApartmentNumber = "3",
                Place = "Place2",
                PostCode = "00-000",
                Phone = "000-000-000",
                Email = "test@test.com"
            };

            var mockAddressRepository = new Mock<IAddressRepository>();
            mockAddressRepository.Setup(x => x.AddAsync(address));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<Address>(command))
                .Returns(address);

            var createAddresHandler = new CreateAddressHandler(
                mockMapper.Object, 
                mockAddressRepository.Object);

            var result = await createAddresHandler.Handle(command, It.IsAny<CancellationToken>());

            result.Should().BeEquivalentTo(result);
            mockAddressRepository.Verify(x => x.AddAsync(address), Times.Once());
        }
    }
}