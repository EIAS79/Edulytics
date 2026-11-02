using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Original IM 2021 Grade 2 Unit 1 Lesson 4 source-alignment:
/// five-card How Close (choose three, target twenty), then flexible within-20
/// sums/differences. Exact source URL/body/standard and old learner-body
/// portions are required before any guarded replacement.
/// </summary>
public static class CommonCoreG2Unit1HowCloseSourceTeachingCorrection
{
    public const string Version = "us-ccss-g2-u01-l04-how-close-source-v1";
    private const string Code = "PED:US-CCSS-MATH:G2:U01:L04";
    private const string SourceUrl =
        "https://im.kendallhunt.com/k5/teachers/grade-2/unit-1/lesson-4/preparation.html";
    private const string CanonicalSourceSha =
        "069597e84a1a66f69940167ea431d2926cf83a2e214872693c6a270938d10720";

    private const string OldExplanation =
        "Learning goal: You will add and subtract within 20 in a way that makes sense to them.\n" +
        "The work in this lesson builds from grade 1, where you add and subtract within 20 with different strategies. " +
        "This is a chance to formatively assess how you find sums and differences in this range. " +
        "Connecting cubes organized in towers of 10 and singles should be made available for you to use, " +
        "however encourage you not to use them if you don’t need them.";

    private const string OldConcepts =
        "Key ideas:\n- Add and subtract within 20 in a way that makes sense to them\n" +
        "- The work in this lesson builds from grade 1, where you add and subtract within 20 with different strategies\n" +
        "- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.";

    private const string OldSteps =
        "Step 1: Identify the whole and the part being removed or compared.\n" +
        "Step 2: Represent the situation with objects, a drawing, number line, or equation.\n" +
        "Step 3: Write a subtraction relationship that matches the representation.\n" +
        "Step 4: Find the difference using an efficient strategy.\n" +
        "Step 5: Check the result with the original quantities or a related addition fact.";

    private const string OldMistakes =
        "Common mistake: mixing up the whole and the part being removed or compared. " +
        "Check that the subtraction relationship matches the representation.";

    private const string OldSummary =
        "You will add and subtract within 20 in a way that makes sense to them. " +
        "The work in this lesson builds from grade 1, where you add and subtract within 20 with different strategies.";

    private const string NewExplanation =
        "Goal: Add and subtract within 20, choosing and explaining a method that makes sense to you.\n" +
        "Warm-up: use a known sum to 10, such as 8 + 2, to work out 8 + 5 or 7 + 6.\n" +
        "How Close: draw five distinct digit cards from 0 through 9. Select exactly THREE cards, " +
        "add them and write an equation. Compare how far your sum is from 20; more than one combination " +
        "may be equally close. Explain why your choice is as close as you can make it.\n" +
        "Then solve addition and subtraction problems within 20 using cubes, number bonds, " +
        "drawings or mental strategies. Discuss another valid method with a partner.";

    private const string NewConcepts =
        "Choose 3 of 5 distinct digit cards, with the 10 card removed, to make a sum as close " +
        "as possible to target 20; compare absolute distances rather than blindly choosing the largest cards.\n" +
        "Make ten to add: 7 + 6 = 7 + 3 + 3 = 13.\n" +
        "Use a known ten to subtract: 17 − 9 = (17 − 7) − 2 = 8, or " +
        "9 + 1 + 7 = 17, so 17 − 9 = 8.\n" +
        "Different correct equations, drawings and explanations can show the same total or difference.";

    private const string NewSteps =
        "Step 1: For How Close, read all five distinct cards (0–9); choose three different cards, " +
        "and record their sum as a three-addend equation.\n" +
        "Step 2: Compare the chosen sum with 20. The closeness is the absolute difference from 20; " +
        "try alternative triples and look for another equally close solution.\n" +
        "Step 3: For addition within 20, split an addend to make ten (8 + 5 = 8 + 2 + 3 = 13).\n" +
        "Step 4: For subtraction within 20, count on from the smaller number or break a number " +
        "apart to reach ten (17 − 9 = 8).\n" +
        "Step 5: Show your thinking with a number equation, drawing, cubes or words, " +
        "and check the result. Explain the method to a partner or teacher; " +
        "this explanation is not proved by entering the numeric answer alone.";

