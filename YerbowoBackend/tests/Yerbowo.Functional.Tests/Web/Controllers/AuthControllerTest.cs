namespace Yerbowo.Functional.Tests.Web.Controllers;

public class AuthControllerTest : ApiTestBase
{
    private readonly HttpClient _httpClient;
    private readonly IServiceScopeFactory _scope;
    private const string PrimaryPassword = "secret";

    public AuthControllerTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
        _scope = WebApplicationFactory.Services.GetRequiredService<IServiceScopeFactory>();
        _httpClient = CreateClient();
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
    public async Task Login_Should_ReturnStatusCodeOk()
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
        using (var scope = _scope.CreateScope())
        {
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var userDb = await userRepository.GetAsync(email);

            var confirmEmailCommand = new ConfirmEmailCommand()
            {
                Email = email,
                Token = userDb.VerificationToken
            };

            var response = await AuthHelper.ConfirmEmailAsync(_httpClient, confirmEmailCommand);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
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