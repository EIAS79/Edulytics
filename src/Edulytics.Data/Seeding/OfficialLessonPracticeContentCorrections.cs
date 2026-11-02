using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Approved learner-facing correction for the UAE Grade 9 Advanced L6 lesson
/// group. The official curriculum identity and OutcomeCodes remain unchanged;
/// only the Edulytics-authored learner body is strengthened so the published
/// lesson genuinely demonstrates both systems of equations and inequalities.
/// </summary>
public static class OfficialLessonPracticeContentCorrections
{
    public const string CorrectionContentVersion =
        "official-practice-alignment-v1";

    public static bool IsTarget(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        string.Equals(
            document.PackCode,
            MathematicsCurriculumPackRegistry.UaeCode,
            StringComparison.Ordinal) &&
        lesson.LessonCode.StartsWith(
            "PED:UAE:G9:ADV:T1:L6-",
            StringComparison.Ordinal) &&
        lesson.OutcomeCodes.Count > 0;

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

            var english = lesson.Translations.FirstOrDefault(x =>
                x.CultureCode.StartsWith(
                    "en",
                    StringComparison.OrdinalIgnoreCase));
            if (english is null)
                continue;

            if (!SupportingPracticeTargetRuleRegistry.TryResolveReviewedExactTitle(
                    lesson.LessonCode,
                    english.Title,
                    out var rule) ||
                rule is null ||
                !string.Equals(
                    rule.Id,
                    "uae-systems-inequalities",
                    StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    $"UAE L6 correction target has no reviewed exact Practice rule: {lesson.LessonCode}.");
            }

            english.Explanation =
                rule.Content.Concept + " " +
                "This learner-facing correction preserves the accepted UAE official OutcomeCodes and strengthens the Edulytics-authored worked mathematics.";
            english.KeyConceptsAndRules =
                rule.Content.Concept;
            english.WorkedExamples =
                "Worked example: " + rule.Content.WorkedExample;
            english.StepByStepSolutions =
                "Solution method: " + rule.Content.Solution;
            english.CommonMistakes =
                rule.Content.CommonMistake;
            english.QuickSummary =
                rule.Content.Summary;
        }
    }
}
