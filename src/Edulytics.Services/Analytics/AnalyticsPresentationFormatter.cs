namespace Edulytics.Services.Analytics;

public static class AnalyticsPresentationFormatter
{
    public static string ClassFilterLabel(string? className) =>
        className?.Trim() ?? string.Empty;

    public static string OutcomeLabel(string? code)
    {
        var value = code?.Trim() ?? string.Empty;
        if (value.Length == 0)
            return string.Empty;

        var parts = value.Split(
            ':',
            StringSplitOptions.RemoveEmptyEntries |
            StringSplitOptions.TrimEntries);

        if (parts.Length < 2)
            return value;

        if (parts[0].Equals("PL", StringComparison.OrdinalIgnoreCase))
            return PolishOutcomeLabel(parts, value);

        var framework = parts[0].ToUpperInvariant() switch
        {
            "CAM" => "Cambridge",
            "DFE" => "DfE",
            "UAE" => "UAE MOE",
            "MOE" => "MOE",
            _ => string.Empty
        };

        var locator = parts[^1];
        if (framework.Length > 0)
            return $"{framework} {locator}";

        // Unknown colon-delimited codes are preserved unless they explicitly
        // use the internal ...:OUT:... shape. Truncating every code to its
        // final segment would destroy meaningful identifiers used by other
        // curricula, especially Polish national curriculum outcomes.
        return parts.Length >= 3 &&
               parts[1].Equals("OUT", StringComparison.OrdinalIgnoreCase)
            ? locator
            : value;
    }

    private static string PolishOutcomeLabel(
        IReadOnlyList<string> parts,
        string original)
    {
        var stageIndex = parts.Count >= 4 &&
                         parts[1].Equals("REQ", StringComparison.OrdinalIgnoreCase) &&
                         parts[2].Equals("PL", StringComparison.OrdinalIgnoreCase)
            ? 3
            : 1;

        if (parts.Count < stageIndex + 4)
            return original;

        var stage = parts[stageIndex].ToUpperInvariant() switch
        {
            "UPPER" => "Upper",
            _ => parts[stageIndex]
        };

        var track = parts[^3].ToLowerInvariant() switch
        {
            "core" => "Core",
            "basic" => "Basic",
            "extended" => "Extended",
            _ => parts[^3]
        };

        return $"Polish {stage} {track} {parts[^2]}.{parts[^1]}";
    }

    public static string OutcomeDescription(
        string? description,
        string displayLabel,
        string? subjectName)
    {
        var value = description?.Trim() ?? string.Empty;
        if (value.Length == 0)
            return BuildAlignedDescription(displayLabel, subjectName);

        if (!ContainsTechnicalReferenceText(value))
            return value;

        return BuildAlignedDescription(displayLabel, subjectName);
    }

    private static bool ContainsTechnicalReferenceText(string value)
    {
        var markers = new[]
        {
            "reference-only",
            "source locator",
            "copyright",
            "wording is not reproduced",
            "not reproduced",
            "objective wording",
            "academic reference authority",
            "ogl material",
            "source teaching sequence"
        };

        return markers.Any(marker =>
            value.Contains(marker, StringComparison.OrdinalIgnoreCase));
    }

    private static string BuildAlignedDescription(
        string displayLabel,
        string? subjectName)
    {
        var label = displayLabel.Trim();
        var subject = subjectName?.Trim() ?? string.Empty;

        if (label.Length == 0)
            return subject.Length == 0
                ? "Learning outcome."
                : $"{subject} learning outcome.";

        return subject.Length == 0
            ? $"Skill aligned to {label}."
            : $"{subject} skill aligned to {label}.";
    }
}
