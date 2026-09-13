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
            : ImportBatchEditResult.Failure(result.Error == Core.Imports.ImportPersistenceError.Concurrency
                ? ImportErrorCode.ConcurrencyConflict
                : ImportErrorCode.Persistence);
    }

    public async Task<ImportBatchEditResult> RecordInvitationOutcomesAsync(
        Guid actorUserId,
        Guid batchId,
        IReadOnlyList<ImportInvitationOutcome> outcomes,
        CancellationToken cancellationToken = default)
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
            row.Values["__InvitationStatus"] = map.TryGetValue(email, out var sent)
                ? sent ? "Sent" : "Failed"
                : "NotRequired";
        }

        var result = await editing.UpdateCompletedRowsJsonAsync(
            schoolId,
            batchId,
            JsonSerializer.Serialize(parsed),
            cancellationToken);

        return result.Succeeded
            ? ImportBatchEditResult.Success()
            : ImportBatchEditResult.Failure(ImportErrorCode.Persistence);
    }

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
