using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.Json;
using Edulytics.Core.Constants;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Imports;
using Edulytics.Core.Interfaces;
using Edulytics.Core.Users;

namespace Edulytics.Services.Imports;

/// <summary>
/// Product-facing import coordinator. It preserves the hardened Phase 11 import
/// engine for students/classes/results while adding the account-creation flows
/// expected by the current Edulytics product: bulk Teacher creation + assignment
/// and bulk Subject Supervisor creation. Validation never creates accounts;
/// accounts and invitation tokens are created only during Confirm.
/// </summary>
public sealed class ProductDataImportService : IDataImportService
{
    private static readonly string[] SupervisorHeaders = ["Email"];

    private readonly DataImportService _inner;
    private readonly IImportRepository _imports;
    private readonly ISchoolUserRepository _users;
    private readonly ISchoolRepository _schools;
    private readonly IApplicationTransactionManager _transactions;
    private readonly ImportFileParser _parser;

    public ProductDataImportService(
        DataImportService inner,
        IImportRepository imports,
        ISchoolUserRepository users,
        ISchoolRepository schools,
        IApplicationTransactionManager transactions,
        ImportFileParser parser)
    {
        _inner = inner;
        _imports = imports;
        _users = users;
        _schools = schools;
        _transactions = transactions;
        _parser = parser;
    }

    public IReadOnlyList<string> GetTemplateHeaders(ImportType type) =>
        type == ImportType.SubjectSupervisors
            ? SupervisorHeaders
            : _inner.GetTemplateHeaders(type);

    public async Task<ImportResult<ImportWorkspace>> GetWorkspaceAsync(
        Guid actorUserId,
        CancellationToken cancellationToken = default)
    {
        var scope = await ResolveScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
            return ImportResult<ImportWorkspace>.Failure(scope.Error!.Value);

        var baseResult = await _inner.GetWorkspaceAsync(actorUserId, cancellationToken);
        if (!baseResult.Succeeded || baseResult.Value is null)
            return baseResult;

        if (scope.Role != RoleNames.SchoolAdmin)
            return baseResult;

        var options = new[]
        {
            new ImportTypeOption(ImportType.SubjectSupervisors, SupervisorHeaders)
        };

        return ImportResult<ImportWorkspace>.Success(
            new ImportWorkspace(options, baseResult.Value.Batches));
    }

    public async Task<ImportResult<ImportBatchDetail>> GetBatchAsync(
        Guid actorUserId,
        Guid batchId,
        CancellationToken cancellationToken = default)
    {
        var result = await _inner.GetBatchAsync(actorUserId, batchId, cancellationToken);
        if (!result.Succeeded || result.Value is null ||
            result.Value.Type != ImportType.SubjectSupervisors)
        {
            return result;
        }

        var scope = await ResolveScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
            return ImportResult<ImportBatchDetail>.Failure(scope.Error!.Value);

        return ImportResult<ImportBatchDetail>.Success(
            result.Value with
            {
                CanConfirm = result.Value.Status == ImportBatchStatus.Validated &&
                             scope.Role == RoleNames.SchoolAdmin
            });
    }

    public async Task<ImportResult<ImportBatchDetail>> UploadAsync(
        Guid actorUserId,
        ImportType importType,
        string fileName,
        byte[] bytes,
        CancellationToken cancellationToken = default)
    {
        if (importType is not (ImportType.Teachers or ImportType.SubjectSupervisors))
        {
            return await _inner.UploadAsync(
                actorUserId,
                importType,
                fileName,
                bytes,
                cancellationToken);
        }

        var scope = await ResolveScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
            return ImportResult<ImportBatchDetail>.Failure(scope.Error!.Value);

        if (!CanImportType(scope.Role!, importType))
            return ImportResult<ImportBatchDetail>.Failure(ImportErrorCode.AccessDenied);

        var parsed = _parser.Parse(fileName, bytes);
        if (!parsed.Succeeded || parsed.File is null)
            return ImportResult<ImportBatchDetail>.Failure(ParseError(parsed.Error));

        var hash = Convert.ToHexString(SHA256.HashData(bytes));
        var existing = await _imports.FindIdempotentAsync(
            scope.School!.Id,
            actorUserId,
            importType,
            hash,
            cancellationToken);

        if (existing is not null)
            return await GetBatchAsync(actorUserId, existing.Id, cancellationToken);

        var snapshot = await _imports.GetSnapshotAsync(scope.School.Id, cancellationToken);
        var schoolUsers = await _users.ListBySchoolAsync(scope.School.Id, cancellationToken);

        var issues = importType == ImportType.Teachers
            ? ValidateTeacherRows(parsed.File, snapshot, schoolUsers)
            : ValidateSupervisorRows(parsed.File, schoolUsers);

        var batch = new ImportBatch
        {
            Id = Guid.NewGuid(),
            SchoolId = scope.School.Id,
            ImportType = importType,
            Status = issues.Count == 0
                ? ImportBatchStatus.Validated
                : ImportBatchStatus.ValidationFailed,
            OriginalFileName = Path.GetFileName(fileName),
            FileHash = hash,
            RowsJson = JsonSerializer.Serialize(parsed.File),
            RowCount = parsed.File.Rows.Count,
            ValidRowCount = ValidRowCount(parsed.File.Rows.Count, issues),
            ErrorCount = issues.Count,
            UploadedByUserId = actorUserId,
            CreatedAtUtc = DateTime.UtcNow
        };

        var saved = await _imports.AddBatchAsync(
            batch,
            ToEntities(scope.School.Id, batch.Id, issues),
            cancellationToken);

        if (!saved.Succeeded)
            return ImportResult<ImportBatchDetail>.Failure(ImportErrorCode.Persistence);

        return await GetBatchAsync(actorUserId, batch.Id, cancellationToken);
    }

