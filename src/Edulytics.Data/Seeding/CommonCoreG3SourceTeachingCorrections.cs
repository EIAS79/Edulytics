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
        // Common Core grade packs share one programme/version identifier.
        // Documents without Grade 3 must be left completely untouched.
        if (grade3.Length == 0) return;
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

            if (string.Equals(lesson.LessonCode,
                "PED:US-CCSS-MATH:G3:U01:L01", StringComparison.Ordinal))
            {
                // U01 L01 is the first sequential A-B-C lesson. The baseline
                // generic examples do not teach the 2021 reveal sequence.
                // Authored from the public lesson/preparation and independent
                // Edulytics student companion; NOT an original-art reproduction.
                en.Explanation =
                    "We are learning to read picture and bar graphs and to ask " +
                    "questions that the data can answer. A picture-graph key tells " +
                    "how many people each symbol represents; a bar graph uses a " +
                    "numbered axis. First, study six upright picture columns " +
                    "without a title or category names. The numbers of symbols " +
                    "from left to right are 4, 3, 1, 2, 7 and 6, with one symbol " +
                    "representing one learner. We cannot know what the columns " +
                    "represent from their heights alone. Next, reveal the title " +
                    "'How We Get Home'; it narrows the possible context but still " +
                    "does not identify the six categories. Finally, read the " +
                    "fully labelled picture graph together with a bar graph " +
                    "showing the SAME data. Walk 4, Bike 3, Skateboard 1, Car 2, " +
                    "Bus 7, Train 6; the bar axis runs from 0 to 8 in steps of 1. " +
                    "Compare the displays and propose two different questions " +
                    "that can actually be answered. Discuss the reasons with a " +
                    "partner. The discussion and student-authored questions need " +
                    "teacher review and are not auto-graded.";

                en.WorkedExamples =
                    "Example 1 - Notice and wonder: a six-column picture graph " +
                    "has 4, 3, 1, 2, 7, 6 symbols, but no title or labels. " +
                    "Describe the visible counts without guessing a proven " +
                    "category name.\n" +
                    "Example 2 - Picture time: add only the title 'How We Get " +
                    "Home'. Suggest possible ways of getting home, explaining " +
                    "why they are hypotheses and not labelled facts.\n" +
                    "Example 3 - Compare representations: the categories are " +
                    "Walk 4, Bike 3, Skateboard 1, Car 2, Bus 7, Train 6. " +
                    "On a picture graph one symbol means one learner; on the " +
                    "bar graph use a vertical 0-to-8 axis in steps of 1. " +
                    "Explain what is the same and different, then write two " +
                    "answerable questions. A teacher checks the reasoning.";

                en.QuickSummary =
                    "The same six category counts can be shown as a picture " +
                    "graph or a vertical bar graph. First examine the six " +
                    "unlabelled columns, then the titled but unlabelled graph, " +
                    "and only then the fully labelled pair. A title provides " +
                    "context, a picture key defines one symbol, and the bar " +
                    "axis shows the numerical scale. Good graph questions " +
                    "must be answerable from the shown data; oral reasoning, " +
                    "partner work and invented questions require teacher review.";
            }
            else
            {
                // Keep other lessons untouched pending exact source review:
                // generic base explanations must not be certified as parity.
            }

            // Never embed restricted publisher Cool-down prompts or artwork.
            // No database migration, publication or A-C approval occurs here.
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
                goal.Contains("Required Preparation", StringComparison.OrdinalIgnoreCase) ||
                goal.Contains("Lesson Timeline", StringComparison.OrdinalIgnoreCase) ||
                goal.Contains("Teacher Reflection", StringComparison.OrdinalIgnoreCase) ||
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
