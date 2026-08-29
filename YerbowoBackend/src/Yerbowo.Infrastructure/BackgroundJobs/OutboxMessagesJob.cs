namespace Yerbowo.Infrastructure.BackgroundJobs;

internal sealed class OutboxMessagesJob(
    IServiceScopeFactory scopeFactory,
    ILogger<OutboxMessagesJob> logger,
    IInterfaceConverterJsonOptions interfaceConverterJsonOptions) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Process: Outbox messages.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessOutboxMessages(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("OutboxMessagesJob cancelled.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while processing outbox messages.");
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }

    private async Task ProcessOutboxMessages(CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<YerbowoContext>();

        List<OutboxMessage> messages = await dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProccessedAt == null)
            .OrderBy(m => m.CreatedAt)
            .ThenBy(m => m.Id)
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

            IDomainEvent? domainEvent = JsonSerializer.Deserialize<IDomainEvent>(outboxMessage.Content, jsonDeserializerOptions);

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