    public async Task<ImportResult<ImportBatchDetail>> ConfirmAsync(
        Guid actorUserId,
        Guid batchId,
        byte[] expectedRowVersion,
        CancellationToken cancellationToken = default)
    {
        var scope = await ResolveScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
            return ImportResult<ImportBatchDetail>.Failure(scope.Error!.Value);

        var batch = await _imports.GetAsync(scope.School!.Id, batchId, cancellationToken);
        if (batch is null)
            return ImportResult<ImportBatchDetail>.Failure(ImportErrorCode.BatchNotFound);

        if (batch.ImportType == ImportType.Teachers)
        {
            return await ConfirmTeachersAsync(
                scope,
                actorUserId,
                batch,
                expectedRowVersion,
                cancellationToken);
        }

        if (batch.ImportType == ImportType.SubjectSupervisors)
        {
            return await ConfirmSupervisorsAsync(
                scope,
                actorUserId,
                batch,
                expectedRowVersion,
                cancellationToken);
        }

        return await _inner.ConfirmAsync(
            actorUserId,
            batchId,
            expectedRowVersion,
            cancellationToken);
    }

    private async Task<ImportResult<ImportBatchDetail>> ConfirmTeachersAsync(
        ScopeResult scope,
        Guid actorUserId,
        ImportBatch batch,
        byte[] expectedRowVersion,
        CancellationToken cancellationToken)
    {
        if (!CanImportType(scope.Role!, ImportType.Teachers))
            return ImportResult<ImportBatchDetail>.Failure(ImportErrorCode.AccessDenied);

        if (batch.Status == ImportBatchStatus.Completed)
            return await GetBatchAsync(actorUserId, batch.Id, cancellationToken);

        if (batch.Status != ImportBatchStatus.Validated || batch.ErrorCount != 0)
            return ImportResult<ImportBatchDetail>.Failure(ImportErrorCode.BatchHasErrors);

        var parsed = Deserialize(batch);
        if (parsed is null)
            return ImportResult<ImportBatchDetail>.Failure(ImportErrorCode.Persistence);

        var snapshot = await _imports.GetSnapshotAsync(scope.School!.Id, cancellationToken);
        var schoolUsers = await _users.ListBySchoolAsync(scope.School.Id, cancellationToken);
        var issues = ValidateTeacherRows(parsed, snapshot, schoolUsers);
        if (issues.Count > 0)
            return ImportResult<ImportBatchDetail>.Failure(ImportErrorCode.BatchStateChanged);

        var invitations = new List<ImportInvitationCandidate>();
        await using var transaction = await _transactions.BeginAsync(cancellationToken);

        foreach (var email in parsed.Rows
                     .Select(x => Value(x, "Email").Trim())
                     .Distinct(StringComparer.OrdinalIgnoreCase))
        {
            var existing = schoolUsers.FirstOrDefault(x =>
                string.Equals(x.Email, email, StringComparison.OrdinalIgnoreCase));
            if (existing is not null)
                continue;

            var created = await _users.CreateAsync(
                scope.School.Id,
                email,
                RoleNames.Teacher,
                cancellationToken);

            if (!created.Succeeded || created.User is null ||
                string.IsNullOrWhiteSpace(created.PasswordSetupToken))
            {
                await transaction.RollbackAsync(cancellationToken);
                return ImportResult<ImportBatchDetail>.Failure(
                    created.Error == SchoolUserPersistenceError.DuplicateEmail
                        ? ImportErrorCode.ConcurrencyConflict
                        : ImportErrorCode.Persistence);
            }

            schoolUsers = schoolUsers.Append(created.User).ToArray();
            invitations.Add(new ImportInvitationCandidate(
                created.User.Id,
                created.User.Email,
                created.PasswordSetupToken,
                scope.School.Name));
        }

        var applied = await _inner.ConfirmAsync(
            actorUserId,
            batch.Id,
            expectedRowVersion,
            cancellationToken);

        if (!applied.Succeeded || applied.Value is null)
        {
            await transaction.RollbackAsync(cancellationToken);
            return applied;
        }

        await transaction.CommitAsync(cancellationToken);
        return ImportResult<ImportBatchDetail>.Success(
            applied.Value with { Invitations = invitations });
    }

