using System.Security.Cryptography;
using System.Text;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Two narrowly targeted Grade 2 Unit 9 lesson-content repairs. The original
/// publisher-backed source packs are never modified. This is an engineering
/// correction candidate, not independent academic certification.
/// </summary>
public static class CommonCoreG2TapeDiagramAndStoryResolutionCorrection
{
    public const string TapeDiagramLessonCode = "PED:US-CCSS-MATH:G2:U09:L11";
    public const string SolveStoriesLessonCode = "PED:US-CCSS-MATH:G2:U09:L13";

    public const string TapeDiagramContentVersion =
        "us-ccss-g2-u09-l11-tape-diagram-relationships-correction-v1";
    public const string SolveStoriesContentVersion =
        "us-ccss-g2-u09-l13-solve-represent-story-correction-v1";

    private const string PreviousGenericSteps =
        "Step 1: Identify what the task asks you to find or show.\n" +
        "Step 2: Represent the important quantities or relationships.\n" +
        "Step 3: Apply the key idea from the lesson.\n" +
        "Step 4: Explain how the representation supports the result.\n" +
        "Step 5: Check that the result matches the question.";

    public const string TapeDiagramConcepts =
        "A tape diagram uses bars or bar segments to show how quantities in a story are related.\n" +
        "A part-whole diagram shows the total bar as a combination of parts. A comparison " +
        "diagram can show how much greater one quantity is than another with aligned bars.\n" +
        "The same numbers can appear in two different stories, so match the question and " +
        "the meaning of every segment before choosing the addition or subtraction equation.";

    public const string TapeDiagramExamples =
        "Example 1 — Compare two amounts. One box holds 47 blue beads. " +
        "A second box holds 19 fewer red beads. The red bar is 19 shorter than " +
        "the blue bar. There are 47 - 19 = 28 red beads. " +
        "Check: 28 + 19 = 47.\n" +
        "Example 2 — Show a part and the remainder. A jar starts with 47 shells. " +
        "A child removes 19 shells. Draw one bar of 47 split into a removed part " +
        "of 19 and a remaining part of 28. The equation is 47 - 19 = 28. " +
        "Unlike Example 1, 47 is the total in one group, not one of two compared groups.\n" +
        "Example 3 — Unknown part. A 90-unit bar consists of parts 23, 17, " +
        "and an unknown part. Add 23 + 17 = 40; subtract 90 - 40 = 50. " +
        "The diagram and equation agree: 23 + 17 + 50 = 90.\n" +
        "Example 4 — Write a story from a tape diagram. Two bars show 63 items " +
        "and 26 fewer items. Ask how many are in the smaller set. " +
        "A matching equation is 63 - 26 = 37; verify 37 + 26 = 63.";

    public const string TapeDiagramSteps =
        "Step 1: Read the story carefully and identify the total, known parts, " +
        "compared quantities, and unknown.\n" +
        "Step 2: Draw or examine a tape diagram and label each bar or segment " +
        "with its meaning and units.\n" +
        "Step 3: Decide whether the bars show a part-whole relationship or " +
        "a comparison; do not match only by repeated numbers.\n" +
        "Step 4: Write a matching addition or subtraction equation, and find " +
        "the unknown. For 47 with 19 fewer, 47 - 19 = 28.\n" +
        "Step 5: Re-read the question, check the equation with 28 + 19 = 47, " +
        "and explain why the tape lengths match the story.";

    public const string TapeDiagramMistakes =
        "Mistake: matching a tape diagram because its numbers match a story " +
        "without checking their meaning. Correction: label the total, each " +
        "part, and the quantity being compared before choosing an equation.\n" +
        "Mistake: treating the longer comparison bar as a part-whole total. " +
        "Correction: distinguish two compared bars from a single bar divided " +
        "into parts.\n" +
        "Mistake: leaving an unknown bar segment unlabeled. Correction: " +
        "state what the unknown counts and check with the original story.";

    public const string SolveStoriesConcepts =
        "A complete story-problem solution connects the question, quantities, " +
        "representation, calculation, and final answer with units.\n" +
        "Pictures, tape diagrams, number lines, base-ten drawings, and equations " +
        "can represent the same addition or subtraction relationship.\n" +
        "Different correct methods may look different. Explain why each " +
        "representation matches the story and check that the answer addresses " +
        "the original question.";

