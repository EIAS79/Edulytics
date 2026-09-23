using Edulytics.Core.Users;

namespace Edulytics.Core.Interfaces;

public interface ISchoolUserRepository
{
    Task<SchoolUserRecord?> GetActorAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SchoolUserRecord>> ListBySchoolAsync(
        Guid schoolId,
        CancellationToken cancellationToken = default);

    async Task<SchoolUserPage> QueryBySchoolAsync(
        Guid schoolId,
        SchoolUserListQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var page = Math.Max(1, query.Page);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);
        var search = query.Search?.Trim();
        var role = query.Role?.Trim();

        IEnumerable<SchoolUserRecord> filtered =
            await ListBySchoolAsync(schoolId, cancellationToken);

        if (!string.IsNullOrWhiteSpace(search))
        {
            filtered = filtered.Where(x =>
                x.Email.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(role))
        {
            filtered = filtered.Where(x =>
                x.Roles.Contains(role, StringComparer.Ordinal));
        }

        if (query.IsActive.HasValue)
            filtered = filtered.Where(x => x.IsActive == query.IsActive.Value);

        if (query.IsLocked.HasValue)
            filtered = filtered.Where(x => x.IsLocked == query.IsLocked.Value);

        var ordered = filtered
            .OrderBy(x => x.Email, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var total = ordered.Length;

        return new SchoolUserPage(
            ordered.Skip((page - 1) * pageSize).Take(pageSize).ToArray(),
            page,
            pageSize,
            total);
    }

    Task<SchoolUserRecord?> GetBySchoolAndIdAsync(
        Guid schoolId,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<SchoolUserPersistenceResult> CreateAsync(
        Guid schoolId,
        string email,
        string role,
        CancellationToken cancellationToken = default);

    Task<SchoolUserPersistenceResult> SetActiveAsync(
        Guid schoolId,
        Guid userId,
        bool isActive,
        CancellationToken cancellationToken = default);

    Task<SchoolUserPersistenceResult> SetLockedAsync(
        Guid schoolId,
        Guid userId,
        bool isLocked,
        CancellationToken cancellationToken = default);

    Task<SchoolUserPersistenceResult> SetRoleAsync(
        Guid schoolId,
        Guid userId,
        string role,
        CancellationToken cancellationToken = default);

    Task<SchoolUserPersistenceResult> GeneratePasswordSetupAsync(
        Guid schoolId,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<SchoolUserPersistenceResult> CompletePasswordSetupAsync(
        Guid userId,
        string token,
        string newPassword,
        CancellationToken cancellationToken = default);
}
