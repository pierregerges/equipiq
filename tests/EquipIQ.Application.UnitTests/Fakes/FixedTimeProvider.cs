namespace EquipIQ.Application.UnitTests.Fakes;

/// <summary>A clock that always says the same time, so tests give the same result every run.</summary>
public sealed class FixedTimeProvider(DateTimeOffset now) : TimeProvider
{
    public override DateTimeOffset GetUtcNow() => now;
}