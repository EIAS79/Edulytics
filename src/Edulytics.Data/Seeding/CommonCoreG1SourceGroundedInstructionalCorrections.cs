using System.Security.Cryptography;
using System.Text;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Source-bound, Edulytics-authored Grade 1 instructional repair candidates.
/// Addresses lessons where generated example arithmetic was unrelated to the
/// publisher's learning goal (or taught negative integer subtraction in G1).
/// These edits are NOT independent mathematics reviewer signoff.
/// Original third-party source packs are kept byte-for-byte unchanged.
/// </summary>
public static class CommonCoreG1SourceGroundedInstructionalCorrections
{
    public const string DataQuestionsLessonCode = "PED:US-CCSS-MATH:G1:U01:L13";
    public const string JungleSurveyLessonCode = "PED:US-CCSS-MATH:G1:U01:L15";
    public const string TensLessonCode = "PED:US-CCSS-MATH:G1:U04:L03";

    public const string DataQuestionsVersion = "us-ccss-g1-u01-l13-data-questions-correction-v1";
    public const string JungleSurveyVersion = "us-ccss-g1-u01-l15-survey-and-tallies-correction-v1";
    public const string TensVersion = "us-ccss-g1-u04-l03-tens-strategy-correction-v1";

    private sealed record Repair(
        string Code, string ExpectedUrl, string OriginalCanonicalSha,
        string OldBodySha, string CorrectedVersion, string[] Outcomes,
        string OldSteps, string Concepts, string Examples, string Steps,
        string CommonMistakes);

    private const string OldAddSteps =
        "Step 1: Identify the two quantities, 7 and 1.\n" +
        "Step 2: Start at 7 and count on 1 more, or combine the two groups.\n" +
        "Step 3: The total is 8, so 7 + 1 = 8.\n" +
        "Step 4: Check that the result matches the quantities and the lesson goal.";

    private const string OldJungleSteps =
        "Step 1: Identify the starting quantity 1 and the amount 4 being removed or compared.\n" +
        "Step 2: Use subtraction to find the difference.\n" +
        "Step 3: 1 − 4 = -3.\n" +
        "Step 4: Check that the answer fits the original situation.";

    private const string OldTensSteps =
        "Step 1: Identify the starting quantity 2 and the amount 3 being removed or compared.\n" +
        "Step 2: Use subtraction to find the difference.\n" +
        "Step 3: 2 − 3 = -1.\n" +
        "Step 4: Check that the answer fits the original situation.";

    public const string DataQuestionsConcepts =
        "Learning goals: Ask and answer questions about information shown in a picture graph, " +
        "tally chart, or category-count table.\n" +
        "The categories, labels, and counts tell which questions can be answered from the display. " +
        "You can find the number in a category, compare two category counts, or add counts to find a total. " +
        "Do not answer a question if the required information is not displayed.";

    public const string DataQuestionsExamples =
        "Example 1 — Can the graph answer the question? A chart shows 7 children chose art, " +
        "5 chose music, and 3 chose sports. A question you can answer is: " +
        "How many chose music? Answer: 5 children, because the music category has 5.\n" +
        "Example 2 — Ask a comparison question. How many more children chose art than sports? " +
        "Use the category counts: 7 - 3 = 4 children. Check: 3 + 4 = 7.\n" +
        "Example 3 — Ask about the total. How many children took part? " +
        "Add 7 + 5 + 3 = 15 children.\n" +
        "Example 4 — Recognize a question the data cannot answer. " +
        "What is each child's favorite color? The display shows activity choices, not colors. " +
        "We need different data before answering.";

    public const string DataQuestionsSteps =
        "Step 1: Read the graph or table title, all category labels, and the count for each category.\n" +
        "Step 2: Ask a question that uses information the display actually provides.\n" +
        "Step 3: Find the relevant category count, compare two counts, or combine the counts. " +
        "For 7 art and 3 sports votes, 7 - 3 = 4 more chose art.\n" +
        "Step 4: Write an answer including what was counted, such as 4 children.\n" +
        "Step 5: Point to the labels and counts that support your answer; " +
        "identify questions the display does not answer.";

