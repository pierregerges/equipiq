using EquipIQ.Core.WorkOrders;
using FluentValidation;

namespace EquipIQ.Application.WorkOrders.UpdateWorkOrder;

public sealed class UpdateWorkOrderCommandValidator : AbstractValidator<UpdateWorkOrderCommand>
{
    public UpdateWorkOrderCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty();

        RuleFor(command => command.Title)
            .NotEmpty()
            .MaximumLength(WorkOrder.TitleMaxLength);

        RuleFor(command => command.Description)
            .NotEmpty()
            .MaximumLength(WorkOrder.DescriptionMaxLength);

        RuleFor(command => command.EquipmentName)
            .NotEmpty()
            .MaximumLength(WorkOrder.EquipmentNameMaxLength);

        RuleFor(command => command.Priority).IsInEnum();
    }
}