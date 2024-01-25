namespace Yerbowo.Unit.Tests.Application.Addresses.Command;

public class RemoveAddressHandlerTest
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock;
    private readonly RemoveAddressHandler _handler;
    private readonly Address _address;

    public RemoveAddressHandlerTest()
    {
        _addressRepositoryMock = new Mock<IAddressRepository>();
        _handler = new RemoveAddressHandler(_addressRepositoryMock.Object, StringLocalizerFactory.Create());

        _address = new Address(1, "aliasTest", "firstNameTest", "lastNameTest",
            "streetTest", "buildingNumberTest", "apartmentNumberTest", "placeTest",
            "postCodeTest", "phoneTest", "emailTest");
    }

    [Fact]
    public async Task Should_RemoveProductCorrectly()
    {
        var request = new RemoveAddressCommand(1);
        var addresses = new List<Address>();

        _addressRepositoryMock.Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync(_address);
        _addressRepositoryMock.Setup(x => x.RemoveAsync(_address))
            .Callback<Address>(a => addresses.Add(a));

        await _handler.Handle(request, CancellationToken.None);

        _addressRepositoryMock.Verify(x => x.RemoveAsync(_address), Times.Once());
        addresses.Should().AllBeEquivalentTo(_address);
    }

    [Theory]
    [InlineData("en-US", "Address not found")]
    [InlineData("pl-PL", "Nie znaleziono adresu")]
    public async Task Should_ThrowException_WhenProductIsNull(string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        var request = new RemoveAddressCommand(999);

        _addressRepositoryMock.Setup(x => x.GetAsync(request.Id))
            .Returns(Task.FromResult<Address>(null));

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        _addressRepositoryMock.Verify(x => x.RemoveAsync(It.IsAny<Address>()), Times.Never);
    }

    [Theory]
    [InlineData("en-US", "Address not found")]
    [InlineData("pl-PL", "Nie znaleziono adresu")]
    public async Task Should_ThrowException_WhenProductIsRemoved(string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        _address.IsRemoved = true;

        var request = new RemoveAddressCommand(2);

        _addressRepositoryMock
            .Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync(_address);

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        _addressRepositoryMock.Verify(x => x.RemoveAsync(It.IsAny<Address>()), Times.Never);
    }
}