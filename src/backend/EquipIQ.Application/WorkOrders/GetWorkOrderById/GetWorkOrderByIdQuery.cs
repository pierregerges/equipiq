using Mediator;

namespace EquipIQ.Application.WorkOrders.GetWorkOrderById;

public sealed record GetWorkOrderByIdQuery(Guid Id) : IQuery<WorkOrderDto>;