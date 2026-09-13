using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.Json;
using Edulytics.Core.Constants;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Core.Users;
using Edulytics.Services.Auditing;

namespace Edulytics.Services.Imports;

public sealed class DataImportService : IDataImportService
{
    private readonly IImportRepository _imports;
    private readonly ISchoolUserRepository _users;
    private readonly ISchoolRepository _schools;
    private readonly IApplicationTransactionManager _transactions;
    private readonly ImportFileParser _parser;
    private readonly ImportValidationEngine _validator;
    private readonly ImportPlanBuilder _planBuilder;
    private readonly IAuditService? _audit;

    public DataImportService(
        IImportRepository imports,
        ISchoolUserRepository users,
        ISchoolRepository schools,
        IApplicationTransactionManager transactions,
        ImportFileParser parser,
        ImportValidationEngine validator,
        ImportPlanBuilder planBuilder,
        IAuditService? audit = null)
    {
        _imports = imports;
        _users = users;
        _schools = schools;
        _transactions = transactions;
        _parser = parser;
        _validator = validator;
        _planBuilder = planBuilder;
        _audit = audit;
    }

    public static bool CanImportType(string role, ImportType type) =>
        role switch
        {
            RoleNames.SubjectSupervisor =>
                type is ImportType.Students or ImportType.Teachers or ImportType.Classes,
            RoleNames.Teacher =>
                type == ImportType.AssessmentResults,
            _ => false
        };

    public IReadOnlyList<string> GetTemplateHeaders(ImportType type) =>
        _validator.RequiredHeaders(type);

    public async Task<ImportResult<ImportWorkspace>> GetWorkspaceAsync(
        Guid actorUserId,
        CancellationToken cancellationToken = default)
    {
        var scope = await ResolveScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
            return ImportResult<ImportWorkspace>.Failure(scope.Error!.Value);

        var batches = await _imports.ListAsync(scope.School!.Id, cancellationToken);
        var visible = scope.Role == RoleNames.SchoolAdmin ||
                      scope.Role == RoleNames.SubjectSupervisor
            ? batches
            : batches
                .Where(x =>
                    x.UploadedByUserId == actorUserId &&
                    x.ImportType == ImportType.AssessmentResults)
                .ToArray();

        var options = Enum.GetValues<ImportType>()
            .Where(x => CanImportType(scope.Role!, x))
            .Select(x => new ImportTypeOption(x, _validator.RequiredHeaders(x)))
            .ToArray();

        return ImportResult<ImportWorkspace>.Success(
            new ImportWorkspace(
                options,
                visible.Select(MapListItem).ToArray()));
    }

    public async Task<ImportResult<ImportBatchDetail>> GetBatchAsync(
        Guid actorUserId,
        Guid batchId,
        CancellationToken cancellationToken = default)
    {
        var scope = await ResolveScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
            return ImportResult<ImportBatchDetail>.Failure(scope.Error!.Value);

        var batch = await _imports.GetAsync(scope.School!.Id, batchId, cancellationToken);
        if (batch is null)
            return ImportResult<ImportBatchDetail>.Failure(ImportErrorCode.BatchNotFound);

        if (!CanView(scope.Role!, actorUserId, batch))
            return ImportResult<ImportBatchDetail>.Failure(ImportErrorCode.AccessDenied);

        return await DetailAsync(scope.Role!, actorUserId, batch, cancellationToken);
    }