    public const string DataQuestionsMistakes =
        "Mistake: guessing from the height or area of a picture without reading labels and counts. " +
        "Correction: read the actual values or the scale.\n" +
        "Mistake: asking about information not collected in the survey. " +
        "Correction: first decide whether the display contains the necessary category.\n" +
        "Mistake: adding categories without saying what is counted. " +
        "Correction: include the people or objects counted in the answer.";

    public const string JungleConcepts =
        "Learning goals: Create a survey question, collect responses, organize them into " +
        "categories, show the results, and ask and answer questions about the data.\n" +
        "Each vote belongs to one named category. Tally marks or drawings help keep accurate counts. " +
        "The total votes should match the number of recorded responses. " +
        "Explain what the category counts show, and use the results to make a new question.";

    public const string JungleExamples =
        "Example 1 — Plan the survey. Ask: Which jungle animal would you most like to see, " +
        "a monkey, lion, or parrot? Every respondent chooses just one of the three.\n" +
        "Example 2 — Record the votes. Suppose 4 children choose monkeys, " +
        "3 choose lions, and 2 choose parrots. Use four tally marks for monkeys, " +
        "three for lions, and two for parrots; label every category.\n" +
        "Example 3 — Answer a question. How many children answered? 4 + 3 + 2 = 9. " +
        "How many more chose monkeys than parrots? 4 - 2 = 2.\n" +
        "Example 4 — Interpret the display. Monkeys have the most votes (4) " +
        "and parrots have the fewest (2). A larger picture of a parrot does not " +
        "mean more votes; use the tallies.";

    public const string JungleSteps =
        "Step 1: Choose a survey question about jungle animals and list possible responses.\n" +
        "Step 2: Collect one answer from each person; record a tally for the chosen category.\n" +
        "Step 3: Count the tallies and create a labeled table or picture graph. " +
        "For 4 monkeys, 3 lions, and 2 parrots, show counts 4, 3, and 2.\n" +
        "Step 4: Ask questions the data can answer: Which has the most votes? " +
        "How many votes in all? Find 4 + 3 + 2 = 9.\n" +
        "Step 5: Check that the category totals equal the responses collected. " +
        "Explain your findings and compare with a classmate.";

    public const string JungleMistakes =
        "Mistake: putting a response in two categories. Correction: count one vote per person.\n" +
        "Mistake: using different-sized symbols as if they were equal votes. " +
        "Correction: use one tally per response, or state the graph key.\n" +
        "Mistake: making a subtraction story with an impossible negative number of animals. " +
        "Correction: compare the counts using a greater count minus a smaller count.";

    public const string TensConcepts =
        "Learning goals: Add 10 to and subtract 10 from whole-number multiples of 10, " +
        "using towers of ten, drawings, and skip-counting.\n" +
        "Each tower holds 10 cubes. Adding one tower adds 10 to the number; " +
        "removing one tower subtracts 10 when at least one tower is present.\n" +
        "Keep the number of tens and the total number of cubes distinct: " +
        "three tens are 30 cubes, not 3 cubes.";

    public const string TensExamples =
        "Example 1 — Add a ten. Start with two towers of 10 cubes (20 cubes). " +
        "Add one tower: 20 + 10 = 30 cubes, or three tens.\n" +
        "Example 2 — Subtract a ten. Start with six towers of 10 cubes (60). " +
        "Remove one tower: 60 - 10 = 50, or five tens.\n" +
        "Example 3 — Use place value. For 80 - 10, think eight tens minus one ten. " +
        "Eight tens become seven tens: 80 - 10 = 70.\n" +
        "Example 4 — Check by reversing the operation: 40 + 10 = 50, " +
        "so 50 - 10 = 40. Show the result with towers or a number line.";

