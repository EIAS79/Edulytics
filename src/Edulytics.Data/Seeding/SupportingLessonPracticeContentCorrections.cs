using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Versioned learner-facing remediation for Supporting lessons whose reviewed
/// target rule supplies stronger target-specific English pedagogy. Official
/// curriculum identities and OutcomeCodes are never changed.
/// </summary>
public static class SupportingLessonPracticeContentCorrections
{
    public const string CorrectionContentVersion =
        "supporting-practice-remediation-v1";

    public static bool IsTarget(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson)
    {
        if (lesson.OutcomeCodes.Count != 0 ||
            CambridgePrimaryStage6LessonContentCorrections.IsTarget(document, lesson))
        {
            return false;
        }

        var english = lesson.Translations.FirstOrDefault(x =>
            x.CultureCode.StartsWith("en", StringComparison.OrdinalIgnoreCase));
        if (english is null)
            return false;

        return SupportingPracticeTargetRuleRegistry.TryResolve(
            lesson.LessonCode,
            english.Title,
            out _);
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

            var english = lesson.Translations.Single(x =>
                x.CultureCode.StartsWith("en", StringComparison.OrdinalIgnoreCase));

            if (!SupportingPracticeTargetRuleRegistry.TryResolve(
                    lesson.LessonCode,
                    english.Title,
                    out var rule) ||
                rule is null)
            {
                continue;
            }

            english.Explanation =
                rule.Content.Concept + " " +
                "This Supporting lesson remains pedagogical content and does not create or imply an official curriculum OutcomeCode.";
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