    public async Task<ImportResult<ImportBatchDetail>> UploadAsync(
        Guid actorUserId,
        ImportType importType,
        string fileName,
        byte[] bytes,
        CancellationToken cancellationToken = default)
    {
        var scope = await ResolveScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
            return ImportResult<ImportBatchDetail>.Failure(scope.Error!.Value);

        if (!CanImportType(scope.Role!, importType))
            return ImportResult<ImportBatchDetail>.Failure(ImportErrorCode.AccessDenied);

        var parsed = _parser.Parse(fileName, bytes);
        if (!parsed.Succeeded)
            return ImportResult<ImportBatchDetail>.Failure(ParseError(parsed.Error!.Value));

        var hash = Convert.ToHexString(SHA256.HashData(bytes));
        var existing = await _imports.FindIdempotentAsync(
            scope.School!.Id,
            actorUserId,
            importType,
            hash,
            cancellationToken);

        if (existing is not null)
            return await DetailAsync(scope.Role!, actorUserId, existing, cancellationToken);

        var snapshot = await _imports.GetSnapshotAsync(scope.School.Id, cancellationToken);
        var schoolUsers = await _users.ListBySchoolAsync(scope.School.Id, cancellationToken);

        var issues = _validator.Validate(
                importType,
                parsed.File!,
                snapshot,
                schoolUsers,
                actorUserId,
                scope.Role!)
            .ToList();

        if (importType == ImportType.Students)
        {
            issues.AddRange(
                ValidateStudentAccountColumns(parsed.File!, schoolUsers));
        }

        var batchId = Guid.NewGuid();
        var batch = new ImportBatch
        {
            Id = batchId,
            SchoolId = scope.School.Id,
            ImportType = importType,
            Status = issues.Count == 0
                ? ImportBatchStatus.Validated
                : ImportBatchStatus.ValidationFailed,
            OriginalFileName = Path.GetFileName(fileName),
            FileHash = hash,
            RowsJson = JsonSerializer.Serialize(parsed.File),
            RowCount = parsed.File!.Rows.Count,
            ValidRowCount = ValidRowCount(parsed.File.Rows.Count, issues),
            ErrorCount = issues.Count,
            UploadedByUserId = actorUserId,
            CreatedAtUtc = DateTime.UtcNow
        };

        var errors = ToEntities(scope.School.Id, batchId, issues);

        await QueueAuditAsync(
            scope,
            "ImportBatch.Uploaded",
            "ImportBatch",
            batch.Id,
            oldValues: null,
            newValues: new Dictionary<string, object?>
            {
                ["importType"] = batch.ImportType.ToString(),
                ["status"] = batch.Status.ToString(),
                ["rowCount"] = batch.RowCount,
                ["validRowCount"] = batch.ValidRowCount,
                ["errorCount"] = batch.ErrorCount
            },
            "Import batch uploaded and validated.",
            cancellationToken);

        var saved = await _imports.AddBatchAsync(batch, errors, cancellationToken);
        if (!saved.Succeeded)
        {
            var raced = await _imports.FindIdempotentAsync(
                scope.School.Id,
                actorUserId,
                importType,
                hash,
                cancellationToken);

            if (raced is not null)
                return await DetailAsync(scope.Role!, actorUserId, raced, cancellationToken);

            return ImportResult<ImportBatchDetail>.Failure(ImportErrorCode.Persistence);
        }

        var persisted = await _imports.GetAsync(scope.School.Id, batchId, cancellationToken);
        if (persisted is null)
            return ImportResult<ImportBatchDetail>.Failure(ImportErrorCode.Persistence);

        return await DetailAsync(scope.Role!, actorUserId, persisted, cancellationToken);
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

        if (!CanView(scope.Role!, actorUserId, batch) ||
            !CanImportType(scope.Role!, batch.ImportType))
        {
            return ImportResult<ImportBatchDetail>.Failure(ImportErrorCode.AccessDenied);
        }

        if (batch.Status == ImportBatchStatus.Completed)
            return await DetailAsync(scope.Role!, actorUserId, batch, cancellationToken);

        if (batch.Status != ImportBatchStatus.Validated || batch.ErrorCount != 0)
            return ImportResult<ImportBatchDetail>.Failure(ImportErrorCode.BatchHasErrors);

        ParsedImportFile? parsed;
        try
        {
            parsed = JsonSerializer.Deserialize<ParsedImportFile>(batch.RowsJson);
        }
        catch
        {
            parsed = null;
        }

        if (parsed is null)
            return ImportResult<ImportBatchDetail>.Failure(ImportErrorCode.Persistence);

        var snapshot = await _imports.GetSnapshotAsync(scope.School.Id, cancellationToken);
        var schoolUsers = await _users.ListBySchoolAsync(scope.School.Id, cancellationToken);

        var issues = _validator.Validate(
                batch.ImportType,
                parsed,
                snapshot,
                schoolUsers,
                actorUserId,
                scope.Role!)
            .ToList();

        if (batch.ImportType == ImportType.Students)
        {
            issues.AddRange(
                ValidateStudentAccountColumns(parsed, schoolUsers));
        }

        if (issues.Count > 0)
        {
            var errors = ToEntities(scope.School.Id, batch.Id, issues);

            await QueueAuditAsync(
                scope,
                "ImportBatch.ValidationFailed",
                "ImportBatch",
                batch.Id,
                oldValues: new Dictionary<string, object?>
                {
                    ["status"] = batch.Status.ToString(),
                    ["errorCount"] = batch.ErrorCount
                },
                newValues: new Dictionary<string, object?>
                {
                    ["status"] = ImportBatchStatus.ValidationFailed.ToString(),
                    ["validRowCount"] = ValidRowCount(parsed.Rows.Count, issues),
                    ["errorCount"] = issues.Count
                },
                "Import batch failed revalidation.",
                cancellationToken);

            var failed = await _imports.MarkValidationFailedAsync(
                scope.School.Id,
                batch.Id,
                expectedRowVersion,
                ValidRowCount(parsed.Rows.Count, issues),
                errors,
                cancellationToken);

            return ImportResult<ImportBatchDetail>.Failure(
                failed.Error == Edulytics.Core.Imports.ImportPersistenceError.Concurrency
                    ? ImportErrorCode.ConcurrencyConflict
                    : ImportErrorCode.BatchStateChanged);
        }

        var now = DateTime.UtcNow;
        var invitationCandidates = new List<ImportInvitationCandidate>();
        var usersForPlan = schoolUsers.ToList();

        await using var transaction = batch.ImportType == ImportType.Students
            ? await _transactions.BeginAsync(cancellationToken)
            : null;

        if (batch.ImportType == ImportType.Students)
        {
            foreach (var row in parsed.Rows)
            {
                var email = RowValue(row, "Email").Trim();
                var created = await _users.CreateAsync(
                    scope.School.Id,
                    email,
                    RoleNames.Student,
                    cancellationToken);

                if (!created.Succeeded ||
                    created.User is null ||
                    string.IsNullOrWhiteSpace(created.PasswordSetupToken))
                {
                    if (transaction is not null)
                        await transaction.RollbackAsync(cancellationToken);

                    return ImportResult<ImportBatchDetail>.Failure(
                        created.Error == SchoolUserPersistenceError.DuplicateEmail
                            ? ImportErrorCode.ConcurrencyConflict
                            : ImportErrorCode.Persistence);
                }

                usersForPlan.Add(created.User);
                invitationCandidates.Add(
                    new ImportInvitationCandidate(
                        created.User.Id,
                        created.User.Email,
                        created.PasswordSetupToken,
                        scope.School.Name));
            }
        }

        var plan = _planBuilder.Build(
            scope.School.Id,
            actorUserId,
            batch.Id,
            batch.ImportType,
            parsed,
            snapshot,
            usersForPlan,
            now);

        if (batch.ImportType == ImportType.Students)
        {
            for (var index = 0; index < plan.Students.Count; index++)
            {
                var email = RowValue(parsed.Rows[index], "Email").Trim();
                var user = usersForPlan.Single(x =>
                    x.Roles.Count == 1 &&
                    x.Roles[0] == RoleNames.Student &&
                    string.Equals(x.Email, email, StringComparison.OrdinalIgnoreCase));

                plan.Students[index].UserId = user.Id;
            }
        }

        await QueueAuditAsync(
            scope,
            "ImportBatch.Completed",
            "ImportBatch",
            batch.Id,
            oldValues: new Dictionary<string, object?>
            {
                ["status"] = batch.Status.ToString()
            },
            newValues: new Dictionary<string, object?>
            {
                ["status"] = ImportBatchStatus.Completed.ToString(),
                ["importType"] = batch.ImportType.ToString(),
                ["subjects"] = plan.Subjects.Count,
                ["classes"] = plan.Classes.Count,
                ["students"] = plan.Students.Count,
                ["enrollments"] = plan.Enrollments.Count,
                ["teacherAssignments"] = plan.TeacherAssignments.Count,
                ["assessmentResults"] = plan.AssessmentResults.Count,
                ["studentAnswers"] = plan.StudentAnswers.Count,
                ["curriculumMappings"] = plan.CurriculumMappings.Count
            },
            "Import batch applied successfully.",
            cancellationToken);

        var applied = await _imports.ApplyAsync(
            scope.School.Id,
            batch.Id,
            actorUserId,
            expectedRowVersion,
            plan,
            now,
            cancellationToken);

        if (!applied.Succeeded)
        {
            if (transaction is not null)
                await transaction.RollbackAsync(cancellationToken);

            return ImportResult<ImportBatchDetail>.Failure(
                applied.Error switch
                {
                    Edulytics.Core.Imports.ImportPersistenceError.Concurrency =>
                        ImportErrorCode.ConcurrencyConflict,
                    Edulytics.Core.Imports.ImportPersistenceError.InvalidState =>
                        ImportErrorCode.BatchStateChanged,
                    Edulytics.Core.Imports.ImportPersistenceError.NotFound =>
                        ImportErrorCode.BatchNotFound,
                    Edulytics.Core.Imports.ImportPersistenceError.SeatLimit =>
                        ImportErrorCode.SeatLimitReached,
                    _ => ImportErrorCode.Persistence
                });
        }

        if (transaction is not null)
            await transaction.CommitAsync(cancellationToken);

        var completed = await _imports.GetAsync(scope.School.Id, batch.Id, cancellationToken);
        if (completed is null)
            return ImportResult<ImportBatchDetail>.Failure(ImportErrorCode.Persistence);

        var detail = await DetailAsync(scope.Role!, actorUserId, completed, cancellationToken);
        if (!detail.Succeeded || detail.Value is null)
            return detail;

        return ImportResult<ImportBatchDetail>.Success(
            detail.Value with
            {
                Invitations = invitationCandidates
            });
    }

