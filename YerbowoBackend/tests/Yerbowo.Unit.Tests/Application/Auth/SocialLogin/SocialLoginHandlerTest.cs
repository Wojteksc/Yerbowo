using Yerbowo.Application.Auth;
using Yerbowo.Application.Auth.SocialLogin;
using Yerbowo.Application.Extensions;
using Yerbowo.Application.Services.Jwt;
using Yerbowo.Domain.Extensions;
using Yerbowo.Domain.Users;
using Yerbowo.Infrastructure.Data.Users;

namespace Yerbowo.Unit.Tests.Application.Auth.SocialLogin;

public class SocialLoginHandlerTest
{
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IMapper> _mockMapper;
    private Mock<IJwtHandler> _mockJwtHandler;

    private readonly User user;

    public SocialLoginHandlerTest()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockJwtHandler = new Mock<IJwtHandler>();

        user = new User("firstName", "lastName", "email@email.com", "companyName",
                "user", "http://www.test.pl", "Facebook", "password");
    }

    [Fact]
    public async Task Should_CreateNewAccount_When_UserDoesNotExistInDatabase()
    {
        var socialLoginCommand = new SocialLoginCommand()
        {
            FirstName = "firstName",
            LastName = "lastName",
            Provider = "Facebook",
            Email = "email@email.com",
            PhotoUrl = "http://www.test.pl"
        };

        _mockUserRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User>(null));

        _mockMapper.Setup(x => x.Map<User>(socialLoginCommand))
            .Returns(user);

        _mockUserRepository.Setup(x => x.AddAsync(user));

        _mockJwtHandler.Setup(x => x.CreateToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(
                new TokenDto()
                {
                    Expires = DateTime.UtcNow.ToTimeStamp(),
                    Role = "user",
                    Token = "token"
                });

        var socialLoginHandler = new SocialLoginHandler(
          _mockUserRepository.Object,
          _mockMapper.Object,
          _mockJwtHandler.Object);

        ResponseToken response = await socialLoginHandler.Handle(socialLoginCommand, It.IsAny<CancellationToken>());
        response.Token.Role.Should().Be("user");
        response.PhotoUrl.Should().Be(socialLoginCommand.PhotoUrl);
    }

    [Fact]
    public async Task Should_SetPhotoUrl_When_UserDoesNotHavePhotoUrl()
    {
        user.SetPhotoUrl(string.Empty);

        var socialLoginCommand = new SocialLoginCommand()
        {
            FirstName = "firstName",
            LastName = "lastName",
            Provider = "Facebook",
            Email = "email@email.com",
            PhotoUrl = "http://www.test.pl"
        };

        _mockUserRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        _mockMapper.Setup(x => x.Map<User>(socialLoginCommand))
            .Returns(user);

        _mockUserRepository.Setup(x => x.AddAsync(user));

        _mockJwtHandler.Setup(x => x.CreateToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(
                new TokenDto()
                {
                    Expires = DateTime.UtcNow.ToTimeStamp(),
                    Role = "user",
                    Token = "token"
                });

        var socialLoginHandler = new SocialLoginHandler(
          _mockUserRepository.Object,
          _mockMapper.Object,
          _mockJwtHandler.Object);

        ResponseToken response = await socialLoginHandler.Handle(socialLoginCommand, It.IsAny<CancellationToken>());
        response.Token.Role.Should().Be("user");
        response.PhotoUrl.Should().Be(socialLoginCommand.PhotoUrl);
    }

    [Fact]
    public async Task Should_ThrowException_When_EmailIsNull()
    {
        var socialLoginCommand = new SocialLoginCommand()
        {
            FirstName = "Test",
            LastName = "Test",
            Provider = "Facebook"
        };

        var socialLoginHandler = new SocialLoginHandler(
            _mockUserRepository.Object,
            _mockMapper.Object,
            _mockJwtHandler.Object);

        Func<Task> act = () => socialLoginHandler.Handle(socialLoginCommand, It.IsAny<CancellationToken>());
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
        Assert.Equal($"Na Twoim koncie {(socialLoginCommand.Provider.ToTitle())} nie jest zapisany adres e-mail", exception.Message);
    }

    [Fact]
    public async Task Should_ThrowException_When_UserIsRemoved()
    {
        var socialLoginCommand = new SocialLoginCommand()
        {
            FirstName = "Test",
            LastName = "Test",
            Provider = "Facebook",
            Email = "test@gmail.com",
            PhotoUrl = "http://www.test.pl"
        };

        user.IsRemoved = true;

        _mockUserRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        var socialLoginHandler = new SocialLoginHandler(
          _mockUserRepository.Object,
          _mockMapper.Object,
          _mockJwtHandler.Object);

        Func<Task> act = () => socialLoginHandler.Handle(socialLoginCommand, It.IsAny<CancellationToken>());
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
        Assert.Equal($"Konto nie istnieje", exception.Message);
    }
}