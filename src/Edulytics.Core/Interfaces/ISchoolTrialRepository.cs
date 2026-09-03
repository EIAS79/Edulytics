namespace Edulytics.Core.Interfaces;

public sealed record SchoolTrialWindow(
    Guid SchoolId,
    DateTime StartsAtUtc,
    DateTime EndsAtUtc,
    DateTime? EndedAtUtc,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc)
{
    public bool IsUsableAt(DateTime utcNow) =>
        EndedAtUtc is null &&
        utcNow >= StartsAtUtc &&
        utcNow < EndsAtUtc;
}

public interface ISchoolTrialRepository
{
    Task<SchoolTrialWindow?> GetAsync(
        Guid schoolId,
        CancellationToken cancellationToken = default);

    Task<bool> CreateAsync(
        SchoolTrialWindow trial,
        CancellationToken cancellationToken = default);

    Task EndAsync(
        Guid schoolId,
        DateTime endedAtUtc,
        CancellationToken cancellationToken = default);
}
