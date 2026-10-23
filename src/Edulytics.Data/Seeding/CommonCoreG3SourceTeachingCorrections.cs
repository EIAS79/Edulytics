using System.Text.Json;
using Edulytics.Core.Curriculum;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Authoring-stage 2021 source-skill corrections for exactly 143 Grade 3
/// lessons. These corrections do not certify the original figure geometry,
/// teacher-led activities, subquestion-by-subquestion parity, tests, or
/// deployment. The signed-off source comparison remains a separate gate.
/// </summary>
public static class CommonCoreG3SourceTeachingCorrections
{
    public const string PackCode = "US-CCSS-MATH";
    public const string BaseContentVersion = "phase30-common-core-rich-content-v1";
    public const string CorrectionContentVersion =
        "g3-2021-source-skill-rich-v2-authoring-v1";
    private const string Prefix = "PED:US-CCSS-MATH:G3:";
    private const string Resource = "Edulytics.Data.Grade3.SourceTeachingCorrections2021";
    private static readonly Lazy<IReadOnlyDictionary<string, Correction>> Records =
        new(Load, LazyThreadSafetyMode.ExecutionAndPublication);

    public static int CorrectionCount => Records.Value.Count;

    public static bool IsTarget(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        string.Equals(document.PackCode, PackCode, StringComparison.Ordinal) &&
        string.Equals(document.ContentVersion, BaseContentVersion, StringComparison.Ordinal) &&
        lesson.LessonCode.StartsWith(Prefix, StringComparison.Ordinal) &&
        Records.Value.TryGetValue(lesson.LessonCode, out var rule) &&
        string.Equals(lesson.CanonicalBodySha256, rule.BaseBodyHash, StringComparison.Ordinal) &&
        string.Equals(lesson.SourceEdition, "1st Edition (2021)", StringComparison.Ordinal);

    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string previous) =>
        IsTarget(document, lesson) ? CorrectionContentVersion : previous;

    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingContentVersion) =>
        IsTarget(document, lesson) &&
        string.Equals(existingContentVersion, BaseContentVersion, StringComparison.Ordinal);

    public static void Apply(CanonicalLessonContentPackDocument document)
    {
        if (!string.Equals(document.PackCode, PackCode, StringComparison.Ordinal) ||
            !string.Equals(document.ContentVersion, BaseContentVersion, StringComparison.Ordinal))
            return;

        var grade3 = document.Lessons.Where(
            lesson => lesson.LessonCode.StartsWith(Prefix, StringComparison.Ordinal)).ToArray();
        if (grade3.Length != 143)
            throw new InvalidOperationException(
                $"Expected the full Grade 3 2021 set of 143 lessons, got {grade3.Length}.");

        foreach (var lesson in grade3)
        {
            if (!IsTarget(document, lesson))
                throw new InvalidOperationException(
                    $"Grade 3 source hash, edition or lesson identity changed: {lesson.LessonCode}. " +
                    "Re-review the source before authoring any replacement body.");

            var rule = Records.Value[lesson.LessonCode];
            var en = lesson.Translations.SingleOrDefault(
                t => string.Equals(t.CultureCode, "en", StringComparison.Ordinal))
                ?? throw new InvalidOperationException(
                    $"Missing canonical English Grade 3 translation: {lesson.LessonCode}.");

            en.KeyConceptsAndRules =
                "Mathematical learning focus: " + rule.LearningGoal +
                "\nReview the original 2021 learning goals and class activity context before " +
                "drawing conclusions from a graph, diagram, equation or measurement." +
                "\nActivity families requiring teacher evidence: " + rule.ActivityNames;

            en.StepByStepSolutions = string.Join("\n",
                rule.TeachingSteps.Select((step, i) => $"Step {i + 1}: {step}"));

            en.CommonMistakes =
                "Check this lesson-specific risk: " + rule.MisconceptionCaution +
                "\nA correct numeric value never certifies a source diagram, physical " +
                "manipulative, peer task or mathematical explanation by itself.";

            en.QuickSummary =
                "Learning goal: " + rule.LearningGoal +
                "\nApproach: " + rule.TeachingSteps[0] +
                " Show the mathematical representation and explain the meaning.";

            // Retain previously authored Explanation/WorkedExamples pending
            // exact item-by-item source verification and provenance review.
            // No restricted publisher Cool-down prompt or art is embedded.
        }
    }

    private static IReadOnlyDictionary<string, Correction> Load()
    {
        using var stream = typeof(CommonCoreG3SourceTeachingCorrections)
            .Assembly.GetManifestResourceStream(Resource)
            ?? throw new InvalidOperationException("Embedded Grade 3 source teaching resource not found.");
        using var json = JsonDocument.Parse(stream);
        var root = json.RootElement;
        if (root.GetProperty("targetLessonCount").GetInt32() != 143 ||
            root.GetProperty("baseContentVersion").GetString() != BaseContentVersion ||
            root.GetProperty("proposedEffectiveContentVersion").GetString() != CorrectionContentVersion)
            throw new InvalidOperationException("Unexpected Grade 3 authoring resource baseline or version.");

        var map = new Dictionary<string, Correction>(StringComparer.Ordinal);
        foreach (var item in root.GetProperty("lessons").EnumerateArray())
        {
            var code = item.GetProperty("lessonCode").GetString() ?? "";
            var hash = item.GetProperty("baselineCanonicalBodySha256").GetString() ?? "";
            var goal = item.GetProperty("sourceGoal").GetString()?.Trim() ?? "";
            var activities = item.GetProperty("originalActivityNames").GetString()?.Trim() ?? "";
            var caution = item.GetProperty("studentMisconceptionCaution").GetString()?.Trim() ?? "";
            var steps = item.GetProperty("teachingSteps").EnumerateArray()
                .Select(s => s.GetString()?.Trim() ?? "").ToArray();
            if (!code.StartsWith(Prefix, StringComparison.Ordinal) || hash.Length != 64 ||
                goal.Length < 15 || caution.Length < 15 || steps.Length < 2 ||
                steps.Any(string.IsNullOrWhiteSpace) || !map.TryAdd(code,
                    new Correction(hash, goal, activities, caution, steps)))
                throw new InvalidOperationException("Invalid or duplicate Grade 3 teaching rule: " + code);
        }

        if (map.Count != 143)
            throw new InvalidOperationException($"Expected 143 distinct Grade 3 rules; got {map.Count}.");
        return map;
    }

    private sealed record Correction(
        string BaseBodyHash,
        string LearningGoal,
        string ActivityNames,
        string MisconceptionCaution,
        IReadOnlyList<string> TeachingSteps);
}