    public const string TensSteps =
        "Step 1: Read the starting multiple of 10; 60 means six groups of ten cubes.\n" +
        "Step 2: Decide whether the task adds one group of ten or takes one away.\n" +
        "Step 3: Model the action with ten-cube towers or jump forward/backward " +
        "once by 10 on a number line.\n" +
        "Step 4: Write the matching equation with multiples of ten: 60 - 10 = 50.\n" +
        "Step 5: Check by the inverse fact 50 + 10 = 60 and describe how " +
        "the number of tens changed.";

    public const string TensMistakes =
        "Mistake: reading three towers as 3 cubes. Correction: each tower is worth 10, " +
        "so three towers represent 30.\n" +
        "Mistake: subtracting 1 instead of 10 when removing one tower. " +
        "Correction: one tower means one ten.\n" +
        "Mistake: using a negative one-digit subtraction example rather than " +
        "the intended tens relationship. Correction: model 60 - 10 = 50 with towers.";

    private static readonly IReadOnlyDictionary<string, Repair> Repairs =
        new Dictionary<string, Repair>(StringComparer.Ordinal)
        {
            [DataQuestionsLessonCode] = new Repair(
                DataQuestionsLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-13/preparation.html",
                "619ca1727ce80aad07b6487cedd836c7017369cf251c425d2090c2d32662affa",
                "9d47d76f36eaa815741f5ce83bf807b30c490f30a48cb550812981ddef1864b9",
                DataQuestionsVersion, ["CCSS:1.MD.C.4", "CCSS:1.OA.C.5", "CCSS:1.OA.C.6"],
                OldAddSteps, DataQuestionsConcepts, DataQuestionsExamples, DataQuestionsSteps, DataQuestionsMistakes),
            [JungleSurveyLessonCode] = new Repair(
                JungleSurveyLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-15/preparation.html",
                "68afdf12b866c914150e889635bda34a23420fc2ead634c36082b2fc4712f4ed",
                "524c8057fd7450477edf66b60aa88c2057435e8abbb44ca6db0924f85b932882",
                JungleSurveyVersion, ["CCSS:1.MD.C.4"],
                OldJungleSteps, JungleConcepts, JungleExamples, JungleSteps, JungleMistakes),
            [TensLessonCode] = new Repair(
                TensLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-3/preparation.html",
                "e353f2b71bd5eb8973dcf261433992a18a0bd8f454d84f27c0db0c33270f4a16",
                "bcb915fa6c9669eebf4dfb723002e53aacccb21001948f1f11822dd480e5cf94",
                TensVersion,
                ["CCSS:1.NBT.B.2", "CCSS:1.NBT.C.4", "CCSS:1.NBT.C.5",
                 "CCSS:1.NBT.C.6", "CCSS:1.OA.C.5", "CCSS:1.OA.C.6", "CCSS:1.OA.D.8"],
                OldTensSteps, TensConcepts, TensExamples, TensSteps, TensMistakes)
        };

