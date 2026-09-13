using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Imports;
using Edulytics.Core.Interfaces;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Data.Repositories;

public sealed class ImportBatchEditingRepository(EdulyticsDbContext db) : IImportBatchEditingRepository
{
    public async Task<ImportPersistenceResult> UpdateStagedRowsAsync(
        Guid schoolId,
        Guid batchId,
        byte[] expectedRowVersion,
        string rowsJson,
        int rowCount,
        int validRowCount,
        IReadOnlyList<ImportValidationError> errors,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var batch = await db.ImportBatches.FirstOrDefaultAsync(
                x => x.SchoolId == schoolId && x.Id == batchId,
                cancellationToken);

            if (batch is null)
                return ImportPersistenceResult.Failure(ImportPersistenceError.NotFound);

            if (batch.Status == ImportBatchStatus.Completed)
                return ImportPersistenceResult.Failure(ImportPersistenceError.InvalidState);

            db.Entry(batch).Property(x => x.RowVersion).OriginalValue = expectedRowVersion;

            var oldErrors = await db.ImportValidationErrors
                .Where(x => x.SchoolId == schoolId && x.ImportBatchId == batchId)
                .ToArrayAsync(cancellationToken);

            if (oldErrors.Length > 0)
                db.ImportValidationErrors.RemoveRange(oldErrors);
            if (errors.Count > 0)
                db.ImportValidationErrors.AddRange(errors);

            batch.RowsJson = rowsJson;
            batch.RowCount = rowCount;
            batch.ValidRowCount = validRowCount;
            batch.ErrorCount = errors.Count;
            batch.Status = errors.Count == 0
                ? ImportBatchStatus.Validated
                : ImportBatchStatus.ValidationFailed;

            await db.SaveChangesAsync(cancellationToken);
            return ImportPersistenceResult.Success();
        }
        catch (DbUpdateConcurrencyException)
        {
            db.ChangeTracker.Clear();
            return ImportPersistenceResult.Failure(ImportPersistenceError.Concurrency);
        }
        catch (DbUpdateException)
        {
            db.ChangeTracker.Clear();
            return ImportPersistenceResult.Failure(ImportPersistenceError.Constraint);
        }
    }

    public async Task<ImportPersistenceResult> UpdateCompletedRowsJsonAsync(
        Guid schoolId,
        Guid batchId,
        string rowsJson,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var batch = await db.ImportBatches.FirstOrDefaultAsync(
                x => x.SchoolId == schoolId && x.Id == batchId,
                cancellationToken);

            if (batch is null)
                return ImportPersistenceResult.Failure(ImportPersistenceError.NotFound);
            if (batch.Status != ImportBatchStatus.Completed)
                return ImportPersistenceResult.Failure(ImportPersistenceError.InvalidState);

            batch.RowsJson = rowsJson;
            await db.SaveChangesAsync(cancellationToken);
            return ImportPersistenceResult.Success();
        }
        catch (DbUpdateConcurrencyException)
        {
            db.ChangeTracker.Clear();
            return ImportPersistenceResult.Failure(ImportPersistenceError.Concurrency);
        }
        catch (DbUpdateException)
        {
            db.ChangeTracker.Clear();
            return ImportPersistenceResult.Failure(ImportPersistenceError.Constraint);
        }
    }
}
