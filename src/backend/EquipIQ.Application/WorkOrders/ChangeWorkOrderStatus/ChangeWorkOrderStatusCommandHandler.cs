using EquipIQ.Core.Common;
using EquipIQ.Core.WorkOrders;
using Mediator;

namespace EquipIQ.Application.WorkOrders.ChangeWorkOrderStatus;

public sealed class ChangeWorkOrderStatusCommandHandler(
    IWorkOrderRepository workOrderRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<ChangeWorkOrderStatusCommand, WorkOrderDto>
{
    public async ValueTask<WorkOrderDto> Handle(
        ChangeWorkOrderStatusCommand command,
        CancellationToken cancellationToken)
    {
        var workOrder = await workOrderRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(WorkOrder), command.Id);

        // The entity decides whether this move is allowed (for example Open -> InProgress).
        workOrder.ChangeStatus(command.Status);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return workOrder.ToDto();
    }
} 