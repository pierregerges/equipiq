using EquipIQ.Core.Common;
using EquipIQ.Core.WorkOrders;
using Mediator;

namespace EquipIQ.Application.WorkOrders.GetWorkOrders;

public sealed class GetWorkOrdersQueryHandler(IWorkOrderRepository workOrderRepository)
    : IQueryHandler<GetWorkOrdersQuery, PagedResult<WorkOrderDto>>
{
    public async ValueTask<PagedResult<WorkOrderDto>> Handle(
        GetWorkOrdersQuery query,
        CancellationToken cancellationToken)
    {
        var criteria = new WorkOrderSearchCriteria(
            query.Status,
            query.Priority,
            query.SearchText,
            query.Page,
            query.PageSize);

        var page = await workOrderRepository.SearchAsync(criteria, cancellationToken);

        // workOrder => workOrder.ToDto() is a tiny function that says “take one work order, turn it into a DTO."
        // page.Map(...) is the helper we added to PagedResult. It walks through every item on the page and converts it.
        // The paging numbers are kept. The result is a PagedResult<WorkOrderDto> with the same Page, PageSize, TotalCount, and TotalPages, so the Angular page can still draw “Page 2 of 5.”
        return page.Map(workOrder => workOrder.ToDto());
    }
} 