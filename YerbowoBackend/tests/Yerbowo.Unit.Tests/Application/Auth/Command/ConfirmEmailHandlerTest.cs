namespace Yerbowo.Unit.Tests.Application.Auth.Command;

public class ConfirmEmailHandlerTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly ConfirmEmailHandler _handler;
    private readonly ConfirmEmailCommand _request;
    private readonly User _user;

    public ConfirmEmailHandlerTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new ConfirmEmailHandler(_userRepositoryMock.Object, StringLocalizerFactory.Create());

        _request = new ConfirmEmailCommand() { Email = "email@email.com", Token = "1234567890" };
        _user = new User("firstName", "lastName", "email@email.com", "password");
    }

    [Fact]
    public async Task Should_ConfirmEmailCorrectly()
    {
        _user.SetVerificationToken("1234567890");
        
        _userRepositoryMock.Setup(x => x.GetAsync(_request.Email))
            .ReturnsAsync(_user);
        
        var result = await _handler.Handle(_request, CancellationToken.None);

        _userRepositoryMock.Verify(x => x.UpdateAsync(_user), Times.Once);
        result.Should().Be(default);
    }

    [Theory]
    [InlineData("en-US", "Bad request")]
    [InlineData("pl-PL", "Nieprawidłowe żądanie")]
    public async Task Should_ThrowException_When_TokensDoNotMatch(string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        _user.SetVerificationToken("123456789");
        _userRepositoryMock.Setup(x => x.GetAsync(_request.Email))
            .ReturnsAsync(_user);

        var exception = await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(_request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never());
    }

    [Theory]
    [InlineData("en-US", "Bad request")]
    [InlineData("pl-PL", "Nieprawidłowe żądanie")]
    public async Task Should_ThrowException_When_UserWasNotFound(string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        _userRepositoryMock.Setup(x => x.GetAsync(_request.Email))
            .Returns(Task.FromResult<User>(null));

        var exception = await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(_request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never());
    }

    [Theory]
    [InlineData("en-US", "The e-mail address was already confirmed")]
    [InlineData("pl-PL", "Adres e-mail został już potwierdzony")]
    public async Task Should_ThrowException_When_EmailWasVerifiedAgain(string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        _user.SetVerificationToken("1234567890");
        _user.SetVerificationDate(DateTime.UtcNow);
        _userRepositoryMock.Setup(x => x.GetAsync(_request.Email))
            .ReturnsAsync(_user);

        var exception = await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(_request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never());
    }
}