    private async Task QueueAuditAsync(
        ScopeResult scope,
        string action,
        string entityType,
        Guid entityId,
        IReadOnlyDictionary<string, object?>? oldValues,
        IReadOnlyDictionary<string, object?>? newValues,
        string resultSummary,
        CancellationToken cancellationToken)
    {
        if (_audit is null || scope.School is null || scope.Actor is null)
            return;

        await _audit.QueueAsync(
            new AuditEvent(
                SchoolId: scope.School.Id,
                Action: action,
                EntityType: entityType,
                EntityId: entityId.ToString("D"),
                Feature: "DataImport",
                OldValues: oldValues,
                NewValues: newValues,
                ResultSummary: resultSummary,
                ActorUserIdOverride: scope.Actor.Id,
                ActorRoleOverride: scope.Role ?? string.Empty),
            cancellationToken);
    }

    private async Task<ImportResult<ImportBatchDetail>> DetailAsync(
        string actorRole,
        Guid actorUserId,
        ImportBatch batch,
        CancellationToken cancellationToken)
    {
        ParsedImportFile? parsed;
        try
        {
            parsed = JsonSerializer.Deserialize<ParsedImportFile>(batch.RowsJson);
        }
        catch
        {
            parsed = null;
        }

        if (parsed is null)
            return ImportResult<ImportBatchDetail>.Failure(ImportErrorCode.Persistence);

        var errors = await _imports.GetErrorsAsync(
            batch.SchoolId,
            batch.Id,
            cancellationToken);

        return ImportResult<ImportBatchDetail>.Success(
            new ImportBatchDetail(
                batch.Id,
                batch.ImportType,
                batch.Status,
                batch.OriginalFileName,
                batch.RowCount,
                batch.ValidRowCount,
                batch.ErrorCount,
                batch.CreatedAtUtc,
                batch.CompletedAtUtc,
                batch.RowVersion,
                parsed.Headers,
                parsed.Rows
                    .Take(100)
                    .Select(x => new ImportPreviewRow(x.RowNumber, x.Values))
                    .ToArray(),
                errors
                    .Select(x => new ImportValidationErrorItem(
                        x.RowNumber,
                        x.ColumnName,
                        x.Code,
                        x.RawValue))
                    .ToArray(),
                batch.Status == ImportBatchStatus.Validated &&
                CanImportType(actorRole, batch.ImportType) &&
                (actorRole == RoleNames.SubjectSupervisor ||
                 batch.UploadedByUserId == actorUserId)));
    }

