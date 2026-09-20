using System.Text.RegularExpressions;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Learner-facing remediation for the 1,569 published Polish National
/// Mathematics lessons. The official OutcomeCode remains unchanged; the broad
/// Phase-29 fallback body is replaced by content scoped to the exact reviewed
/// OutcomeCode -> Practice target mapping.
/// </summary>
public static partial class PolishLessonPracticeContentCorrections
{
    public const string CorrectionContentVersion =
        "polish-outcome-practice-remediation-v1";

    public static bool IsTarget(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson)
    {
        if (!string.Equals(
                document.PackCode,
                MathematicsCurriculumPackRegistry.PolandCode,
                StringComparison.Ordinal) ||
            lesson.IsSupporting ||
            lesson.OutcomeCodes.Count != 1)
        {
            return false;
        }

        var outcomeCode = lesson.OutcomeCodes[0];
        return PolishOutcomePracticeMapRegistry.TryResolve(outcomeCode, out _) &&
               PolishOutcomeSourceEvidenceRegistry.TryResolve(outcomeCode, out _) &&
               lesson.Translations.Any(x =>
                   x.CultureCode.StartsWith("pl", StringComparison.OrdinalIgnoreCase));
    }

    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string priorExpectedVersion) =>
        IsTarget(document, lesson)
            ? CorrectionContentVersion
            : priorExpectedVersion;

    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingContentVersion) =>
        IsTarget(document, lesson) &&
        (string.Equals(
             existingContentVersion,
             document.ContentVersion,
             StringComparison.Ordinal) ||
         string.Equals(
             existingContentVersion,
             CorrectionContentVersion,
             StringComparison.Ordinal));

    public static void ApplyApprovedCorrections(
        CanonicalLessonContentPackDocument document)
    {
        foreach (var lesson in document.Lessons)
        {
            if (!IsTarget(document, lesson))
                continue;

            var outcomeCode = lesson.OutcomeCodes.Single();
            if (!PolishOutcomePracticeMapRegistry.TryResolve(
                    outcomeCode,
                    out var mapping) ||
                mapping is null ||
                !PolishOutcomeSourceEvidenceRegistry.TryResolve(
                    outcomeCode,
                    out var source) ||
                source is null)
            {
                throw new InvalidOperationException(
                    $"Polish remediation target lacks exact evidence/mapping: {lesson.LessonCode}.");
            }

            var polish = lesson.Translations.Single(x =>
                x.CultureCode.StartsWith("pl", StringComparison.OrdinalIgnoreCase));

            var officialTarget = CleanOfficialTarget(source.OfficialText);
            var shortTarget = BuildShortTarget(officialTarget);
            var rules = mapping.TargetRules
                .OrderBy(x => x.Id, StringComparer.Ordinal)
                .ToArray();

            if (rules.Length == 0)
            {
                throw new InvalidOperationException(
                    $"Polish remediation mapping has no target rules: {outcomeCode}.");
            }

            polish.Title = shortTarget;
            polish.Explanation =
                $"Cel lekcji jest przypisany bezpośrednio do wymagania {outcomeCode}. " +
                $"Uczeń ćwiczy dokładnie ten zakres: {officialTarget} " +
                "Treść Edulytics nie rozszerza wymagania poza zakres potwierdzony w oficjalnym źródle.";

            polish.KeyConceptsAndRules =
                $"Dokładny zakres: {officialTarget} " +
                "Practice korzysta wyłącznie z zatwierdzonych umiejętności: " +
                string.Join(
                    "; ",
                    rules.Select(rule =>
                        $"{Humanize(rule.Id)} [{rule.SkillId}]"));

            polish.WorkedExamples = string.Join(
                " ",
                rules.Select(
                    (rule, index) =>
                        $"Przykład {index + 1} — {Humanize(rule.Id)}: " +
                        rule.Content.WorkedExample));

            polish.StepByStepSolutions = string.Join(
                " ",
                rules.Select(
                    (rule, index) =>
                        $"Metoda {index + 1}: {rule.Content.Solution}"));

            polish.CommonMistakes = string.Join(
                " ",
                rules.Select(
                    rule =>
                        $"Uwaga — {Humanize(rule.Id)}: {rule.Content.CommonMistake}"));

            polish.QuickSummary =
                $"Po tej lekcji uczeń potrafi: {shortTarget.TrimEnd('.', ';')}.";

            lesson.AdaptationStatus =
                "Edulytics outcome-specific learner content reconstructed from pinned official " +
                "OutcomeCode evidence and the reviewed exact Practice target map.";
        }
    }

    internal static string CleanOfficialTarget(string raw)
    {
        var value = Whitespace().Replace(raw ?? string.Empty, " ").Trim();

        value = Regex.Replace(
            value,
            @"\s+(?:Warunki i sposób realizacji|III\. Edukacja społeczna).*?$",
            string.Empty,
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        value = Regex.Replace(
            value,
            @"\s+(?:[IVXLC]+|\d+)\.\s*$",
            string.Empty,
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        return value.Trim();
    }

    internal static string BuildShortTarget(string officialTarget)
    {
        var value = officialTarget.Trim();
        var split = value.IndexOf(';');
        if (split > 20)
            value = value[..split];

        value = Regex.Replace(
            value,
            @"\s+(?:[IVXLC]+|\d+)\.\s*$",
            string.Empty,
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)
            .Trim()
            .TrimEnd('.', ';');

        if (value.Length > 180)
            value = TruncateWithoutSplittingSurrogatePair(
                    value,
                    177)
                .TrimEnd() + "…";

        if (value.Length == 0)
            return "Cel matematyczny zgodny z wymaganiem programu";

        return char.ToUpperInvariant(value[0]) + value[1..];
    }

    private static string TruncateWithoutSplittingSurrogatePair(
        string value,
        int maxCodeUnits)
    {
        if (value.Length <= maxCodeUnits)
            return value;

        var length = maxCodeUnits;

        if (length > 0 &&
            length < value.Length &&
            char.IsHighSurrogate(value[length - 1]) &&
            char.IsLowSurrogate(value[length]))
        {
            length--;
        }

        return value[..length];
    }

    private static string Humanize(string ruleId) =>
        ruleId
            .Replace('-', ' ')
            .Replace("polish ", string.Empty, StringComparison.OrdinalIgnoreCase);

    [GeneratedRegex(@"\s+")]
    private static partial Regex Whitespace();
}
