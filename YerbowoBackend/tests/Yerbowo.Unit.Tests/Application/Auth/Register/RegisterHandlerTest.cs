using MediatR;
using Xunit.Sdk;
using Yerbowo.Application.Auth.Register;
using Yerbowo.Application.Auth.Register.Events;
using Yerbowo.Domain.Users;
using Yerbowo.Infrastructure.Data.Users;
using Yerbowo.Unit.Tests.Helpers;

namespace Yerbowo.Unit.Tests.Application.Auth.Register;

public class RegisterHandlerTest
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IMediator> _mediator;

    private readonly User user;
    private readonly RegisterCommand registerCommand;

    public RegisterHandlerTest()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockMapper = new Mock<IMapper>();
        _mediator = new Mock<IMediator>();

        user = new User("firstName", "lastName", "email@email.com", "companyName",
            "role", "photoUrl", "provider", "password");
        registerCommand = new RegisterCommand() { Email = "email@email.com", Password = "password" };
    }

    [Fact]
    public async Task Should_CreateUser_When_DataAreCorrect()
    {
        _mockUserRepository.Setup(x => x.ExistsAsync(registerCommand.Email))
            .ReturnsAsync(false);

        _mockUserRepository.Setup(x => x.AddAsync(user))
            .ReturnsAsync(true);

        _mockMapper.Setup(x => x.Map<User>(It.IsAny<RegisterCommand>()))
            .Returns(user);

        var handler = new RegisterHandler(
            _mockUserRepository.Object, 
            _mockMapper.Object,
            _mediator.Object);

        var result = await handler.Handle(registerCommand, It.IsAny<CancellationToken>());

        _mockUserRepository.Verify(x => x.AddAsync(user), Times.Once());
        _mediator.Verify(x => x.Publish(It.IsAny<RegisterEndedEvent>(), It.IsAny<CancellationToken>()), Times.Once());
        result.Should().Be(default(MediatR.Unit));
    }

    
    [Fact]
    public async Task Should_ThrowException_When_CreateUserWithTheSameEmail()
    {
        _mockUserRepository.Setup(x => x.ExistsAsync(registerCommand.Email))
            .ReturnsAsync(true);

        _mockUserRepository.Setup(x => x.AddAsync(user));

        _mockMapper.Setup(x => x.Map<User>(It.IsAny<RegisterCommand>()))
            .Returns(user);

        var handler = new RegisterHandler(
            _mockUserRepository.Object,
            _mockMapper.Object,
            _mediator.Object);

        Func<Task> act = () => handler.Handle(registerCommand, It.IsAny<CancellationToken>());

        var exception = await Assert.ThrowsAsync<Exception>(act);
        Assert.Equal("Ten adres e-mail jest już używany, proszę wybierz inny albo zaloguj się.", exception.Message);
        _mockUserRepository.Verify(x => x.AddAsync(user), Times.Never());
    }

    [Fact]
    public async Task Should_ThrowException_WhenSomethingGoneWrongDuringPublishEvent()
    {
        _mockUserRepository.Setup(x => x.ExistsAsync(registerCommand.Email))
            .ReturnsAsync(false);

        _mockUserRepository.Setup(x => x.AddAsync(user))
            .ReturnsAsync(false);

        _mockMapper.Setup(x => x.Map<User>(It.IsAny<RegisterCommand>()))
            .Returns(user);

        var handler = new RegisterHandler(
            _mockUserRepository.Object,
            _mockMapper.Object,
            _mediator.Object);

        Func<Task> act = () => handler.Handle(registerCommand, It.IsAny<CancellationToken>());

        var exception = await Assert.ThrowsAsync<Exception>(act);
        Assert.Equal("Nieudana próba rejestracji konta. Skontaktuj się z administratorem.", exception.Message);
        _mockUserRepository.Verify(x => x.AddAsync(user), Times.Once);
    }
}