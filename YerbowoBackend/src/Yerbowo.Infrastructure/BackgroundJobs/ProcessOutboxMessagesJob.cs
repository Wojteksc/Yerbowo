namespace Yerbowo.Infrastructure.BackgroundJobs;

public class ProcessOutboxMessagesJob : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;
    private readonly IInterfaceConverterJsonOptions _interfaceConverterJsonOptions;

    public ProcessOutboxMessagesJob(
        IServiceScopeFactory scopeFactory,
        ILogger<ProcessOutboxMessagesJob> logger,
        IInterfaceConverterJsonOptions interfaceConverterJsonOptions)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _interfaceConverterJsonOptions = interfaceConverterJsonOptions;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Process outbox messages job running.");

        await WaitForCreationDatabase();

        while (!cancellationToken.IsCancellationRequested)
        {
            await ProcessOutboxMessages(cancellationToken);

            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task ProcessOutboxMessages(CancellationToken cancellationToken)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<YerbowoContext>();

        List<OutboxMessage> messages = await dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProccessedAt == null)
            .Take(20)
            .ToListAsync(cancellationToken);

        if (!messages.Any())
        {
            return;
        }

        var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

        foreach (OutboxMessage outboxMessage in messages)
        {
            var jsonDeserializerOptions = _interfaceConverterJsonOptions.GetJsonOptions(className: outboxMessage.Type);

            IDomainEvent? domainEvent = JsonSerializer
                .Deserialize<IDomainEvent>(outboxMessage.Content, jsonDeserializerOptions);

            if (domainEvent is null)
            {
                continue;
            }

            await publisher.Publish(domainEvent, cancellationToken);

            outboxMessage.ProccessedAt = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task WaitForCreationDatabase()
    {
        bool dbExists;
        do
        {
            await Task.Delay(500);
            await using var scope = _scopeFactory.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<YerbowoContext>();
            dbExists = dbContext.GetService<IDatabaseCreator>().CanConnect();
        }
        while (!dbExists);
    }
}