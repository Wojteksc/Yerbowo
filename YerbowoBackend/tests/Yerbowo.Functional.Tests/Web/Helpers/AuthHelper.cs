namespace Yerbowo.Functional.Tests.Web.Helpers;

public static class AuthHelper
{
	public async static Task<HttpResponseMessage> RegisterAsync(HttpClient httpClient, RegisterCommand registerCommand)
	{
		HttpRequestMessage request = GetHttpRequestMessage(registerCommand);
		
		return await httpClient.PostAsync("api/auth/register", request.Content);
	}
    
	public async static Task<HttpResponseMessage> ConfirmEmailAsync(HttpClient httpClient, ConfirmEmailCommand confirmEmailCommand)
    {
        HttpRequestMessage request = GetHttpRequestMessage(confirmEmailCommand);

        return await httpClient.PostAsync("api/auth/confirmEmail", request.Content);
    }

    public async static Task<(HttpResponseMessage response, ResponseToken token)> LoginAsync(HttpClient httpClient, LoginCommand loginCommand)
	{
		HttpRequestMessage request = GetHttpRequestMessage(loginCommand);

		HttpResponseMessage response = await httpClient.PostAsync("api/auth/login", request.Content);

		var stringResponse = await response.Content.ReadAsStringAsync();
		var tokenDto = JsonSerializer.Deserialize<ResponseToken>(stringResponse, 
			new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

		SetToken(httpClient, tokenDto.Token.Token);

		return (response, tokenDto);
	}

	public async static Task<HttpResponseMessage> SocialLoginAsync(HttpClient httpClient, SocialLoginCommand socialLoginCommand)
	{
		HttpRequestMessage inputMessage = GetHttpRequestMessage(socialLoginCommand);

		return await httpClient.PostAsync("api/auth/socialLogin", inputMessage.Content);
	}

	public async static Task<UserDetailsDto> SignUp(HttpClient httpClient, RegisterCommand registerCommand, IServiceProvider serviceProvider)
	{
        await RegisterAsync(httpClient, registerCommand);

        var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

        var userDb = await userRepository.GetAsync(registerCommand.Email);

        var confirmEmailCommand = new ConfirmEmailCommand()
        {
            Email = registerCommand.Email,
            Token = userDb.VerificationToken
        };

        await ConfirmEmailAsync(httpClient, confirmEmailCommand);

        var loginCommand = new LoginCommand()
        {
            FirstName = registerCommand.FirstName,
            LastName = registerCommand.LastName,
            Email = registerCommand.Email,
			Password = registerCommand.Password
		};

        await LoginAsync(httpClient, loginCommand);

        var responseUser = await httpClient.GetAsync($"api/users/{loginCommand.Email}");
        var responseUserString = await responseUser.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserDetailsDto>(responseUserString, 
			new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        return user;
    }

	public static void SetToken(HttpClient httpClient, string token)
	{
		httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
	}

	private static HttpRequestMessage GetHttpRequestMessage<T>(T command)
	{
		string serializedData = JsonSerializer.Serialize(command);

		return new HttpRequestMessage
		{
			Content = new StringContent(serializedData, Encoding.UTF8, "application/json")
		};
	}
}