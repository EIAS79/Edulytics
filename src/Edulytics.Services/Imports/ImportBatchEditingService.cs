using System.Text.Json;
using Edulytics.Core.Constants;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Core.Users;

namespace Edulytics.Services.Imports;

public sealed record ImportInvitationOutcome(string Email, bool Sent);

public sealed record ImportBatchEditResult(bool Succeeded, ImportErrorCode? Error)
{
    public static ImportBatchEditResult Success() => new(true, null);
    public static ImportBatchEditResult Failure(ImportErrorCode error) => new(false, error);
}

public sealed class ImportBatchEditingService(
    IImportRepository imports,
    IImportBatchEditingRepository editing,
    ISchoolUserRepository users)
{
    public async Task<ImportBatchEditResult> RemoveRowsAsync(
        Guid actorUserId,
        Guid batchId,
        byte[] expectedRowVersion,
        IReadOnlyCollection<int> rowNumbers,
        CancellationToken cancellationToken = default)
    {
        if (rowNumbers.Count == 0)
            return ImportBatchEditResult.Failure(ImportErrorCode.InvalidFile);

        var actor = await users.GetActorAsync(actorUserId, cancellationToken);
        if (actor?.SchoolId is not Guid schoolId || actor.Roles.Count != 1)
            return ImportBatchEditResult.Failure(ImportErrorCode.AccessDenied);

        var batch = await imports.GetAsync(schoolId, batchId, cancellationToken);
        if (batch is null)
            return ImportBatchEditResult.Failure(ImportErrorCode.BatchNotFound);
        if (!CanEdit(actor.Roles[0], actorUserId, batch))
            return ImportBatchEditResult.Failure(ImportErrorCode.AccessDenied);
        if (batch.Status == ImportBatchStatus.Completed)
            return ImportBatchEditResult.Failure(ImportErrorCode.BatchStateChanged);

        ParsedImportFile? parsed;
        try { parsed = JsonSerializer.Deserialize<ParsedImportFile>(batch.RowsJson); }
        catch { parsed = null; }
        if (parsed is null)
            return ImportBatchEditResult.Failure(ImportErrorCode.Persistence);

        var remove = rowNumbers.Where(x => x > 1).ToHashSet();
        var remainingRows = parsed.Rows.Where(x => !remove.Contains(x.RowNumber)).ToArray();
        if (remainingRows.Length == 0)
            return ImportBatchEditResult.Failure(ImportErrorCode.EmptyFile);

        var currentErrors = await imports.GetErrorsAsync(schoolId, batchId, cancellationToken);
        var remainingErrors = currentErrors
            .Where(x => x.RowNumber <= 1 || !remove.Contains(x.RowNumber))
            .Where(x => !string.Equals(x.Code, "DuplicateRow", StringComparison.Ordinal) ||
                        IsStillDuplicate(batch.ImportType, x, remainingRows))
            .Select(x => new ImportValidationError
            {
                Id = Guid.NewGuid(),
                SchoolId = schoolId,
                ImportBatchId = batchId,
                RowNumber = x.RowNumber,
                ColumnName = x.ColumnName,
                Code = x.Code,
                RawValue = x.RawValue
            })
            .ToArray();

        var invalidRows = remainingErrors.Where(x => x.RowNumber > 1)
            .Select(x => x.RowNumber).Distinct().Count();
        var updated = new ParsedImportFile(parsed.Headers, remainingRows);

        var result = await editing.UpdateStagedRowsAsync(
            schoolId,
            batchId,
            expectedRowVersion,
            JsonSerializer.Serialize(updated),
            remainingRows.Length,
            Math.Max(0, remainingRows.Length - invalidRows),
            remainingErrors,
            cancellationToken);

        return result.Succeeded
            ? ImportBatchEditResult.Success()
            : ImportBatchEditResult.Failure(MapPersistenceError(result.Error));
    }

    public async Task<ImportBatchEditResult> DiscardBatchAsync(
        Guid actorUserId,
        Guid batchId,
        CancellationToken cancellationToken = default)
    {
        var actor = await users.GetActorAsync(actorUserId, cancellationToken);
        if (actor?.SchoolId is not Guid schoolId || actor.Roles.Count != 1)
            return ImportBatchEditResult.Failure(ImportErrorCode.AccessDenied);

        var batch = await imports.GetAsync(schoolId, batchId, cancellationToken);
        if (batch is null)
            return ImportBatchEditResult.Failure(ImportErrorCode.BatchNotFound);
        if (!CanEdit(actor.Roles[0], actorUserId, batch))
            return ImportBatchEditResult.Failure(ImportErrorCode.AccessDenied);
        if (batch.Status == ImportBatchStatus.Completed)
            return ImportBatchEditResult.Failure(ImportErrorCode.BatchStateChanged);

        var result = await editing.DeleteStagedBatchAsync(
            schoolId,
            batchId,
            cancellationToken);

        return result.Succeeded
            ? ImportBatchEditResult.Success()
            : ImportBatchEditResult.Failure(MapPersistenceError(result.Error));
    }

    public Task<ImportBatchEditResult> RecordInvitationOutcomesAsync(
        Guid actorUserId,
        Guid batchId,
        IReadOnlyList<ImportInvitationOutcome> outcomes,
        CancellationToken cancellationToken = default) =>
        RecordInvitationOutcomesCoreAsync(
            actorUserId,
            batchId,
            outcomes,
            initializeMissing: false,
            cancellationToken);

    public Task<ImportBatchEditResult> RecordInitialInvitationOutcomesAsync(
        Guid actorUserId,
        Guid batchId,
        IReadOnlyList<ImportInvitationOutcome> outcomes,
        CancellationToken cancellationToken = default) =>
        RecordInvitationOutcomesCoreAsync(
            actorUserId,
            batchId,
            outcomes,
            initializeMissing: true,
            cancellationToken);

    private async Task<ImportBatchEditResult> RecordInvitationOutcomesCoreAsync(
        Guid actorUserId,
        Guid batchId,
        IReadOnlyList<ImportInvitationOutcome> outcomes,
        bool initializeMissing,
        CancellationToken cancellationToken)
    {
        var actor = await users.GetActorAsync(actorUserId, cancellationToken);
        if (actor?.SchoolId is not Guid schoolId)
            return ImportBatchEditResult.Failure(ImportErrorCode.AccessDenied);

        var batch = await imports.GetAsync(schoolId, batchId, cancellationToken);
        if (batch is null)
            return ImportBatchEditResult.Failure(ImportErrorCode.BatchNotFound);
        if (batch.Status != ImportBatchStatus.Completed)
            return ImportBatchEditResult.Failure(ImportErrorCode.BatchStateChanged);

        ParsedImportFile? parsed;
        try { parsed = JsonSerializer.Deserialize<ParsedImportFile>(batch.RowsJson); }
        catch { parsed = null; }
        if (parsed is null)
            return ImportBatchEditResult.Failure(ImportErrorCode.Persistence);

        var map = outcomes
            .GroupBy(x => x.Email, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(x => x.Key, x => x.Last().Sent, StringComparer.OrdinalIgnoreCase);

        foreach (var row in parsed.Rows)
        {
            row.Values["__ImportStatus"] = "Imported";
            var email = Value(row, "Email");
            if (email.Length == 0)
                continue;

            if (map.TryGetValue(email, out var sent))
            {
                row.Values["__InvitationStatus"] = sent ? "Sent" : "Failed";
            }
            else if (initializeMissing && !row.Values.ContainsKey("__InvitationStatus"))
            {
                row.Values["__InvitationStatus"] = "NotRequired";
            }
        }

        var result = await editing.UpdateCompletedRowsJsonAsync(
            schoolId,
            batchId,
            JsonSerializer.Serialize(parsed),
            cancellationToken);

        return result.Succeeded
            ? ImportBatchEditResult.Success()
            : ImportBatchEditResult.Failure(MapPersistenceError(result.Error));
    }

    private static bool IsStillDuplicate(
        ImportType type,
        ImportValidationError error,
        IReadOnlyList<ImportFileRow> rows)
    {
        var target = rows.FirstOrDefault(x => x.RowNumber == error.RowNumber);
        if (target is null)
            return false;

        var key = DuplicateKey(type, error.ColumnName, target);
        if (key is null)
            return true;

        return rows.Count(row => string.Equals(
            DuplicateKey(type, error.ColumnName, row),
            key,
            StringComparison.OrdinalIgnoreCase)) > 1;
    }

    private static string? DuplicateKey(
        ImportType type,
        string columnName,
        ImportFileRow row)
    {
        if (type == ImportType.Students &&
            (columnName.Equals("StudentNumber", StringComparison.OrdinalIgnoreCase) ||
             columnName.Equals("Email", StringComparison.OrdinalIgnoreCase)))
        {
            return Value(row, columnName).ToUpperInvariant();
        }

        if (type == ImportType.SubjectSupervisors &&
            columnName.Equals("Email", StringComparison.OrdinalIgnoreCase))
        {
            return Value(row, "Email").ToUpperInvariant();
        }

        if (type == ImportType.Classes &&
            columnName.Equals("Code", StringComparison.OrdinalIgnoreCase))
        {
            return $"{Value(row, "AcademicYear").ToUpperInvariant()}|{Value(row, "Code").ToUpperInvariant()}";
        }

        if (type == ImportType.Teachers &&
            columnName.Equals("Email", StringComparison.OrdinalIgnoreCase))
        {
            return string.Join('|',
                Value(row, "Email").ToUpperInvariant(),
                Value(row, "AcademicYear").ToUpperInvariant(),
                Value(row, "ClassCode").ToUpperInvariant(),
                Value(row, "SubjectCode").ToUpperInvariant());
        }

        return null;
    }

    private static ImportErrorCode MapPersistenceError(Core.Imports.ImportPersistenceError? error) =>
        error switch
        {
            Core.Imports.ImportPersistenceError.Concurrency => ImportErrorCode.ConcurrencyConflict,
            Core.Imports.ImportPersistenceError.InvalidState => ImportErrorCode.BatchStateChanged,
            Core.Imports.ImportPersistenceError.NotFound => ImportErrorCode.BatchNotFound,
            _ => ImportErrorCode.Persistence
        };

    private static string Value(ImportFileRow row, string key) =>
        row.Values.FirstOrDefault(x => string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase)).Value?.Trim()
        ?? string.Empty;

    private static bool CanEdit(string role, Guid actorUserId, ImportBatch batch) =>
        role switch
        {
            RoleNames.SchoolAdmin => batch.ImportType == ImportType.SubjectSupervisors,
            RoleNames.SubjectSupervisor => batch.ImportType is ImportType.Students or ImportType.Teachers or ImportType.Classes,
            RoleNames.Teacher => batch.UploadedByUserId == actorUserId && batch.ImportType == ImportType.AssessmentResults,
            _ => false
        };
}
