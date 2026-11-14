using EquipIQ.Core.Common;
using EquipIQ.Core.WorkOrders;
using Mediator;

namespace EquipIQ.Application.WorkOrders.GetWorkOrders;

/// <summary>
/// "Show me a page of work orders." Every filter is optional.
/// </summary>
public sealed record GetWorkOrdersQuery(
    WorkOrderStatus? Status = null,
    WorkOrderPriority? Priority = null,
    string? SearchText = null,
    int Page = 1,
    int PageSize = WorkOrderSearchCriteria.DefaultPageSize) : IQuery<PagedResult<WorkOrderDto>>;