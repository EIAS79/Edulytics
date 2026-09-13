using System.Globalization;
using System.Text;
using Edulytics.Core.Enums;
using Edulytics.Services.Assessments;
using Edulytics.Services.Imports;

namespace Edulytics.Web.Imports;

public sealed record AdaptedImportUpload(
    string FileName,
    byte[] Bytes);

public static class MathOnlyImportAdapter
{
    private static readonly ImportType[] SupportedTypes =
    [
        ImportType.Students,
        ImportType.Teachers,
        ImportType.Classes,
        ImportType.AssessmentResults
    ];

    private static readonly IReadOnlyList<string> StudentHeaders =
    [
        "StudentNumber",
        "FirstName",
        "LastName",
        "Email",
        "AcademicYear",
        "ClassCode"
    ];

    private static readonly IReadOnlyList<string> TeacherAssignmentHeaders =
    [
        "Email",
        "AcademicYear",
        "ClassCode"
    ];

    private static readonly IReadOnlyList<string> FriendlyAssessmentResultHeaders =
    [
        "AssessmentTitle",
        "AssessmentDate",
        "ClassCode",
        "StudentNumber",
        "StudentName",
        "QuestionOrder",
        "Score"
    ];

    public static bool IsSupported(ImportType type) =>
        SupportedTypes.Contains(type);

    public static IReadOnlyList<ImportTypeOption> FilterOptions(
        IReadOnlyList<ImportTypeOption> options) =>
        options
            .Where(x => IsSupported(x.Type))
            .Select(x =>
                x.Type switch
                {
                    ImportType.Students =>
                        new ImportTypeOption(x.Type, StudentHeaders),
                    ImportType.Teachers =>
                        new ImportTypeOption(x.Type, TeacherAssignmentHeaders),
                    ImportType.AssessmentResults =>
                        new ImportTypeOption(x.Type, FriendlyAssessmentResultHeaders),
                    _ => x
                })
            .ToArray();

    public static IReadOnlyList<string> TemplateHeaders(
        ImportType type,
        IReadOnlyList<string> serviceHeaders) =>
        type switch
        {
            ImportType.Students => StudentHeaders,
            ImportType.Teachers => TeacherAssignmentHeaders,
            ImportType.AssessmentResults => FriendlyAssessmentResultHeaders,
            _ => serviceHeaders
        };

    public static AdaptedImportUpload NormalizeUpload(
        ImportType type,
        string fileName,
        byte[] bytes)
    {
        if (type != ImportType.Teachers)
            return new(fileName, bytes);

        var parsed = new ImportFileParser().Parse(fileName, bytes);
        if (!parsed.Succeeded)
            return new(fileName, bytes);

        var headers = parsed.File!.Headers
            .Where(x =>
                !string.Equals(
                    x,
                    "SubjectCode",
                    StringComparison.OrdinalIgnoreCase))
            .ToList();

        headers.Add("SubjectCode");

        var builder = new StringBuilder();
        builder.AppendLine(string.Join(",", headers.Select(EscapeCsv)));

        foreach (var row in parsed.File.Rows)
        {
            var values = headers.Select(header =>
            {
                if (string.Equals(
                        header,
                        "SubjectCode",
                        StringComparison.OrdinalIgnoreCase))
                {
                    return "MATH";
                }

                return Value(row, header);
            });

            builder.AppendLine(string.Join(",", values.Select(EscapeCsv)));
        }

        return CsvUpload(fileName, builder.ToString());
    }

    public static AdaptedImportUpload NormalizeAssessmentResults(
        string fileName,
        byte[] bytes,
        AssessmentWorkspace workspace)
    {
        var parsed = new ImportFileParser().Parse(fileName, bytes);
        if (!parsed.Succeeded)
            return new(fileName, bytes);

        var actualHeaders = parsed.File!.Headers.ToHashSet(
            StringComparer.OrdinalIgnoreCase);

        if (FriendlyAssessmentResultHeaders.Any(x => !actualHeaders.Contains(x)))
            return new(fileName, bytes);

        var outputHeaders = new[]
        {
            "AssessmentTitle",
            "AssessmentDate",
            "ClassCode",
            "StudentNumber",
            "StudentName",
            "QuestionOrder",
            "Score",
            "AssessmentId"
        };

        var builder = new StringBuilder();
        builder.AppendLine(string.Join(",", outputHeaders.Select(EscapeCsv)));

        foreach (var row in parsed.File.Rows)
        {
            var title = Value(row, "AssessmentTitle").Trim();
            var dateText = Value(row, "AssessmentDate").Trim();
            var classCode = NormalizeCode(Value(row, "ClassCode"));

            var classIds = workspace.ClassGroups
                .Where(x => NormalizeCode(x.Code) == classCode)
                .Select(x => x.Id)
                .ToHashSet();

            var dateValid = DateOnly.TryParseExact(
                dateText,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date);

            var matches = dateValid
                ? workspace.Assessments
                    .Where(x =>
                        classIds.Contains(x.ClassGroupId) &&
                        x.AssessmentDate == date &&
                        string.Equals(
                            x.Title.Trim(),
                            title,
                            StringComparison.OrdinalIgnoreCase))
                    .ToArray()
                : [];

            var assessmentId = matches.Length == 1
                ? matches[0].Id.ToString("D")
                : $"UNRESOLVED:{title}|{dateText}|{classCode}";

            var values = new[]
            {
                title,
                dateText,
                classCode,
                Value(row, "StudentNumber"),
                Value(row, "StudentName"),
                Value(row, "QuestionOrder"),
                Value(row, "Score"),
                assessmentId
            };

            builder.AppendLine(string.Join(",", values.Select(EscapeCsv)));
        }

        return CsvUpload(fileName, builder.ToString());
    }

    private static AdaptedImportUpload CsvUpload(string fileName, string content) =>
        new(
            $"{Path.GetFileNameWithoutExtension(fileName)}.csv",
            new UTF8Encoding(false).GetBytes(content));

    private static string Value(ImportFileRow row, string header) =>
        row.Values.FirstOrDefault(x =>
            string.Equals(
                x.Key,
                header,
                StringComparison.OrdinalIgnoreCase)).Value
        ?? string.Empty;

    private static string NormalizeCode(string value) =>
        value.Trim().ToUpperInvariant();

    private static string EscapeCsv(string value)
    {
        if (!value.Contains(',') &&
            !value.Contains('"') &&
            !value.Contains('\n') &&
            !value.Contains('\r'))
        {
            return value;
        }

        return $"\"{value.Replace("\"", "\"\"")}\"";
    }
}
