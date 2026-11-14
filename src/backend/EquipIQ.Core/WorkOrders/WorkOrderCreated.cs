using EquipIQ.Core.Common;

namespace EquipIQ.Core.WorkOrders;

/// <summary>Raised when a new work order is created.</summary>
public sealed record WorkOrderCreated(
    Guid WorkOrderId,
    string Title,
    WorkOrderPriority Priority,
    DateTimeOffset OccurredAt) : IDomainEvent;