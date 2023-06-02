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
            _jwtHandlerMock.Object);
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

    [Fact]
    public async Task Should_ThrowException_When_UserIsNull()
    {
        _userRepositoryMock.Setup(x => x.GetAsync(_request.Email))
            .Returns(Task.FromResult<User>(null));

        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _handler.Handle(_request, CancellationToken.None));
        exception.Message.Should().Be("Niepoprawne dane logowania");
        _passwordValidatorMock.Verify(x =>
            x.Equals(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Never);
    }

    [Fact]
    public async Task Should_ThrowException_When_UserIsRemoved()
    {
        _user.IsRemoved = true;

        _userRepositoryMock.Setup(x => x.GetAsync(_request.Email))
            .ReturnsAsync(_user);
        
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _handler.Handle(_request, CancellationToken.None));
        exception.Message.Should().Be("Niepoprawne dane logowania");
        _passwordValidatorMock.Verify(x =>
            x.Equals(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Never);
    }

    [Fact]
    public async Task Should_ThrowException_When_UserDidNotConfirmEmail()
    {
        _userRepositoryMock.Setup(x => x.GetAsync(_user.Email))
            .ReturnsAsync(_user);

        _passwordValidatorMock.Setup(x => x.Equals(
            It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
            .Returns(true);

        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _handler.Handle(_request, CancellationToken.None));
        exception.Message.Should().Be(
            "Rejestracja w sklepie nie została potwierdzona. Odbierz pocztę i kliknij w link potwierdzający.");
        _jwtHandlerMock.Verify(x => 
            x.CreateToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}