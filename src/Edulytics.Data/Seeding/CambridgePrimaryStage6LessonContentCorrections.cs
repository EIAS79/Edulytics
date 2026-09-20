using Edulytics.Core.Curriculum;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Narrow, versioned corrections for four Cambridge Primary Stage 6
/// supporting lessons whose original Phase 29 bodies were too generic for
/// the exact lesson skill. The official curriculum graph and outcome mapping
/// are intentionally untouched.
/// </summary>
public static class CambridgePrimaryStage6LessonContentCorrections
{
    public const string PackCode =
        "CAMBRIDGE-INTL-MATH";

    public const string BaseContentVersion =
        "phase29-cambridge-primary-stage6-dfe-ogl-v1";

    public const string CorrectionContentVersion =
        "phase29-cambridge-primary-stage6-alignment-v2";

    public const string TwoUnknownsLessonCode =
        "PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-4:APPLY";

    public const string ScaleReadingBuildLessonCode =
        "PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:BUILD";

    public const string ScaleReadingLessonCode =
        "PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:APPLY";

    public const string FractionComparisonLessonCode =
        "PED:CAMBRIDGE-INTL-MATH:S6:6F-3:BUILD";

    private static readonly HashSet<string> TargetLessonCodes =
        new(StringComparer.Ordinal)
        {
            TwoUnknownsLessonCode,
            ScaleReadingBuildLessonCode,
            ScaleReadingLessonCode,
            FractionComparisonLessonCode
        };

    public static bool IsTarget(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        string.Equals(
            document.PackCode,
            PackCode,
            StringComparison.Ordinal) &&
        string.Equals(
            document.ContentVersion,
            BaseContentVersion,
            StringComparison.Ordinal) &&
        TargetLessonCodes.Contains(lesson.LessonCode);

    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        IsTarget(document, lesson)
            ? CorrectionContentVersion
            : document.ContentVersion;

    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingContentVersion) =>
        IsTarget(document, lesson) &&
        string.Equals(
            existingContentVersion,
            BaseContentVersion,
            StringComparison.Ordinal);

    public static void ApplyApprovedCorrections(
        CanonicalLessonContentPackDocument document)
    {
        if (!string.Equals(
                document.PackCode,
                PackCode,
                StringComparison.Ordinal) ||
            !string.Equals(
                document.ContentVersion,
                BaseContentVersion,
                StringComparison.Ordinal))
        {
            return;
        }

        foreach (var lesson in document.Lessons)
        {
            if (!TargetLessonCodes.Contains(lesson.LessonCode))
                continue;

            var translation =
                lesson.Translations.SingleOrDefault(
                    x => string.Equals(
                        x.CultureCode,
                        "en",
                        StringComparison.Ordinal))
                ?? throw new InvalidOperationException(
                    $"Stage 6 alignment correction requires English canonical content for {lesson.LessonCode}.");

            ApplyLessonCorrection(
                lesson.LessonCode,
                translation);
        }
    }

    private static void ApplyLessonCorrection(
        string lessonCode,
        CanonicalLessonContentPackTranslation translation)
    {
        switch (lessonCode)
        {
            case TwoUnknownsLessonCode:
                ApplyTwoUnknownsCorrection(translation);
                break;

            case ScaleReadingBuildLessonCode:
                ApplyScaleReadingBuildCorrection(translation);
                break;

            case ScaleReadingLessonCode:
                ApplyScaleReadingCorrection(translation);
                break;

            case FractionComparisonLessonCode:
                ApplyFractionComparisonCorrection(translation);
                break;

            default:
                throw new InvalidOperationException(
                    $"Unsupported Stage 6 correction target: {lessonCode}.");
        }
    }

