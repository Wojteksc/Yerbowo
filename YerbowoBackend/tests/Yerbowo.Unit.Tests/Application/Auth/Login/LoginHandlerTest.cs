using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Yerbowo.Application.Auth;
using Yerbowo.Application.Auth.Login;
using Yerbowo.Application.Services;
using Yerbowo.Domain.Users;
using Yerbowo.Infrastructure.Data.Users;

namespace Yerbowo.Unit.Tests.Application.Auth.Login
{
    public class LoginHandlerTest
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IPasswordValidator> _mockPasswordValidator;
        private readonly Mock<IJwtHandler> _mockJwtHandler;

        private readonly User user;

        public LoginHandlerTest()
        {
            user = new User("firstName", "lastName", "email@email.com", "companyName",
                "role", "photoUrl", "provider", "password");

            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordValidator = new Mock<IPasswordValidator>();
            _mockJwtHandler = new Mock<IJwtHandler>();
        }

        [Fact]
        public async Task Should_ReturnToken_When_LoginDetailsAreCorrect()
        {
            _mockUserRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _mockPasswordValidator.Setup(x => x.Equals(
                It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(true);

            _mockJwtHandler.Setup(x => x.CreateToken(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new TokenDto());

            var loginHandler = new LoginHandler(
                _mockUserRepository.Object,
                _mockPasswordValidator.Object,
                _mockJwtHandler.Object);

            var loginCommand = new LoginCommand
            {
                Email = It.IsAny<string>()
            };

            var result = await loginHandler.Handle(loginCommand, It.IsAny<CancellationToken>());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Should_ThrowException_When_LoginDetailsAreIncorrect()
        {
            _mockUserRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _mockPasswordValidator.Setup(x => x.Equals(
                It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(false);

            _mockJwtHandler.Setup(x => x.CreateToken(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(It.IsAny<TokenDto>());

            var loginHandler = new LoginHandler(
                _mockUserRepository.Object,
                _mockPasswordValidator.Object,
                _mockJwtHandler.Object);

            var loginCommand = new LoginCommand
            {
                Email = "email@email.com",
            };

            Func<Task> act = () => loginHandler.Handle(loginCommand, It.IsAny<CancellationToken>());
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
            Assert.Equal("Niepoprawne dane logowania", exception.Message);
        }

        [Fact]
        public async Task Should_ThrowException_When_UserDoesNotExist()
        {
            _mockUserRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<User>(null));

            _mockPasswordValidator.Setup(x => x.Equals(
                It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(false);

            var loginHandler = new LoginHandler(
                _mockUserRepository.Object,
                _mockPasswordValidator.Object,
                _mockJwtHandler.Object);

            var loginCommand = new LoginCommand
            {
                Email = It.IsAny<string>()
            };

            Func<Task> act = () => loginHandler.Handle(loginCommand, It.IsAny<CancellationToken>());
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
            Assert.Equal("Niepoprawne dane logowania", exception.Message);
        }
    }
}