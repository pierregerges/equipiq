namespace EquipIQ.Core.Common;

/// <summary>
/// Marker for "something important happened in the domain".
/// Core defines it itself so it does not depend on any library.
/// </summary>
public interface IDomainEvent
{
    DateTimeOffset OccurredAt { get; }
}