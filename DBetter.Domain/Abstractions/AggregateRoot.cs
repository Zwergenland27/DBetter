using MediatR;

namespace DBetter.Domain.Abstractions;

public class AggregateRoot<TId> : Entity<TId>, IHasDomainEvent
where TId: notnull
{
    private readonly List<IDomainEvent> _domainEvents = [];
    
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.ToList().AsReadOnly();
    
    protected AggregateRoot(TId id) : base(id)
    {
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}