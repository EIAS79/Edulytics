using System.Text;
using Edulytics.Core.Enums;
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

    private static readonly IReadOnlyList<string> TeacherAssignmentHeaders =
    [
        "Email",
        "AcademicYear",
        "ClassCode"
    ];

    public static bool IsSupported(ImportType type) =>
        SupportedTypes.Contains(type);

    public static IReadOnlyList<ImportTypeOption> FilterOptions(
        IReadOnlyList<ImportTypeOption> options) =>
        options
            .Where(x => IsSupported(x.Type))
            .Select(x =>
                x.Type == ImportType.Teachers
                    ? new ImportTypeOption(
                        x.Type,
                        TeacherAssignmentHeaders)
                    : x)
            .ToArray();

    public static IReadOnlyList<string> TemplateHeaders(
        ImportType type,
        IReadOnlyList<string> serviceHeaders) =>
        type == ImportType.Teachers
            ? TeacherAssignmentHeaders
            : serviceHeaders;

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

                var pair = row.Values.FirstOrDefault(x =>
                    string.Equals(
                        x.Key,
                        header,
                        StringComparison.OrdinalIgnoreCase));

                return pair.Value ?? string.Empty;
            });

            builder.AppendLine(string.Join(",", values.Select(EscapeCsv)));
        }

        var normalizedName =
            $"{Path.GetFileNameWithoutExtension(fileName)}.csv";

        return new(
            normalizedName,
            new UTF8Encoding(false).GetBytes(builder.ToString()));
    }

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