    public const string SolveStoriesExamples =
        "Example 1 — Show an unknown starting amount. A class has some stickers, " +
        "receives 14, and then has 38. Draw a tape bar for 38 split into a " +
        "known part 14 and an unknown part. The starting amount is 24 because " +
        "38 - 14 = 24 and 24 + 14 = 38.\n" +
        "Example 2 — Two ways to subtract. A team has 72 pencils and gives away " +
        "26. Split 26 into 20 and 6: 72 - 20 = 52, then 52 - 6 = 46. " +
        "A tape diagram shows a total of 72 split into 26 and 46. " +
        "Check: 46 + 26 = 72.\n" +
        "Example 3 — Represent addition. A box holds 43 apples and 18 pears. " +
        "Use tens and ones: 43 + 10 = 53, then 53 + 8 = 61. " +
        "A two-part bar shows 43 + 18 = 61 pieces of fruit.\n" +
        "Example 4 — Review a partner's representation. A story says one " +
        "collection has 40 blocks and another has 15 fewer. A matching bar " +
        "pair has a 15-block difference, not a combined total of 55. " +
        "The second collection has 40 - 15 = 25 blocks; 25 + 15 = 40.";

    public const string SolveStoriesSteps =
        "Step 1: Read your complete story and underline its question; decide " +
        "which quantity is unknown.\n" +
        "Step 2: Represent the story using a picture, labeled tape diagram, " +
        "number line, or equation with the same numbers and units.\n" +
        "Step 3: Solve using an addition or subtraction strategy that matches " +
        "the representation. For 72 pencils with 26 given away, " +
        "72 - 20 - 6 = 46.\n" +
        "Step 4: Explain each calculation and state the answer in a sentence: " +
        "46 pencils remain.\n" +
        "Step 5: Check with the inverse addition 46 + 26 = 72, compare your " +
        "method with another representation, and revise any unclear labels.";

    public const string SolveStoriesMistakes =
        "Mistake: showing an equation without identifying what the numbers " +
        "mean. Correction: label each quantity and include the unit.\n" +
        "Mistake: drawing bars that do not match the quantities or question. " +
        "Correction: compare the diagram against the story before solving.\n" +
        "Mistake: presenting a calculation without the answer to the story " +
        "question. Correction: give a clear sentence and verify the result " +
        "using an inverse calculation or a second model.";

    private sealed record Correction(
        string LessonCode,
        string SourceUrl,
        string CanonicalSha256,
        string OldPersistedSha256,
        string CorrectedVersion,
        string[] Outcomes,
        string Concepts,
        string Examples,
        string Steps,
        string Mistakes);

    private static readonly IReadOnlyDictionary<string, Correction> Repairs =
        new Dictionary<string, Correction>(StringComparer.Ordinal)
        {
            [TapeDiagramLessonCode] = new(
                TapeDiagramLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-2/unit-9/lesson-11/preparation.html",
                "a9cd3e8f1ae6a0d9c2a7053216464815e1572fa49ff3edab2db8a61f43e0d8dd",
                "7aa4fda63b72d0599e894d2954a41cea03128b63c667c4883934653b419cf5b7",
                TapeDiagramContentVersion,
                ["CCSS:2.NBT.B.5", "CCSS:2.OA.A.1"],
                TapeDiagramConcepts, TapeDiagramExamples,
                TapeDiagramSteps, TapeDiagramMistakes),
            [SolveStoriesLessonCode] = new(
                SolveStoriesLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-2/unit-9/lesson-13/preparation.html",
                "d06a0120de3209c2c81684edfe7db8065fff36692067056ed038ba3456358a9e",
                "0d73f8641ba1ec4e43b2df7031257766f814b11bcb54edfb70a785ced2fc72fa",
                SolveStoriesContentVersion,
                ["CCSS:2.NBT.B.5", "CCSS:2.OA.A.1"],
                SolveStoriesConcepts, SolveStoriesExamples,
                SolveStoriesSteps, SolveStoriesMistakes)
        };