    private async Task<ScopeResult> ResolveScopeAsync(
        Guid actorUserId,
        CancellationToken cancellationToken)
    {
        var actor = await _users.GetActorAsync(actorUserId, cancellationToken);

        if (actor is null ||
            !actor.IsActive ||
            actor.IsLocked ||
            !actor.SchoolId.HasValue ||
            actor.Roles.Count != 1)
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

    private static bool CanView(
        string role,
        Guid actorUserId,
        ImportBatch batch) =>
        role == RoleNames.SchoolAdmin ||
        role == RoleNames.SubjectSupervisor ||
        (role == RoleNames.Teacher &&
         batch.UploadedByUserId == actorUserId &&
         batch.ImportType == ImportType.AssessmentResults);

    private static IReadOnlyList<ImportValidationIssue> ValidateStudentAccountColumns(
        ParsedImportFile file,
        IReadOnlyList<SchoolUserRecord> schoolUsers)
    {
        var errors = new List<ImportValidationIssue>();
        if (!file.Headers.Any(x =>
                string.Equals(x, "Email", StringComparison.OrdinalIgnoreCase)))
        {
            errors.Add(new ImportValidationIssue(1, "Email", "MissingColumn", null));
            return errors;
        }

        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var existing = schoolUsers
            .Select(x => x.Email)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var row in file.Rows)
        {
            var email = RowValue(row, "Email").Trim();
            if (email.Length == 0)
            {
                errors.Add(new ImportValidationIssue(
                    row.RowNumber,
                    "Email",
                    "Required",
                    email));
                continue;
            }

            if (!IsValidEmail(email))
            {
                errors.Add(new ImportValidationIssue(
                    row.RowNumber,
                    "Email",
                    "InvalidType",
                    email));
                continue;
            }

            if (!seen.Add(email))
            {
                errors.Add(new ImportValidationIssue(
                    row.RowNumber,
                    "Email",
                    "DuplicateRow",
                    email));
            }

            if (existing.Contains(email))
            {
                errors.Add(new ImportValidationIssue(
                    row.RowNumber,
                    "Email",
                    "ExistingConflict",
                    email));
            }
        }

        return errors;
    }