    private static void ApplyTwoUnknownsCorrection(
        CanonicalLessonContentPackTranslation translation)
    {
        translation.Explanation =
            "This Cambridge Primary Stage 6 supporting lesson develops the open DfE Year 6 ready-to-progress focus “Solve problems with 2 unknowns”. A two-unknown problem gives two quantities whose values are not known and two independent facts that link them. Represent both facts before calculating. For example, if x + y = 46 and y − x = 8, then the total and difference must be true at the same time: x = 19 and y = 27. A pair is a solution only when it satisfies both relationships. The lesson is Edulytics-authored from OGL material; Cambridge remains the academic reference authority and no Cambridge objective wording is reproduced here.";

        translation.KeyConceptsAndRules =
            "Source focus: Solve problems with 2 unknowns. Define what each unknown represents. Write two independent relationships from the two facts in the problem. Preserve equality while eliminating, substituting or reasoning from the total and difference. Solve for both unknowns, not just one. Check the final pair in both original relationships. Appropriate representations include two equations, a bar model, a table or another diagram that keeps both linked quantities visible.";

        translation.WorkedExamples =
            "Example A: Two boxes contain 46 counters altogether. Box B has 8 more counters than Box A. Let x be Box A and y be Box B. Then x + y = 46 and y − x = 8. Subtract the difference from the total: 46 − 8 = 38. Split 38 equally: x = 19. Then y = 27. Check: 19 + 27 = 46 and 27 − 19 = 8. Example B: Two numbers total 54 and one is twice the other. Let x be the smaller and y be the larger. Then x + y = 54 and y = 2x. Substitute: x + 2x = 54, so 3x = 54, x = 18 and y = 36. Check both relationships.";

        translation.StepByStepSolutions =
            "Step 1: Name the two unknown quantities, for example x and y. Step 2: Translate the first fact into a relationship. Step 3: Translate the second independent fact into another relationship. Step 4: Use both relationships together to find one unknown by elimination, substitution or structured reasoning. Step 5: Find the second unknown. Step 6: Substitute the pair back into both original relationships. If either fact fails, the pair is not a solution.";

        translation.CommonMistakes =
            "Do not solve only one relationship: many pairs can satisfy x + y = a fixed total. Do not assume the unknowns are equal unless the problem says so. Do not accept a pair just because it gives the correct total; it must also satisfy the second relationship. Keep the meaning of x and y consistent throughout the solution, and verify both equations independently.";

        translation.QuickSummary =
            "Two unknowns need two independent relationships. Represent both, solve them together, find both values, and verify the final pair in both original facts.";
    }

    private static void ApplyScaleReadingBuildCorrection(
        CanonicalLessonContentPackTranslation translation)
    {
        translation.Explanation =
            "This lesson is about reading a scale that is split into equal intervals. Start by looking at two labelled values. Find the total change between them, then divide by the number of equal spaces to find what one interval is worth. For example, from 20 to 60 across 4 equal intervals, the total change is 40 and each interval is 10. The marks are therefore 20, 30, 40, 50 and 60. Once you know the interval value, count from a labelled mark to the pointer. The lesson is Edulytics-authored from OGL material; Cambridge remains the academic reference authority and no Cambridge objective wording is reproduced here.";

        translation.KeyConceptsAndRules =
            "A scale uses equal spaces to represent equal numerical changes. Count spaces, not tick marks. Interval value = (higher labelled value − lower labelled value) ÷ number of equal intervals. Then move from a known label by that interval value until you reach the marked point. Keep the measurement unit with the answer.";

        translation.WorkedExamples =
            "Example A: A scale runs from 20 to 60 in 4 equal intervals. The change is 60 − 20 = 40. One interval is 40 ÷ 4 = 10. The marks are 20, 30, 40, 50, 60, so a pointer on the third interval after 20 shows 50. Example B: A scale runs from 10 to 30 in 5 equal intervals. The change is 20. One interval is 20 ÷ 5 = 4. The marks are 10, 14, 18, 22, 26, 30.";

        translation.StepByStepSolutions =
            "Step 1: Find two labelled values on the scale. Step 2: Count the equal spaces between them. Step 3: Subtract to find the total change. Step 4: Divide the total change by the number of spaces to find one interval. Step 5: Count intervals from a known label to the pointer. Step 6: Check that the answer lies between the surrounding labels and includes the correct unit.";

        translation.CommonMistakes =
            "Do not count tick marks when the question asks about intervals: 5 intervals have 6 boundary marks. Do not assume each small mark is worth 1. Do not read the pointer before finding the interval value.";

        translation.QuickSummary =
            "Find the value of one equal interval first, then count intervals from a known label to read the scale.";
    }

