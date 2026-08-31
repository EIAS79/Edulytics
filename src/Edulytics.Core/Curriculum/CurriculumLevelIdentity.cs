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
    string? Pathway)
{
    public string DisplayLabel =>
        string.IsNullOrWhiteSpace(Pathway)
            ? $"{Label} — level {LogicalLevel}"
            : $"{Label} — {Pathway} — level {LogicalLevel}";
}

public static class CurriculumLevelIdentityRegistry
{
    private const string SharedPathwayToken = "SHARED";

    public static IReadOnlyList<CurriculumLevelIdentity> All { get; } =
        MathematicsCurriculumPackRegistry.All
            .SelectMany(pack => pack.Levels.Select(level => Create(pack.Code, level)))
            .GroupBy(x => x.Key, StringComparer.Ordinal)
            .Select(x => x.First())
            .OrderBy(x => x.PackCode, StringComparer.Ordinal)
            .ThenBy(x => x.LogicalLevel)
            .ThenBy(x => x.Pathway ?? string.Empty, StringComparer.Ordinal)
            .ToArray();

    public static IReadOnlyList<CurriculumLevelIdentity> ForPack(
        string? packCode)
    {
        var normalized = NormalizePackCode(packCode);
        if (normalized.Length == 0)
            return [];

        return All
            .Where(x => string.Equals(x.PackCode, normalized, StringComparison.Ordinal))
            .ToArray();
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
    /// identity existed. Label and legacy order may be used together, but the
    /// method fails closed whenever they still identify more than one pathway.
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
        string? pathway)
    {
        var normalizedPackCode = NormalizePackCode(packCode);
        if (normalizedPackCode.Length == 0)
            throw new ArgumentException("Pack code is required.", nameof(packCode));

        if (logicalLevel is < 1 or > 13)
            throw new ArgumentOutOfRangeException(nameof(logicalLevel));

        var pathwayToken = string.IsNullOrWhiteSpace(pathway)
            ? SharedPathwayToken
            : NormalizeToken(pathway);

        return $"{normalizedPackCode}:L{logicalLevel:D2}:{pathwayToken}";
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
