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

            // Only after the original edition and canonical SHA gates above:
            // replace the generic U01 L15 learner body with its individually
            // source-reviewed 2021 first-party teaching, examples and methods.
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit1Lesson15RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit1Lesson15RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit1Lesson16RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit1Lesson16RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit1Lesson17RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit1Lesson17RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit1Lesson18RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit1Lesson18RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit1Lesson19RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit1Lesson19RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit1Lesson20RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit1Lesson20RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit1Lesson21RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit1Lesson21RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit2Lesson1RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit2Lesson1RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit2Lesson2RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit2Lesson2RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit2Lesson3RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit2Lesson3RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit2Lesson4RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit2Lesson4RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit2Lesson5RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit2Lesson5RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit2Lesson6RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit2Lesson6RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit2Lesson7RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit2Lesson7RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit2Lesson8RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit2Lesson8RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit2Lesson9RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit2Lesson9RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit2Lesson10RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit2Lesson10RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit2Lesson11RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit2Lesson11RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit2Lesson12RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit2Lesson12RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit2Lesson13RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit2Lesson13RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit2Lesson14RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit2Lesson14RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit2Lesson15RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit2Lesson15RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit3Lesson1RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit3Lesson1RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit3Lesson2RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit3Lesson2RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit3Lesson3RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit3Lesson3RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit3Lesson4RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit3Lesson4RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit3Lesson5RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit3Lesson5RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit3Lesson6RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit3Lesson6RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit3Lesson7RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit3Lesson7RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode,
                CommonCoreG3Unit3Lesson8RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit3Lesson8RichV2.Apply(lesson);
            // Source-indexed candidate only: ordinals 45–143 never imply full original-source parity.
            CommonCoreG3RemainingSourceIntakeRichV2.Apply(lesson);
            // Override generic source-intake draft only for sequentially closed 2021 U03:L09.
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit3Lesson9RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit3Lesson9RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit3Lesson10RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit3Lesson10RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit3Lesson11RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit3Lesson11RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit3Lesson12RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit3Lesson12RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit3Lesson13RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit3Lesson13RichV2.Apply(lesson);
            // First-party source-text draft for ordinal 50; original PDF/figure proof still open.
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit3Lesson14RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit3Lesson14RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit3Lesson15RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit3Lesson15RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit3Lesson16RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit3Lesson16RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit3Lesson17RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit3Lesson17RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit3Lesson18RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit3Lesson18RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit3Lesson19RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit3Lesson19RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit3Lesson20RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit3Lesson20RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit3Lesson21RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit3Lesson21RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit4Lesson1RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit4Lesson1RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit4Lesson2RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit4Lesson2RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit4Lesson3RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit4Lesson3RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit4Lesson4RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit4Lesson4RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit4Lesson5RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit4Lesson5RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit4Lesson6RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit4Lesson6RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit4Lesson7RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit4Lesson7RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit4Lesson8RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit4Lesson8RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit4Lesson9RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit4Lesson9RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit4Lesson10RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit4Lesson10RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit4Lesson11RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit4Lesson11RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit4Lesson12RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit4Lesson12RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit4Lesson13RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit4Lesson13RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit4Lesson14RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit4Lesson14RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit4Lesson15RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit4Lesson15RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit4Lesson16RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit4Lesson16RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit4Lesson17RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit4Lesson17RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit4Lesson18RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit4Lesson18RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit4Lesson19RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit4Lesson19RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit4Lesson20RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit4Lesson20RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit4Lesson21RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit4Lesson21RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit4Lesson22RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit4Lesson22RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit5Lesson1RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit5Lesson1RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit5Lesson2RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit5Lesson2RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit5Lesson3RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit5Lesson3RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit5Lesson4RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit5Lesson4RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit5Lesson5RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit5Lesson5RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit5Lesson6RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit5Lesson6RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit5Lesson7RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit5Lesson7RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit5Lesson8RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit5Lesson8RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit5Lesson9RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit5Lesson9RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit5Lesson10RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit5Lesson10RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit5Lesson11RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit5Lesson11RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit5Lesson12RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit5Lesson12RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit5Lesson13RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit5Lesson13RichV2.Apply(lesson);
            if (string.Equals(lesson.LessonCode, CommonCoreG3Unit5Lesson14RichV2.LessonCode, StringComparison.Ordinal))
                CommonCoreG3Unit5Lesson14RichV2.Apply(lesson);

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

                en.KeyConceptsAndRules =
                    "Lesson 3.1.1 learning target: read a categorical " +
                    "picture graph and a bar graph, and ask questions that " +
                    "the graphs can answer. The picture graph's key says " +
                    "one symbol means one student; the bar graph uses a " +
                    "vertical number axis from 0 through 8 in ones. " +
                    "Do not give anonymous columns invented category " +
                    "labels as though they are known. A title provides " +
                    "context, not all missing labels. Once categories " +
                    "are revealed, both graphs must preserve Walk 4, " +
                    "Bike 3, Skateboard 1, Car 2, Bus 7 and Train 6 " +
                    "(23 students in all). Discuss two learner-authored " +
                    "questions, and verify that the displayed data " +
                    "actually answer them. Teacher-led explanation, " +
                    "Math Community norms and graph comparison are " +
                    "not digitally auto-graded.";

                en.StepByStepSolutions =
                    "Step 1 (15-minute Notice and Wonder): look at " +
                    "the 4, 3, 1, 2, 7, 6 anonymous picture columns. " +
                    "Record an observation and a question without " +
                    "guessing a fixed category label. Think quietly, " +
                    "then discuss with a partner.\n" +
                    "Step 2 (10-minute Picture Time): show the " +
                    "title How We Get Home, retain the six values " +
                    "and one-symbol key, and suggest possible " +
                    "categories with reasons. Explain why the " +
                    "labels cannot be determined from the title.\n" +
                    "Step 3 (20-minute paired graph analysis): " +
                    "reveal Walk 4, Bike 3, Skateboard 1, Car 2, " +
                    "Bus 7, Train 6 in the picture graph and in " +
                    "a bar graph with y-axis 0..8 in ones. Check " +
                    "every bar/column against its category and key.\n" +
                    "Step 4: describe how the picture and bar " +
                    "graphs are alike and different, then say " +
                    "what the graphs tell us. Write two distinct " +
                    "questions whose answers can be read from " +
                    "these graphs. Teacher reviews explanations.\n" +
                    "Step 5 (10-minute synthesis): explain the " +
                    "role of labels, title, picture key, and bar " +
                    "scale. Invite learners to restate a peer's " +
                    "reasoning and discuss Doing Math / Norms.\n" +
                    "Step 6 (5-minute independent Edulytics " +
                    "Cool-down): read a NEW graph and respond. " +
                    "Numeric responses can be checked; invented " +
                    "questions and explanations require review. " +
                    "This is not the restricted publisher item.";

                en.CommonMistakes =
                    "Mistake: interpreting the unnamed first " +
                    "and second graphs as if their category " +
                    "names were supplied. Repair: distinguish " +
                    "a plausible guess from a shown label.\n" +
                    "Mistake: treating one picture as more " +
                    "than one learner or misreading the bar " +
                    "scale. Repair: verify one symbol equals " +
                    "one student and the axis advances by " +
                    "exactly one, starting at zero.\n" +
                    "Mistake: confusing columns or comparing " +
                    "different categories. Repair: match Walk " +
                    "4, Bike 3, Skateboard 1, Car 2, Bus 7 " +
                    "and Train 6 in BOTH representations.\n" +
                    "Mistake: offering a question that the " +
                    "graph cannot answer or treating partner " +
                    "reasoning as a numeric grade. Repair: " +
                    "require source-visible evidence and a " +
                    "teacher rubric; open questions are not " +
                    "automatic passes.";

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
            else if (string.Equals(lesson.LessonCode,
                "PED:US-CCSS-MATH:G3:U01:L02", StringComparison.Ordinal))
            {
                // Preserve real classroom survey provenance: NO fixed L01
                // travel graph or invented class-wide answers. First-party
                // wording follows 2021 public teacher lesson/preparation.
                en.Explanation =
                    "Our goal is to represent real class-collected categorical " +
                    "data using a picture graph and a bar graph, then solve " +
                    "one- and two-step more/fewer questions within 20. Start " +
                    "with 3 brief dot images (6, 8 and 10 dots) and compare " +
                    "the second and third arrangements: 8 + 2 = 10. Next, " +
                    "every learner places one response in a teacher-prepared " +
                    "class survey about how they travel home. The teacher " +
                    "may change the category labels to fit the pupils. " +
                    "Give the picture graph a title and key, then draw a " +
                    "vertical bar graph of the SAME collected dataset, " +
                    "with a labelled number axis beginning at zero. " +
                    "Use the recorded category values, not fixed answers " +
                    "from another class. Evaluate four true/false statements " +
                    "against the actual graph; the teacher chooses labels " +
                    "for one-step and two-step comparison questions. " +
                    "Students must explain equations and conclusions to " +
                    "partners; teacher reviews the collection, drawing, " +
                    "justification and Math Community participation.";

                en.KeyConceptsAndRules =
                    "A picture graph and a bar graph must show the SAME " +
                    "survey counts. Each pupil contributes exactly one " +
                    "real response; one picture stands for one learner. " +
                    "The bar height equals the category count on a " +
                    "common number axis. First learn the numbers from " +
                    "the real class; only then ask 'how many more/fewer'. " +
                    "For an arithmetic task that combines two categories, " +
                    "add those two counts and compare with a third. " +
                    "Use addition/subtraction within 20 for automatically " +
                    "graded arithmetic; a larger class size does not " +
                    "prove any particular numerical solution. " +
                    "Categories and one/two-step question choices are " +
                    "teacher-set and need not have fictional source values.";

                en.WorkedExamples =
                    "Warm-up grouping: 8 dots become 10 with 2 more, " +
                    "so 8 + 2 = 10. Describe a grouping strategy rather " +
                    "than simply naming the result.\n" +
                    "Graph construction: place one marked note for " +
                    "each real pupil in one teacher-selected travel " +
                    "category; give the picture graph a title and " +
                    "one-response key. Count each category; transfer " +
                    "those SAME counts to a bar graph with a consistent " +
                    "scale. The values depend on the actual class.\n" +
                    "One-step comparison example: if the class counts " +
                    "for two teacher-selected categories are a and b, " +
                    "compute the difference between them and explain " +
                    "which category is greater. Only generate an " +
                    "auto-gradable item when the actual within-20 " +
                    "arithmetic scope has been checked.\n" +
                    "Two-step comparison example: if two distinct " +
                    "teacher-selected categories contain a and b " +
                    "learners and a third contains c, compare (a+b) " +
                    "with c. Use the classroom counts; do not supply " +
                    "made-up values as if they were collected.";

                en.StepByStepSolutions =
                    "Step 1 (10-minute dot warm-up): notice dot " +
                    "groups and explain 8+2=10 orally with a peer.\n" +
                    "Step 2 (15-minute class-survey activity): " +
                    "collect actual sticky-note responses, choose " +
                    "contextually accurate labels, then construct " +
                    "a titled picture graph with a one-pupil key.\n" +
                    "Step 3: transfer exactly the same observed " +
                    "class values to a bar graph with a numbered " +
                    "vertical axis and category labels. Have " +
                    "students justify equivalent representations.\n" +
                    "Step 4 (20-minute graph questions): decide " +
                    "the four source-style true/false comparison " +
                    "claims using the real categories, then let the " +
                    "teacher fill the category blanks for a one-step " +
                    "and a two-step how-many-more question.\n" +
                    "Step 5 (10-minute synthesis): share a true " +
                    "equation, explain bar-versus-picture scale " +
                    "and revise the Math Community norms.\n" +
                    "Step 6 (5-minute independent Edulytics " +
                    "Cool-down): solve an original separate " +
                    "graph-reading problem. Hand-drawn graphs " +
                    "and explanations require teacher review.";

                en.CommonMistakes =
                    "Mistake: using the fixed data from Lesson 1 " +
                    "or another fictional survey in place of " +
                    "actual class responses. Repair: collect " +
                    "and verify the present class counts.\n" +
                    "Mistake: using different numbers in the " +
                    "picture graph and bar graph, or omitting " +
                    "the title, key, or axis. Repair: verify " +
                    "each count before comparing.\n" +
                    "Mistake: grading a class-dependent true/false " +
                    "statement against hard-coded correct/incorrect " +
                    "answers. Repair: calculate each statement " +
                    "from a validated supplied dataset.\n" +
                    "Mistake: grading oral reasoning, graph " +
                    "drawing or invented questions as a binary " +
                    "numeric pass. Repair: use teacher review; " +
                    "reject out-of-scope numeric items rather " +
                    "than guessing an answer.";

                en.QuickSummary =
                    "Read the real data from your class, draw " +
                    "matching picture and bar graphs with a title, " +
                    "one-pupil key and common axis, and use those " +
                    "same data to solve one- and two-step " +
                    "more/fewer problems within 20. Discuss " +
                    "your equations and improve the community " +
                    "discussion norms. The independently authored " +
                    "Edulytics Cool-down is not the publisher item.";
            }
            else if (string.Equals(lesson.LessonCode,
                "PED:US-CCSS-MATH:G3:U01:L03", StringComparison.Ordinal))
            {
                // The official 2021 unit lesson index and canonical catalogue
                // both use "Scaled Picture Graphs PLC Activity", while the
                // individual lesson page heading abbreviates to
                // "Scaled Picture Graphs". Keep the canonical indexed title;
                // do NOT silently rename an officially indexed lesson.
                en.Explanation =
                    "A scaled picture graph uses a key such as one symbol " +
                    "for 2 or for 5 objects, rather than one object. " +
                    "Begin with the mental calculations 50+10, 50+12, " +
                    "60+13, and 65+13 to revisit addition and tens. " +
                    "Compare two Favorite Sports picture graphs with " +
                    "EXACTLY the same survey: Football 25, Soccer 15, " +
                    "Basketball 20, Tennis 5, or 65 students in all. " +
                    "The first graph has one face per student; the " +
                    "second has one face per five students (5, 3, 4 " +
                    "and 1 symbols respectively). Next read scaled " +
                    "flower and book graphs with DIFFERENT keys. " +
                    "Flowers: each picture means 5 flowers, with " +
                    "Roses 2 symbols, Tulips 1, Daisies 5, Violets 3. " +
                    "Books: each picture means 2 students, with " +
                    "Joke 4, Comic 1, Science 4 and Mystery 3 symbols. " +
                    "Explain totals, check what the graph can actually " +
                    "answer and create two original questions about " +
                    "each graph. Teachers evaluate question design, " +
                    "mental strategies and group discussion.";

                en.KeyConceptsAndRules =
                    "Multiply symbol count by the displayed picture " +
                    "key before reporting category quantities. Sports " +
                    "have 65 respondents whether drawn using 65 unit " +
                    "pictures or 13 pictures with a key of five. " +
                    "Flowers: 10 roses, 5 tulips, 25 daisies and 15 " +
                    "violets; total 55 flowers. Books: 8 joke-book, " +
                    "2 comic-book, 8 science-book and 6 mystery-book " +
                    "respondents; total 24. Graph questions must " +
                    "be answerable from the shown category data. " +
                    "Do not use the flowers' key of 5 to read the " +
                    "books' key of 2, or count pictures as people.";

                en.WorkedExamples =
                    "Mental warm-up: 50+10=60; 50+12=62; " +
                    "60+13=73; 65+13=78. Explain the tens/ones " +
                    "strategy orally.\n" +
                    "Sports comparison: 25+15+20+5=65 " +
                    "learners. For key 5, the symbols are " +
                    "5+3+4+1=13 and 13x5=65; the two graphs " +
                    "must represent identical data.\n" +
                    "Flowers graph: 2 roses-symbols x5=10 " +
                    "roses; 1 tulips-symbol x5=5; 5 daisies-" +
                    "symbols x5=25; 3 violets-symbols x5=15. " +
                    "The four categories total 55 flowers.\n" +
                    "Books graph: 4 joke-symbols x2=8; " +
                    "1 comic-symbol x2=2; 4 science-symbols " +
                    "x2=8; 3 mystery-symbols x2=6, total " +
                    "24 learners. Learners author two " +
                    "answerable questions for EACH of the " +
                    "flowers and books displays.";

                en.StepByStepSolutions =
                    "Step 1 (10-minute Number Talk): answer " +
                    "50+10, 50+12, 60+13 and 65+13 mentally; " +
                    "share a tens-and-ones explanation.\n" +
                    "Step 2 (15-minute So Many Responses): " +
                    "count 25,15,20,5 in a key-one sports " +
                    "graph, then use groups of five to " +
                    "interpret its equivalent key-five graph.\n" +
                    "Step 3 (20-minute scaled questions): " +
                    "count roses/tulips/daisies/violets using " +
                    "key 5, then jokes/comics/science/mysteries " +
                    "using key 2. Record correct values " +
                    "for all four categories in each graph.\n" +
                    "Step 4: write two distinct questions " +
                    "about the flowers and two about the " +
                    "books that the data can answer. Compare " +
                    "interpretations with a partner; teacher " +
                    "reviews the question quality.\n" +
                    "Step 5 (10-minute synthesis): explain " +
                    "why a key above one compresses a larger " +
                    "dataset and what an answerable graph " +
                    "question requires. Reflect on a helpful " +
                    "Doing Math class norm.\n" +
                    "Step 6 (5-minute independent Edulytics " +
                    "Cool-down): answer a new source-skill " +
                    "equivalent graph, not the restricted " +
                    "publisher Birds in the Park assessment.";

                en.CommonMistakes =
                    "Mistake: reading one picture as one " +
                    "flower/student when its key is 5 or 2. " +
                    "Repair: multiply the number of pictures " +
                    "by the stated key for EACH graph.\n" +
                    "Mistake: concluding the sports two " +
                    "graphs show different class sizes " +
                    "because one has fewer drawings. " +
                    "Repair: compare category quantities: " +
                    "65 students in either representation.\n" +
                    "Mistake: assigning a flowers key of " +
                    "five to the books graph whose key is " +
                    "two. Repair: read every separate key.\n" +
                    "Mistake: automatically accepting a " +
                    "question with unavailable data, or " +
                    "auto-grading oral reasoning. Repair: " +
                    "ask which specific graph values support " +
                    "the question and use teacher review.";

                en.QuickSummary =
                    "The same sports data may use 65 one-unit " +
                    "pictures or 13 pictures with key five. " +
                    "Scale-five flower and scale-two book " +
                    "graphs need different multiplication " +
                    "and each answerable question needs " +
                    "a shown data value. Open questions, " +
                    "paired reasoning and community norms " +
                    "need teacher evaluation; no publisher " +
                    "assessment prompt is copied.";
            }
            else if (string.Equals(lesson.LessonCode,
                "PED:US-CCSS-MATH:G3:U01:L04", StringComparison.Ordinal))
            {
                // Numbers must come from the real classroom preference survey.
                // An odd quantity requires a half-symbol when the key is two.
                en.Explanation =
                    "Create a scaled picture graph from a real class survey " +
                    "about how classmates WOULD LIKE to travel (not their " +
                    "actual journey home). The official 2021 response " +
                    "choices are Car, Train, Boat, Balloon, Plane, " +
                    "and Helicopter. Collect one response per pupil, " +
                    "record and count every category, then represent " +
                    "THOSE observed totals with a student-chosen symbol. " +
                    "A whole symbol represents two people and a half " +
                    "symbol represents one. An odd count needs a half " +
                    "mark, not a rounded whole. Use a meaningful " +
                    "title, a key and exact category labels. " +
                    "Compare student-created graphs and discuss " +
                    "class Math Community norms. Teachers review " +
                    "real data provenance, drawing and explanations.";

                en.KeyConceptsAndRules =
                    "With a key of two students per full symbol, " +
                    "an even category count n requires n/2 " +
                    "whole pictures. An odd n requires (n-1)/2 " +
                    "whole pictures and ONE half picture. " +
                    "For example five learners need two whole " +
                    "and one half pictures; this is a hypothetical " +
                    "number, not a claim about the actual class. " +
                    "Adding two to any count adds one whole symbol. " +
                    "Adding one to an EVEN count adds a half; " +
                    "adding one to an ODD count turns an " +
                    "existing half into a whole symbol.";

                en.WorkedExamples =
                    "Dot warm-up: identify groups of five and " +
                    "explain how skip-counting finds a total.\n" +
                    "Actual classroom survey: record preferred " +
                    "Car/Train/Boat/Balloon/Plane/Helicopter " +
                    "choices. Each collected preference is one " +
                    "pupil; use actual tally totals when drawing.\n" +
                    "For a hypothetical count of 5 students " +
                    "at key two, draw 2 whole and 1 half mark.\n" +
                    "Original source synthesis: when TWO more " +
                    "pupils prefer Balloon, ADD one whole mark. " +
                    "When ONE more prefers Car, add a half " +
                    "mark if its old count is even or complete " +
                    "the existing half if its old count is odd.";

                en.StepByStepSolutions =
                    "Step 1 (10-minute warm-up): group dots " +
                    "by fives; describe and compare strategies.\n" +
                    "Step 2 (15-minute class survey): pupils " +
                    "choose preferred means of travel; collect " +
                    "individual responses using the original " +
                    "six category names and compute the tallies.\n" +
                    "Step 3 (20-minute picture graph): choose " +
                    "a simple symbol and a key of two students; " +
                    "draw whole and half symbols accurately, " +
                    "including for odd-number categories.\n" +
                    "Step 4: compare your labelled, titled " +
                    "graph with a partner, checking the counts.\n" +
                    "Step 5 (10-minute synthesis): reason about " +
                    "Balloon plus two and Car plus one; " +
                    "revisit Math Community norms.\n" +
                    "Step 6 (5-minute independent Edulytics " +
                    "Cool-down): answer new scale-two questions; " +
                    "teacher reviews open drawings/explanations.";

                en.CommonMistakes =
                    "An odd pupil count cannot be rounded " +
                    "to whole pictures; half of the two-unit " +
                    "symbol represents the final student.\n" +
                    "When one new pupil joins an ODD category, " +
                    "complete its existing half mark instead " +
                    "of always adding another unpaired half.\n" +
                    "Do not recycle the fixed L01 graph or " +
                    "the L02 class travel-home survey, because " +
                    "the L04 question asks how pupils WOULD " +
                    "LIKE to travel. Only teacher-attested " +
                    "classroom responses can establish the " +
                    "actual values. Pair work and graph " +
                    "drawing are not machine-certified.";

                en.QuickSummary =
                    "The original survey uses six preferred " +
                    "travel categories and a scale of two. " +
                    "A whole mark means two students, a " +
                    "half means one. Balloon plus two adds " +
                    "one full mark; Car plus one depends " +
                    "on whether Car already has a half mark. " +
                    "Teachers validate the real data and " +
                    "students' independent graph drawings.";
            }
            else if (string.Equals(lesson.LessonCode,
                "PED:US-CCSS-MATH:G3:U01:L05", StringComparison.Ordinal))
            {
                // Students must reuse their REAL L04 travel-preference data,
                // not the fixed teacher comparison dataset or invented class
                // answers. Original teacher graph scales 1 and 2 differ.
                en.Explanation =
                    "We learn to draw scaled bar graphs and explain " +
                    "a choice of scale. Mental warm-up: four twos " +
                    "make 8 and eight twos make 16; four fives " +
                    "make 20 and eight fives make 40. Compare " +
                    "two provided bar graphs of the SAME travel-" +
                    "home data: Bike 12, Walk 4, Bus or Van 6, " +
                    "Car 3. One graph has a vertical axis " +
                    "0..12 by ones; the second axis goes 0..24 " +
                    "by twos. The data do not change when the " +
                    "axis changes. Next use your actual class " +
                    "travel-PREFERENCE survey graph from L04 " +
                    "to draw a NEW scaled bar graph; choose " +
                    "increments of 2 or 5 and explain why. " +
                    "The chart must show precise class counts " +
                    "even if a bar ends between the numbered " +
                    "ticks. Partners discuss and teachers check " +
                    "the L04 survey provenance and actual " +
                    "pupil-created graphs.";

                en.KeyConceptsAndRules =
                    "The axis scale is the difference between " +
                    "adjacent labelled ticks, not a multiplier " +
                    "to apply again to a bar's data count. " +
                    "Both provided source graphs have the " +
                    "same Bike 12, Walk 4, Bus/Van 6, Car 3 " +
                    "(25 learners), although their axes " +
                    "count by ones and twos respectively. " +
                    "For the L04 class preference dataset, " +
                    "choose a common tick interval of 2 or " +
                    "5 and place every bar at its ACTUAL " +
                    "height. An odd count on a scale-two " +
                    "axis lies between tick lines, not at a " +
                    "rounded integer multiple of two.";

                en.WorkedExamples =
                    "Four 2s = 8, eight 2s = 16; four " +
                    "5s = 20, eight 5s = 40. The second " +
                    "expression of each pair doubles the " +
                    "first result.\n" +
                    "Original bar-graph comparison: Bike 12, " +
                    "Walk 4, Bus or Van 6, Car 3. Total " +
                    "12+4+6+3=25. The first graph uses " +
                    "0..12 in ones and the second 0..24 " +
                    "in twos; neither changes the data.\n" +
                    "Independent student bar graph: " +
                    "retrieve the SAME six category totals " +
                    "from your actual L04 travel preference " +
                    "survey. Represent every real value " +
                    "using chosen scale 2 or 5. Explain " +
                    "any bar ending between ticks. Never " +
                    "substitute this fixed 25-person example " +
                    "for your own class survey.";

                en.StepByStepSolutions =
                    "Step 1 (10-minute Number Talk): " +
                    "count by 2 and 5; show how doubling " +
                    "four equal addends gives eight.\n" +
                    "Step 2 (10-minute graph comparison): " +
                    "read the SAME Bike 12, Walk 4, " +
                    "Bus/Van 6, Car 3 categories from " +
                    "the axis-by-1 and axis-by-2 displays.\n" +
                    "Step 3 (25-minute graph creation): " +
                    "reuse the original L04 class travel " +
                    "preferences and make a bar graph " +
                    "using tick interval 2 or 5 with " +
                    "a title and clear category labels.\n" +
                    "Step 4: preserve non-multiple counts " +
                    "exactly, explain between-tick bar " +
                    "ends, compare partner graphs and " +
                    "defend your scale choice.\n" +
                    "Step 5 (10-minute lesson synthesis): " +
                    "advise a friend on choosing the scale " +
                    "while preserving original values.\n" +
                    "Step 6 (5-minute independent Edulytics " +
                    "Cool-down): practise a new graph and " +
                    "submit drawings/reasoning for teacher review.";

                en.CommonMistakes =
                    "Mistake: multiply the reported bar " +
                    "height by its axis tick interval " +
                    "again. Repair: read the height " +
                    "directly from the numbered axis.\n" +
                    "Mistake: change the category counts " +
                    "when replotting the SAME survey " +
                    "with a different tick interval. " +
                    "Repair: compare all four data values.\n" +
                    "Mistake: round 3 learners to 2 or " +
                    "4 when the chosen axis counts by 2. " +
                    "Repair: show the bar top halfway " +
                    "between 2 and 4.\n" +
                    "Mistake: substitute the publisher's " +
                    "25-person demonstration graph for " +
                    "the separate L04 class survey. " +
                    "Repair: verify actual prior lesson " +
                    "records with the teacher.";

                en.QuickSummary =
                    "Change the bar-axis interval, not the " +
                    "data. The 2021 source's 12/4/6/3 " +
                    "example uses axis increments 1 and " +
                    "2; the pupil then reuses authentic " +
                    "L04 preferred-travel counts to create " +
                    "a bar graph with scale 2 or 5. Place " +
                    "non-multiple heights accurately and " +
                    "explain choices with a partner.";
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

