using System.Text.RegularExpressions;

namespace Edulytics.Web.Presentation;

/// <summary>
/// Removes editorial/source-governance wording from learner-facing lesson copy
/// while preserving the canonical content and staff-facing provenance data.
/// </summary>
public static partial class StudentLessonPresentationFilter
{
    [GeneratedRegex(
        @"\bThis\s+Cambridge\s+Primary\s+Stage\s+\d+\s+supporting\s+lesson\s+develops\b",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex CambridgeSupportingIntroRegex();

    [GeneratedRegex(
        @"\s*This\s+Supporting\s+lesson\s+remains\s+pedagogical\s+content\s+and\s+does\s+not\s+create\s+or\s+imply\s+an\s+official\s+curriculum\s+OutcomeCode\.?",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex SupportingOutcomeDisclaimerRegex();

    [GeneratedRegex(
        @"\s*The\s+lesson\s+is\s+Edulytics-authored\s+from\s+OGL\s+material;\s*Cambridge\s+remains\s+the\s+academic\s+reference\s+authority\s+and\s+no\s+Cambridge\s+objective\s+wording\s+is\s+reproduced\s+here\.?",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex CambridgeProvenanceDisclaimerRegex();

    public static string Clean(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var cleaned = CambridgeSupportingIntroRegex().Replace(
            value,
            "This lesson develops");

        cleaned = SupportingOutcomeDisclaimerRegex().Replace(cleaned, " ");
        cleaned = CambridgeProvenanceDisclaimerRegex().Replace(cleaned, " ");

        return Regex.Replace(
                cleaned,
                @"\s{2,}",
                " ",
                RegexOptions.CultureInvariant)
            .Trim();
    }
}
