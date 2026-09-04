using System.Globalization;
using Edulytics.Core.Reports;
using Edulytics.Services.Reports;

namespace Edulytics.Web.ViewModels.Reports;

public sealed class ReportIndexViewModel
{
    public required ReportCatalog Catalog { get; init; }
    public required ReportRequest Request { get; init; }

    public ReportDocument? Document { get; init; }

    public IReadOnlyList<ReportExportListItem>
        Jobs { get; init; } =
        [];

    public string CsvIdempotencyKey { get; } =
        Guid.NewGuid().ToString("N");

    public string XlsxIdempotencyKey { get; } =
        Guid.NewGuid().ToString("N");

    public static string KindKey(
        ReportKind kind) =>
        kind switch
        {
            ReportKind.School =>
                "ReportKindSchool",

            ReportKind.Class =>
                "ReportKindClass",

            ReportKind.Subject =>
                "ReportKindSubject",

            ReportKind.Student =>
                "ReportKindStudent",

            ReportKind.LearningOutcome =>
                "ReportKindLearningOutcome",

            _ =>
                "ReportsTitle"
        };

    public static string StatusKey(
        ReportExportJobStatus status) =>
        "ReportJobStatus" + status;

    public static string FormatKey(
        ReportExportFormat format) =>
        format switch
        {
            ReportExportFormat.Csv =>
                "ExportCsv",

            _ =>
                "ExportXlsx"
        };

    public static string Display(
        ReportCell cell) =>
        cell.Kind switch
        {
            ReportCellKind.Integer =>
                (cell.NumberValue ?? 0m)
                    .ToString(
                        "0",
                        CultureInfo.CurrentCulture),

            ReportCellKind.Decimal =>
                (cell.NumberValue ?? 0m)
                    .ToString(
                        "0.##",
                        CultureInfo.CurrentCulture),

            ReportCellKind.Percentage =>
                (cell.NumberValue ?? 0m)
                    .ToString(
                        "0.##",
                        CultureInfo.CurrentCulture)
                + "%",

            ReportCellKind.DateTime =>
                cell.DateTimeValue
                    ?.ToString(
                        "yyyy-MM-dd HH:mm 'UTC'",
                        CultureInfo.CurrentCulture)
                ?? string.Empty,

            _ =>
                cell.TextValue ?? string.Empty
        };

    public static bool HasRequiredSelection(
        ReportRequest request) =>
        ReportRequestPolicy
            .HasRequiredSelection(request);

    public static bool ShowAcademicYear(
        ReportKind kind) =>
        ReportRequestPolicy
            .UsesAcademicYear(kind);

    public static bool ShowClass(
        ReportKind kind) =>
        ReportRequestPolicy
            .UsesClass(kind);

    public static bool ShowSubject(
        ReportKind kind) =>
        ReportRequestPolicy
            .UsesSubject(kind);

    public static bool ShowStudent(
        ReportKind kind) =>
        ReportRequestPolicy
            .UsesStudent(kind);

    public static bool ShowLearningOutcome(
        ReportKind kind) =>
        ReportRequestPolicy
            .UsesLearningOutcome(kind);

    public static bool RequireAcademicYear(
        ReportKind kind) =>
        ReportRequestPolicy
            .RequiresAcademicYear(kind);

    public static bool RequireClass(
        ReportKind kind) =>
        ReportRequestPolicy
            .RequiresClass(kind);
}
