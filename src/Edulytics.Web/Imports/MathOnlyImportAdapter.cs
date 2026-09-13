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
        ImportType.SubjectSupervisors,
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
        "ClassName"
    ];

    private static readonly IReadOnlyList<string> TeacherHeaders =
    [
        "Email",
        "AcademicYear",
        "ClassName"
    ];

    private static readonly IReadOnlyList<string> SupervisorHeaders =
    [
        "Email"
    ];

    private static readonly IReadOnlyList<string> FriendlyAssessmentResultHeaders =
    [
        "AssessmentTitle",
        "AssessmentDate",
        "ClassName",
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
                        new ImportTypeOption(x.Type, TeacherHeaders),
                    ImportType.SubjectSupervisors =>
                        new ImportTypeOption(x.Type, SupervisorHeaders),
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
            ImportType.Teachers => TeacherHeaders,
            ImportType.SubjectSupervisors => SupervisorHeaders,
            ImportType.AssessmentResults => FriendlyAssessmentResultHeaders,
            _ => serviceHeaders
        };

    public static AdaptedImportUpload NormalizeUpload(
        ImportType type,
        string fileName,
        byte[] bytes,
        AssessmentWorkspace? workspace = null)
    {
        return type switch
        {
            ImportType.Students => NormalizeStudents(fileName, bytes, workspace),
            ImportType.Teachers => NormalizeTeachers(fileName, bytes, workspace),
            ImportType.AssessmentResults when workspace is not null =>
                NormalizeAssessmentResults(fileName, bytes, workspace),
            _ => new AdaptedImportUpload(fileName, bytes)
        };
    }

    private static AdaptedImportUpload NormalizeStudents(
        string fileName,
        byte[] bytes,
        AssessmentWorkspace? workspace)
    {
        var parsed = new ImportFileParser().Parse(fileName, bytes);
        if (!parsed.Succeeded || parsed.File is null || workspace is null)
            return new(fileName, bytes);

        var actualHeaders = parsed.File.Headers.ToHashSet(StringComparer.OrdinalIgnoreCase);
        if (StudentHeaders.Any(x => !actualHeaders.Contains(x)))
            return new(fileName, bytes);

        var outputHeaders = StudentHeaders.Concat(["ClassCode"]).ToArray();
        var builder = new StringBuilder();
        builder.AppendLine(string.Join(",", outputHeaders.Select(EscapeCsv)));

        foreach (var row in parsed.File.Rows)
        {
            var className = Value(row, "ClassName").Trim();
            var classCode = ResolveClassCode(workspace, className);
            var values = StudentHeaders
                .Select(header => Value(row, header))
                .Concat([classCode]);
            builder.AppendLine(string.Join(",", values.Select(EscapeCsv)));
        }

        return CsvUpload(fileName, builder.ToString());
    }

    private static AdaptedImportUpload NormalizeTeachers(
        string fileName,
        byte[] bytes,
        AssessmentWorkspace? workspace)
    {
        var parsed = new ImportFileParser().Parse(fileName, bytes);
        if (!parsed.Succeeded || parsed.File is null || workspace is null)
            return new(fileName, bytes);

        var actualHeaders = parsed.File.Headers.ToHashSet(StringComparer.OrdinalIgnoreCase);
        if (TeacherHeaders.Any(x => !actualHeaders.Contains(x)))
            return new(fileName, bytes);

        var outputHeaders = TeacherHeaders.Concat(["ClassCode", "SubjectCode"]).ToArray();
        var builder = new StringBuilder();
        builder.AppendLine(string.Join(",", outputHeaders.Select(EscapeCsv)));

        foreach (var row in parsed.File.Rows)
        {
            var className = Value(row, "ClassName").Trim();
            var classCode = ResolveClassCode(workspace, className);
            var values = TeacherHeaders
                .Select(header => Value(row, header))
                .Concat([classCode, "MATH"]);
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
        if (!parsed.Succeeded || parsed.File is null)
            return new(fileName, bytes);

        var actualHeaders = parsed.File.Headers.ToHashSet(
            StringComparer.OrdinalIgnoreCase);

        if (FriendlyAssessmentResultHeaders.Any(x => !actualHeaders.Contains(x)))
            return new(fileName, bytes);

        var outputHeaders = FriendlyAssessmentResultHeaders
            .Concat(["ClassCode", "AssessmentId"])
            .ToArray();

        var builder = new StringBuilder();
        builder.AppendLine(string.Join(",", outputHeaders.Select(EscapeCsv)));

        foreach (var row in parsed.File.Rows)
        {
            var title = Value(row, "AssessmentTitle").Trim();
            var dateText = Value(row, "AssessmentDate").Trim();
            var className = Value(row, "ClassName").Trim();

            var classIds = workspace.ClassGroups
                .Where(x => string.Equals(
                    x.Name.Trim(),
                    className,
                    StringComparison.OrdinalIgnoreCase))
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
                : $"UNRESOLVED:{title}|{dateText}|{className}";

            var matchedClass = matches.Length == 1
                ? workspace.ClassGroups.SingleOrDefault(x => x.Id == matches[0].ClassGroupId)
                : null;
            var classCode = matchedClass is null
                ? ResolveClassCode(workspace, className)
                : NormalizeCode(matchedClass.Code);

            var values = FriendlyAssessmentResultHeaders
                .Select(header => Value(row, header))
                .Concat([classCode, assessmentId]);

            builder.AppendLine(string.Join(",", values.Select(EscapeCsv)));
        }

        return CsvUpload(fileName, builder.ToString());
    }

    private static string ResolveClassCode(
        AssessmentWorkspace workspace,
        string className)
    {
        if (string.IsNullOrWhiteSpace(className))
            return string.Empty;

        var codes = workspace.ClassGroups
            .Where(x => string.Equals(
                x.Name.Trim(),
                className.Trim(),
                StringComparison.OrdinalIgnoreCase))
            .Select(x => NormalizeCode(x.Code))
            .Where(x => x.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return codes.Length == 1
            ? codes[0]
            : $"UNRESOLVED:{className.Trim()}";
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
