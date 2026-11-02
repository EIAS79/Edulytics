namespace Edulytics.Core.Users;

public sealed record SchoolUserAcademicContext(
    Guid AcademicYearId,
    string AcademicYearName,
    Guid AcademicProgramId,
    string AcademicProgramName,
    Guid ClassGroupId,
    string ClassGroupName);

public sealed record SchoolUserRecord(
    Guid Id,
    Guid? SchoolId,
    string Email,
    bool IsActive,
    bool IsLocked,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc,
    IReadOnlyList<string> Roles,
    string? DisplayName = null,
    string? StudentNumber = null,
    IReadOnlyList<SchoolUserAcademicContext>? AcademicContexts = null);

public enum SchoolUserPersistenceError
{
    None = 0,
    DuplicateEmail = 1,
    NotFound = 2,
    RoleFailure = 3,
    IdentityFailure = 4,
    InvalidToken = 5,
    PasswordPolicy = 6,
    Conflict = 7
}

public sealed record SchoolUserPersistenceResult(
    bool Succeeded,
    SchoolUserPersistenceError Error,
    SchoolUserRecord? User = null,
    string? PasswordSetupToken = null)
{
    public static SchoolUserPersistenceResult Success(
        SchoolUserRecord user,
        string? token = null) =>
        new(
            true,
            SchoolUserPersistenceError.None,
            user,
            token);

    public static SchoolUserPersistenceResult Failure(
        SchoolUserPersistenceError error) =>
        new(
            false,
            error);
}


public sealed record SchoolUserListQuery(
    string? Search = null,
    string? Role = null,
    bool? IsActive = null,
    bool? IsLocked = null,
    int Page = 1,
    int PageSize = 50,
    string? Name = null,
    string? UserId = null,
    string? Email = null,
    Guid? AcademicYearId = null,
    Guid? AcademicProgramId = null,
    Guid? ClassGroupId = null);

public sealed record SchoolUserDirectoryFilterOption(
    Guid Id,
    string Label);

public sealed record SchoolUserClassFilterOption(
    Guid Id,
    string Label,
    Guid AcademicYearId,
    Guid AcademicProgramId);

public sealed record SchoolUserDirectoryFilterOptions(
    IReadOnlyList<SchoolUserDirectoryFilterOption> AcademicYears,
    IReadOnlyList<SchoolUserDirectoryFilterOption> AcademicPrograms,
    IReadOnlyList<SchoolUserClassFilterOption> Classes)
{
    public static SchoolUserDirectoryFilterOptions Empty { get; } =
        new([], [], []);
}

public sealed record SchoolUserPage(
    IReadOnlyList<SchoolUserRecord> Users,
    int Page,
    int PageSize,
    int TotalCount)
{
    public int TotalPages =>
        TotalCount == 0
            ? 1
            : (int)Math.Ceiling(TotalCount / (double)PageSize);
}
