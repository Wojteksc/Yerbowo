using Microsoft.Extensions.Configuration;
using Serilog;
using Yerbowo.Api.Builders;

namespace Yerbowo.Functional.Tests.Web;

public abstract class ApiTestBase : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly WebApplicationFactory<Startup> _webApplicationFactory;

    public WebApplicationFactory<Startup> WebApplicationFactory => _webApplicationFactory;
    public User User { get; private set; }

    public ApiTestBase(WebApplicationFactory<Startup> factory)
    {
        Log.Information("ApiTestBase Start");
        var config = ConfigBuilder
                .CreateConfigBuilder()
                .AddInMemoryCollection(new[] { new KeyValuePair<string, string>("UseInMemoryDatabase", "true") })
                .Build();

        _webApplicationFactory = factory.WithWebHostBuilder(
            builder => builder.UseConfiguration(config)
            //.ConfigureAppConfiguration(ConfigureAppConfiguration)
            .UseEnvironment("Testing"));

        //Porobić logi, sprawdzić w którym miejscu zawiesza się pipeline


        User = GetUserByEmail("yerbowoTestAdmin@functionalTestYerbowo.com");
        Log.Information("ApiTestBase End");
    }

    private User GetUserByEmail(string email)
    {
        using (var scope = WebApplicationFactory.Server.Services.CreateScope())
        {
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            return userRepository.GetAsync(email).Result;
        }
    }

    protected virtual void ConfigureAppConfiguration(IConfigurationBuilder configuration)
    {
        // For testing, we want the in memory database to be used so this can be run in CI/CD without spinning up a DB for it.
        configuration.AddInMemoryCollection(new[] { new KeyValuePair<string, string>("UseInMemoryDatabase", "true") });
        
        //configuration.AddJsonFile("appsettings.json");
        //configuration.AddJsonFile("appsettings.Development.json", optional: true);
        //configuration.AddJsonFile("appsettings.Cloud.json", optional: true);
    }

    protected virtual HttpClient CreateClient()
    {
        Log.Information("_webApplicationFactory.CreateClient() Start");
        var client = _webApplicationFactory.CreateClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        AuthHelper.LoginAsync(client, new LoginCommand() { Email = User.Email, Password = "Haslo123." }).Wait();

        Log.Information("_webApplicationFactory.CreateClient() End");
        return client;
    }
}