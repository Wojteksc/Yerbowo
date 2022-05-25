using Yerbowo.Application.Auth;
using Yerbowo.Application.Auth.Login;
using Yerbowo.Application.Auth.Register;
using Yerbowo.Application.Auth.SocialLogin;
using Yerbowo.Functional.Tests.Web.Helpers;

namespace Yerbowo.Functional.Tests.Web.Controllers;

[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class AuthControllerTest : IClassFixture<WebTestFixture>
{
	private readonly HttpClient _httpClient;
	private const string PrimaryEmail = "authControllerTest@gmail.com";
	private const string PrimaryPassword = "secret";

	public AuthControllerTest(WebTestFixture factory)
	{
		_httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions());
	}

	[Fact, Priority(0)]
	public async Task Register_Should_ReturnStatusCodeCreated()
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

		response.StatusCode.Should().Be(HttpStatusCode.Created);
	}

	[Fact]
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
			Email = "authControllerTestSocialLogin@gmail.com",
			Provider = "GOOGLE"
		};

		var response = await AuthHelper.SocialLoginAsync(_httpClient, socialLoginCommand);
		var stringResponse = await response.Content.ReadAsStringAsync();
		var tokenDto = JsonConvert.DeserializeObject<ResponseToken>(stringResponse);

		response.StatusCode.Should().Be(HttpStatusCode.OK);
		tokenDto.Should().NotBe(null);
	}
}