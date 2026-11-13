using FluentValidation;

namespace EquipIQ.Application.WorkOrders.ChangeWorkOrderStatus;

public sealed class ChangeWorkOrderStatusCommandValidator : AbstractValidator<ChangeWorkOrderStatusCommand>
{
    public ChangeWorkOrderStatusCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty();
        RuleFor(command => command.Status).IsInEnum();
    }
}