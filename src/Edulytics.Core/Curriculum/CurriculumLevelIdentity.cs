using System.Text;

namespace Edulytics.Core.Curriculum;

/// <summary>
/// Stable product identity for one curriculum level/pathway inside a verified
/// Mathematics curriculum pack. Display labels are not used as identifiers.
/// </summary>
public sealed record CurriculumLevelIdentity(
    string Key,
    string PackCode,
    int LogicalLevel,
    string Label,
    string Stage,
    string? Pathway,
    string? CourseCode = null)
{
    public string DisplayLabel =>
        string.IsNullOrWhiteSpace(Pathway)
            ? $"{Label} — level {LogicalLevel}"
            : $"{Label} — {Pathway} — level {LogicalLevel}";
}

public static class CurriculumLevelIdentityRegistry
{
    private const string SharedPathwayToken = "SHARED";

    public sealed record AmericanHighSchoolCourse(string Code, string Label, string Pathway);
    // Exact CourseCode, NativeLevel and Pathway from the nine accepted
    // src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-*.json files.
    public static IReadOnlyList<AmericanHighSchoolCourse> SourceBackedAmericanHighSchoolCourses { get; } =
    [
        new("ALG1", "Algebra 1", "Traditional"),
        new("GEO", "Geometry", "Traditional"),
        new("ALG2", "Algebra 2", "Traditional"),
        new("TRAD-SUPPLEMENT", "Traditional Advanced Supplements", "Traditional"),
        new("ADV-ALG-FUNC", "Advanced Algebra & Functions", "Fourth Courses"),
        new("ADV-TRIG-GEO", "Advanced Trigonometry & Geometry", "Fourth Courses"),
        new("COMPLEX", "Complex Numbers", "Fourth Courses"),
        new("PROB-DECISION", "Probability & Decision Making", "Fourth Courses"),
        new("VECTOR-MATRIX", "Vectors & Matrices", "Fourth Courses")
    ];

    public static IReadOnlyList<CurriculumLevelIdentity> All { get; } =
        MathematicsCurriculumPackRegistry.All
            // Retain the old placeholder identity for reading existing school
            // records, but offer actual source-backed HS pathways for NEW
            // American school adoptions. No implicit Algebra 1/2 assignment.
            .SelectMany(pack => pack.Levels.SelectMany(level =>
                ExpandSourceBackedHighSchoolPathways(pack.Code, level)))
            .GroupBy(x => x.Key, StringComparer.Ordinal)
            .Select(x => x.First())
            .OrderBy(x => x.PackCode, StringComparer.Ordinal)
            .ThenBy(x => x.LogicalLevel)
            .ThenBy(x => x.Pathway ?? string.Empty, StringComparer.Ordinal)
            .ThenBy(x => x.CourseCode ?? string.Empty, StringComparer.Ordinal)
            .ToArray();

    public static IReadOnlyList<CurriculumLevelIdentity> ForPack(
        string? packCode)
    {
        var normalized = NormalizePackCode(packCode);
        if (normalized.Length == 0)
            return [];

        return All
            .Where(x => string.Equals(x.PackCode, normalized, StringComparison.Ordinal))
            // Existing generic keys remain resolvable via Find(), but are not
            // valid selectable new American HS paths.
            .Where(x => !IsUnresolvedAmericanHighSchoolPathway(x))
            .ToArray();
    }

    public static bool IsUnresolvedAmericanHighSchoolPathway(
        CurriculumLevelIdentity? level) =>
        level is not null &&
        level.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode &&
        level.LogicalLevel is >= 10 and <= 13 &&
        string.IsNullOrWhiteSpace(level.CourseCode);

    private static IEnumerable<CurriculumLevelIdentity> ExpandSourceBackedHighSchoolPathways(
        string packCode, AcademicLevelMapping level)
    {
        yield return Create(packCode, level);
        if (packCode != MathematicsCurriculumPackRegistry.CommonCoreCode ||
            level.LogicalLevel is < 10 or > 13 ||
            !string.Equals(level.Pathway, "Course/pathway mapping",
                StringComparison.Ordinal))
            yield break;

        // Pathway-only identities remain readable for legacy adoptions but
        // cannot be selected for NEW schools because they mix distinct courses.
        yield return Create(packCode, level with { Pathway = "Traditional" });
        yield return Create(packCode, level with { Pathway = "Fourth Courses" });
        foreach (var course in SourceBackedAmericanHighSchoolCourses)
        {
            yield return new CurriculumLevelIdentity(
                BuildKey(packCode, level.LogicalLevel, course.Pathway, course.Code),
                NormalizePackCode(packCode),
                level.LogicalLevel,
                course.Label,
                level.Stage,
                course.Pathway,
                course.Code);
        }
    }

    public static CurriculumLevelIdentity? Find(string? key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return null;

        var normalized = key.Trim().ToUpperInvariant();
        return All.SingleOrDefault(
            x => string.Equals(x.Key, normalized, StringComparison.Ordinal));
    }

