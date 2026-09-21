using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Edulytics.Core.Academics;
using Edulytics.Core.Enums;
using Edulytics.Services.Assessments;
using Edulytics.Services.Imports;

namespace Edulytics.Web.Imports;

public sealed record AdaptedImportUpload(
    string FileName,
    byte[] Bytes);

public sealed record ClassImportLevelOption(
    string DisplayName,
    string GradeLevelName,
    Guid CurriculumAdoptionId);

public static class MathOnlyImportAdapter
{
    private static readonly ImportType[] SupportedTypes =
    [
        ImportType.Students,
        ImportType.Teachers,
        ImportType.SubjectSupervisors,
        ImportType.Classes
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

    private static readonly IReadOnlyList<string> ClassTemplateHeaders =
    [
        "GradeLevel",
        "Name"
    ];

    private static readonly IReadOnlyList<string> InternalClassHeaders =
    [
        "AcademicYear",
        "GradeLevel",
        "Name"
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
                    ImportType.Classes =>
                        new ImportTypeOption(x.Type, ClassTemplateHeaders),
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
            ImportType.Classes => ClassTemplateHeaders,
            _ => serviceHeaders
        };

    public static IReadOnlyList<ClassImportLevelOption> ClassLevelOptions(
        AcademicStructureSnapshot academicStructure,
        Guid? academicYearId = null)
    {
        var grades = academicStructure.GradeLevels
            .ToDictionary(x => x.Id);
        var programs = academicStructure.AcademicPrograms
            .ToDictionary(x => x.Id);

        var candidates = academicStructure.CurriculumAdoptions
            .Where(x =>
                x.IsActive &&
                x.AcademicYearId.HasValue &&
                (!academicYearId.HasValue ||
                 x.AcademicYearId.Value == academicYearId.Value) &&
                x.AcademicProgramId != Guid.Empty &&
                grades.ContainsKey(x.GradeLevelId))
            .Select(x =>
            {
                var grade = grades[x.GradeLevelId];
                var baseLabel = string.IsNullOrWhiteSpace(x.CurriculumLevelLabel)
                    ? grade.Name.Trim()
                    : x.CurriculumLevelLabel.Trim();
                var pathway = x.CurriculumPathway?.Trim() ?? string.Empty;
                var programName = programs.GetValueOrDefault(x.AcademicProgramId)?.Name?.Trim()
                    ?? string.Empty;

                return new ClassLevelCandidate(
                    x.Id,
                    x.AcademicYearId!.Value,
                    x.GradeLevelId,
                    grade.Name.Trim(),
                    baseLabel,
                    pathway,
                    programName,
                    x.CurriculumLevelKey?.Trim() ?? string.Empty);
            })
            .ToArray();

        var contextual = candidates
            .Select(candidate =>
            {
                var siblings = candidates
                    .Where(x =>
                        x.AcademicYearId == candidate.AcademicYearId &&
                        string.Equals(
                            x.BaseLabel,
                            candidate.BaseLabel,
                            StringComparison.OrdinalIgnoreCase))
                    .ToArray();

                return new LabeledClassLevelCandidate(
                    candidate,
                    siblings.Length > 1
                        ? BuildDisambiguatedLevelLabel(candidate)
                        : candidate.BaseLabel);
            })
            .ToArray();

        var keyed = contextual
            .Select(item =>
            {
                var collisions = contextual.Count(x =>
                    x.Candidate.AcademicYearId == item.Candidate.AcademicYearId &&
                    string.Equals(
                        x.DisplayName,
                        item.DisplayName,
                        StringComparison.OrdinalIgnoreCase));

                var display = item.DisplayName;
                if (collisions > 1 &&
                    item.Candidate.CurriculumLevelKey.Length > 0)
                {
                    display =
                        $"{display} — {item.Candidate.CurriculumLevelKey}";
                }

                return new LabeledClassLevelCandidate(
                    item.Candidate,
                    display);
            })
            .ToArray();

        return keyed
            .GroupBy(x => new
            {
                x.Candidate.AcademicYearId,
                DisplayName = x.DisplayName.ToUpperInvariant()
            })
            .SelectMany(group =>
            {
                var ordered = group
                    .OrderBy(x => x.Candidate.AdoptionId)
                    .ToArray();

                return ordered.Select((item, index) =>
                    new ClassImportLevelOption(
                        ordered.Length == 1
                            ? item.DisplayName
                            : $"{item.DisplayName} — option {index + 1}",
                        item.Candidate.GradeLevelName,
                        item.Candidate.AdoptionId));
            })
            .OrderBy(x => x.DisplayName, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    public static bool LooksLikeClassesUpload(
        string fileName,
        byte[] bytes)
    {
        var parsed = new ImportFileParser().Parse(fileName, bytes);
        if (!parsed.Succeeded || parsed.File is null)
            return false;

        var headers = parsed.File.Headers.ToHashSet(
            StringComparer.OrdinalIgnoreCase);

        return ClassTemplateHeaders.All(headers.Contains) &&
               !headers.Contains("Email") &&
               !headers.Contains("StudentNumber");
    }

    public static bool UsesLegacyClassesTemplate(
        string fileName,
        byte[] bytes)
    {
        var parsed = new ImportFileParser().Parse(fileName, bytes);
        if (!parsed.Succeeded || parsed.File is null)
            return false;

        var headers = parsed.File.Headers.ToHashSet(
            StringComparer.OrdinalIgnoreCase);

        return ClassTemplateHeaders.All(headers.Contains) &&
               headers.Contains("AcademicYear") &&
               !headers.Contains("Email") &&
               !headers.Contains("StudentNumber");
    }

    public static AdaptedImportUpload NormalizeUpload(
        ImportType type,
        string fileName,
        byte[] bytes,
        AssessmentWorkspace? workspace = null,
        AcademicStructureSnapshot? academicStructure = null,
        string? selectedAcademicYear = null)
    {
        return type switch
        {
            ImportType.Students =>
                NormalizeStudents(fileName, bytes, workspace, academicStructure),
            ImportType.Teachers =>
                NormalizeTeachers(fileName, bytes, workspace, academicStructure),
            ImportType.Classes =>
                NormalizeClasses(
                    fileName,
                    bytes,
                    academicStructure,
                    selectedAcademicYear),
            _ => new AdaptedImportUpload(fileName, bytes)
        };
    }

    private static AdaptedImportUpload NormalizeStudents(
        string fileName,
        byte[] bytes,
        AssessmentWorkspace? workspace,
        AcademicStructureSnapshot? academicStructure)
    {
        var parsed = new ImportFileParser().Parse(fileName, bytes);
        if (!parsed.Succeeded || parsed.File is null ||
            (academicStructure is null && workspace is null))
        {
            return new(fileName, bytes);
        }

        var actualHeaders = parsed.File.Headers.ToHashSet(StringComparer.OrdinalIgnoreCase);
        if (StudentHeaders.Any(x => !actualHeaders.Contains(x)))
            return new(fileName, bytes);

        var outputHeaders = StudentHeaders.Concat(["ClassCode"]).ToArray();
        var builder = new StringBuilder();
        builder.AppendLine(string.Join(",", outputHeaders.Select(EscapeCsv)));

        foreach (var row in parsed.File.Rows)
        {
            var academicYear = Value(row, "AcademicYear").Trim();
            var className = Value(row, "ClassName").Trim();
            var classCode = academicStructure is not null
                ? ResolveClassCode(academicStructure, academicYear, className)
                : ResolveClassCode(workspace!, className);
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
        AssessmentWorkspace? workspace,
        AcademicStructureSnapshot? academicStructure)
    {
        var parsed = new ImportFileParser().Parse(fileName, bytes);
        if (!parsed.Succeeded || parsed.File is null ||
            (academicStructure is null && workspace is null))
        {
            return new(fileName, bytes);
        }

        var actualHeaders = parsed.File.Headers.ToHashSet(StringComparer.OrdinalIgnoreCase);
        if (TeacherHeaders.Any(x => !actualHeaders.Contains(x)))
            return new(fileName, bytes);

        var outputHeaders = TeacherHeaders.Concat(["ClassCode", "SubjectCode"]).ToArray();
        var builder = new StringBuilder();
        builder.AppendLine(string.Join(",", outputHeaders.Select(EscapeCsv)));

        foreach (var row in parsed.File.Rows)
        {
            var academicYear = Value(row, "AcademicYear").Trim();
            var className = Value(row, "ClassName").Trim();
            var classCode = academicStructure is not null
                ? ResolveClassCode(academicStructure, academicYear, className)
                : ResolveClassCode(workspace!, className);
            var values = TeacherHeaders
                .Select(header => Value(row, header))
                .Concat([classCode, "MATH"]);
            builder.AppendLine(string.Join(",", values.Select(EscapeCsv)));
        }

        return CsvUpload(fileName, builder.ToString());
    }

    private static AdaptedImportUpload NormalizeClasses(
        string fileName,
        byte[] bytes,
        AcademicStructureSnapshot? academicStructure,
        string? selectedAcademicYear)
    {
        var parsed = new ImportFileParser().Parse(fileName, bytes);
        if (!parsed.Succeeded || parsed.File is null)
            return new(fileName, bytes);

        var actualHeaders = parsed.File.Headers.ToHashSet(StringComparer.OrdinalIgnoreCase);
        if (ClassTemplateHeaders.Any(x => !actualHeaders.Contains(x)))
            return new(fileName, bytes);

        var academicYear = selectedAcademicYear?.Trim() ?? string.Empty;
        if (academicYear.Length == 0)
            return new(fileName, bytes);

        var selectedYear = academicStructure?.AcademicYears
            .SingleOrDefault(x =>
                x.Status == AcademicStructureStatus.Active &&
                string.Equals(
                    x.Name.Trim(),
                    academicYear,
                    StringComparison.OrdinalIgnoreCase));

        var levelOptions = selectedYear is null || academicStructure is null
            ? []
            : ClassLevelOptions(academicStructure, selectedYear.Id);
        var carriesAdoptionIdentity = academicStructure is not null;

        var outputHeaders = carriesAdoptionIdentity
            ? InternalClassHeaders.Concat(["CurriculumAdoptionId", "Code"]).ToArray()
            : InternalClassHeaders.Concat(["Code"]).ToArray();
        var builder = new StringBuilder();
        builder.AppendLine(string.Join(",", outputHeaders.Select(EscapeCsv)));

        foreach (var row in parsed.File.Rows)
        {
            var submittedLevel = Value(row, "GradeLevel").Trim();
            var name = Value(row, "Name");
            var option = ResolveClassLevelOption(levelOptions, submittedLevel);
            var gradeLevel = option?.GradeLevelName ?? submittedLevel;
            var adoptionIdentity = option is null
                ? $"UNRESOLVED:{submittedLevel}"
                : option.CurriculumAdoptionId.ToString("D");
            var code = GenerateClassCode(academicYear, name);

            var values = carriesAdoptionIdentity
                ? new[]
                {
                    academicYear,
                    gradeLevel,
                    name,
                    adoptionIdentity,
                    code
                }
                : new[]
                {
                    academicYear,
                    gradeLevel,
                    name,
                    code
                };

            builder.AppendLine(string.Join(",", values.Select(EscapeCsv)));
        }

        return CsvUpload(fileName, builder.ToString());
    }

    private static ClassImportLevelOption? ResolveClassLevelOption(
        IReadOnlyList<ClassImportLevelOption> options,
        string submittedLevel)
    {
        var displayMatches = options
            .Where(x => string.Equals(
                x.DisplayName,
                submittedLevel,
                StringComparison.OrdinalIgnoreCase))
            .ToArray();

        if (displayMatches.Length == 1)
            return displayMatches[0];

        var legacyMatches = options
            .Where(x => string.Equals(
                x.GradeLevelName,
                submittedLevel,
                StringComparison.OrdinalIgnoreCase))
            .ToArray();

        return legacyMatches.Length == 1
            ? legacyMatches[0]
            : null;
    }

    private static string BuildDisambiguatedLevelLabel(
        ClassLevelCandidate candidate)
    {
        var parts = new List<string> { candidate.BaseLabel };
        if (candidate.Pathway.Length > 0)
            parts.Add(candidate.Pathway);
        if (candidate.ProgramName.Length > 0)
            parts.Add(candidate.ProgramName);
        return string.Join(" — ", parts);
    }

    private static string ResolveClassCode(
        AcademicStructureSnapshot academicStructure,
        string academicYearName,
        string className)
    {
        if (string.IsNullOrWhiteSpace(academicYearName) ||
            string.IsNullOrWhiteSpace(className))
        {
            return string.Empty;
        }

        var yearIds = academicStructure.AcademicYears
            .Where(x =>
                x.Status == AcademicStructureStatus.Active &&
                string.Equals(
                    x.Name.Trim(),
                    academicYearName.Trim(),
                    StringComparison.OrdinalIgnoreCase))
            .Select(x => x.Id)
            .ToHashSet();

        if (yearIds.Count != 1)
            return $"UNRESOLVED:{academicYearName.Trim()}|{className.Trim()}";

        var codes = academicStructure.ClassGroups
            .Where(x =>
                yearIds.Contains(x.AcademicYearId) &&
                x.Status == AcademicStructureStatus.Active &&
                string.Equals(
                    x.Name.Trim(),
                    className.Trim(),
                    StringComparison.OrdinalIgnoreCase))
            .Select(x => NormalizeCode(x.Code))
            .Where(x => x.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return codes.Length == 1
            ? codes[0]
            : $"UNRESOLVED:{academicYearName.Trim()}|{className.Trim()}";
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

    private static string GenerateClassCode(string academicYear, string className)
    {
        var key = $"{academicYear.Trim().ToUpperInvariant()}\n{className.Trim().ToUpperInvariant()}";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        return $"CLS-{Convert.ToHexString(hash.AsSpan(0, 6))}";
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

    private sealed record LabeledClassLevelCandidate(
        ClassLevelCandidate Candidate,
        string DisplayName);

    private sealed record ClassLevelCandidate(
        Guid AdoptionId,
        Guid AcademicYearId,
        Guid GradeLevelId,
        string GradeLevelName,
        string BaseLabel,
        string Pathway,
        string ProgramName,
        string CurriculumLevelKey);
}
