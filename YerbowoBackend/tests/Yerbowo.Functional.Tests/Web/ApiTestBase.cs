namespace Yerbowo.Functional.Tests.Web;

public abstract class ApiTestBase : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly WebApplicationFactory<Startup> _webApplicationFactory;

    public WebApplicationFactory<Startup> WebApplicationFactory => _webApplicationFactory;

    public User User { get; private set; }

    public ApiTestBase(WebApplicationFactory<Startup> factory)
    {
        string environment = "";

#if DEBUG
        environment = "Development";
#else
        environment = "Production";
        string root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../../"));
        string envFilePath = Path.Combine(root, ".env");

        if (!File.Exists(envFilePath))
        {
            throw new FileNotFoundException($".env file not found at expected location: {envFilePath}");
        }

        DotNetEnv.Env.Load(envFilePath);
#endif

        _webApplicationFactory = factory.WithWebHostBuilder(
            builder => builder
            .ConfigureTestServices(services =>
            {
                var descriptor = services.Single(s => s.ImplementationType == typeof(OutboxMessagesJob));
                services.Remove(descriptor);
            })
            .ConfigureAppConfiguration(ConfigureAppConfiguration)
            .UseEnvironment(environment));
        
        ExecuteDatabaseInitializerJob();

        User = GetUserByEmail("yerbowoTestAdmin@functionalTestYerbowo.com");
    }

    protected virtual void ConfigureAppConfiguration(IConfigurationBuilder configuration)
    {
        // For testing, we want the in memory database to be used so this can be run in CI/CD without spinning up a DB for it.
        configuration
            .AddInMemoryCollection(new[] { new KeyValuePair<string, string>("UseInMemoryDatabase", "true") })
            .AddEnvironmentVariables();
    }

    protected virtual HttpClient CreateClient()
    {
        var client = _webApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions() { AllowAutoRedirect = false });
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var loginCommand = new LoginCommand() { Email = User.Email, Password = "Haslo123." };
        
        Task.Run(async () => await AuthHelper.LoginAsync(client, loginCommand)).Wait();
     
        return client;
    }

    private User GetUserByEmail(string email)
    {
        using (var scope = WebApplicationFactory.Server.Services.CreateScope())
        {
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            return userRepository.GetAsync(email).Result;
        }
    }

    private void ExecuteDatabaseInitializerJob()
    {
        try
        {
            using var scope = WebApplicationFactory.Server.Services.CreateScope();
            var scopeFactory = scope.ServiceProvider.GetRequiredService<IServiceScopeFactory>();
            var loggerService = scope.ServiceProvider.GetRequiredService<ILogger<DatabaseInitializerJob>>();
            var job = new DatabaseInitializerJob(scopeFactory, loggerService);
            Task.Run(async () => await job.StartAsync(default)).Wait();
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Task cancelled");
        }
        catch (Exception ex)
        {
            throw new Exception($"Something went wrong while executing {nameof(DatabaseInitializerJob)}", ex);
        }
    }
}