using EquipIQ.Core.Common;
using EquipIQ.Core.WorkOrders;

namespace EquipIQ.Application.UnitTests.Fakes;

/// <summary>A work order "database" that is just a list in memory.</summary>
public sealed class FakeWorkOrderRepository : IWorkOrderRepository
{
    public List<WorkOrder> Items { get; } = [];

    /// <summary>The filters the handler passed to the last search.</summary>
    public WorkOrderSearchCriteria? LastCriteria { get; private set; }

    public Task<WorkOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        Task.FromResult(Items.FirstOrDefault(workOrder => workOrder.Id == id));

    public Task<PagedResult<WorkOrder>> SearchAsync(
        WorkOrderSearchCriteria criteria,
        CancellationToken cancellationToken)
    {
        LastCriteria = criteria;
        return Task.FromResult(new PagedResult<WorkOrder>(Items, criteria.Page, criteria.PageSize, Items.Count));
    }

    public Task AddAsync(WorkOrder workOrder, CancellationToken cancellationToken)
    {
        Items.Add(workOrder);
        return Task.CompletedTask;
    }

    public void Remove(WorkOrder workOrder) => Items.Remove(workOrder);
}