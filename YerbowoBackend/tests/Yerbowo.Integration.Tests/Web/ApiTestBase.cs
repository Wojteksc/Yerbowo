using Yerbowo.Integration.Tests.Infrastructure;

namespace Yerbowo.Integration.Tests.Web;

public abstract class ApiTestBase : IClassFixture<WebApplicationFactory<Startup>>, IAsyncLifetime
{
    private readonly WebApplicationFactory<Startup> _baseFactory;
    private readonly TestDatabaseManager _databaseManager = new();

    private WebApplicationFactory<Startup> _webApplicationFactory;

    protected HttpClient _httpClient;

    protected IServiceScopeFactory _scope;

    public User User { get; private set; }

    public string Environment { get; private set; } = "Test";

    public ApiTestBase(WebApplicationFactory<Startup> factory)
    {
#if DEBUG
        Environment = "Development";
#endif
        _baseFactory = factory;
    }

    public virtual async Task InitializeAsync()
    {
        await _databaseManager.StartAsync();

        _webApplicationFactory = TestHostFactory.Create(
            _baseFactory,
            Environment,
            ConfigureAppConfiguration);

        var _ = _webApplicationFactory.Server;

        _scope = _webApplicationFactory.Services
            .GetRequiredService<IServiceScopeFactory>();

        await TestDatabaseManager.EnsureCreatedAsync(_webApplicationFactory.Services);

        var userSeeder = new UserSeeder(_scope);
        await userSeeder.SeedAsync();

        User = await GetUser(UserSeeder.DefaultEmail);

        _httpClient = await CreateHttpClient();
    }

    public async Task DisposeAsync()
    {
        await _databaseManager.DisposeAsync();
    }

    protected virtual void ConfigureAppConfiguration(IConfigurationBuilder configuration)
    {
        Console.WriteLine($"[TEST] Using connection string: {_databaseManager.ConnectionString}");

        configuration.AddInMemoryCollection(
            new[]
            {
                new KeyValuePair<string, string>(
                    "ConnectionStrings:DefaultConnection",
                    _databaseManager.ConnectionString)
            });

        if (Environment == "Test")
        {
            LoadEnvOrSystemVariables(configuration);

            configuration.AddAzureKeyVault();
        }
    }

    private static void LoadEnvOrSystemVariables(IConfigurationBuilder configuration)
    {
        string envFilePath = GetEnvFilePath();

        if (File.Exists(envFilePath))
        {
            DotNetEnv.Env.Load(envFilePath);
        }
        else
        {
            configuration.AddEnvironmentVariables();
        }
    }

    private static string GetEnvFilePath()
    {
        string root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../../"));

        return Path.Combine(root, ".env");
    }

    private async Task<HttpClient> CreateHttpClient()
    {
        var client = _webApplicationFactory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        var loginCommand = new LoginCommand
        {
            Email = User.Email,
            Password = UserSeeder.DefaultPassword
        };

        await AuthHelper.LoginAsync(client, loginCommand);

        return client;
    }

    private async Task<User> GetUser(string email)
    {
        await using var scope = _scope.CreateAsyncScope();

        var userRepository = scope.ServiceProvider
            .GetRequiredService<IUserRepository>();

        return await userRepository.GetActiveByEmailAsync(email);
    }
}