using System.Security.Cryptography;
using System.Text;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Narrow Edulytics-original correction for Grade 2 Unit 9 Lesson 12,
/// anchored to the actual publisher lesson and official 2.OA.A.1 outcome.
/// Corrects an algebraic-transformation procedure in a story-writing lesson.
/// Original upstream source bytes remain unchanged; release remains NO-GO.
/// </summary>
public static class CommonCoreG2StoryProblemsSourceCorrection
{
    public const string LessonCode = "PED:US-CCSS-MATH:G2:U09:L12";
    public const string CorrectedContentVersion =
        "us-ccss-g2-u09-l12-story-writing-source-correction-v1";

    private const string ExpectedSourceUrl =
        "https://im.kendallhunt.com/k5/teachers/grade-2/unit-9/lesson-12/preparation.html";
    private const string OriginalCanonicalBodySha256 =
        "3d8f619b82e233f6d61276aaf5154f0aa416685ba9e153b8121bf9f8b5ad60ef";
    private const string ExpectedOldPersistedBodySha256 =
        "3ea666ef337e3611a2f18cac013051d33eb076df9dffcea54e45411288e617b0";

    private const string PreviousSteps =
        "Step 1: Identify the unknowns and the relationships between quantities.\n" +
        "Step 2: Translate the relationships into expressions, equations, graphs, or tables.\n" +
        "Step 3: Apply valid algebraic operations or transformations.\n" +
        "Step 4: Solve or interpret the result.\n" +
        "Step 5: Check the result in the original equation, representation, or context.";

    public const string NewKeyConcepts =
        "A story problem has a situation, quantities, something that happens or is compared, and a clear question.\n" +
        "An equation such as 18 + ? = 31 can describe a starting amount and an unknown change. " +
        "The unknown must represent the same quantity in the story and in the equation.\n" +
        "Addition stories can combine or increase quantities. Subtraction stories can describe " +
        "taking away or finding a difference. Read the story again to check which quantity is unknown.";

    public const string NewWorkedExamples =
        "Example 1 — Unknown change in an addition story. Equation: 18 + ? = 31. " +
        "A class has 18 stickers and receives some more, bringing the total to 31. " +
        "Question: How many stickers did the class receive? The answer is 13, because 18 + 13 = 31. " +
        "The story, question, and equation describe the same quantities.\n" +
        "Example 2 — Unknown start in an addition story. Equation: ? + 7 = 25. " +
        "A child has some marbles, then finds 7 more and now has 25. " +
        "Question: How many marbles were there at first? The answer is 18, because 18 + 7 = 25.\n" +
        "Example 3 — Unknown amount taken away. Equation: 64 - ? = 29. " +
        "A basket holds 64 apples. Some are taken away, leaving 29. " +
        "Question: How many apples were taken away? The answer is 35, because 64 - 35 = 29.\n" +
        "Example 4 — Unknown starting amount in a subtraction story. Equation: ? - 12 = 19. " +
        "Someone gives away 12 crayons and has 19 left. " +
        "Question: How many crayons were there at first? The answer is 31, because 31 - 12 = 19.";

    public const string NewSteps =
        "Step 1: Read the given addition or subtraction equation and identify its known numbers and unknown.\n" +
        "Step 2: Choose a familiar context and assign a clear meaning and unit to every quantity.\n" +
        "Step 3: Tell what happens or what is compared. Use the same order of quantities as the equation.\n" +
        "Step 4: Write one question asking for the unknown quantity, not for a number that is already given.\n" +
        "Step 5: Solve and check the example: for 18 + ? = 31, 18 + 13 = 31. " +
        "Re-read the story to verify that 13 answers the written question.";

    public const string NewMistakes =
        "Mistake: writing a list of numbers without a situation or question. " +
        "Correction: describe what happened and state what needs to be found.\n" +
        "Mistake: asking for a quantity that the equation already gives. " +
        "Correction: make the story question refer to the unknown.\n" +
        "Mistake: changing the order of a subtraction story so that it represents a different equation. " +
        "Correction: check the starting amount, change, and result against the original equation.";

