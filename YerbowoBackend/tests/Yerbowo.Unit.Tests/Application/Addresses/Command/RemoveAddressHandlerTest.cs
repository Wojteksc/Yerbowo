namespace Yerbowo.Unit.Tests.Application.Addresses.Command;

public class RemoveAddressHandlerTest
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock;
    private readonly RemoveAddressHandler _handler;
    private readonly Address _address;

    private IStringLocalizer<SharedResource> _localizer;

    public RemoveAddressHandlerTest()
    {
        _addressRepositoryMock = new();

        _localizer = StringLocalizerFactory.Create();

        _handler = new RemoveAddressHandler(_addressRepositoryMock.Object, _localizer);

        _address = new Address(1, "aliasTest", "firstNameTest", "lastNameTest",
            "streetTest", "buildingNumberTest", "apartmentNumberTest", "placeTest",
            "postCodeTest", "phoneTest", "emailTest");
    }

    [Fact]
    public async Task Should_RemoveProductCorrectly()
    {
        var request = new RemoveAddressCommand(1);
        var addresses = new List<Address>();

        _addressRepositoryMock
            .Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync(_address);
        
        _addressRepositoryMock
            .Setup(x => x.RemoveAsync(_address))
            .Callback<Address>(a => addresses.Add(a));

        await _handler.Handle(request, CancellationToken.None);

        _addressRepositoryMock.Verify(x => x.RemoveAsync(_address), Times.Once());
        addresses.Should().AllBeEquivalentTo(_address);
    }

    [Theory]
    [InlineData("en-US" )]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_WhenProductIsNull(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.AddressNotFound];

        var request = new RemoveAddressCommand(999);

        _addressRepositoryMock
            .Setup(x => x.GetAsync(request.Id))
            .Returns(Task.FromResult<Address>(null));

        var exception = await Assert.ThrowsAsync<AddressNotFoundException>(() => _handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        _addressRepositoryMock.Verify(x => x.RemoveAsync(It.IsAny<Address>()), Times.Never);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_WhenProductIsRemoved(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.AddressNotFound];

        _address.IsRemoved = true;

        var request = new RemoveAddressCommand(2);

        _addressRepositoryMock
            .Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync(_address);

        var exception = await Assert.ThrowsAsync<AddressNotFoundException>(() => _handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        _addressRepositoryMock.Verify(x => x.RemoveAsync(It.IsAny<Address>()), Times.Never);
    }
}