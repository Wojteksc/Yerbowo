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
        private readonly User user;
        public LoginHandlerTest()
        {
            user = new User("firstName", "lastName", "email@email.com", "companyName",
                "role", "photoUrl", "provider", "password");
        }

        [Fact]
        public async Task Login_When_Incorrect_Data_Should_Return_Message()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            var mockPasswordValidator = new Mock<IPasswordValidator>();
            mockPasswordValidator.Setup(x => x.Equals(
                It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(false);

            var mockJwtHandler = new Mock<IJwtHandler>();
            mockJwtHandler.Setup(x => x.CreateToken(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(It.IsAny<TokenDto>());

            var loginHandler = new LoginHandler(
                mockUserRepository.Object,
                mockPasswordValidator.Object,
                mockJwtHandler.Object);

            var loginCommand = new LoginCommand
            {
                Email = "email@email.com",
            };

            Func<Task> act = () => loginHandler.Handle(loginCommand, It.IsAny<CancellationToken>());
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
            Assert.Equal("Niepoprawne dane logowania", exception.Message);
        }

        [Fact]
        public async Task Login_When_Correct_Data_Should_Return_Token()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            var mockPasswordValidator = new Mock<IPasswordValidator>();
            mockPasswordValidator.Setup(x => x.Equals(
                It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(true);

            var mockJwtHandler = new Mock<IJwtHandler>();
            mockJwtHandler.Setup(x => x.CreateToken(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new TokenDto());

            var loginHandler = new LoginHandler(
                mockUserRepository.Object,
                mockPasswordValidator.Object,
                mockJwtHandler.Object);

            var loginCommand = new LoginCommand
            {
                Email = It.IsAny<string>()
            };

            var result = await loginHandler.Handle(loginCommand, It.IsAny<CancellationToken>());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Login_When_User_Does_Not_Exist_Should_Return_Correct_Message()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(x => x.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<User>(null));

            var mockPasswordValidator = new Mock<IPasswordValidator>();
            mockPasswordValidator.Setup(x => x.Equals(
                It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(false);

            var mockJwtHandler = new Mock<IJwtHandler>();

            var loginHandler = new LoginHandler(
                mockUserRepository.Object,
                mockPasswordValidator.Object,
                mockJwtHandler.Object);

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
