using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Exact original 2021 IM Grade 2 U01 L05 teaching correction:
/// seven physical digit cards, four chosen to build TWO 2-digit addends
/// near 50, and flexible place-value addition with sum up to 50.
/// </summary>
public static class CommonCoreG2Unit1AddWithin50SourceTeachingCorrection
{
    public const string Version = "us-ccss-g2-u01-l05-add50-source-v1";
    private const string LessonCode = "PED:US-CCSS-MATH:G2:U01:L05";
    private const string SourceUrl =
        "https://im.kendallhunt.com/k5/teachers/grade-2/unit-1/lesson-5/preparation.html";
    private const string SourceSha =
        "f7e2c3f27b641e5ebbb9f33e3a8b5544c51f25e4a4b4e14e1d2cc1d0400fa52e";

    private const string BeforeExplanation =
        "Learning goal: You will add within 50.\nYou added within 100 in grade 1.";
    private const string BeforeConcepts =
        "Key ideas:\n- Add within 50 in a way that makes sense to them\n" +
        "- You added within 100 in grade 1\n" +
        "- The addends are the quantities being combined and the sum is the resulting total; " +
        "the expression must match the representation.";
    private const string BeforeSteps =
        "Step 1: Identify the quantities being combined.\n" +
        "Step 2: Represent each quantity with objects, a drawing, number line, or expression.\n" +
        "Step 3: Write an addition relationship that matches the representation.\n" +
        "Step 4: Find the total using an efficient strategy.\n" +
        "Step 5: Check that the total matches the original quantities.";
    private const string BeforeMistakes =
        "Common mistake: counting an item twice or combining the wrong quantities. " +
        "Check the addends and verify the total.";
    private const string BeforeSummary =
        "You will add within 50. You added within 100 in grade 1.";

    private const string AfterExplanation =
        "Goal: Find sums no greater than 50 using any correct place-value or make-ten method.\n" +
        "In the original How Close Stage 3 game, start with seven digit cards; " +
        "choose exactly FOUR physical cards and arrange them into TWO two-digit numbers. " +
        "Write their addition equation and compare the result to a target of 50 " +
        "(the source recommends 50 at the start of Grade 2 rather than 100). " +
        "More than one choice may be equally close; explain why your arrangement works.\n" +
        "Next add within 50 using tens and ones, cubes, drawings, equations or mental reasoning. " +
        "A partner or teacher reviews your strategy explanation.";

    private const string AfterConcepts =
        "How Close Stage 3 uses 7 cards and selects 4 physical cards for two valid two-digit numbers; " +
        "the digit 0 cannot be a tens digit. A repeated digit printed on different cards is allowed.\n" +
        "The target for this lesson's round is 50. The original stage may use 100 later.\n" +
        "Compose a ten in the ones place: 38 + 6 = 30 + 14 = 44.\n" +
        "Add tens and ones by place value, regrouping when necessary: " +
        "27 + 15 = (20 + 10) + (7 + 5) = 42.\n" +
        "Different correct student methods are allowed; the same correct result is not proof of the reasoning used.";

    private const string AfterSteps =
        "Step 1: For How Close, show all seven digit cards. Choose exactly four DIFFERENT card positions. " +
        "Arrange them as two two-digit numbers; do not put zero in a tens position.\n" +
        "Step 2: Write an equation for the two-digit sum; find its distance from 50. " +
        "Try other legal arrangements and check whether a different choice is equally close.\n" +
        "Step 3: To add numbers within 50, split them into tens and ones, then add tens and ones separately.\n" +
        "Step 4: If the ones sum reaches 10, regroup 10 ones as 1 ten. " +
        "For 27 + 15, use 20 + 10 = 30 and 7 + 5 = 12, giving 42.\n" +
        "Step 5: Check the final total with cubes, a second method, or an inverse fact. " +
        "Show your solution using drawings, equations or words and explain it to your partner or teacher.";

    private const string AfterMistakes =
        "Mistake: using three cards and a target of 20 from the previous How Close stage; " +
        "this lesson uses FOUR of SEVEN and a target of 50.\n" +
        "Mistake: making a one-digit number from four cards, or using the SAME physical card in both numbers.\n" +
        "Mistake: treating duplicate printed digits on separate physical cards as the same card; " +
        "two six cards may both be selected if both were drawn.\n" +
        "Mistake: ignoring a new ten when 7 ones + 5 ones = 12 ones in 27 + 15.\n" +
        "Mistake: treating a single optimum arrangement as the only acceptable answer; equal-distance ties exist.";

