using Yerbowo.Application.Auth.ConfirmEmail;
using Yerbowo.Domain.Users;
using Yerbowo.Infrastructure.Data.Users;

namespace Yerbowo.Unit.Tests.Application.Auth.Verify;

public class ConfirmEmailHandlerTest
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    public ConfirmEmailHandlerTest()
    {
        _mockUserRepository = new Mock<IUserRepository>();
    }

    [Fact]
    public async Task Should_ConfirmEmail_When_DataAreCorrect()
    {
        var confirmEmailCommand = new ConfirmEmailCommand() { Email = "email@email.com", Token = "1234567890" };
        var user = new User("firstName", "lastName", "email@email.com", "companyName",
        "role", "photoUrl", "provider", "password");
        user.SetVerificationToken("1234567890");
        _mockUserRepository.Setup(x => x.GetAsync(confirmEmailCommand.Email))
            .ReturnsAsync(user);

        var handler = new ConfirmEmailHandler(_mockUserRepository.Object);

        var result = await handler.Handle(confirmEmailCommand, It.IsAny<CancellationToken>());

        _mockUserRepository.Verify(x => x.SaveAllAsync(), Times.Once);
        result.Should().Be(default(MediatR.Unit));
    }

    [Fact]
    public async Task Should_ThrowException_When_TokensDoNotMatch()
    {
        var confirmEmailCommand = new ConfirmEmailCommand() { Email = "email@email.com", Token = "1234567890" };
        var user = new User("firstName", "lastName", "email@email.com", "companyName",
        "role", "photoUrl", "provider", "password");
        user.SetVerificationToken("123456789");
        _mockUserRepository.Setup(x => x.GetAsync(confirmEmailCommand.Email))
            .ReturnsAsync(user);

        var handler = new ConfirmEmailHandler(_mockUserRepository.Object);

        Func<Task> func = () => handler.Handle(confirmEmailCommand, It.IsAny<CancellationToken>());
        var exception = await Assert.ThrowsAsync<Exception>(func);
        Assert.Equal("Nieprawidłowe żądanie potwierdzenia adresu e-mail.", exception.Message);
        _mockUserRepository.Verify(x => x.SaveAllAsync(), Times.Never());
    }

    [Fact]
    public async Task Should_ThrowException_When_EmailsDoNotMatch()
    {
        var confirmEmailCommand = new ConfirmEmailCommand() { Email = "email@email.com", Token = "1234567890" };
        var user = new User("firstName", "lastName", "test@email.com", "companyName",
        "role", "photoUrl", "provider", "password");
        user.SetVerificationToken("1234567890");
        _mockUserRepository.Setup(x => x.GetAsync(confirmEmailCommand.Email))
            .ReturnsAsync(user);

        var handler = new ConfirmEmailHandler(_mockUserRepository.Object);

        Func<Task> func = () => handler.Handle(confirmEmailCommand, It.IsAny<CancellationToken>());
        var exception = await Assert.ThrowsAsync<Exception>(func);
        Assert.Equal("Nieprawidłowe żądanie potwierdzenia adresu e-mail.", exception.Message);
        _mockUserRepository.Verify(x => x.SaveAllAsync(), Times.Never());
    }

    [Fact]
    public async Task Should_ThrowException_When_UserWasNotFound()
    {
        var confirmEmailCommand = new ConfirmEmailCommand() { Email = "email@email.com", Token = "1234567890" };

        _mockUserRepository.Setup(x => x.GetAsync(confirmEmailCommand.Email))
            .Returns(Task.FromResult<User>(null));

        var handler = new ConfirmEmailHandler(_mockUserRepository.Object);

        Func<Task> func = () => handler.Handle(confirmEmailCommand, It.IsAny<CancellationToken>());
        var exception = await Assert.ThrowsAsync<Exception>(func);
        Assert.Equal("Nieprawidłowe żądanie potwierdzenia adresu e-mail.", exception.Message);
        _mockUserRepository.Verify(x => x.SaveAllAsync(), Times.Never());
    }

    [Fact]
    public async Task Should_ThrowException_When_EmailWasVerifiedAgain()
    {
        var confirmEmailCommand = new ConfirmEmailCommand() { Email = "email@email.com", Token = "1234567890" };
        var user = new User("firstName", "lastName", "email@email.com", "companyName",
        "role", "photoUrl", "provider", "password");
        user.SetVerificationToken("1234567890");
        user.SetVerificationDate(DateTime.UtcNow);
        _mockUserRepository.Setup(x => x.GetAsync(confirmEmailCommand.Email))
            .ReturnsAsync(user);

        var handler = new ConfirmEmailHandler(_mockUserRepository.Object);

        Func<Task> func = () => handler.Handle(confirmEmailCommand, It.IsAny<CancellationToken>());
        var exception = await Assert.ThrowsAsync<Exception>(func);
        Assert.Equal("Adres e-mail był już potwierdzony.", exception.Message);
        _mockUserRepository.Verify(x => x.SaveAllAsync(), Times.Never());
    }
}
