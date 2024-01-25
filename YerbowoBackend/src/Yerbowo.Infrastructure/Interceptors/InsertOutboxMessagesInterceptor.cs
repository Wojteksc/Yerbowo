namespace Yerbowo.Infrastructure.Interceptors;

public class InsertOutboxMessagesInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result, 
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            InsertOutboxMessages(eventData.Context);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void InsertOutboxMessages(DbContext context)
    {
        var dateNow = DateTime.UtcNow;
        var outboxMessages = context.ChangeTracker
            .Entries<AgregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(aggregatRoot =>
            {
                var domainEvents = aggregatRoot.GetDomainEvents();

                aggregatRoot.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Type = domainEvent.GetType().Name,
                Content = JsonSerializer.Serialize(domainEvent),
                CreatedAt = dateNow,
                UpdatedAt = dateNow
            })
        .ToList();

        context.Set<OutboxMessage>().AddRange(outboxMessages);
    }
}