using EquipIQ.Core.Common;
using EquipIQ.Core.WorkOrders;
using Mediator;

namespace EquipIQ.Application.WorkOrders.UpdateWorkOrder;

public sealed class UpdateWorkOrderCommandHandler(
    IWorkOrderRepository workOrderRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateWorkOrderCommand, WorkOrderDto>
{
    public async ValueTask<WorkOrderDto> Handle(
        UpdateWorkOrderCommand command,
        CancellationToken cancellationToken)
    {
        var workOrder = await workOrderRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(WorkOrder), command.Id);

        workOrder.UpdateDetails(
            command.Title,
            command.Description,
            command.EquipmentName,
            command.Priority);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return workOrder.ToDto();
    }
}