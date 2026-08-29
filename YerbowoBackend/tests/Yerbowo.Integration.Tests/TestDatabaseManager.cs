namespace Yerbowo.Integration.Tests.Infrastructure;

public sealed class TestDatabaseManager : IAsyncDisposable
{
    private readonly MsSqlContainer _container = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithCleanUp(true)
        .WithName($"sql-server-integration-test-{Guid.NewGuid()}")
        .Build();

    private string _databaseName;

    public string ConnectionString { get; private set; }

    public static async Task EnsureCreatedAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var context = scope.ServiceProvider
            .GetRequiredService<YerbowoContext>();

        await context.Database.EnsureCreatedAsync();
    }

    public async Task StartAsync()
    {
        await _container.StartAsync();

        var masterBuilder = new SqlConnectionStringBuilder(
            _container.GetConnectionString())
        {
            InitialCatalog = "master"
        };

        using (var masterConn = new SqlConnection(masterBuilder.ConnectionString))
        {
            int attempts = 0;
            Exception lastEx = null;

            while (attempts < 10)
            {
                try
                {
                    await masterConn.OpenAsync();
                    break;
                }
                catch (Exception ex)
                {
                    lastEx = ex;
                    attempts++;
                    await Task.Delay(1000);
                }
            }

            if (!masterConn.State.HasFlag(ConnectionState.Open))
            {
                throw new InvalidOperationException(
                    "Unable to connect to SQL Server in container.",
                    lastEx);
            }

            _databaseName = "Yerbowo_Test_" + Guid.NewGuid().ToString("N");

            using var createCmd = masterConn.CreateCommand();
            createCmd.CommandText = $"CREATE DATABASE [{_databaseName}]";

            await createCmd.ExecuteNonQueryAsync();
        }

        var testBuilder = new SqlConnectionStringBuilder(
            _container.GetConnectionString())
        {
            InitialCatalog = _databaseName
        };

        ConnectionString = testBuilder.ConnectionString;
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(_databaseName))
            {
                try
                {
                    var masterBuilder = new SqlConnectionStringBuilder(
                        _container.GetConnectionString())
                    {
                        InitialCatalog = "master"
                    };

                    using var masterConn =
                        new SqlConnection(masterBuilder.ConnectionString);

                    await masterConn.OpenAsync();

                    using var dropCmd = masterConn.CreateCommand();

                    dropCmd.CommandText =
                        $"ALTER DATABASE [{_databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; " +
                        $"DROP DATABASE [{_databaseName}];";

                    await dropCmd.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Error while dropping test database {_databaseName}: {ex.Message}");
                }
            }

            await _container.StopAsync();
            await _container.DisposeAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Error while disposing db container: {ex.Message}");
        }
    }
}