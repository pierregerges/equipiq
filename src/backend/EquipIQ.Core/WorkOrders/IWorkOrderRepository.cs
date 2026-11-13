namespace EquipIQ.Core.WorkOrders;

/// <summary>
/// How the app stores and finds work orders. Core only describes it;
/// Infrastructure supplies the real database code.
/// </summary>
/// 
/// <remarks>
/// ***Update need no method: 
/// changes to a loaded work order are saved by SaveChangesAsync in the IUnitOfWork interface.***
public interface IWorkOrderRepository
{
    Task<WorkOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task AddAsync(WorkOrder workOrder, CancellationToken cancellationToken);

    void Remove(WorkOrder workOrder);
} 