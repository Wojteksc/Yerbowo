namespace Yerbowo.Unit.Tests.Application.Auth.Command;

public class SocialLoginHandlerTest
{
    private readonly Mock<IUserRepository> userRepository;
    private readonly Mock<ITokenGenerator> authenticator;

    private readonly SocialLoginHandler handler;
    private readonly SocialLoginCommand request;
    private readonly User user;

    private IStringLocalizer<SharedResource> localizer;

    public SocialLoginHandlerTest()
    {
        userRepository = new();
        authenticator = new();

        localizer = StringLocalizerFactory.Create();

        handler = new SocialLoginHandler(
          userRepository.Object,
          AutoMapperConfig.Initialize(),
          authenticator.Object,
          localizer);

       user = new User("firstName", "lastName", "email@email.com", "user", null, 
            "http://www.test.pl", "Facebook");

        request = new SocialLoginCommand()
        {
            FirstName = "firstName",
            LastName = "lastName",
            Provider = "Facebook",
            Email = "email@email.com",
            PhotoUrl = "http://www.test.pl"
        };
    }

    [Fact]
    public async Task Should_CreateNewAccount_When_UserDoesNotExistInDatabase()
    {

        var users = new List<User>();
        var token = new TokenDto("token");

        userRepository
            .Setup(x => x.GetAsync(request.Email))
            .ReturnsAsync((User?)null);

        userRepository
            .Setup(x => x.AddAsync(It.IsAny<User>()))
            .Callback<User>(user => users.Add(user));

        authenticator
            .Setup(x => x.CreateToken(It.IsAny<int>(), user.Email, user.Role))
            .Returns(token);

        var response = await handler.Handle(request, CancellationToken.None);

        users.Should().ContainSingle();
        userRepository.Verify(x => x.AddAsync(users.Single()), Times.Once());
        userRepository.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never());

        response.Should().NotBeNull();
        response.Token.Should().Be(token);

        users.Single().Should().BeEquivalentTo(user, options =>
            options.Excluding(u => u.Password));
    }

    [Fact]
    public async Task Should_SetPhotoUrl_When_UserDoesNotHavePhotoUrl()
    {
        var token = new TokenDto("token");
        user.SetPhotoUrl(null);

        var users = new List<User>();

        userRepository
            .Setup(x => x.GetAsync(request.Email))
            .ReturnsAsync(user);

        userRepository
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .Callback<User>(user => users.Add(user));

        authenticator
            .Setup(x => x.CreateToken(user.Id, user.Email, user.Role))
            .Returns(new TokenDto("token"));

        var response = await handler.Handle(request, CancellationToken.None);

        userRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never());
        userRepository.Verify(x => x.UpdateAsync(users.First()), Times.Once());
        response.Should().Be(new ResponseToken(token, "http://www.test.pl"));
        users.Should().AllBeEquivalentTo(user);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_When_EmailIsNull(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = localizer[Localizations.UserHasNoEmail];

        var request = new SocialLoginCommand()
        {
            FirstName = "Test",
            LastName = "Test",
            Provider = "Facebook"
        };

        var exception = await Assert.ThrowsAsync<UserHasNoEmailException>(
                () => handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Be(string.Format(expectedMessage, request.Provider.ToTitle()));
        userRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never());
        userRepository.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never());
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_When_UserIsRemoved(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = localizer[Localizations.UserNotFound];

        var request = new SocialLoginCommand()
        {
            FirstName = "Test",
            LastName = "Test",
            Provider = "Facebook",
            Email = "test@gmail.com",
            PhotoUrl = "http://www.test.pl"
        };

        user.IsRemoved = true;

        userRepository
            .Setup(x => x.GetAsync(request.Email))
            .ReturnsAsync(user);

        var exception = await Assert.ThrowsAsync<UserNotFoundException>(
                () => handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        userRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never());
        userRepository.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never());
    }
}