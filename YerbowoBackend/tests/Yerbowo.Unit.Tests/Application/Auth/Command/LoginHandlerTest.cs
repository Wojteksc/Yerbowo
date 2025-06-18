namespace Yerbowo.Unit.Tests.Application.Auth.Command;

public class LoginHandlerTest
{
    private readonly Mock<IUserRepository> userRepository;
    private readonly Mock<IPasswordManager> passwordManager;
    private readonly Mock<IAuthenticator> authenticator;

    private readonly LoginHandler _handler;
    private readonly LoginCommand _request;
    private readonly User _user;

    private IStringLocalizer<SharedResource> _localizer;

    public LoginHandlerTest()
    {
        userRepository = new();
        passwordManager = new();
        authenticator = new();

        _user = new User("firstName", "lastName", "email@email.com");
        _request = new LoginCommand { Email = "email@email.com" };
        _localizer = StringLocalizerFactory.Create();

        _handler = new LoginHandler(
            userRepository.Object,
            passwordManager.Object,
            authenticator.Object,
            _localizer);
    }

    [Fact]
    public async Task Should_ReturnToken_When_LoginDataAreCorrect()
    {
        var token = new TokenDto("token");

        _user.SetVerificationDate(DateTime.UtcNow);

        userRepository
            .Setup(x => x.GetAsync(_request.Email))
            .ReturnsAsync(_user);

        passwordManager
            .Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        authenticator
            .Setup(x => x.CreateToken(_user.Id, _user.Email, _user.Role))
            .Returns(token);

        var response = await _handler.Handle(_request, CancellationToken.None);

        response.Should().Be(new ResponseToken(token, null));
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_When_UserDoesNotExist(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.UserInvalidCredentails];

        userRepository.Setup(x => x.GetAsync(_request.Email))
            .Returns(Task.FromResult<User>(null));

        var exception = await Assert.ThrowsAsync<UserInvalidCredentailsException>(
            () => _handler.Handle(_request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        passwordManager.Verify(x =>
            x.Validate(It.IsAny<string>(), It.IsAny<string>())
            , Times.Never);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_When_UserIsRemoved(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.UserInvalidCredentails];

        _user.IsRemoved = true;

        userRepository.Setup(x => x.GetAsync(_request.Email))
            .ReturnsAsync(_user);
        
        var exception = await Assert.ThrowsAsync<UserInvalidCredentailsException>(
            () => _handler.Handle(_request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        passwordManager.Verify(x =>
            x.Validate(It.IsAny<string>(), It.IsAny<string>())
            , Times.Never);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_When_UserDidNotConfirmEmail(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = _localizer[Localizations.UserRegistrationWasNotConfirmed];

        userRepository.Setup(x => x.GetAsync(_user.Email))
            .ReturnsAsync(_user);

        passwordManager
            .Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);

        var exception = await Assert.ThrowsAsync<UserRegistrationWasNotConfirmedException>(
            () => _handler.Handle(_request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        authenticator.Verify(x => 
            x.CreateToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())
            , Times.Never);
    }
}