    public static bool IsTarget(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        string.Equals(document.PackCode, MathematicsCurriculumPackRegistry.CommonCoreCode,
            StringComparison.Ordinal) &&
        Repairs.ContainsKey(lesson.LessonCode);

    public static void Apply(CanonicalLessonContentPackDocument document)
    {
        if (!string.Equals(document.PackCode,
                MathematicsCurriculumPackRegistry.CommonCoreCode,
                StringComparison.Ordinal))
            return;

        foreach (var lesson in document.Lessons.Where(x => Repairs.ContainsKey(x.LessonCode)))
        {
            var repair = Repairs[lesson.LessonCode];
            if (!string.Equals(lesson.SourceUrl, repair.SourceUrl, StringComparison.Ordinal) ||
                !string.Equals(lesson.CanonicalBodySha256, repair.CanonicalSha256,
                    StringComparison.OrdinalIgnoreCase) ||
                !lesson.OutcomeCodes.SequenceEqual(repair.Outcomes))
                throw new InvalidOperationException(
                    $"G2 Unit 9 source/standard identity drift: {lesson.LessonCode}");

            var english = lesson.Translations.SingleOrDefault(x =>
                string.Equals(x.CultureCode, "en", StringComparison.Ordinal))
                ?? throw new InvalidOperationException(
                    $"Missing G2 Unit 9 English translation: {lesson.LessonCode}");

            if (string.Equals(english.StepByStepSolutions, repair.Steps,
                    StringComparison.Ordinal))
            {
                if (!string.Equals(english.KeyConceptsAndRules, repair.Concepts,
                        StringComparison.Ordinal) ||
                    !string.Equals(english.WorkedExamples, repair.Examples,
                        StringComparison.Ordinal) ||
                    !string.Equals(english.CommonMistakes, repair.Mistakes,
                        StringComparison.Ordinal))
                    throw new InvalidOperationException(
                        $"Partially changed G2 Unit 9 corrected lesson: {lesson.LessonCode}");
                continue;
            }

            if (!string.Equals(english.StepByStepSolutions, PreviousGenericSteps,
                    StringComparison.Ordinal) ||
                !string.Equals(ComputeBodySha256(english), repair.OldPersistedSha256,
                    StringComparison.Ordinal))
                throw new InvalidOperationException(
                    $"Refusing G2 Unit 9 source change on unexpected existing prose: {lesson.LessonCode}");

            english.KeyConceptsAndRules = repair.Concepts;
            english.WorkedExamples = repair.Examples;
            english.StepByStepSolutions = repair.Steps;
            english.CommonMistakes = repair.Mistakes;
        }
    }

    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string previousVersion) =>
        IsTarget(document, lesson)
            ? Repairs[lesson.LessonCode].CorrectedVersion
            : previousVersion;

    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingVersion) =>
        IsTarget(document, lesson) &&
        string.Equals(existingVersion,
            CommonCoreRichContentCorrections.CorrectionContentVersion,
            StringComparison.Ordinal);

    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingVersion,
        IReadOnlyCollection<CurriculumLessonContentTranslation> translations)
    {
        if (!CanUpgradeExisting(document, lesson, existingVersion))
            throw new InvalidOperationException(
                $"G2 Unit 9 correction requires exact prior version: {lesson.LessonCode}");

        if (translations.Count == 0)
            return; // JSON-backed metadata-only storage.

        if (translations.Count != 1 ||
            !string.Equals(translations.Single().CultureCode,
                "en", StringComparison.Ordinal))
            throw new InvalidOperationException(
                $"G2 Unit 9 unexpected stored cultures: {lesson.LessonCode}");

        var repair = Repairs[lesson.LessonCode];
        var oldStoredSha = ComputeBodySha256(translations.Single());
        var current = lesson.Translations.Single(t => t.CultureCode == "en");
        var expectedNewSha = ComputeBodySha256(current);

        if (!string.Equals(oldStoredSha, repair.OldPersistedSha256,
                StringComparison.Ordinal) &&
            !string.Equals(oldStoredSha, expectedNewSha, StringComparison.Ordinal))
            throw new InvalidOperationException(
                $"Unrecognized G2 Unit 9 persisted content; refusing silent replacement: {lesson.LessonCode}");
    }

    private static string ComputeBodySha256(
        CanonicalLessonContentPackTranslation english) =>
        ComputeBodySha256(
            english.CultureCode, english.Title, english.Explanation,
            english.KeyConceptsAndRules, english.WorkedExamples,
            english.StepByStepSolutions, english.CommonMistakes,
            english.QuickSummary);

    private static string ComputeBodySha256(
        CurriculumLessonContentTranslation stored) =>
        ComputeBodySha256(
            stored.CultureCode, stored.Title, stored.Explanation,
            stored.KeyConceptsAndRules, stored.WorkedExamples,
            stored.StepByStepSolutions, stored.CommonMistakes, stored.QuickSummary);

    private static string ComputeBodySha256(params string[] fields) =>
        Convert.ToHexString(SHA256.HashData(
            Encoding.UTF8.GetBytes(string.Join("\n", fields) + "\n")))
            .ToLowerInvariant();
}
