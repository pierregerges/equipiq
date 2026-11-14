namespace EquipIQ.Core.Common;

/// <summary>One page of results, plus the numbers a screen needs to show page links.</summary>
public sealed record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount)
{
    public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>Converts each item (for example entity to DTO), keeping the paging numbers.</summary>
    public PagedResult<TOut> Map<TOut>(Func<T, TOut> map) =>
        new(Items.Select(map).ToList(), Page, PageSize, TotalCount);
} 