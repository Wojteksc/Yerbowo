using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Yerbowo.Application.Auth;
using Yerbowo.Application.Auth.ConfirmEmail;
using Yerbowo.Application.Auth.Login;
using Yerbowo.Application.Auth.Register;
using Yerbowo.Application.Auth.SocialLogin;
using Yerbowo.Functional.Tests.Web.Helpers;
using Yerbowo.Infrastructure.Data.Users;

namespace Yerbowo.Functional.Tests.Web.Controllers;

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class AuthControllerTest : IClassFixture<WebTestFixture>
{
	private readonly HttpClient _httpClient;
	private readonly IServiceProvider _serviceProvider;
	private const string PrimaryEmail = "mailsendersc@testemail.com";
	private const string PrimaryPassword = "secret";

	public AuthControllerTest(WebTestFixture factory)
	{
		_httpClient = factory.CreateClient();
		_serviceProvider = factory.Services;
	}

	[Fact, Priority(0)]
	public async Task Register_Should_ReturnStatusCodeOk()
	{
		var registerCommand = new RegisterCommand
		{
			Email = PrimaryEmail,
			ConfirmEmail = PrimaryEmail,
			Password = PrimaryPassword,
			ConfirmPassword = PrimaryPassword,
			FirstName = "FirstTest",
			LastName = "LastTest",
		};

		var response = await AuthHelper.RegisterAsync(_httpClient, registerCommand);

		response.StatusCode.Should().Be(HttpStatusCode.OK);
	}

    [Fact, Priority(1)]
    public async Task ConfirmEmail_Should_ReturnStatusCodeOk()
	{
        var userRepository = _serviceProvider.GetService<IUserRepository>();

        var userDb = await userRepository.GetAsync(PrimaryEmail);

        var confirmEmailCommand = new ConfirmEmailCommand()
		{
            Email = PrimaryEmail,
            Token = userDb.VerificationToken
        };

        var response = await AuthHelper.ConfirmEmailAsync(_httpClient, confirmEmailCommand);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact, Priority(2)]
    public async Task Login_Should_ReturnToken()
	{
		var loginCommand = new LoginCommand
		{
			Email = PrimaryEmail,
			Password = PrimaryPassword
		};

		var message = await AuthHelper.LoginAsync(_httpClient, loginCommand);

		message.response.StatusCode.Should().Be(HttpStatusCode.OK);
		message.token.Should().NotBe(null);
	}

	[Fact]
	public async Task SocialLogin_Should_ReturnToken()
	{
		var socialLoginCommand = new SocialLoginCommand
		{
			Email = "authControllerTestSocialLogin@testemail.com",
			Provider = "GOOGLE"
		};

		var response = await AuthHelper.SocialLoginAsync(_httpClient, socialLoginCommand);
		var stringResponse = await response.Content.ReadAsStringAsync();
		var tokenDto = JsonConvert.DeserializeObject<ResponseToken>(stringResponse);

		response.StatusCode.Should().Be(HttpStatusCode.OK);
		tokenDto.Should().NotBe(null);
	}
}