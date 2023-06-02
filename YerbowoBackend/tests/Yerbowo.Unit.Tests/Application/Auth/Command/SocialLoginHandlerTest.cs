namespace Yerbowo.Unit.Tests.Application.Auth.Command;

public class SocialLoginHandlerTest
{
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IJwtHandler> _jwtHandlerMock;

    private readonly SocialLoginHandler _handler;
    private readonly SocialLoginCommand _request;
    private readonly User _user;

    public SocialLoginHandlerTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _jwtHandlerMock = new Mock<IJwtHandler>();

        _handler = new SocialLoginHandler(
          _userRepositoryMock.Object,
          AutoMapperConfig.Initialize(),
          _jwtHandlerMock.Object);

        _user = new User("firstName", "lastName", "email@email.com", "password", "user", null, 
            "http://www.test.pl", "Facebook");

        _request = new SocialLoginCommand()
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

        _userRepositoryMock.Setup(x => x.GetAsync(_request.Email))
            .Returns(Task.FromResult<User>(null));

        _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>()))
            .Callback<User>(user => users.Add(user));

        _jwtHandlerMock.Setup(x => x.CreateToken(
            _user.Id, _user.Email, _user.Role))
            .Returns(new TokenDto());

        var response = await _handler.Handle(_request, CancellationToken.None);
        response.Should().NotBeNull();
        _userRepositoryMock.Verify(x => x.AddAsync(users.First()), Times.Once());
        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never());
        users.Should().AllBeEquivalentTo(_user,
            options => options
            .Excluding(x => x.PasswordHash)
            .Excluding(x => x.PasswordSalt));
    }

    [Fact]
    public async Task Should_SetPhotoUrl_When_UserDoesNotHavePhotoUrl()
    {
        _user.SetPhotoUrl(null);

        var users = new List<User>();

        _userRepositoryMock.Setup(x => x.GetAsync(_request.Email))
            .ReturnsAsync(_user);

        _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .Callback<User>(user => users.Add(user));

        _jwtHandlerMock.Setup(x => x.CreateToken(
            _user.Id, _user.Email, _user.Role))
            .Returns(new TokenDto());

        var response = await _handler.Handle(_request, CancellationToken.None);
        response.Should().NotBeNull();
        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never());
        _userRepositoryMock.Verify(x => x.UpdateAsync(users.First()), Times.Once());
        users.Should().AllBeEquivalentTo(_user);
    }

    [Fact]
    public async Task Should_ThrowException_When_EmailIsNull()
    {
        var request = new SocialLoginCommand()
        {
            FirstName = "Test",
            LastName = "Test",
            Provider = "Facebook"
        };

        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Be($"Na Twoim koncie {request.Provider.ToTitle()} nie jest zapisany adres e-mail");
        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never());
        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never());
    }

    [Fact]
    public async Task Should_ThrowException_When_UserIsRemoved()
    {
        var request = new SocialLoginCommand()
        {
            FirstName = "Test",
            LastName = "Test",
            Provider = "Facebook",
            Email = "test@gmail.com",
            PhotoUrl = "http://www.test.pl"
        };

        _user.IsRemoved = true;

        _userRepositoryMock.Setup(x => x.GetAsync(request.Email))
            .ReturnsAsync(_user);

        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _handler.Handle(request, CancellationToken.None));
        exception.Message.Should().Be("Konto nie istnieje");
        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never());
        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never());
    }
}