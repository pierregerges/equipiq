using EquipIQ.Core.WorkOrders;
using Mediator;

namespace EquipIQ.Application.WorkOrders.UpdateWorkOrder;

public sealed record UpdateWorkOrderCommand(
    Guid Id,
    string Title,
    string Description,
    string EquipmentName,
    WorkOrderPriority Priority) : ICommand<WorkOrderDto>;