    private async Task<ImportResult<ImportBatchDetail>> ConfirmSupervisorsAsync(
        ScopeResult scope,
        Guid actorUserId,
        ImportBatch batch,
        byte[] expectedRowVersion,
        CancellationToken cancellationToken)
    {
        if (!CanImportType(scope.Role!, ImportType.SubjectSupervisors))
            return ImportResult<ImportBatchDetail>.Failure(ImportErrorCode.AccessDenied);

        if (batch.Status == ImportBatchStatus.Completed)
            return await GetBatchAsync(actorUserId, batch.Id, cancellationToken);

        if (batch.Status != ImportBatchStatus.Validated || batch.ErrorCount != 0)
            return ImportResult<ImportBatchDetail>.Failure(ImportErrorCode.BatchHasErrors);

        var parsed = Deserialize(batch);
        if (parsed is null)
            return ImportResult<ImportBatchDetail>.Failure(ImportErrorCode.Persistence);

        var schoolUsers = await _users.ListBySchoolAsync(scope.School!.Id, cancellationToken);
        var issues = ValidateSupervisorRows(parsed, schoolUsers);
        if (issues.Count > 0)
            return ImportResult<ImportBatchDetail>.Failure(ImportErrorCode.BatchStateChanged);

        var invitations = new List<ImportInvitationCandidate>();
        await using var transaction = await _transactions.BeginAsync(cancellationToken);

        foreach (var row in parsed.Rows)
        {
            var email = Value(row, "Email").Trim();
            var created = await _users.CreateAsync(
                scope.School.Id,
                email,
                RoleNames.SubjectSupervisor,
                cancellationToken);

            if (!created.Succeeded || created.User is null ||
                string.IsNullOrWhiteSpace(created.PasswordSetupToken))
            {
                await transaction.RollbackAsync(cancellationToken);
                return ImportResult<ImportBatchDetail>.Failure(
                    created.Error == SchoolUserPersistenceError.DuplicateEmail
                        ? ImportErrorCode.ConcurrencyConflict
                        : ImportErrorCode.Persistence);
            }

            invitations.Add(new ImportInvitationCandidate(
                created.User.Id,
                created.User.Email,
                created.PasswordSetupToken,
                scope.School.Name));
        }

        var plan = new ImportApplyPlan();
        var applied = await _imports.ApplyAsync(
            scope.School.Id,
            batch.Id,
            actorUserId,
            expectedRowVersion,
            plan,
            DateTime.UtcNow,
            cancellationToken);

        if (!applied.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);
            return ImportResult<ImportBatchDetail>.Failure(MapPersistenceError(applied.Error));
        }

        await transaction.CommitAsync(cancellationToken);
        var detail = await GetBatchAsync(actorUserId, batch.Id, cancellationToken);
        if (!detail.Succeeded || detail.Value is null)
            return detail;