    private static bool IsValidEmail(string value)
    {
        try
        {
            var address = new MailAddress(value);
            return string.Equals(
                address.Address,
                value,
                StringComparison.OrdinalIgnoreCase);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private static string RowValue(ImportFileRow row, string column) =>
        row.Values.FirstOrDefault(x =>
            string.Equals(x.Key, column, StringComparison.OrdinalIgnoreCase)).Value
        ?? string.Empty;

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
                RawValue = SanitizeRaw(x.RawValue)
            })
            .ToArray();

    private static string? SanitizeRaw(string? value)
    {
        if (value is null)
            return null;

        var clean = value.Replace("\0", string.Empty, StringComparison.Ordinal);
        return clean.Length <= 500 ? clean : clean[..500];
    }

    private static ImportBatchListItem MapListItem(ImportBatch batch) =>
        new(
            batch.Id,
            batch.ImportType,
            batch.Status,
            batch.OriginalFileName,
            batch.RowCount,
            batch.ErrorCount,
            batch.CreatedAtUtc,
            batch.CompletedAtUtc);

    private static ImportErrorCode ParseError(ImportFileParseError error) =>
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

    private sealed record ScopeResult(
        bool Succeeded,
        SchoolUserRecord? Actor,
        School? School,
        string? Role,
        ImportErrorCode? Error)
    {
        public static ScopeResult Ok(
            SchoolUserRecord actor,
            School school,
            string role) =>
            new(true, actor, school, role, null);

        public static ScopeResult Fail(ImportErrorCode error) =>
            new(false, null, null, null, error);
    }
}