    public static bool IsTarget(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        string.Equals(document.PackCode, MathematicsCurriculumPackRegistry.CommonCoreCode,
            StringComparison.Ordinal) &&
        string.Equals(lesson.LessonCode, LessonCode, StringComparison.Ordinal);

    public static void Apply(CanonicalLessonContentPackDocument document)
    {
        if (!string.Equals(document.PackCode, MathematicsCurriculumPackRegistry.CommonCoreCode,
                StringComparison.Ordinal))
            return;

        foreach (var lesson in document.Lessons.Where(x =>
                     string.Equals(x.LessonCode, LessonCode, StringComparison.Ordinal)))
        {
            if (!string.Equals(lesson.SourceUrl, ExpectedSourceUrl, StringComparison.Ordinal) ||
                !string.Equals(lesson.CanonicalBodySha256, OriginalCanonicalBodySha256,
                    StringComparison.OrdinalIgnoreCase) ||
                !lesson.OutcomeCodes.SequenceEqual(["CCSS:2.OA.A.1"]))
                throw new InvalidOperationException(
                    $"G2 story-writing source/standard identity drift: {lesson.LessonCode}.");

            var english = lesson.Translations.SingleOrDefault(t =>
                string.Equals(t.CultureCode, "en", StringComparison.Ordinal))
                ?? throw new InvalidOperationException(
                    "G2 story-writing source lacks English translation.");

            if (string.Equals(english.StepByStepSolutions, NewSteps, StringComparison.Ordinal))
            {
                if (!string.Equals(english.KeyConceptsAndRules, NewKeyConcepts, StringComparison.Ordinal) ||
                    !string.Equals(english.WorkedExamples, NewWorkedExamples, StringComparison.Ordinal) ||
                    !string.Equals(english.CommonMistakes, NewMistakes, StringComparison.Ordinal))
                    throw new InvalidOperationException("Partial G2 story-writing correction drift.");
                return;
            }

            if (!string.Equals(english.StepByStepSolutions, PreviousSteps, StringComparison.Ordinal) ||
                !string.Equals(ComputeBodySha256(
                    english.CultureCode, english.Title, english.Explanation,
                    english.KeyConceptsAndRules, english.WorkedExamples,
                    english.StepByStepSolutions, english.CommonMistakes, english.QuickSummary),
                    ExpectedOldPersistedBodySha256, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    "Refusing G2 story-writing correction of unexpected original prose.");

            english.KeyConceptsAndRules = NewKeyConcepts;
            english.WorkedExamples = NewWorkedExamples;
            english.StepByStepSolutions = NewSteps;
            english.CommonMistakes = NewMistakes;
        }
    }

    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string priorVersion) =>
        IsTarget(document, lesson) ? CorrectedContentVersion : priorVersion;

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
        IReadOnlyCollection<CurriculumLessonContentTranslation> persistedTranslations)
    {
        if (!CanUpgradeExisting(document, lesson, existingVersion))
            throw new InvalidOperationException(
                "G2 story-writing upgrade requires the exact prior content version.");

        // ReadFromJson uses metadata without persisted text; it is a valid path.
        if (persistedTranslations.Count == 0)
            return;

        if (persistedTranslations.Count != 1 ||
            !string.Equals(persistedTranslations.Single().CultureCode,
                "en", StringComparison.Ordinal))
            throw new InvalidOperationException(
                "Refusing G2 story-writing upgrade with unexpected translation cultures.");

        var previous = persistedTranslations.Single();
        var oldSha = ComputeBodySha256(
            previous.CultureCode, previous.Title, previous.Explanation,
            previous.KeyConceptsAndRules, previous.WorkedExamples,
            previous.StepByStepSolutions, previous.CommonMistakes, previous.QuickSummary);

        var corrected = lesson.Translations.Single(t => t.CultureCode == "en");
        var newSha = ComputeBodySha256(
            corrected.CultureCode, corrected.Title, corrected.Explanation,
            corrected.KeyConceptsAndRules, corrected.WorkedExamples,
            corrected.StepByStepSolutions, corrected.CommonMistakes, corrected.QuickSummary);

        if (!string.Equals(oldSha, ExpectedOldPersistedBodySha256, StringComparison.Ordinal) &&
            !string.Equals(oldSha, newSha, StringComparison.Ordinal))
            throw new InvalidOperationException(
                "Refusing G2 story-writing replacement of unrecognized persisted text.");
    }

    private static string ComputeBodySha256(params string[] fields) =>
        Convert.ToHexString(SHA256.HashData(
            Encoding.UTF8.GetBytes(string.Join("\n", fields) + "\n")))
            .ToLowerInvariant();
}
