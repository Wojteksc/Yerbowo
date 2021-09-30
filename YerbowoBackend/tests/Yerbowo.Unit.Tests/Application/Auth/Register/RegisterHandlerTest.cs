using AutoMapper;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Yerbowo.Application.Auth.Register;
using Yerbowo.Domain.Users;
using Yerbowo.Infrastructure.Data.Users;

namespace Yerbowo.Unit.Tests.Application.Auth.Register
{
    public class RegisterHandlerTest
    {
        private readonly User user;
        private readonly RegisterCommand registerCommand;

        public RegisterHandlerTest()
        {
            user = new User("firstName", "lastName", "email@email.com", "companyName",
                "role", "photoUrl", "provider", "password");
            registerCommand = new RegisterCommand() { Email = user.Email, Password = "password" };
        }

        [Fact]
        public async Task Reigster_should_add_user_correctly()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(x => x.ExistsAsync(registerCommand.Email))
                .ReturnsAsync(false);
            mockUserRepository.Setup(x => x.AddAsync(user));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<User>(It.IsAny<RegisterCommand>()))
                .Returns(user);

            var handler = new RegisterHandler(mockUserRepository.Object, mockMapper.Object);
            await handler.Handle(registerCommand, 
                It.IsAny<CancellationToken>());

            mockUserRepository.Verify(x => x.AddAsync(user), Times.Once());
        }

        [Fact]
        public async Task Reigster_should_throw_exception_when_add_user_by_the_same_user_mail()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(x => x.ExistsAsync(registerCommand.Email))
                .ReturnsAsync(true);
            mockUserRepository.Setup(x => x.AddAsync(user));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<User>(It.IsAny<RegisterCommand>()))
                .Returns(user);

            var handler = new RegisterHandler(mockUserRepository.Object, mockMapper.Object);
            Func<Task> act = () => handler.Handle(
                new RegisterCommand() { Email = user.Email, Password = "password" },
                It.IsAny<CancellationToken>());

            var exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Nazwa użytkownika jest zajęta", exception.Message);
            mockUserRepository.Verify(x => x.AddAsync(user), Times.Never());
        }
    }
}
