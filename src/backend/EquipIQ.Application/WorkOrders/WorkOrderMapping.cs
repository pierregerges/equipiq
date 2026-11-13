using EquipIQ.Core.WorkOrders;

namespace EquipIQ.Application.WorkOrders;

public static class WorkOrderMapping
{
    public static WorkOrderDto ToDto(this WorkOrder workOrder) =>
        new(
            workOrder.Id,
            workOrder.Title,
            workOrder.Description,
            workOrder.EquipmentName,
            workOrder.Priority,
            workOrder.Status,
            workOrder.CreatedBy,
            workOrder.CreatedByAgent,
            workOrder.CreatedAt);
}