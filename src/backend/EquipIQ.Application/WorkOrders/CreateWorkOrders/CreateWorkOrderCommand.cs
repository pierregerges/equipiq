using EquipIQ.Core.WorkOrders;
using Mediator;

namespace EquipIQ.Application.WorkOrders.CreateWorkOrder;

/// <summary>
/// "Please create a work order." A command changes data and returns the result.
/// </summary>
public sealed record CreateWorkOrderCommand(
    string Title,
    string Description,
    string EquipmentName,
    WorkOrderPriority Priority,
    string CreatedBy,
    bool CreatedByAgent = false) : ICommand<WorkOrderDto>;