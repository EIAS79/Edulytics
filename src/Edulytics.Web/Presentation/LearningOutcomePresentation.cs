using System.Text.RegularExpressions;

namespace Edulytics.Web.Presentation;

public static class LearningOutcomePresentation
{
    private static readonly string[] ConcisePrefixes =
    [
        "Understand and use ",
        "Understand ",
        "Recognise and use ",
        "Recognize and use ",
        "Know and use ",
        "Use "
    ];

    private static readonly Regex ReferenceCodePattern = new(
        @"^(?<stage>\d+)(?<family>[A-Za-z]+)(?:\.(?<item>\d+))?$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly Regex ReferenceFamilyTopicPattern = new(
        @"Stage\s+(?<stage>[^\s]+).*reference\s+family\s+(?<family>[A-Za-z]+)",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    public static string DisplayCode(string? code)
    {
        var value = (code ?? string.Empty).Trim();
        if (value.Length == 0)
            return string.Empty;

        var separator = value.LastIndexOf(':');
        return separator >= 0 && separator < value.Length - 1
            ? value[(separator + 1)..]
            : value;
    }

    public static string DisplayTitle(string? code, string? description)
    {
        var value = (description ?? string.Empty).Trim();
        if (value.Length == 0 || IsReferenceOnlyDescription(value))
            return ReferenceOnlyTitle(code);

        value = value.TrimEnd('.', ' ', '\t', '\r', '\n');
        foreach (var prefix in ConcisePrefixes)
        {
            if (!value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) || value.Length <= prefix.Length)
                continue;

            value = value[prefix.Length..].Trim();
            break;
        }

        if (value.Length == 0)
            return ReferenceOnlyTitle(code);

        return char.ToUpperInvariant(value[0]) + value[1..];
    }

    public static string DisplayTopicTitle(string? topicName)
    {
        var value = (topicName ?? string.Empty).Trim();
        if (value.Length == 0)
            return string.Empty;

        var match = ReferenceFamilyTopicPattern.Match(value);
        if (!match.Success)
            return value;

        var stage = match.Groups["stage"].Value;
        var family = match.Groups["family"].Value;
        return $"Stage {stage} · {ReferenceFamilyArea(family)}";
    }

    public static bool IsReferenceOnlyDescription(string? description)
    {
        var value = (description ?? string.Empty).Trim();
        return value.StartsWith("Edulytics reference-only", StringComparison.OrdinalIgnoreCase)
            || value.Contains("Official copyrighted", StringComparison.OrdinalIgnoreCase);
    }

    private static string ReferenceOnlyTitle(string? code)
    {
        var displayCode = DisplayCode(code);
        var match = ReferenceCodePattern.Match(displayCode);
        if (!match.Success)
            return displayCode.Length == 0 ? "Mathematics" : displayCode;

        var area = ReferenceFamilyArea(match.Groups["family"].Value);
        var item = match.Groups["item"].Value;
        return item.Length == 0
            ? area
            : $"{area} · {item}";
    }

    private static string ReferenceFamilyArea(string family)
    {
        var normalized = family.Trim();
        if (normalized.Equals("Ae", StringComparison.OrdinalIgnoreCase))
            return "Algebraic expressions";
        if (normalized.Equals("Np", StringComparison.OrdinalIgnoreCase))
            return "Number and proportional reasoning";
        if (normalized.Equals("Gg", StringComparison.OrdinalIgnoreCase))
            return "Geometry";

        if (normalized.StartsWith('N'))
            return "Number";
        if (normalized.StartsWith('A'))
            return "Algebra";
        if (normalized.StartsWith('G'))
            return "Geometry";
        if (normalized.StartsWith('M'))
            return "Measurement";
        if (normalized.StartsWith('D') || normalized.StartsWith('S'))
            return "Data and statistics";
        if (normalized.StartsWith('P'))
            return "Probability";

        return "Mathematics";
    }
}
