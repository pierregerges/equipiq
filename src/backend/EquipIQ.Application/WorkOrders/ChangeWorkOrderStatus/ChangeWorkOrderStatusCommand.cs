using EquipIQ.Core.WorkOrders;
using Mediator;

namespace EquipIQ.Application.WorkOrders.ChangeWorkOrderStatus;

public sealed record ChangeWorkOrderStatusCommand(Guid Id, WorkOrderStatus Status)
    : ICommand<WorkOrderDto>;