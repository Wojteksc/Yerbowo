namespace Yerbowo.Unit.Tests.Application.Users.Command;

public class ChangeUserHandlerTest
{
    private readonly Mock<IUserRepository> userRepository = new();
    private readonly Mock<IPasswordManager> passwordManager = new();
    private readonly ChangeUserHandler handler;

    private IStringLocalizer<SharedResource> localizer;

    private Guid UserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public ChangeUserHandlerTest()
    {
        localizer = StringLocalizerFactory.Create();

        handler = new ChangeUserHandler(
            AutoMapperConfig.Initialize(),
            userRepository.Object,
            passwordManager.Object,
            StringLocalizerFactory.Create());
    }

    [Fact]
    public async Task Should_UpdateUserCorrectlyWithPassword()
    {
        var user = new User(UserId,"firstName", "lastName", "email@email.com", "user", "companyName");
        user.SetPassword("password");
        var request = new ChangeUserCommand
        {
            Id = UserId,
            FirstName = "firstName_NEW",
            LastName = "lastName_NEW",
            CompanyName = "companyName_NEW",
            Email = "email@emailNEW.com",
            ConfirmEmail = "email@emailNEW.com",
            CurrentPassword = "password",
            NewPassword = "password_NEW",
            ConfirmPassword = "password_NEW"
        };

        var expectedUpdatedUser = new User(
            UserId,
            "firstName_NEW",
            "lastName_NEW",
            "email@emailNEW.com",
            "user",
            "companyName_NEW");
        expectedUpdatedUser.SetPassword("newhashedPassword");

        User userDb = null;

        userRepository
            .Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync(user);

        userRepository
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .Callback<User>(u => userDb = u);
        
        passwordManager
            .Setup(x => x.Validate("password", "password"))
            .Returns(true);

        passwordManager
            .Setup(x => x.Secure("password_NEW"))
            .Returns("newhashedPassword");

        await handler.Handle(request, CancellationToken.None);
        userRepository.Verify(x => x.UpdateAsync(user), Times.Once);
        userDb.Should().BeEquivalentTo(expectedUpdatedUser);
    }

    [Fact]
    public async Task Should_UpdateUserCorrectlyWithoutPassword()
    {
        var user = new User(UserId,"firstName", "lastName", "email@email.com", "user", "companyName");
        user.SetPassword("password");

        var request = new ChangeUserCommand
        {
            Id = UserId,
            FirstName = "firstName_NEW",
            LastName = "lastName_NEW",
            CompanyName = "companyName_NEW",
            Email = "email@emailNEW.com",
            ConfirmEmail = "email@emailNEW.com",
            CurrentPassword = "password"
        };

        var expectedUpdatedUser = new User(
            UserId,
            "firstName_NEW",
            "lastName_NEW",
            "email@emailNEW.com",
            "user",
            "companyName_NEW");
        expectedUpdatedUser.SetPassword("password");

        User userDb = null;

        userRepository
            .Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync(user);

        userRepository
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .Callback<User>(u => userDb = u);
        
        passwordManager
            .Setup(x => x.Validate("password", "password"))
            .Returns(true);

        await handler.Handle(request, CancellationToken.None);
        userRepository.Verify(x => x.UpdateAsync(user), Times.Once);
        passwordManager.Verify(x => x.Secure(It.IsAny<string>()), Times.Never());
        userDb.Should().BeEquivalentTo(expectedUpdatedUser);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_When_UserDoesNotExist(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = localizer[Localizations.UserNotFound];

        var request = new ChangeUserCommand { Id = Guid.Parse("99999999-9999-9999-9999-999999999999") };

        userRepository
            .Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync((User)null);

        Func<Task> act = () => handler.Handle(request, CancellationToken.None);
        var exception = await Assert.ThrowsAsync<UserNotFoundException>(act);
        exception.Message.Should().Be(expectedMessage);
        userRepository.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pl-PL")]
    public async Task Should_ThrowException_When_PasswordIsInvalid(string culture)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        string expectedMessage = localizer[Localizations.UserPasswordIsIncorrect];

        var user = new User(UserId, "firstName", "lastName", "email@email.com", "user", "companyName");

        var request = new ChangeUserCommand { Id = UserId, CurrentPassword = "password_xyz" };

        userRepository.Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync(user);

        Func<Task> act = () => handler.Handle(request, CancellationToken.None);
        var exception = await Assert.ThrowsAsync<UserPasswordIsIncorrectException>(act);
        exception.Message.Should().Be(expectedMessage);
        userRepository.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
    }
}