    private const string AfterSummary =
        "Add numbers with totals up to 50 using place value or make ten. " +
        "In How Close, select four of seven digit cards, build two two-digit numbers " +
        "and compare their sum with 50, explaining your equation.";

    public static bool IsTarget(CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        document.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode &&
        document.VersionCode == "CCSSM-2010" &&
        document.ContentVersion == "phase30-common-core-rich-content-v1" &&
        lesson.LessonCode == LessonCode;

    private static void Guard(CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson)
    {
        if (!IsTarget(document, lesson) || lesson.SourceUrl != SourceUrl ||
            !string.Equals(lesson.CanonicalBodySha256, SourceSha,
                StringComparison.OrdinalIgnoreCase) ||
            !lesson.OutcomeCodes.SequenceEqual(new[] { "CCSS:2.NBT.B.5" }))
            throw new InvalidOperationException(
                "Unexpected source identity for G2 U01 L05; refusing override");
    }

    private static bool IsBefore(CanonicalLessonContentPackTranslation t) =>
        t.Explanation == BeforeExplanation &&
        t.KeyConceptsAndRules == BeforeConcepts &&
        t.StepByStepSolutions == BeforeSteps &&
        t.CommonMistakes == BeforeMistakes &&
        t.QuickSummary == BeforeSummary;

    private static bool IsAfter(CanonicalLessonContentPackTranslation t) =>
        t.Explanation == AfterExplanation &&
        t.KeyConceptsAndRules == AfterConcepts &&
        t.StepByStepSolutions == AfterSteps &&
        t.CommonMistakes == AfterMistakes &&
        t.QuickSummary == AfterSummary;

    private static bool IsBefore(CurriculumLessonContentTranslation t) =>
        t.Explanation == BeforeExplanation &&
        t.KeyConceptsAndRules == BeforeConcepts &&
        t.StepByStepSolutions == BeforeSteps &&
        t.CommonMistakes == BeforeMistakes &&
        t.QuickSummary == BeforeSummary;

    private static bool IsAfter(CurriculumLessonContentTranslation t) =>
        t.Explanation == AfterExplanation &&
        t.KeyConceptsAndRules == AfterConcepts &&
        t.StepByStepSolutions == AfterSteps &&
        t.CommonMistakes == AfterMistakes &&
        t.QuickSummary == AfterSummary;

    public static void Apply(CanonicalLessonContentPackDocument document)
    {
        foreach (var lesson in document.Lessons)
        {
            if (!IsTarget(document, lesson)) continue;
            Guard(document, lesson);
            var en = lesson.Translations.Single(t => t.CultureCode == "en");
            if (IsAfter(en)) continue;
            if (!IsBefore(en))
                throw new InvalidOperationException(
                    "G2 U01 L05 older lesson body differs; no overwrite");
            en.Explanation = AfterExplanation;
            en.KeyConceptsAndRules = AfterConcepts;
            en.StepByStepSolutions = AfterSteps;
            en.CommonMistakes = AfterMistakes;
            en.QuickSummary = AfterSummary;
        }
    }

    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson, string priorVersion) =>
        IsTarget(document, lesson) ? Version : priorVersion;

    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson, string existingVersion) =>
        IsTarget(document, lesson) &&
        existingVersion == CommonCoreRichContentCorrections.CorrectionContentVersion;

    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson, string existingVersion,
        IReadOnlyCollection<CurriculumLessonContentTranslation> persisted)
    {
        Guard(document, lesson);
        if (!CanUpgradeExisting(document, lesson, existingVersion))
            throw new InvalidOperationException("G2 U01 L05 requires original rich-content version");
        if (persisted.Count == 0) return; // JSON-backed metadata may persist no prose.
        if (persisted.Count != 1 || persisted.Single().CultureCode != "en")
            throw new InvalidOperationException("G2 U01 L05 persisted translation count unexpected");
        var old = persisted.Single();
        var now = lesson.Translations.Single(t => t.CultureCode == "en");
        if (old.Title != now.Title || old.WorkedExamples != now.WorkedExamples ||
            !(IsBefore(old) || IsAfter(old)))
            throw new InvalidOperationException(
                "G2 U01 L05 persisted prose hash/body mismatch; refusal required");
    }
}