    private static void ApplyScaleReadingCorrection(
        CanonicalLessonContentPackTranslation translation)
    {
        translation.Explanation =
            "This Cambridge Primary Stage 6 supporting lesson develops the open DfE Year 6 ready-to-progress focus “Reading scales with 2, 4, 5 or 10 intervals”. A scale is divided into equal intervals. First find the total change between two labelled values, then divide by the number of equal intervals: interval value = (end value − start value) ÷ number of intervals. After that, count equal intervals from a known label to the pointer. The drawing, labels, interval count and answer must describe the same scale. The lesson is Edulytics-authored from OGL material; Cambridge remains the academic reference authority and no Cambridge objective wording is reproduced here.";

        translation.KeyConceptsAndRules =
            "Source focus: Reading scales with 2, 4, 5 or 10 intervals. Count the spaces between marks, not the number of tick marks. Calculate one interval as total change divided by 2, 4, 5 or 10 equal intervals. Read a pointer by starting from a known labelled value and adding or subtracting the interval value for each space moved. Keep the measurement unit attached to the value. A valid visual scale must use equal spacing and the same numerical interval value throughout.";

        translation.WorkedExamples =
            "Example A: A scale runs from 20 to 60 in 4 equal intervals. The total change is 40, so one interval is 40 ÷ 4 = 10. A pointer three intervals after 20 marks 50. Example B: A scale runs from 10 to 30 in 5 equal intervals. The total change is 20, so one interval is 20 ÷ 5 = 4. A pointer three intervals after 10 marks 22. Example C: A scale from 0 to 100 has 10 equal intervals, so each interval is 10; the seventh interval marks 70.";

        translation.StepByStepSolutions =
            "Step 1: Identify two known labelled values on the scale. Step 2: Count the equal intervals between them; intervals are spaces, not marks. Step 3: Find the total change by subtracting the lower labelled value from the higher one. Step 4: Divide the total change by the number of intervals to find one interval value. Step 5: Count intervals from a known label to the pointer and add or subtract that amount. Step 6: Check that the result lies in the correct position and has the correct unit.";

        translation.CommonMistakes =
            "Do not divide by the number of tick marks when the question gives the number of intervals. Five intervals have six boundary marks. Do not assume each small mark represents 1. Do not read the pointer before calculating the interval value. Check that equal visual spacing represents equal numerical change and that the final value is between the surrounding labelled values.";

        translation.QuickSummary =
            "For a scale with 2, 4, 5 or 10 equal intervals, find one interval with (end − start) ÷ intervals, then count intervals from a known label to the pointer and keep the unit.";
    }

    private static void ApplyFractionComparisonCorrection(
        CanonicalLessonContentPackTranslation translation)
    {
        translation.Explanation =
            "This Cambridge Primary Stage 6 supporting lesson develops the open DfE Year 6 ready-to-progress focus “Compare fractions with different denominators”. Fractions can be compared only by their values relative to the same whole. When denominators differ, rewrite the fractions as equivalent fractions with a common denominator, or use another valid representation such as equal-whole fraction bars or a number line. For example, 2/3 = 8/12 and 3/4 = 9/12, so 3/4 is larger. Equivalent forms change the numerator and denominator together without changing the fraction’s value. The lesson is Edulytics-authored from OGL material; Cambridge remains the academic reference authority and no Cambridge objective wording is reproduced here.";

        translation.KeyConceptsAndRules =
            "Source focus: Compare fractions with different denominators. The fractions must refer to the same-sized whole. Use a common denominator by making equivalent fractions, then compare the numerators. As a numerical check, cross-products can compare a/b and c/d by comparing a × d with c × b. Equal-whole fraction bars or a number line should preserve the same values shown by the calculation. Never decide which fraction is larger by looking at denominator digits alone.";

        translation.WorkedExamples =
            "Example A: Compare 2/3 and 3/4. A common denominator is 12: 2/3 = 8/12 and 3/4 = 9/12. Since 9/12 > 8/12, 3/4 is larger. Example B: Compare 3/5 and 5/8. A common denominator is 40: 3/5 = 24/40 and 5/8 = 25/40, so 3/5 < 5/8. Example C: A student says 3/8 > 1/2 because 8 > 2. This is false: 1/2 = 4/8, and 4/8 > 3/8. Equal-whole fraction bars or a number line show the same comparison.";

        translation.StepByStepSolutions =
            "Step 1: Check that the fractions describe the same whole. Step 2: Choose a useful common denominator, usually the least common multiple of the denominators, or use an equivalent visual representation. Step 3: Rewrite both fractions without changing their values. Step 4: Compare the new numerators because the denominators are now the same. Step 5: Write <, > or = and explain the comparison. Step 6: Check with fraction bars, a number line or cross-products when useful.";

        translation.CommonMistakes =
            "Do not assume the fraction with the larger denominator is larger; more equal parts make each part smaller. Do not compare only numerators when denominators differ. Do not change only the denominator when creating an equivalent fraction: multiply or divide numerator and denominator by the same non-zero factor. Make sure visual models use equal-sized wholes before comparing shaded parts.";

        translation.QuickSummary =
            "To compare fractions with different denominators, preserve each fraction’s value, create equivalent forms with a common denominator or use an equal-whole visual model, then compare the values and justify <, > or =.";
    }
}
