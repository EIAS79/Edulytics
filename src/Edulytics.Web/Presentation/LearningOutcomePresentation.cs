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
            return DisplayCode(code);

        value = value.TrimEnd('.', ' ', '\t', '\r', '\n');
        foreach (var prefix in ConcisePrefixes)
        {
            if (!value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) || value.Length <= prefix.Length)
                continue;

            value = value[prefix.Length..].Trim();
            break;
        }

        if (value.Length == 0)
            return DisplayCode(code);

        return char.ToUpperInvariant(value[0]) + value[1..];
    }

    public static bool IsReferenceOnlyDescription(string? description)
    {
        var value = (description ?? string.Empty).Trim();
        return value.StartsWith("Edulytics reference-only", StringComparison.OrdinalIgnoreCase)
            || value.Contains("Official copyrighted", StringComparison.OrdinalIgnoreCase);
    }
}
