using EquipIQ.Core.Common;
using EquipIQ.Core.WorkOrders;
using Mediator;

namespace EquipIQ.Application.WorkOrders.DeleteWorkOrder;

public sealed class DeleteWorkOrderCommandHandler(
    IWorkOrderRepository workOrderRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteWorkOrderCommand>
{
    public async ValueTask<Unit> Handle(
        DeleteWorkOrderCommand command,
        CancellationToken cancellationToken)
    {
        var workOrder = await workOrderRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(WorkOrder), command.Id);

        workOrderRepository.Remove(workOrder);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}