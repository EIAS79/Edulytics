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

        var framework = parts[0].ToUpperInvariant() switch
        {
            "CAM" => "Cambridge",
            "DFE" => "DfE",
            "UAE" => "UAE MOE",
            "MOE" => "MOE",
            _ => string.Empty
        };

        var locator = parts[^1];
        return framework.Length == 0
            ? locator
            : $"{framework} {locator}";
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
            "not reproduced"
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
