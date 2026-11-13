namespace EquipIQ.Core.Common;

/// <summary>
/// Thrown when a business rule is broken. The API will turn it into a 400 response.
/// </summary>
public class DomainException(string message) : Exception(message);