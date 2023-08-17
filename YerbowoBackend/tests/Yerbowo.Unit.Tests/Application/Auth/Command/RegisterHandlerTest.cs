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
            _mediatorMock.Object,
            StringLocalizerFactory.Create());
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

    [Theory]
    [InlineData("en-US", "The email address you provided is already in use on another account")]
    [InlineData("pl-PL", "Podany adres e-mail jest już używany na innym koncie")]
    public async Task Should_ThrowException_When_CreateUserWithTheSameEmail(string culture, string expectedMessage)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

        _userRepositoryMock.Setup(x => x.ExistsAsync(_request.Email))
            .ReturnsAsync(true);

        _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>()))
            .ReturnsAsync(false);

        var exception = await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(_request, CancellationToken.None));
        exception.Message.Should().Be(expectedMessage);
        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never());
    }
}