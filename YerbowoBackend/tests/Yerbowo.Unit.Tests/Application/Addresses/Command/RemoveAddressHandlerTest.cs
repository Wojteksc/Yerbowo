namespace Yerbowo.Unit.Tests.Application.Addresses.Command;

public class RemoveAddressHandlerTest
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock;
    private readonly RemoveAddressHandler _handler;
    private readonly Address _address;

    Guid AddressId = Guid.Parse("00000000-0000-0000-0000-000000000001");
    Guid UserId = Guid.Parse("10000000-0000-0000-0000-000000000002");

    private IStringLocalizer<SharedResource> _localizer;

    public RemoveAddressHandlerTest()
    {
        _addressRepositoryMock = new();

        _localizer = StringLocalizerFactory.Create();

        _handler = new RemoveAddressHandler(_addressRepositoryMock.Object, _localizer);

        _address = new Address(AddressId, UserId, "aliasTest", "firstNameTest", "lastNameTest",
            "streetTest", "buildingNumberTest", "apartmentNumberTest", "placeTest",
            "postCodeTest", "phoneTest", "emailTest");
    }

    [Fact]
    public async Task Should_RemoveProductCorrectly()
    {
        var request = new RemoveAddressCommand(AddressId);
        Address address = null;

        _addressRepositoryMock
            .Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync(_address);
        
        _addressRepositoryMock
            .Setup(x => x.RemoveAsync(_address))
            .Callback<Address>(a => address = a);

        await _handler.Handle(request, CancellationToken.None);

        _addressRepositoryMock.Verify(x => x.RemoveAsync(_address), Times.Once());
        address.Should().BeEquivalentTo(_address);
    }

    [Theory]
    [InlineData("en-US" )]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_WhenProductIsNull(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.AddressNotFound];

        var request = new RemoveAddressCommand(Guid.Parse("99999999-9999-9999-9999-999999999999"));

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

        var request = new RemoveAddressCommand(AddressId);

        _addressRepositoryMock
            .Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync(_address);

        var exception = await Assert.ThrowsAsync<AddressNotFoundException>(() => _handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        _addressRepositoryMock.Verify(x => x.RemoveAsync(It.IsAny<Address>()), Times.Never);
    }
}