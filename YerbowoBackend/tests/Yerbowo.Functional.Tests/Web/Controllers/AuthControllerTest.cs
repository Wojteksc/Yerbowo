using Yerbowo.Application.Functions.Auth.Command;
using Yerbowo.Application.Functions.Auth.Command.ConfirmEmail;
using Yerbowo.Application.Functions.Auth.Command.Login;
using Yerbowo.Application.Functions.Auth.Command.Register;
using Yerbowo.Application.Functions.Auth.Command.SocialLogin;
using Yerbowo.Functional.Tests.Web.Helpers;
using Yerbowo.Infrastructure.Data.Users;

namespace Yerbowo.Functional.Tests.Web.Controllers;

public class AuthControllerTest : IClassFixture<WebTestFixture>
{
	private readonly HttpClient _httpClient;
	private readonly IServiceProvider _serviceProvider;
	private const string PrimaryPassword = "secret";

	public AuthControllerTest(WebTestFixture factory)
	{
		_httpClient = factory.CreateClient();
		_serviceProvider = factory.Services;
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
        var tokenDto = JsonSerializer.Deserialize<ResponseToken>(stringResponse);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        tokenDto.Should().NotBe(null);
    }

    [Fact]
	public async Task Register_Should_ReturnStatusCodeOk()
	{
		string email = "register@testemail.com";
        await RegisterScenario(email);
	}

	[Fact]
    public async Task ConfirmEmail_Should_ReturnStatusCodeOk()
	{
        string email = "confirmEmail@testemail.com";

        await RegisterScenario(email);

		await ConfirmEmailScenario(email);
	}

	[Fact]
    public async Task Login_Should_ReturnToken()
	{
		string email = "login@testemail.com";

		await RegisterScenario(email);

		await ConfirmEmailScenario(email);

		await LoginScenario(email);
	}

    private async Task RegisterScenario(string email)
    {
        var registerCommand = new RegisterCommand
        {
            Email = email,
            ConfirmEmail = email,
            Password = PrimaryPassword,
            ConfirmPassword = PrimaryPassword,
            FirstName = "FirstTest",
            LastName = "LastTest",
        };

        var response = await AuthHelper.RegisterAsync(_httpClient, registerCommand);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task ConfirmEmailScenario(string email)
    {
        var userRepository = _serviceProvider.GetService<IUserRepository>();

        var userDb = await userRepository.GetAsync(email);

        var confirmEmailCommand = new ConfirmEmailCommand()
        {
            Email = email,
            Token = userDb.VerificationToken
        };

        var response = await AuthHelper.ConfirmEmailAsync(_httpClient, confirmEmailCommand);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task LoginScenario(string email)
    {
        var loginCommand = new LoginCommand
        {
            Email = email,
            Password = PrimaryPassword
        };

        var message = await AuthHelper.LoginAsync(_httpClient, loginCommand);

        message.response.StatusCode.Should().Be(HttpStatusCode.OK);
        message.token.Should().NotBe(null);
    }
}