        return ImportResult<ImportBatchDetail>.Success(
            detail.Value with { Invitations = invitations });
    }

    private static List<ImportValidationIssue> ValidateTeacherRows(
        ParsedImportFile file,
        ImportDataSnapshot snapshot,
        IReadOnlyList<SchoolUserRecord> users)
    {
        var errors = new List<ImportValidationIssue>();
        var required = new[] { "Email", "AcademicYear", "ClassCode", "SubjectCode" };
        ValidateHeaders(file, required, errors);
        if (errors.Count > 0)
            return errors;

        var seenAssignments = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var row in file.Rows)
        {
            var email = Value(row, "Email").Trim();
            var yearName = Value(row, "AcademicYear").Trim();
            var classCode = NormalizeCode(Value(row, "ClassCode"));
            var subjectCode = NormalizeCode(Value(row, "SubjectCode"));

            Require(row, "Email", email, errors);
            Require(row, "AcademicYear", yearName, errors);
            Require(row, "ClassCode", classCode, errors);
            Require(row, "SubjectCode", subjectCode, errors);

            if (email.Length > 0 && !ValidEmail(email))
                Add(row, "Email", "InvalidType", email, errors);

            var existing = users.FirstOrDefault(x =>
                string.Equals(x.Email, email, StringComparison.OrdinalIgnoreCase));
            if (existing is not null &&
                (!existing.IsActive || existing.IsLocked ||
                 existing.Roles.Count != 1 || existing.Roles[0] != RoleNames.Teacher))
            {
                Add(row, "Email", "ExistingConflict", email, errors);
            }

            var year = snapshot.AcademicYears.FirstOrDefault(x =>
                x.Status == AcademicStructureStatus.Active &&
                string.Equals(x.Name, yearName, StringComparison.OrdinalIgnoreCase));
            if (yearName.Length > 0 && year is null)
                Add(row, "AcademicYear", "UnknownReference", yearName, errors);

            var classGroup = year is null ? null : snapshot.ClassGroups.FirstOrDefault(x =>
                x.AcademicYearId == year.Id &&
                x.Status == AcademicStructureStatus.Active &&
                x.NormalizedCode == classCode);
            if (year is not null && classCode.Length > 0 && classGroup is null)
                Add(row, "ClassCode", "UnknownReference", classCode, errors);

            var subject = snapshot.Subjects.FirstOrDefault(x =>
                x.Status == AcademicStructureStatus.Active &&
                x.NormalizedCode == subjectCode);
            if (subjectCode.Length > 0 && subject is null)
                Add(row, "SubjectCode", "UnknownReference", subjectCode, errors);

            if (year is not null && classGroup is not null && subject is not null)
            {
                var logical = $"{email}|{year.Id:N}|{classGroup.Id:N}|{subject.Id:N}";
                if (!seenAssignments.Add(logical))
                    Add(row, "Email", "DuplicateRow", email, errors);

                if (existing is not null && snapshot.TeacherAssignments.Any(x =>
                        x.TeacherUserId == existing.Id &&
                        x.ClassGroupId == classGroup.Id &&
                        x.SubjectId == subject.Id))
                {
                    Add(row, "Email", "ExistingConflict", email, errors);
                }
            }
        }

        return errors;
    }

    private static List<ImportValidationIssue> ValidateSupervisorRows(
        ParsedImportFile file,
        IReadOnlyList<SchoolUserRecord> users)
    {
        var errors = new List<ImportValidationIssue>();
        ValidateHeaders(file, SupervisorHeaders, errors);
        if (errors.Count > 0)
            return errors;

        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var existing = users.Select(x => x.Email).ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var row in file.Rows)
        {
            var email = Value(row, "Email").Trim();
            Require(row, "Email", email, errors);
            if (email.Length == 0)
                continue;
            if (!ValidEmail(email))
                Add(row, "Email", "InvalidType", email, errors);
            if (!seen.Add(email))
                Add(row, "Email", "DuplicateRow", email, errors);
            if (existing.Contains(email))
                Add(row, "Email", "ExistingConflict", email, errors);
        }

        return errors;
    }

    private static void ValidateHeaders(
        ParsedImportFile file,
        IEnumerable<string> required,
        List<ImportValidationIssue> errors)
    {
        var headers = file.Headers.ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var header in required)
        {
            if (!headers.Contains(header))
                errors.Add(new ImportValidationIssue(1, header, "MissingColumn", null));
        }
    }

    private static void Require(
        ImportFileRow row,
        string column,
        string value,
        List<ImportValidationIssue> errors)
    {
        if (string.IsNullOrWhiteSpace(value))
            Add(row, column, "Required", value, errors);
    }

    private static void Add(
        ImportFileRow row,
        string column,
        string code,
        string? raw,
        List<ImportValidationIssue> errors) =>
        errors.Add(new ImportValidationIssue(row.RowNumber, column, code, raw));

    private static bool ValidEmail(string value)
    {
        try
        {
            var address = new MailAddress(value);
            return string.Equals(address.Address, value, StringComparison.OrdinalIgnoreCase);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private static string Value(ImportFileRow row, string column) =>
        row.Values.FirstOrDefault(x =>
            string.Equals(x.Key, column, StringComparison.OrdinalIgnoreCase)).Value
        ?? string.Empty;

    private static string NormalizeCode(string value) => value.Trim().ToUpperInvariant();

    private static ParsedImportFile? Deserialize(ImportBatch batch)
    {
        try
        {
            return JsonSerializer.Deserialize<ParsedImportFile>(batch.RowsJson);
        }
        catch
        {
            return null;
        }
    }

    private static int ValidRowCount(
        int total,
        IReadOnlyList<ImportValidationIssue> issues)
    {
        var invalidRows = issues
            .Where(x => x.RowNumber > 1)
            .Select(x => x.RowNumber)
            .Distinct()
            .Count();
        return Math.Max(0, total - invalidRows);
    }

    private static ImportValidationError[] ToEntities(
        Guid schoolId,
        Guid batchId,
        IReadOnlyList<ImportValidationIssue> issues) =>
        issues.Select(x => new ImportValidationError
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            ImportBatchId = batchId,
            RowNumber = x.RowNumber,
            ColumnName = x.ColumnName,
            Code = x.Code,
            RawValue = x.RawValue is null
                ? null
                : x.RawValue.Replace("\0", string.Empty, StringComparison.Ordinal)[..Math.Min(500, x.RawValue.Replace("\0", string.Empty, StringComparison.Ordinal).Length)]
        }).ToArray();

    private static ImportErrorCode ParseError(ImportFileParseError? error) =>
        error switch
        {
            ImportFileParseError.UnsupportedFile => ImportErrorCode.UnsupportedFile,
            ImportFileParseError.TooLarge => ImportErrorCode.FileTooLarge,
            ImportFileParseError.TooManyRows => ImportErrorCode.TooManyRows,
            ImportFileParseError.TooManyColumns => ImportErrorCode.TooManyColumns,
            ImportFileParseError.DuplicateHeader => ImportErrorCode.DuplicateHeader,
            ImportFileParseError.EmptyFile => ImportErrorCode.EmptyFile,
            _ => ImportErrorCode.InvalidFile
        };

    private static ImportErrorCode MapPersistenceError(ImportPersistenceError error) =>
        error switch
        {
            ImportPersistenceError.Concurrency => ImportErrorCode.ConcurrencyConflict,
            ImportPersistenceError.InvalidState => ImportErrorCode.BatchStateChanged,
            ImportPersistenceError.NotFound => ImportErrorCode.BatchNotFound,
            ImportPersistenceError.SeatLimit => ImportErrorCode.SeatLimitReached,
            _ => ImportErrorCode.Persistence
        };

    private static bool CanImportType(string role, ImportType type) =>
        role switch
        {
            RoleNames.SchoolAdmin => type == ImportType.SubjectSupervisors,
            RoleNames.SubjectSupervisor =>
                type is ImportType.Students or ImportType.Teachers or ImportType.Classes,
            RoleNames.Teacher => type == ImportType.AssessmentResults,
            _ => false
        };

    private async Task<ScopeResult> ResolveScopeAsync(
        Guid actorUserId,
        CancellationToken cancellationToken)
    {
        var actor = await _users.GetActorAsync(actorUserId, cancellationToken);
        if (actor is null || !actor.IsActive || actor.IsLocked ||
            !actor.SchoolId.HasValue || actor.Roles.Count != 1)
        {
            return ScopeResult.Fail(ImportErrorCode.AccessDenied);
        }

        var role = actor.Roles[0];
        if (role != RoleNames.SchoolAdmin &&
            role != RoleNames.SubjectSupervisor &&
            role != RoleNames.Teacher)
        {
            return ScopeResult.Fail(ImportErrorCode.AccessDenied);
        }

        var school = await _schools.GetByIdAsync(actor.SchoolId.Value, cancellationToken);
        if (school is null)
            return ScopeResult.Fail(ImportErrorCode.AccessDenied);
        if (school.Status != SchoolStatus.Active)
            return ScopeResult.Fail(ImportErrorCode.SchoolNotActive);

        return ScopeResult.Ok(actor, school, role);
    }

    private sealed record ScopeResult(
        bool Succeeded,
        SchoolUserRecord? Actor,
        School? School,
        string? Role,
        ImportErrorCode? Error)
    {
        public static ScopeResult Ok(SchoolUserRecord actor, School school, string role) =>
            new(true, actor, school, role, null);

        public static ScopeResult Fail(ImportErrorCode error) =>
            new(false, null, null, null, error);
    }
}
