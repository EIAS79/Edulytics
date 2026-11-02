using Edulytics.Core.Enums;
using Edulytics.Services.Imports;
using Edulytics.Web.Imports;

namespace Edulytics.Web.ViewModels.Imports;

public sealed record ImportAcademicYearOption(
    Guid Id,
    string Name);

public sealed record ImportAssessmentResultOption(
    Guid AssessmentId,
    Guid AcademicYearId,
    string Title,
    DateOnly AssessmentDate,
    Guid ClassGroupId,
    string ClassName);

public sealed record ImportIndexViewModel(
    ImportWorkspace Workspace,
    IReadOnlyList<ImportAcademicYearOption> AcademicYears,
    IReadOnlyList<ImportAcademicYearOption> AssessmentAcademicYears,
    IReadOnlyList<ImportAssessmentResultOption> AssessmentResultOptions,
    Guid? SelectedAssessmentId);

public sealed record ImportDetailsViewModel(
    ImportBatchDetail Batch,
    IReadOnlyDictionary<Guid, Edulytics.Services.Assessments.AssessmentResultsWorkspace> AssessmentWorkspaces)
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

    public string? ClassesAcademicYear =>
        Batch.Type == ImportType.Classes &&
        Batch.PreviewRows.FirstOrDefault() is { } row &&
        row.Values.TryGetValue("AcademicYear", out var year)
            ? year
            : null;

    public string RowVersionBase64 =>
        Convert.ToBase64String(
            Batch.RowVersion);
}
