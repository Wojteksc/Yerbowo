namespace Yerbowo.Unit.Tests.Application.Auth.Command;

public class LoginHandlerTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordValidator> _passwordValidatorMock;
    private readonly Mock<IJwtHandler> _jwtHandlerMock;
    private readonly LoginHandler _handler;
    private readonly LoginCommand _request;

    private readonly User _user;

    public LoginHandlerTest()
    {
        _user = new User("firstName", "lastName", "email@email.com", "password");

        _request = new LoginCommand { Email = "email@email.com" };

        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordValidatorMock = new Mock<IPasswordValidator>();
        _jwtHandlerMock = new Mock<IJwtHandler>();

        _handler = new LoginHandler(
            _userRepositoryMock.Object,
            _passwordValidatorMock.Object,
            _jwtHandlerMock.Object,
            StringLocalizerFactory.Create());
    }

    [Fact]
    public async Task Should_ReturnToken_When_LoginDataAreCorrect()
    {
        _user.SetVerificationDate(DateTime.UtcNow);

        _userRepositoryMock.Setup(x => x.GetAsync(_request.Email))
            .ReturnsAsync(_user);

        _passwordValidatorMock.Setup(x => x.Equals(
            It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
            .Returns(true);

        _jwtHandlerMock.Setup(x => x.CreateToken(
            _user.Id, _user.Email, _user.Role))
            .Returns(new TokenDto());

        var result = await _handler.Handle(_request, CancellationToken.None);
        result.Should().NotBe(null);
    }

    [Theory]
    [InlineData("en-US", "Invalid login details")]
    [InlineData("pl-PL", "Niepoprawne dane logowania")]
    public async Task Should_ThrowException_When_UserDoesNotExist(string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        _userRepositoryMock.Setup(x => x.GetAsync(_request.Email))
            .Returns(Task.FromResult<User>(null));

        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _handler.Handle(_request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        _passwordValidatorMock.Verify(x =>
            x.Equals(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Never);
    }

    [Theory]
    [InlineData("en-US", "Invalid login details")]
    [InlineData("pl-PL", "Niepoprawne dane logowania")]
    public async Task Should_ThrowException_When_UserIsRemoved(string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        _user.IsRemoved = true;

        _userRepositoryMock.Setup(x => x.GetAsync(_request.Email))
            .ReturnsAsync(_user);
        
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _handler.Handle(_request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        _passwordValidatorMock.Verify(x =>
            x.Equals(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Never);
    }

    [Theory]
    [InlineData("en-US", "Account registration has not been confirmed. Receive the e-mail and click on the confirmation link.")]
    [InlineData("pl-PL", "Rejestracja konta nie została potwierdzona. Odbierz wiadomość e-mail i kliknij w link potwierdzający.")]
    public async Task Should_ThrowException_When_UserDidNotConfirmEmail(string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        _userRepositoryMock.Setup(x => x.GetAsync(_user.Email))
            .ReturnsAsync(_user);

        _passwordValidatorMock.Setup(x => x.Equals(
            It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
            .Returns(true);

        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _handler.Handle(_request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        _jwtHandlerMock.Verify(x => 
            x.CreateToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}