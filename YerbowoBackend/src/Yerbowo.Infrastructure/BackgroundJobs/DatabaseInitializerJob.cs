namespace Yerbowo.Infrastructure.BackgroundJobs;

internal sealed class DatabaseInitializerJob(
    IServiceScopeFactory scopeFactory,
    ILogger<DatabaseInitializerJob> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Process: Database initialization");

        await using var scope = scopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<YerbowoContext>();
        var passwordManager = scope.ServiceProvider.GetRequiredService<IPasswordManager>();
        var databaseInitalizer = new DatabaseInitializer(dbContext, passwordManager);
        await databaseInitalizer.Seed();
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}