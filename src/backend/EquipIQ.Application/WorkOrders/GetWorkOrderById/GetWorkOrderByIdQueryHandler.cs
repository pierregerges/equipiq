using EquipIQ.Core.Common;
using EquipIQ.Core.WorkOrders;
using Mediator;

namespace EquipIQ.Application.WorkOrders.GetWorkOrderById;

public sealed class GetWorkOrderByIdQueryHandler(IWorkOrderRepository workOrderRepository)
    : IQueryHandler<GetWorkOrderByIdQuery, WorkOrderDto>
{
    public async ValueTask<WorkOrderDto> Handle(
        GetWorkOrderByIdQuery query,
        CancellationToken cancellationToken)
    {
        var workOrder = await workOrderRepository.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(WorkOrder), query.Id);

        return workOrder.ToDto();
    }
} 