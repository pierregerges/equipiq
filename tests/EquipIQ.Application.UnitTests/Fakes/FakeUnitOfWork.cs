using EquipIQ.Core.Common;

namespace EquipIQ.Application.UnitTests.Fakes;

/// <summary>Counts how many times "save" was called.</summary>
public sealed class FakeUnitOfWork : IUnitOfWork
{
    public int SaveCount { get; private set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        SaveCount++;
        return Task.FromResult(1);
    }
} 