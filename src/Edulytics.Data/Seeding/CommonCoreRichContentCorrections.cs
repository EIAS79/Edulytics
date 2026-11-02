using Edulytics.Core.Curriculum;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Approved version boundary for the source-grounded Common Core learner-content
/// rebuild. Curriculum identities, official OutcomeCodes, lesson codes and
/// practice mappings are preserved; only reviewed learner-facing bodies advance.
/// </summary>
public static class CommonCoreRichContentCorrections
{
    public const string CorrectionContentVersion =
        "common-core-rich-content-v1";

    private const string PriorBaseContentVersion =
        "phase29-source-faithful-en-final-v1";

    private const string PriorSupportingContentVersionV1 =
        "supporting-practice-remediation-v1";

    private const string PriorSupportingContentVersionV2 =
        "supporting-practice-remediation-v2";

    public static bool IsTarget(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        string.Equals(
            document.PackCode,
            MathematicsCurriculumPackRegistry.CommonCoreCode,
            StringComparison.Ordinal);

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
        (
            string.Equals(
                existingContentVersion,
                document.ContentVersion,
                StringComparison.Ordinal) ||
            string.Equals(
                existingContentVersion,
                PriorBaseContentVersion,
                StringComparison.Ordinal) ||
            string.Equals(
                existingContentVersion,
                PriorSupportingContentVersionV1,
                StringComparison.Ordinal) ||
            string.Equals(
                existingContentVersion,
                PriorSupportingContentVersionV2,
                StringComparison.Ordinal) ||
            string.Equals(
                existingContentVersion,
                CorrectionContentVersion,
                StringComparison.Ordinal)
        );
}