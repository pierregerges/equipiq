using EquipIQ.Core.WorkOrders;

namespace EquipIQ.Application.WorkOrders;

public sealed record WorkOrderDto(
    Guid Id,
    string Title,
    string Description,
    string EquipmentName,
    WorkOrderPriority Priority,
    WorkOrderStatus Status,
    string CreatedBy,
    bool CreatedByAgent,
    DateTimeOffset CreatedAt);