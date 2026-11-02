using System.Security.Cryptography;
using System.Text;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// One-lesson, source-grounded Grade 1 curricular correction (G1 U01 L01 only).
/// The embedded 2021 first-edition pack is immutable; reject unexpected source,
/// canonical-body or previously stored learner prose. No certification is implied.
/// </summary>
public static class CommonCoreG1CountAndAddPilotCorrection
{
    public const string LessonCode = "PED:US-CCSS-MATH:G1:U01:L01";
    public const string CorrectedVersion = "us-ccss-g1-u01-l01-count-collections-dots-v1";

    private const string SourceUrl =
        "https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-1/preparation.html";
    private const string SourceCanonicalSha =
        "89f1b716ffccfec4511ce19d3a1fb433ea06db8577f3357f00769a030e4a6bb0";

    private const string OldTitle = "Count and Add";
    private const string OldExplanation =
        "In this lesson, you will count objects and relate counting to addition. " +
        "You revisit counting collections, which is an activity introduced in kindergarten. " +
        "You count and represent objects in a way that makes sense to you. " +
        "You then count dots on two dot cubes to find the total.";
    private const string OldConcepts =
        "Learning goals:\n• Count and represent a collection of objects.\n" +
        "• Relate counting to addition.\n\nKey idea: Addition combines quantities. " +
        "You can count on, use a known fact, or decompose a number when that helps. " +
        "Check that the sum is reasonable for the quantities.";
    private const string OldExamples =
        "Example 1: The source task combines 5 and 3. Start at 5 and count on 3 more " +
        "to reach 8. Therefore, 5 + 3 = 8.";
    private const string OldSteps =
        "Step 1: Identify the two quantities, 5 and 3.\n" +
        "Step 2: Start at 5 and count on 3 more, or combine the two groups.\n" +
        "Step 3: The total is 8, so 5 + 3 = 8.\n" +
        "Step 4: Check that the result matches the quantities and the lesson goal.";
    private const string OldMistakes =
        "When counting on, do not recount the starting quantity from 1. " +
        "Start with the first addend and count only the additional amount.";
    private const string OldSummary =
        "In this lesson, you will count objects and relate counting to addition. " +
        "Key idea: Addition combines quantities. You can count on, use a known fact, " +
        "or decompose a number when that helps. Check that the sum is reasonable for the quantities.";

    public const string NewConcepts =
        "Learning goals:\n• Count a collection of up to 20 objects accurately and show your count " +
        "with a drawing, marks or a numeral.\n• Relate counting the dots on two dot cubes " +
        "to an addition expression.\n\nCount every object once; move each counted object " +
        "aside or mark it so none is missed or counted twice. You may organize counters on a " +
        "ten-frame. When two groups are joined, their total can be shown with an addition " +
        "expression such as 5 + 3.";

    public const string NewExamples =
        "Example 1 — Count a collection and show the number. You have 16 counters. " +
        "Move each counter into a counted group while saying the numbers 1 through 16, " +
        "one number per counter. Show ten in a filled ten-frame and six more outside it; " +
        "draw 16 marks or write 16. Check by counting the two groups: 10 and 6 make 16.\n" +
        "Example 2 — Add the dots on two cubes. One cube shows 5 dots and another shows " +
        "3 dots. Count all eight dots, or start at 5 and count three more: 6, 7, 8. " +
        "Record 5 + 3 = 8. Both methods show the same eight dots.\n" +
        "Example 3 — Try another pair. Cubes show 4 dots and 2 dots. " +
        "Count all or count on: 5, 6. Write 4 + 2 = 6. " +
        "For the Add to 10 game, roll again if the total is greater than 10.";

    public const string NewSteps =
        "Step 1: For a collection of up to 20 objects, touch or move each object " +
        "exactly once while saying one counting number per object.\n" +
        "Step 2: Show how many you counted by writing the total or drawing " +
        "a mark for every object; group ten on a ten-frame if helpful.\n" +
        "Step 3: Roll two dot cubes and look at the dots on each cube; " +
        "for example, one shows 5 and the other 3.\n" +
        "Step 4: Find the total by counting all dots or counting on from 5: " +
        "6, 7, 8. Write the matching expression 5 + 3 = 8.\n" +
        "Step 5: Check that your drawing, counters or counted dots match the " +
        "total. In the Add to 10 game, roll again when the total exceeds 10.";

    public const string NewMistakes =
        "Mistake: count an object twice or skip one. Correction: move each counted " +
        "object into a separate group and say one number for each object.\n" +
        "Mistake: say 5, 6, 7 when adding 3 to 5. Correction: keep 5 as the " +
        "starting amount and count three new dots: 6, 7, 8.\n" +
        "Mistake: write 5 + 3 = 7 despite eight dots. Correction: " +
        "recount the dots or check using a ten-frame.";

