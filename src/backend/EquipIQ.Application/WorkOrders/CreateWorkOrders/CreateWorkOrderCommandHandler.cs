using EquipIQ.Core.Common;
using EquipIQ.Core.WorkOrders;
using Mediator;

namespace EquipIQ.Application.WorkOrders.CreateWorkOrder;

public sealed class CreateWorkOrderCommandHandler(
    IWorkOrderRepository workOrderRepository,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider) : ICommandHandler<CreateWorkOrderCommand, WorkOrderDto>
{
    public async ValueTask<WorkOrderDto> Handle(
        CreateWorkOrderCommand command,
        CancellationToken cancellationToken)
    {
        var workOrder = WorkOrder.Create(
            command.Title,
            command.Description,
            command.EquipmentName,
            command.Priority,
            command.CreatedBy,
            command.CreatedByAgent,
            timeProvider.GetUtcNow());

        await workOrderRepository.AddAsync(workOrder, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return workOrder.ToDto();
    }
}