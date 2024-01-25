namespace Yerbowo.Domain.Primitives;

public abstract class AgregateRoot : BaseEntity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected AgregateRoot()
    {
    }

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);
}