    public static string? PackCodeForProgramCode(string? programCode) =>
        programCode?.Trim().ToUpperInvariant() switch
        {
            "BRITISH" => MathematicsCurriculumPackRegistry.CambridgeCode,
            "AMERICAN" => MathematicsCurriculumPackRegistry.CommonCoreCode,
            "UAE" => MathematicsCurriculumPackRegistry.UaeCode,
            "POLISH" => MathematicsCurriculumPackRegistry.PolandCode,
            _ => null
        };

    /// <summary>
    /// Compatibility-only resolver for records created before explicit level
    /// identity existed. Label and legacy order may be used together, and a
    /// small set of deterministic historic label aliases may be normalized.
    /// The method fails closed whenever more than one pathway remains possible.
    /// New writes must never call this method to choose a level.
    /// </summary>
    public static CurriculumLevelIdentity? ResolveLegacy(
        string? packCode,
        string? gradeName,
        int gradeOrder)
    {
        var levels = ForPack(packCode);
        if (levels.Count == 0)
            return null;

        var cleanedName = gradeName?.Trim();
        if (!string.IsNullOrWhiteSpace(cleanedName))
        {
            var byNativeLabel = levels
                .Where(x => string.Equals(
                    x.Label,
                    cleanedName,
                    StringComparison.OrdinalIgnoreCase))
                .ToArray();

            if (byNativeLabel.Length == 1)
                return byNativeLabel[0];

            if (byNativeLabel.Length > 1 && gradeOrder > 0)
            {
                var byLabelAndOrder = byNativeLabel
                    .Where(x => x.LogicalLevel == gradeOrder)
                    .ToArray();

                if (byLabelAndOrder.Length == 1)
                    return byLabelAndOrder[0];

                return null;
            }

            if (byNativeLabel.Length > 1)
                return null;

            var alias = ResolveDeterministicLegacyAlias(
                levels,
                cleanedName);
            if (alias is not null)
                return alias;
        }

        if (gradeOrder <= 0)
            return null;

        var byLogicalLevel = levels
            .Where(x => x.LogicalLevel == gradeOrder)
            .ToArray();

        return byLogicalLevel.Length == 1
            ? byLogicalLevel[0]
            : null;
    }

    public static string BuildKey(
        string packCode,
        int logicalLevel,
        string? pathway,
        string? courseCode = null)
    {
        var normalizedPackCode = NormalizePackCode(packCode);
        if (normalizedPackCode.Length == 0)
            throw new ArgumentException("Pack code is required.", nameof(packCode));

        if (logicalLevel is < 1 or > 13)
            throw new ArgumentOutOfRangeException(nameof(logicalLevel));

        var pathwayToken = string.IsNullOrWhiteSpace(pathway)
            ? SharedPathwayToken
            : NormalizeToken(pathway);

        var baseKey = $"{normalizedPackCode}:L{logicalLevel:D2}:{pathwayToken}";
        return string.IsNullOrWhiteSpace(courseCode)
            ? baseKey
            : $"{baseKey}:COURSE-{NormalizeToken(courseCode)}";
    }

    private static CurriculumLevelIdentity? ResolveDeterministicLegacyAlias(
        IReadOnlyList<CurriculumLevelIdentity> levels,
        string cleanedName)
    {
        if (TryReadHistoricOrdinal(cleanedName, "Grade Level ", out var gradeNumber))
        {
            return SingleByNativeLabel(
                levels,
                $"Grade {gradeNumber}");
        }

        if (TryReadHistoricOrdinal(cleanedName, "Year Level ", out var yearNumber))
        {
            return SingleByNativeLabel(
                levels,
                $"Year {yearNumber}");
        }

        return null;
    }

    private static CurriculumLevelIdentity? SingleByNativeLabel(
        IReadOnlyList<CurriculumLevelIdentity> levels,
        string label)
    {
        var matches = levels
            .Where(x => string.Equals(
                x.Label,
                label,
                StringComparison.OrdinalIgnoreCase))
            .ToArray();

        return matches.Length == 1
            ? matches[0]
            : null;
    }

    private static bool TryReadHistoricOrdinal(
        string value,
        string prefix,
        out int number)
    {
        number = 0;
        if (!value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            return false;

        var suffix = value[prefix.Length..].Trim();
        return int.TryParse(suffix, out number) && number > 0;
    }

    private static CurriculumLevelIdentity Create(
        string packCode,
        AcademicLevelMapping level) =>
        new(
            BuildKey(packCode, level.LogicalLevel, level.Pathway),
            NormalizePackCode(packCode),
            level.LogicalLevel,
            level.NativeLabel,
            level.Stage,
            string.IsNullOrWhiteSpace(level.Pathway) ? null : level.Pathway.Trim());

    private static string NormalizePackCode(string? value) =>
        value?.Trim().ToUpperInvariant() ?? string.Empty;

    private static string NormalizeToken(string value)
    {
        var builder = new StringBuilder(value.Length);
        var pendingDash = false;

        foreach (var character in value.Trim().ToUpperInvariant())
        {
            if (char.IsLetterOrDigit(character))
            {
                if (pendingDash && builder.Length > 0)
                    builder.Append('-');

                builder.Append(character);
                pendingDash = false;
            }
            else
            {
                pendingDash = true;
            }
        }

        return builder.Length == 0
            ? SharedPathwayToken
            : builder.ToString();
    }
}
