namespace Yerbowo.Unit.Tests.Application.Auth.Command;

public class ConfirmRegistrationEmailHandlerTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly ConfirmRegistrationEmailHandler _handler;
    private readonly ConfirmRegistrationEmailCommand _request;
    private readonly User _user;

    Guid UserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    private IStringLocalizer<SharedResource> _localizer;

    public ConfirmRegistrationEmailHandlerTest()
    {
        _userRepositoryMock = new();

        _localizer = StringLocalizerFactory.Create();

        _handler = new ConfirmRegistrationEmailHandler(_userRepositoryMock.Object, _localizer);
        _request = new ConfirmRegistrationEmailCommand("email@email.com", "1234567890");
        _user = new User(UserId, "firstName", "lastName", "email@email.com");
    }

    [Fact]
    public async Task Should_ConfirmEmailCorrectly()
    {
        _user.SetVerificationToken("1234567890");
        
        _userRepositoryMock.Setup(x => x.GetActiveByEmailAsync(_request.Email))
            .ReturnsAsync(_user);
        
        await _handler.Handle(_request, CancellationToken.None);

        _userRepositoryMock.Verify(x => x.UpdateAsync(_user), Times.Once);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_When_TokensDoNotMatch(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.InvalidToken];

        _user.SetVerificationToken("123456789");
        _userRepositoryMock.Setup(x => x.GetActiveByEmailAsync(_request.Email))
            .ReturnsAsync(_user);

        var exception = await Assert.ThrowsAsync<InvalidTokenException>(
            () => _handler.Handle(_request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never());
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_When_UserWasNotFound(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.UserNotFound];

        _userRepositoryMock.Setup(x => x.GetActiveByEmailAsync(_request.Email))
            .Returns(Task.FromResult<User>(null));

        var exception = await Assert.ThrowsAsync<UserNotFoundException>(
            () => _handler.Handle(_request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never());
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_When_EmailWasVerifiedAgain(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.EmailWasAlreadyConfirmed];

        _user.SetVerificationToken("1234567890");
        _user.SetVerificationDate(DateTime.UtcNow);
        _userRepositoryMock.Setup(x => x.GetActiveByEmailAsync(_request.Email))
            .ReturnsAsync(_user);

        var exception = await Assert.ThrowsAsync<EmailWasAlreadyConfirmedException>(
            () => _handler.Handle(_request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never());
    }
}