    public const string NewSummary =
        "Count every object exactly once and show the total using a number, drawing " +
        "or ten-frame. To add two dot cubes, you may count all the dots or count on. " +
        "For cubes showing 5 and 3, count on 6, 7, 8 and write 5 + 3 = 8.";

    public static bool IsTarget(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        document.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode &&
        document.VersionCode == "CCSSM-2010" &&
        lesson.LessonCode == LessonCode;

    public static void Apply(CanonicalLessonContentPackDocument document)
    {
        if (document.PackCode != MathematicsCurriculumPackRegistry.CommonCoreCode ||
            document.VersionCode != "CCSSM-2010") return;

        foreach (var lesson in document.Lessons.Where(x => x.LessonCode == LessonCode))
        {
            if (lesson.SourceUrl != SourceUrl ||
                !string.Equals(lesson.CanonicalBodySha256, SourceCanonicalSha,
                    StringComparison.OrdinalIgnoreCase) ||
                !lesson.OutcomeCodes.SequenceEqual(["CCSS:1.OA.C.5", "CCSS:1.OA.C.6"]))
                throw new InvalidOperationException(
                    "G1 Count and Add source, hash or standard mapping drift.");

            var english = lesson.Translations.SingleOrDefault(x => x.CultureCode == "en")
                ?? throw new InvalidOperationException("G1 Count and Add English content missing.");

            if (MatchesNew(english)) continue; // idempotent
            if (!MatchesOld(english))
                throw new InvalidOperationException(
                    "Refusing partial, unexpected or previously edited Count and Add learner text.");

            english.KeyConceptsAndRules = NewConcepts;
            english.WorkedExamples = NewExamples;
            english.StepByStepSolutions = NewSteps;
            english.CommonMistakes = NewMistakes;
            english.QuickSummary = NewSummary;
        }
    }

    private static bool MatchesOld(CanonicalLessonContentPackTranslation t) =>
        t.Title == OldTitle && t.Explanation == OldExplanation &&
        t.KeyConceptsAndRules == OldConcepts && t.WorkedExamples == OldExamples &&
        t.StepByStepSolutions == OldSteps && t.CommonMistakes == OldMistakes &&
        t.QuickSummary == OldSummary;

    private static bool MatchesNew(CanonicalLessonContentPackTranslation t) =>
        t.Title == OldTitle && t.Explanation == OldExplanation &&
        t.KeyConceptsAndRules == NewConcepts && t.WorkedExamples == NewExamples &&
        t.StepByStepSolutions == NewSteps && t.CommonMistakes == NewMistakes &&
        t.QuickSummary == NewSummary;

    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson, string priorVersion) =>
        IsTarget(document, lesson) ? CorrectedVersion : priorVersion;

    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson, string existingVersion) =>
        IsTarget(document, lesson) &&
        existingVersion == CommonCoreRichContentCorrections.CorrectionContentVersion;

    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingVersion,
        IReadOnlyCollection<CurriculumLessonContentTranslation> persistedTranslations)
    {
        if (!CanUpgradeExisting(document, lesson, existingVersion))
            throw new InvalidOperationException(
                "G1 pilot repair requires exact preceding canonical content version.");

        if (persistedTranslations.Count == 0) return; // metadata-only legacy records
        if (persistedTranslations.Count != 1 ||
            persistedTranslations.Single().CultureCode != "en")
            throw new InvalidOperationException("Unexpected persisted translation cultures.");

        var persisted = persistedTranslations.Single();
        var existingBodyHash = BodyHash(persisted.CultureCode, persisted.Title,
            persisted.Explanation, persisted.KeyConceptsAndRules, persisted.WorkedExamples,
            persisted.StepByStepSolutions, persisted.CommonMistakes, persisted.QuickSummary);
        var oldBodyHash = BodyHash("en", OldTitle, OldExplanation, OldConcepts,
            OldExamples, OldSteps, OldMistakes, OldSummary);

        var updated = lesson.Translations.Single(x => x.CultureCode == "en");
        var newBodyHash = BodyHash(updated.CultureCode, updated.Title,
            updated.Explanation, updated.KeyConceptsAndRules, updated.WorkedExamples,
            updated.StepByStepSolutions, updated.CommonMistakes, updated.QuickSummary);
        if (existingBodyHash != oldBodyHash && existingBodyHash != newBodyHash)
            throw new InvalidOperationException(
                "Refusing Count and Add upgrade: stored learner prose differs from reviewed versions.");
    }

    private static string BodyHash(params string[] values) =>
        Convert.ToHexString(SHA256.HashData(
            Encoding.UTF8.GetBytes(string.Join("\n", values) + "\n")))
            .ToLowerInvariant();
}
