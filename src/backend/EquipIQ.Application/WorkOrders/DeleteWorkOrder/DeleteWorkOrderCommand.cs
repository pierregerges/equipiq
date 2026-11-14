using Mediator;

namespace EquipIQ.Application.WorkOrders.DeleteWorkOrder;

/// <summary>A command with no result: it either works or throws.</summary>
public sealed record DeleteWorkOrderCommand(Guid Id) : ICommand;