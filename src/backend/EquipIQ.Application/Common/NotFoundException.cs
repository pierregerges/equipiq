namespace EquipIQ.Core.Common;

/// <summary>
/// Thrown when something the caller asked for does not exist. The API turns it into a 404.
/// Used in command handlers when the handler can’t find the entity to update or delete, and in query handlers when the handler can’t find the entity to return.
/// </summary>
public sealed class NotFoundException(string entityName, Guid id)
    : DomainException($"{entityName} with id '{id}' was not found.");