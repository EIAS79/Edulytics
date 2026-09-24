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

    Task<SchoolUserDirectoryFilterOptions> GetDirectoryFilterOptionsAsync(
        Guid schoolId,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(SchoolUserDirectoryFilterOptions.Empty);

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
        var name = query.Name?.Trim();
        var userId = query.UserId?.Trim();
        var email = query.Email?.Trim();

        IEnumerable<SchoolUserRecord> filtered =
            await ListBySchoolAsync(schoolId, cancellationToken);

        if (!string.IsNullOrWhiteSpace(search))
        {
            filtered = filtered.Where(x =>
                x.Email.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            filtered = filtered.Where(x =>
                x.Email.Contains(email, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            filtered = filtered.Where(x =>
                !string.IsNullOrWhiteSpace(x.DisplayName) &&
                x.DisplayName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(userId))
        {
            filtered = Guid.TryParse(userId, out var parsedUserId)
                ? filtered.Where(x => x.Id == parsedUserId)
                : [];
        }

        if (query.AcademicYearId.HasValue ||
            query.AcademicProgramId.HasValue ||
            query.ClassGroupId.HasValue)
        {
            filtered = filtered.Where(x =>
                (x.AcademicContexts ?? [])
                    .Any(context =>
                        (!query.AcademicYearId.HasValue ||
                         context.AcademicYearId == query.AcademicYearId.Value) &&
                        (!query.AcademicProgramId.HasValue ||
                         context.AcademicProgramId == query.AcademicProgramId.Value) &&
                        (!query.ClassGroupId.HasValue ||
                         context.ClassGroupId == query.ClassGroupId.Value)));
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
        var totalPages = Math.Max(
            1,
            (int)Math.Ceiling(total / (double)pageSize));
        page = Math.Min(page, totalPages);

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

    async Task<IReadOnlyList<SchoolUserRecord>> ListBySchoolAndIdsAsync(
        Guid schoolId,
        IReadOnlyCollection<Guid> userIds,
        CancellationToken cancellationToken = default)
    {
        if (userIds.Count == 0)
            return [];

        var ids = userIds.ToHashSet();
        var users = await ListBySchoolAsync(schoolId, cancellationToken);
        return users.Where(x => ids.Contains(x.Id)).ToArray();
    }

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
