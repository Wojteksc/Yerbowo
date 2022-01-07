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
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;

        private readonly User user;
        private readonly RegisterCommand registerCommand;

        public RegisterHandlerTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();

            user = new User("firstName", "lastName", "email@email.com", "companyName",
                "role", "photoUrl", "provider", "password");
            registerCommand = new RegisterCommand() { Email = "email@email.com", Password = "password" };
        }

        [Fact]
        public async Task Should_CreateUser_When_DataAreCorrect()
        {
            _mockUserRepository.Setup(x => x.ExistsAsync(registerCommand.Email))
                .ReturnsAsync(false);

            _mockUserRepository.Setup(x => x.AddAsync(user));

            _mockMapper.Setup(x => x.Map<User>(It.IsAny<RegisterCommand>()))
                .Returns(user);

            var handler = new RegisterHandler(_mockUserRepository.Object, _mockMapper.Object);
            await handler.Handle(registerCommand, 
                It.IsAny<CancellationToken>());

            _mockUserRepository.Verify(x => x.AddAsync(user), Times.Once());
        }

        
        [Fact]
        public async Task Should_ThrowException_When_CreateUserWithTheSameEmail()
        {
            _mockUserRepository.Setup(x => x.ExistsAsync(registerCommand.Email))
                .ReturnsAsync(true);

            _mockUserRepository.Setup(x => x.AddAsync(user));

            _mockMapper.Setup(x => x.Map<User>(It.IsAny<RegisterCommand>()))
                .Returns(user);

            var handler = new RegisterHandler(_mockUserRepository.Object, _mockMapper.Object);
            Func<Task> act = () => handler.Handle(registerCommand, It.IsAny<CancellationToken>());

            var exception = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Nazwa użytkownika jest zajęta", exception.Message);
            _mockUserRepository.Verify(x => x.AddAsync(user), Times.Never());
        }
    }
}