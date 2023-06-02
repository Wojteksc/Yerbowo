namespace Yerbowo.Unit.Tests.Application.Users.Command;

public class ChangeUserHandlerTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordValidator> _passwordValidatorMock;
    private readonly ChangeUserHandler _handler;

    public ChangeUserHandlerTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordValidatorMock = new Mock<IPasswordValidator>();

        _handler = new ChangeUserHandler(
            AutoMapperConfig.Initialize(),
            _userRepositoryMock.Object,
            _passwordValidatorMock.Object);
    }

    [Fact]
    public async Task Should_UpdateUserCorrectlyWithPassword()
    {
        var user = new User("firstName", "lastName", "email@email.com", "password", "user", "companyName");

        var request = new ChangeUserCommand
        {
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
            "firstName_NEW",
            "lastName_NEW",
            "email@emailNEW.com",
            "password_NEW",
            "user",
            "companyName_NEW");

        var users = new List<User>();

        _userRepositoryMock.Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync(user);
        _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .Callback<User>(u => users.Add(u))
            .ReturnsAsync(true);
        _passwordValidatorMock.Setup(x => x.Equals(
            It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
            .Returns(true);

        await _handler.Handle(request, CancellationToken.None);
        _userRepositoryMock.Verify(x => x.UpdateAsync(user), Times.Once);
        users.Should().AllBeEquivalentTo(expectedUpdatedUser, x => x
            .Excluding(u => u.PasswordHash)
            .Excluding(u => u.PasswordSalt));

        IPasswordValidator passwordValidator = new PasswordValidator();
        Assert.True(passwordValidator.Equals(request.NewPassword, users.First().PasswordHash, users.First().PasswordSalt));
    }

    [Fact]
    public async Task Should_UpdateUserCorrectlyWithoutPassword()
    {
        var user = new User("firstName", "lastName", "email@email.com", "password", "user", "companyName");

        var request = new ChangeUserCommand
        {
            FirstName = "firstName_NEW",
            LastName = "lastName_NEW",
            CompanyName = "companyName_NEW",
            Email = "email@emailNEW.com",
            ConfirmEmail = "email@emailNEW.com",
            CurrentPassword = "password"
        };

        var expectedUpdatedUser = new User(
            "firstName_NEW",
            "lastName_NEW",
            "email@emailNEW.com",
            "password",
            "user",
            "companyName_NEW");

        var users = new List<User>();

        _userRepositoryMock.Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync(user);
        _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .Callback<User>(u => users.Add(u))
            .ReturnsAsync(true);
        _passwordValidatorMock.Setup(x => x.Equals(
            It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
            .Returns(true);

        await _handler.Handle(request, CancellationToken.None);
        _userRepositoryMock.Verify(x => x.UpdateAsync(user), Times.Once);
        users.Should().AllBeEquivalentTo(expectedUpdatedUser, x => x
            .Excluding(u => u.PasswordHash)
            .Excluding(u => u.PasswordSalt));
        users.First().PasswordSalt.Should().BeEquivalentTo(user.PasswordSalt);
        users.First().PasswordHash.Should().BeEquivalentTo(user.PasswordHash);
    }

    [Fact]
    public async Task Should_ThrowException_When_UserDoesNotExist()
    {
        var request = new ChangeUserCommand { Id = 1 };

        _userRepositoryMock.Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync((User)null);

        Func<Task> act = () => _handler.Handle(request, CancellationToken.None);
        var exception = await Assert.ThrowsAsync<Exception>(act);
        exception.Message.Should().Be("Nie znaleziono użytkownika.");
        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Should_ThrowException_When_PasswordIsInvalid()
    {
        var user = new User("firstName", "lastName", "email@email.com", "password", "user", "companyName");

        var request = new ChangeUserCommand { Id = 1, CurrentPassword = "password_xyz" };

        _userRepositoryMock.Setup(x => x.GetAsync(request.Id))
            .ReturnsAsync(user);

        Func<Task> act = () => _handler.Handle(request, CancellationToken.None);
        var exception = await Assert.ThrowsAsync<Exception>(act);
        exception.Message.Should().Be("Podane hasło jest nieprawidłowe.");
        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
    }
}