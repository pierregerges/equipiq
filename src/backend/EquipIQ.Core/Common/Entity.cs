namespace EquipIQ.Core.Common;

/// <summary>
/// Base class for every entity. It stores the domain events the entity raised,
/// so they can be published later, after the database save succeeds.
/// </summary>
public abstract class Entity
{
    public Guid Id { get; protected set; }


    // Domain events are stored in a list so they can be published later, after the database save succeeds.
    private readonly List<IDomainEvent> _domainEvents = [];
    // The list is exposed as a read-only collection so it can be inspected, but not modified, from outside the entity.
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    // Derived classes can raise domain events by calling this method.
    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    // Derived classes can clear the domain events after they have been published.
    public void ClearDomainEvents() => _domainEvents.Clear();
} 