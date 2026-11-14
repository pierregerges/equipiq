namespace EquipIQ.Core.Common;

/// <summary>
/// "Save everything that changed, as one all-or-nothing step."
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}