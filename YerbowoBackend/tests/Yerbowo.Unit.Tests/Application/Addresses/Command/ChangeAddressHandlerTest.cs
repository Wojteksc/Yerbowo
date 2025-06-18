namespace Yerbowo.Unit.Tests.Application.Addresses.Command;

public class ChangeAddressHandlerTest
{
    private readonly ChangeAddressHandler _handler;
    private readonly Mock<IAddressRepository> _addressRepositoryMock;
    private readonly Address _address;
    private readonly ChangeAddressCommand _request;

    private IStringLocalizer<SharedResource> _localizer;

    public ChangeAddressHandlerTest()
    {
        _addressRepositoryMock = new();

        _localizer = StringLocalizerFactory.Create();

        _handler = new ChangeAddressHandler(
            _addressRepositoryMock.Object, 
            AutoMapperConfig.Initialize(),
            _localizer);

        _address = new Address(1,
            "aliastTest",
            "firstName",
            "LastName",
            "Street",
            "15A",
            "3",
            "Place",
            "21-100",
            "111-222-333",
            "test@test.com");

        _request = new ChangeAddressCommand()
        {
            Id = 1,
            Alias = "aliastTest_new",
            FirstName = "firstName_new",
            LastName = "LastName_new",
            Street = "Street_new",
            BuildingNumber = "15A_new",
            ApartmentNumber = "3_new",
            Place = "Place_new",
            PostCode = "21-100_new",
            Phone = "111-222-333_new",
            Email = "test@test.com_new"
        };
    }

    [Fact]
    public async Task Should_UpdateAddressCorrectly()
    {
        _addressRepositoryMock
           .Setup(x => x.GetAsync(_request.Id))
           .ReturnsAsync(_address);

        _addressRepositoryMock.Setup(x => x.UpdateAsync(_address));

        await _handler.Handle(_request, CancellationToken.None);

        _addressRepositoryMock.Verify(x => x.UpdateAsync(_address), Times.Once);
        _address.Should().BeEquivalentTo(_request, options => options.Excluding(x => x.Id));
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_When_AddressDoesNotExist(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.AddressNotFound];

        _addressRepositoryMock
            .Setup(x => x.GetAsync(_request.Id))
            .ReturnsAsync((Address)null);

        var exception = await Assert.ThrowsAsync<AddressNotFoundException>(() =>_handler.Handle(_request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        _addressRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Address>()), Times.Never);
    }
}