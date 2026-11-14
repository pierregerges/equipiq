using FluentValidation;

namespace EquipIQ.Application.WorkOrders.DeleteWorkOrder;

public sealed class DeleteWorkOrderCommandValidator : AbstractValidator<DeleteWorkOrderCommand>
{
    public DeleteWorkOrderCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty();
    }
}