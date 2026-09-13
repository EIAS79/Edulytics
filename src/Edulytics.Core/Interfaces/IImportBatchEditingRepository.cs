using Edulytics.Core.Entities;
using Edulytics.Core.Imports;

namespace Edulytics.Core.Interfaces;

public interface IImportBatchEditingRepository
{
    Task<ImportPersistenceResult> UpdateStagedRowsAsync(
        Guid schoolId,
        Guid batchId,
        byte[] expectedRowVersion,
        string rowsJson,
        int rowCount,
        int validRowCount,
        IReadOnlyList<ImportValidationError> errors,
        CancellationToken cancellationToken = default);

    Task<ImportPersistenceResult> UpdateCompletedRowsJsonAsync(
        Guid schoolId,
        Guid batchId,
        string rowsJson,
        CancellationToken cancellationToken = default);
}
