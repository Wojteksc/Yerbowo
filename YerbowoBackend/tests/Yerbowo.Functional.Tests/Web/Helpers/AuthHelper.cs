using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Yerbowo.Application.Auth;
using Yerbowo.Application.Auth.Login;
using Yerbowo.Application.Auth.Register;
using Yerbowo.Application.Auth.SocialLogin;
using Yerbowo.Application.Users.GetUserDetails;

namespace Yerbowo.Functional.Tests.Web.Helpers
{
    public static class AuthHelper
	{
		public async static Task<HttpResponseMessage> RegisterAsync(HttpClient httpClient, RegisterCommand registerCommand)
		{
			HttpRequestMessage request = GetHttpRequestMessage(registerCommand);
			
			return await httpClient.PostAsync("api/auth/register", request.Content);
		}

		public async static Task<(HttpResponseMessage response, ResponseToken token)> LoginAsync(HttpClient httpClient, LoginCommand loginCommand)
		{
			HttpRequestMessage request = GetHttpRequestMessage(loginCommand);

			HttpResponseMessage response = await httpClient.PostAsync("api/auth/login", request.Content);

			var stringResponse = await response.Content.ReadAsStringAsync();
			var tokenDto = JsonConvert.DeserializeObject<ResponseToken>(stringResponse);

			SetToken(httpClient, tokenDto.Token.Token);

			return (response, tokenDto);
		}

		public async static Task<HttpResponseMessage> SocialLoginAsync(HttpClient httpClient, SocialLoginCommand socialLoginCommand)
		{
			HttpRequestMessage inputMessage = GetHttpRequestMessage(socialLoginCommand);

			return await httpClient.PostAsync("api/auth/socialLogin", inputMessage.Content);
		}

		public static async Task<UserDetailsDto> SignUp(HttpClient httpClient, RegisterCommand registerCommand)
		{
			await RegisterAsync(httpClient, registerCommand);

			var loginCommand = new LoginCommand()
			{
				FirstName = registerCommand.FirstName,
				LastName = registerCommand.LastName,
				Email = registerCommand.Email,
				Password = registerCommand.Password,
				Provider = registerCommand.Provider,
				PhotoUrl = registerCommand.PhotoUrl
			};

			await LoginAsync(httpClient, loginCommand);

			var responseUser = await httpClient.GetAsync($"api/users/{loginCommand.Email}");
			var responseUserString = await responseUser.Content.ReadAsStringAsync();
			var user = JsonConvert.DeserializeObject<UserDetailsDto>(responseUserString);

			return user;
		}

		public static void SetToken(HttpClient httpClient, string token)
		{
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		}

		private static HttpRequestMessage GetHttpRequestMessage<T>(T command)
		{
			string serializedData = JsonConvert.SerializeObject(command);

			return new HttpRequestMessage
			{
				Content = new StringContent(serializedData, Encoding.UTF8, "application/json")
			};
		}
	}
}
