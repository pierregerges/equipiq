using EquipIQ.Core.WorkOrders;
using FluentValidation;

namespace EquipIQ.Application.WorkOrders.CreateWorkOrder;

public sealed class CreateWorkOrderCommandValidator : AbstractValidator<CreateWorkOrderCommand>
{
    public CreateWorkOrderCommandValidator()
    {
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

        RuleFor(command => command.CreatedBy)
            .NotEmpty()
            .MaximumLength(WorkOrder.CreatedByMaxLength);
    }
}