    public static bool IsTarget(CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        string.Equals(document.PackCode,
            MathematicsCurriculumPackRegistry.CommonCoreCode, StringComparison.Ordinal) &&
        Repairs.ContainsKey(lesson.LessonCode);

    public static void Apply(CanonicalLessonContentPackDocument document)
    {
        if (!string.Equals(document.PackCode,
                MathematicsCurriculumPackRegistry.CommonCoreCode, StringComparison.Ordinal))
            return;

        foreach (var lesson in document.Lessons.Where(l => Repairs.ContainsKey(l.LessonCode)))
        {
            var repair = Repairs[lesson.LessonCode];
            if (!string.Equals(lesson.SourceUrl, repair.ExpectedUrl, StringComparison.Ordinal) ||
                !string.Equals(lesson.CanonicalBodySha256, repair.OriginalCanonicalSha,
                    StringComparison.OrdinalIgnoreCase) ||
                !lesson.OutcomeCodes.SequenceEqual(repair.Outcomes))
                throw new InvalidOperationException(
                    $"G1 instruction source/official-standard drift: {lesson.LessonCode}");

            var en = lesson.Translations.SingleOrDefault(t =>
                string.Equals(t.CultureCode, "en", StringComparison.Ordinal))
                ?? throw new InvalidOperationException("Missing English G1 lesson source");

            if (string.Equals(en.StepByStepSolutions, repair.Steps, StringComparison.Ordinal))
            {
                if (!string.Equals(en.KeyConceptsAndRules, repair.Concepts, StringComparison.Ordinal) ||
                    !string.Equals(en.WorkedExamples, repair.Examples, StringComparison.Ordinal) ||
                    !string.Equals(en.CommonMistakes, repair.CommonMistakes, StringComparison.Ordinal))
                    throw new InvalidOperationException(
                        $"Partial G1 correction drift: {lesson.LessonCode}");
                continue;
            }

            if (!string.Equals(en.StepByStepSolutions, repair.OldSteps, StringComparison.Ordinal) ||
                !string.Equals(ComputeBodySha256(en), repair.OldBodySha,
                    StringComparison.Ordinal))
                throw new InvalidOperationException(
                    $"Refusing G1 instruction correction of unexpected prose: {lesson.LessonCode}");

            en.KeyConceptsAndRules = repair.Concepts;
            en.WorkedExamples = repair.Examples;
            en.StepByStepSolutions = repair.Steps;
            en.CommonMistakes = repair.CommonMistakes;
        }
    }

    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string priorVersion) =>
        IsTarget(document, lesson) ? Repairs[lesson.LessonCode].CorrectedVersion : priorVersion;

    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingVersion) =>
        IsTarget(document, lesson) &&
        string.Equals(existingVersion, CommonCoreRichContentCorrections.CorrectionContentVersion,
            StringComparison.Ordinal);

    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingVersion,
        IReadOnlyCollection<CurriculumLessonContentTranslation> translations)
    {
        if (!CanUpgradeExisting(document, lesson, existingVersion))
            throw new InvalidOperationException(
                $"Expected preceding G1 Common Core learner content version: {lesson.LessonCode}");

        // Content metadata may exist without persisted prose in ReadFromJson mode.
        if (translations.Count == 0)
            return;

        if (translations.Count != 1 || translations.Single().CultureCode != "en")
            throw new InvalidOperationException(
                $"Unexpected G1 stored translation cultures: {lesson.LessonCode}");

        var repair = Repairs[lesson.LessonCode];
        var actualSha = ComputeBodySha256(translations.Single());
        var effective = lesson.Translations.Single(x => x.CultureCode == "en");
        var correctedSha = ComputeBodySha256(effective);
        if (!string.Equals(actualSha, repair.OldBodySha, StringComparison.Ordinal) &&
            !string.Equals(actualSha, correctedSha, StringComparison.Ordinal))
            throw new InvalidOperationException(
                $"Refusing to overwrite unrecognized G1 persisted text: {lesson.LessonCode}");
    }

    private static string ComputeBodySha256(CanonicalLessonContentPackTranslation x) =>
        ComputeBodySha256(x.CultureCode, x.Title, x.Explanation, x.KeyConceptsAndRules,
            x.WorkedExamples, x.StepByStepSolutions, x.CommonMistakes, x.QuickSummary);

    private static string ComputeBodySha256(CurriculumLessonContentTranslation x) =>
        ComputeBodySha256(x.CultureCode, x.Title, x.Explanation, x.KeyConceptsAndRules,
            x.WorkedExamples, x.StepByStepSolutions, x.CommonMistakes, x.QuickSummary);

    private static string ComputeBodySha256(params string[] fields) =>
        Convert.ToHexString(SHA256.HashData(
            Encoding.UTF8.GetBytes(string.Join("\n", fields) + "\n"))).ToLowerInvariant();
}
