using EquipIQ.Core.WorkOrders;
using FluentValidation;

namespace EquipIQ.Application.WorkOrders.GetWorkOrders;

public sealed class GetWorkOrdersQueryValidator : AbstractValidator<GetWorkOrdersQuery>
{
    public GetWorkOrdersQueryValidator()
    {
        RuleFor(query => query.Page).GreaterThanOrEqualTo(1);

        // Stops anyone asking for a million rows in one request.
        RuleFor(query => query.PageSize).InclusiveBetween(1, WorkOrderSearchCriteria.MaxPageSize);

        RuleFor(query => query.SearchText).MaximumLength(WorkOrderSearchCriteria.SearchTextMaxLength);

        RuleFor(query => query.Status).IsInEnum().When(query => query.Status.HasValue);
        RuleFor(query => query.Priority).IsInEnum().When(query => query.Priority.HasValue);
    }
}