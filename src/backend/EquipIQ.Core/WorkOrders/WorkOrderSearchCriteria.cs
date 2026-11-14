namespace EquipIQ.Core.WorkOrders;

/// <summary>What the caller wants from the list. Empty filters mean "don't filter on this".</summary>
public sealed record WorkOrderSearchCriteria(
    WorkOrderStatus? Status,
    WorkOrderPriority? Priority,
    string? SearchText,
    int Page,
    int PageSize)
{
    public const int DefaultPageSize = 20;
    public const int MaxPageSize = 100;
    public const int SearchTextMaxLength = 100;
} 