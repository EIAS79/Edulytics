using Edulytics.Core.Enums;
using Edulytics.Services.Imports;
using Edulytics.Web.Imports;

namespace Edulytics.Web.ViewModels.Imports;

public sealed record ImportIndexViewModel(
    ImportWorkspace Workspace);

public sealed record ImportDetailsViewModel(
    ImportBatchDetail Batch)
{
    public string TypeResourceKey =>
        $"Type{Batch.Type}";

    public string StatusResourceKey =>
        Batch.Status switch
        {
            ImportBatchStatus.Validated =>
                "StatusValidated",

            ImportBatchStatus.ValidationFailed =>
                "StatusValidationFailed",

            _ =>
                "StatusCompleted"
        };

    public bool IsLegacyReadOnly =>
        !MathOnlyImportAdapter.IsSupported(
            Batch.Type);

    public bool CanConfirm =>
        !IsLegacyReadOnly &&
        Batch.CanConfirm;

    public string RowVersionBase64 =>
        Convert.ToBase64String(
            Batch.RowVersion);
}
