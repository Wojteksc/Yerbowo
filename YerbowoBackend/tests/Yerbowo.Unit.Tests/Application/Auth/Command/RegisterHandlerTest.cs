namespace Yerbowo.Unit.Tests.Application.Auth.Command;

public class RegisterHandlerTest
{
    private readonly Mock<IUserRepository> userRepository;
    private readonly Mock<IWebEncoder> webEncoder;
    private readonly Mock<IPasswordManager> passwordManager;

    private readonly User user;
    private readonly RegisterCommand request;
    private readonly RegisterHandler handler;

    private IStringLocalizer<SharedResource> localizer;

    Guid UserId = Guid.Parse("00000000-0000-0000-0000-000000000001");


    public RegisterHandlerTest()
    {
        userRepository = new();
        webEncoder = new();
        passwordManager = new();

        localizer = StringLocalizerFactory.Create();
        user = new User(UserId, "firstName", "lastName", "email@email.com", "user", "companyName");
        user.SetVerificationToken("token");
        user.SetPassword("hashedPassword");

        request = new RegisterCommand() 
        {
            FirstName = "firstName",
            LastName = "lastName",
            CompanyName = "companyName",
            Email = "email@email.com", 
            ConfirmEmail = "email@email.com",
            Password = "password",
            ConfirmPassword = "password"
        };

        handler = new RegisterHandler(
            userRepository.Object,
            AutoMapperConfig.Initialize(),
            localizer,
            webEncoder.Object,
            passwordManager.Object);
    }

    [Fact]
    public async Task Should_RegisterUserCorrectly()
    {
        var users = new List<User>();

        userRepository
            .Setup(x => x.ExistsByEmailAsync(request.Email))
            .ReturnsAsync(false);

        userRepository
            .Setup(x => x.AddAsync(It.IsAny<User>()))
            .Callback<User>(user =>
            {
                typeof(User).GetProperty(nameof(User.Id)).SetValue(user, UserId, null);
                users.Add(user);
            });

        webEncoder
            .Setup(x => x.Base64UrlEncodeGuid())
            .Returns("token");

        passwordManager
            .Setup(x => x.Secure(request.Password))
            .Returns("hashedPassword");

        await handler.Handle(request, CancellationToken.None);

        userRepository.Verify(x => x.AddAsync(users.First()), Times.Once());
        users.Should().AllBeEquivalentTo(user);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_When_CreateUserWithTheSameEmail(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = localizer[Localizations.EmailIsAlreadyInUse];

        userRepository
            .Setup(x => x.ExistsByEmailAsync(request.Email))
            .ReturnsAsync(true);

        userRepository.Setup(x => x.AddAsync(It.IsAny<User>()));

        var exception = await Assert.ThrowsAsync<EmailIsAlreadyInUseException>(
            () => handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        userRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never());
    }
}