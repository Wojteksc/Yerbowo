namespace Yerbowo.Infrastructure.BackgroundJobs;

internal sealed class OutboxMessagesJob(
    IServiceScopeFactory scopeFactory,
    ILogger<OutboxMessagesJob> logger,
    IInterfaceConverterJsonOptions interfaceConverterJsonOptions) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Process: Outbox messages.");

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await ProcessOutboxMessages(cancellationToken);
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
            }
        }
        catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
        {
            logger.LogInformation("OutboxMessagesJob cancelled.");
            logger.LogError(ex.Message);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) 
        => Task.CompletedTask;

    private async Task ProcessOutboxMessages(CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<YerbowoContext>();

        List<OutboxMessage> messages = await dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProccessedAt == null)
            .Take(20)
            .ToListAsync(cancellationToken);

        if (messages.Count == 0)
        {
            return;
        }

        var dispatcher = scope.ServiceProvider.GetRequiredService<IRequestDispatcher>();

        foreach (OutboxMessage outboxMessage in messages)
        {
            var jsonDeserializerOptions = interfaceConverterJsonOptions.GetJsonOptions(className: outboxMessage.Type);

            IDomainEvent? domainEvent = JsonSerializer
                .Deserialize<IDomainEvent>(outboxMessage.Content, jsonDeserializerOptions);

            if (domainEvent is null)
            {
                continue;
            }

            await dispatcher.Publish(domainEvent, cancellationToken);

            outboxMessage.ProccessedAt = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}