namespace Yerbowo.Unit.Tests.Application.Auth.Command;

public class RegisterHandlerTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMediator> _mediatorMock;

    private readonly User _user;
    private readonly RegisterCommand _request;
    private readonly RegisterHandler _handler;

    public RegisterHandlerTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _mediatorMock = new Mock<IMediator>();

        _user = new User("firstName", "lastName", "email@email.com", "password", "user", "companyName");
        _request = new RegisterCommand() 
        {
            FirstName = "firstName",
            LastName = "lastName",
            CompanyName = "companyName",
            Email = "email@email.com", 
            ConfirmEmail = "email@email.com",
            Password = "password",
            ConfirmPassword = "password"
        };

        _handler = new RegisterHandler(
            _userRepositoryMock.Object,
            AutoMapperConfig.Initialize(),
            _mediatorMock.Object);
    }

    [Fact]
    public async Task Should_CreateUserCorrectly()
    {
        var users = new List<User>();

        _userRepositoryMock.Setup(x => x.ExistsAsync(_request.Email))
            .ReturnsAsync(false);

        _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>()))
            .Callback<User>(user => users.Add(user))
            .ReturnsAsync(true);

        var result = await _handler.Handle(_request, CancellationToken.None);

        result.Should().Be(default);
        _userRepositoryMock.Verify(x => x.AddAsync(users.First()), Times.Once());
        _mediatorMock.Verify(x => 
            x.Publish(It.IsAny<RegisterEndedEvent>(), It.IsAny<CancellationToken>()), Times.Once());
       
        users.Should().AllBeEquivalentTo(_user, 
            options => options
            .Excluding(x => x.PasswordHash)
            .Excluding(x => x.PasswordSalt)
            .Excluding(x => x.VerificationToken));

        users.First().VerificationToken.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_ThrowException_When_CreateUserWithTheSameEmail()
    {
        _userRepositoryMock.Setup(x => x.ExistsAsync(_request.Email))
            .ReturnsAsync(true);

        _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>()))
            .ReturnsAsync(false);

        var exception = await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(_request, CancellationToken.None));
        exception.Message.Should().Be("Ten adres e-mail jest już używany, proszę wybierz inny albo zaloguj się.");
        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never());
    }

    [Fact]
    public async Task Should_ThrowException_When_CantAddUser()
    {
        _userRepositoryMock.Setup(x => x.ExistsAsync(_request.Email))
            .ReturnsAsync(false);

        _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>()))
            .ReturnsAsync(false);

        var exception = await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(_request, CancellationToken.None));
        exception.Message.Should().Be("Nieudana próba rejestracji konta. Skontaktuj się z administratorem.");
        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
    }
}