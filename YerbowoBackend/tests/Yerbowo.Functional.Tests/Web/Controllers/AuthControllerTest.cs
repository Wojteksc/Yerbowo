using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Yerbowo.Application.Auth;
using Yerbowo.Application.Auth.Login;
using Yerbowo.Application.Auth.Register;
using Yerbowo.Application.Auth.SocialLogin;
using Yerbowo.Functional.Tests.Web.Helpers;

namespace Yerbowo.Functional.Tests.Web.Controllers
{
	public class AuthControllerTest : IClassFixture<WebTestFixture>
	{
		private readonly HttpClient _httpClient;
		private const string PrimaryEmail = "authControllerTest@gmail.com";
		private const string PrimaryPassword = "secret";

		public AuthControllerTest(WebTestFixture factory)
		{
			_httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions());
		}

		[Fact]
		public async Task Register_Account_Should_Return_Status_Code_201()
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

			Assert.Equal(HttpStatusCode.Created, response.StatusCode);
		}

		[Fact]
		public async Task Login_should_return_not_null_token()
		{
			var loginCommand = new LoginCommand
			{
				Email = PrimaryEmail,
				Password = PrimaryPassword
			};

			var message = await AuthHelper.LoginAsync(_httpClient, loginCommand);

			Assert.Equal(HttpStatusCode.OK, message.response.StatusCode);
			Assert.NotNull(message.token);
		}

		[Fact]
		public async Task Social_login_should_return_not_null_token()
		{
			var socialLoginCommand = new SocialLoginCommand
			{
				Email = "authControllerTestSocialLogin@gmail.com",
				Provider = "GOOGLE"
			};

			var response = await AuthHelper.SocialLoginAsync(_httpClient, socialLoginCommand);
			var stringResponse = await response.Content.ReadAsStringAsync();
			var tokenDto = JsonConvert.DeserializeObject<ResponseToken>(stringResponse);

			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.NotNull(tokenDto);
		}
	}
}
