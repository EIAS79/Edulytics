using Edulytics.Core.Curriculum;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Narrow source-anchored correction of Grade 4 Unit 1 Lesson 1.
/// Original canonical source bytes remain unchanged; the materializer enforces
/// an exact old-body guard and a distinct persisted content version.
/// This engineering correction is NOT independent academic certification.
/// </summary>
public static class CommonCoreG4MultiplesSourceCorrection
{
    public const string LessonCode = "PED:US-CCSS-MATH:G4:U01:L01";
    public const string CorrectedContentVersion = "us-ccss-g4-u01-l01-multiples-source-correction-v1";

    private const string ExpectedSourceUrl =
        "https://im.kendallhunt.com/k5/teachers/grade-4/unit-1/lesson-1/preparation.html";

    private const string OriginalCanonicalBodySha256 =
        "b18e1bed2b1e3ad2d363bf493e5327be0f117e834958a6779ef018cc8c2e8b81";

    private const string PreviousStepByStepSolutions =
        "Step 1: Identify the figure and the attributes named in the task.\n" +
        "Step 2: Measure, count, or compare sides, angles, corners, or other relevant features.\n" +
        "Step 3: Use the attributes to classify, construct, or reason about the figure.\n" +
        "Step 4: Explain which attributes support the result.\n" +
        "Step 5: Check that the conclusion does not depend only on size or orientation.";

    public const string NewKeyConcepts =
        "A rectangle's area in square units is width × length when both sides are measured in unit lengths.\n" +
        "Keeping the width fixed at 3 and choosing whole-number lengths produces areas 3, 6, 9, 12, ... square units: multiples of 3.\n" +
        "The area must be a multiple of each whole-number side length. A product's factors are its side lengths in an equal-square rectangular array.";

    public const string NewWorkedExamples =
        "Example 1 — Rectangles 3 tiles wide. Build rectangles with whole-number lengths 1, 2, 3, 4, and 5. " +
        "The corresponding areas are 3 × 1 = 3, 3 × 2 = 6, 3 × 3 = 9, 3 × 4 = 12, and 3 × 5 = 15 square tiles. " +
        "The areas are multiples of 3 because each is 3 multiplied by a whole number.\n" +
        "Example 2 — Test a possible area. A rectangle 3 units wide cannot have area 28 square units with a whole-number length: " +
        "3 × 9 = 27 and 3 × 10 = 30, so there is no whole number of columns giving 28. " +
        "An area of 30 square units is possible using width 3 and length 10.\n" +
        "Example 3 — Compare widths. A 4-by-7 rectangle has area 28 square units, so 28 is a multiple of 4. " +
        "The same area is not a multiple of 3. Check each claim with multiplication, an array of unit squares, or division with remainder.";

    public const string NewStepByStepSolutions =
        "Step 1: Choose a whole-number width and length, for example 3 rows and 4 columns of unit squares.\n" +
        "Step 2: Use the array or multiplication to find the area: 3 × 4 = 12 square units.\n" +
        "Step 3: Keep the width 3 and change the whole-number length: 3 × 1 = 3, 3 × 2 = 6, 3 × 3 = 9, and so on.\n" +
        "Step 4: To test area 28 with width 3, look for a whole number n such that 3 × n = 28. " +
        "Since 3 × 9 = 27 and 3 × 10 = 30, the area is not possible with whole-number side lengths.\n" +
        "Step 5: State that these areas are multiples of the chosen width, and verify a prediction with a new rectangle or an inverse division check.";

    public const string NewCommonMistakes =
        "Mistake: assuming every area is possible for a fixed whole-number width. " +
        "For width 3, 28 is not possible because 28 is not divisible by 3. " +
        "Mistake: adding side lengths to find area; multiply width × length and use square units. " +
        "Mistake: counting only the boundary tiles instead of every square in the rectangular array.";

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
                !lesson.OutcomeCodes.SequenceEqual(["CCSS:4.OA.B.4"]))
                throw new InvalidOperationException(
                    $"US Common Core G4 multiples source/provenance drift: {lesson.LessonCode}.");

            var english = lesson.Translations.SingleOrDefault(t =>
                string.Equals(t.CultureCode, "en", StringComparison.Ordinal))
                ?? throw new InvalidOperationException(
                    "US Common Core G4 multiples source lacks canonical English translation.");

            if (string.Equals(english.StepByStepSolutions, NewStepByStepSolutions,
                    StringComparison.Ordinal))
            {
                if (!string.Equals(english.KeyConceptsAndRules, NewKeyConcepts,
                        StringComparison.Ordinal) ||
                    !string.Equals(english.WorkedExamples, NewWorkedExamples,
                        StringComparison.Ordinal) ||
                    !string.Equals(english.CommonMistakes, NewCommonMistakes,
                        StringComparison.Ordinal))
                    throw new InvalidOperationException(
                        "US Common Core G4 multiples correction has partially drifted.");
                continue;
            }

            if (!string.Equals(english.StepByStepSolutions,
                    PreviousStepByStepSolutions, StringComparison.Ordinal) ||
                !english.WorkedExamples.Contains(
                    "Build 5 different rectangles with each of the given widths",
                    StringComparison.Ordinal) ||
                !english.KeyConceptsAndRules.Contains(
                    "Use mathematical attributes such as sides",
                    StringComparison.Ordinal))
                throw new InvalidOperationException(
                    "Refusing G4 multiples editorial correction of unexpected existing prose.");

            english.KeyConceptsAndRules = NewKeyConcepts;
            english.WorkedExamples = NewWorkedExamples;
            english.StepByStepSolutions = NewStepByStepSolutions;
            english.CommonMistakes = NewCommonMistakes;
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
}