    private const string NewMistakes =
        "Mistake: choosing two or four cards in How Close. The source activity requires exactly THREE of the FIVE cards.\n" +
        "Mistake: including a card labeled 10; this stage explicitly removes the 10 cards.\n" +
        "Mistake: comparing sums instead of distance from 20; a sum of 21 is closer than a sum of 18.\n" +
        "Mistake: marking a correct alternative triple wrong just because the teacher used a different triple.\n" +
        "Mistake: subtracting 17 − 9 as 17 − 7; to reach 9, use the full nine that was subtracted.";

    private const string NewSummary =
        "Add and subtract within 20 in more than one valid way, and use exactly three of " +
        "five cards to make a sum as close as possible to 20. Record your equation " +
        "and explain your chosen method.";

    public static bool IsTarget(CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        document.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode &&
        document.VersionCode == "CCSSM-2010" &&
        document.ContentVersion == "phase30-common-core-rich-content-v1" &&
        lesson.LessonCode == Code;

    private static void Guard(CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson)
    {
        if (!IsTarget(document, lesson) ||
            lesson.SourceUrl != SourceUrl ||
            !string.Equals(lesson.CanonicalBodySha256, CanonicalSourceSha,
                StringComparison.OrdinalIgnoreCase) ||
            !lesson.OutcomeCodes.SequenceEqual(new[] { "CCSS:2.OA.B.2" }))
            throw new InvalidOperationException(
                "G2 U01 L04 original source identity/outcomes drift");
    }

    private static bool IsPrior(CanonicalLessonContentPackTranslation t) =>
        t.Explanation == OldExplanation && t.KeyConceptsAndRules == OldConcepts &&
        t.StepByStepSolutions == OldSteps && t.CommonMistakes == OldMistakes &&
        t.QuickSummary == OldSummary;

    private static bool IsNew(CanonicalLessonContentPackTranslation t) =>
        t.Explanation == NewExplanation && t.KeyConceptsAndRules == NewConcepts &&
        t.StepByStepSolutions == NewSteps && t.CommonMistakes == NewMistakes &&
        t.QuickSummary == NewSummary;

    private static bool IsPrior(CurriculumLessonContentTranslation t) =>
        t.Explanation == OldExplanation && t.KeyConceptsAndRules == OldConcepts &&
        t.StepByStepSolutions == OldSteps && t.CommonMistakes == OldMistakes &&
        t.QuickSummary == OldSummary;

    private static bool IsNew(CurriculumLessonContentTranslation t) =>
        t.Explanation == NewExplanation && t.KeyConceptsAndRules == NewConcepts &&
        t.StepByStepSolutions == NewSteps && t.CommonMistakes == NewMistakes &&
        t.QuickSummary == NewSummary;

    public static void Apply(CanonicalLessonContentPackDocument document)
    {
        foreach (var lesson in document.Lessons)
        {
            if (!IsTarget(document, lesson)) continue;
            Guard(document, lesson);
            var english = lesson.Translations.Single(t => t.CultureCode == "en");
            if (IsNew(english)) continue;
            if (!IsPrior(english))
                throw new InvalidOperationException(
                    "G2 U01 L04 previously changed learning body; refuse overwrite");
            english.Explanation = NewExplanation;
            english.KeyConceptsAndRules = NewConcepts;
            english.StepByStepSolutions = NewSteps;
            english.CommonMistakes = NewMistakes;
            english.QuickSummary = NewSummary;
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
            throw new InvalidOperationException(
                "G2 U01 L04 only permits exact prior rich-content version");

        if (persisted.Count == 0) return; // Metadata-only JSON source.
        if (persisted.Count != 1 || persisted.Single().CultureCode != "en")
            throw new InvalidOperationException("G2 U01 L04 unexpected persisted translation set");
        var row = persisted.Single();
        var expected = lesson.Translations.Single(t => t.CultureCode == "en");
        if (row.Title != expected.Title || row.WorkedExamples != expected.WorkedExamples ||
            !(IsPrior(row) || IsNew(row)))
            throw new InvalidOperationException(
                "G2 U01 L04 persisted learner prose mismatches reviewed predecessor");
    }
}
