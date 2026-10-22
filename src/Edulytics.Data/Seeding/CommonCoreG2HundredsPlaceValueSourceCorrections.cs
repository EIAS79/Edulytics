using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Source-goal-locked, Edulytics-authored repair proposals for IM Grade 2,
/// Unit 5, lessons 1–3. Fixes mismatched/generic base-ten pedagogy.
/// Does NOT certify these lessons or enable shadow question families.
/// </summary>
public static class CommonCoreG2HundredsPlaceValueSourceCorrections
{
    public const string CorrectedContentVersion =
        "us-ccss-g2-u05-hundreds-source-goal-v1";

    private const string GenericSteps =
        "Step 1: Identify what the task asks you to find or show.\n" +
        "Step 2: Represent the important quantities or relationships.\n" +
        "Step 3: Apply the key idea from the lesson.\n" +
        "Step 4: Explain how the representation supports the result.\n" +
        "Step 5: Check that the result matches the question.";

    private const string IncorrectMeasurementSteps =
        "Step 1: Identify what needs to be measured.\n" +
        "Step 2: Choose the required unit and correct starting point.\n" +
        "Step 3: Place or interpret equal-size units accurately.\n" +
        "Step 4: Count or calculate the units and record the measure.\n" +
        "Step 5: Check the endpoint, scale, and unit label.";

    private sealed record Repair(
        string Code,
        string Url,
        string SourceContentSha256,
        string[] Outcomes,
        string PreviousSteps,
        string Concepts,
        string Examples,
        string Steps,
        string Mistakes);

    private static readonly Repair[] Repairs =
    [
        new(
            "PED:US-CCSS-MATH:G2:U05:L01",
            "https://im.kendallhunt.com/k5/teachers/grade-2/unit-5/lesson-1/preparation.html",
            "8b95f1c712928a475193eb9bb76a77698d724a68e85231a340721f7531f018a1",
            ["CCSS:2.NBT.A.1", "CCSS:2.NBT.A.2"],
            IncorrectMeasurementSteps,
            "A ten contains 10 ones. A hundred contains 10 tens or 100 ones.\n" +
            "Building 1 hundred means gathering ten complete groups of ten. " +
            "The value remains 100 whether you draw one hundred square, ten tens rods, " +
            "or one hundred individual dots.\n" +
            "We can exchange 10 tens for 1 hundred, and exchange 1 hundred back for 10 tens, without changing the number.",
            "Example 1 — Ten rods. You have 7 ten-rods and add 3 ten-rods. " +
            "There are 10 ten-rods altogether: 7 tens + 3 tens = 10 tens = 100 ones = 1 hundred.\n" +
            "Example 2 — Complete a hundred. Nine tens are 90. " +
            "Add ten ones (one more ten) to make 90 + 10 = 100.\n" +
            "Example 3 — Exchange the same value. Draw a hundred block next to ten tens rods. " +
            "Both represent 100, because 10 × 10 = 100.",
            "Step 1: Count how many tens rods are shown.\n" +
            "Step 2: When you have ten tens, join or trade them for one hundred block.\n" +
            "Step 3: Record the relationship 10 tens = 100 ones = 1 hundred.\n" +
            "Step 4: Check another representation by counting ten groups of ten.\n" +
            "Step 5: Explain why trading changes the blocks but not their total value.",
            "Mistake: thinking one ten is one hundred. One ten is 10, but ten tens make 100.\n" +
            "Mistake: counting nine rods as 100. Nine tens are 90; one more ten is needed.\n" +
            "Mistake: treating a hundred block and ten tens rods as different values. Both represent the same 100."),
        new(
            "PED:US-CCSS-MATH:G2:U05:L02",
            "https://im.kendallhunt.com/k5/teachers/grade-2/unit-5/lesson-2/preparation.html",
            "b007f773be8b6f7c7e1a91c85fbcf4c472c3f418dee2af93ac86de98451b6fba",
            ["CCSS:2.NBT.A.1", "CCSS:2.NBT.A.2"],
            GenericSteps,
            "Ten tens equal one hundred. Every new group of ten tens makes another hundred.\n" +
            "Multiples of one hundred can be represented using hundreds blocks, tens rods, " +
            "numbers, words, or equations. Two hundred is 20 tens, three hundred is 30 tens, " +
            "and eight hundred is 80 tens.\n" +
            "Count by hundreds: 100, 200, 300, 400, 500. The same values can be counted by tens.",
            "Example 1 — Make two hundreds. Build 20 tens rods. " +
            "Trade each group of ten rods for a hundred block: 20 tens = 2 hundreds = 200.\n" +
            "Example 2 — Make three hundreds. Build 30 tens. " +
            "Regroup 30 tens into 3 equal groups of 10 tens: 30 tens = 300.\n" +
            "Example 3 — Read and represent eight hundred. " +
            "80 tens are 8 groups of 10 tens, so 80 × 10 = 800. " +
            "Write 800 and draw eight hundred blocks.\n" +
            "Example 4 — One hundred and one ten. Eleven tens are 110, " +
            "or 1 hundred and 1 ten. This is NOT a multiple of 100.",
            "Step 1: Count the tens rods, for example 40 rods.\n" +
            "Step 2: Group the rods in sets of ten. Forty tens make 4 groups of ten tens.\n" +
            "Step 3: Trade each set for a hundred block: 4 hundreds = 400.\n" +
            "Step 4: Write the number in numerals and words: 400, four hundred.\n" +
            "Step 5: Check by skip-counting 100, 200, 300, 400, or by multiplying 40 × 10.",
            "Mistake: reading 30 tens as 30 rather than 300. Each of the 30 rods is worth ten.\n" +
            "Mistake: trading ten tens for ten hundreds. Ten tens equal only one hundred.\n" +
            "Mistake: calling 110 a multiple of 100. Eleven tens are one hundred and one ten."),
        new(
            "PED:US-CCSS-MATH:G2:U05:L03",
            "https://im.kendallhunt.com/k5/teachers/grade-2/unit-5/lesson-3/preparation.html",
            "0a79d7bc3df54dde4cf80c1994ab8668ded02660cdd94edd67562da0eb456b8e",
            ["CCSS:2.NBT.A.1", "CCSS:2.NBT.A.2", "CCSS:2.NBT.B.5"],
            GenericSteps,
            "A three-digit number is built from hundreds, tens, and ones. Each hundred is worth 100; " +
            "each ten is worth 10. For example, 342 = 300 + 40 + 2.\n" +
            "Trade 10 ones for 1 ten or 10 tens for 1 hundred without changing the total. " +
            "Regrouping can turn a collection with more than nine tens or ones into standard " +
            "hundreds-tens-ones form, using fewer base-ten blocks.",
            "Example 1 — Compose the same number with fewer blocks. " +
            "You have 2 hundreds, 28 tens, and 15 ones. " +
            "The value is 200 + 280 + 15 = 495. " +
            "Regroup 28 tens as 2 hundreds and 8 tens, then 15 ones as 1 ten and 5 ones. " +
            "You now have 4 hundreds, 9 tens, and 5 ones: 400 + 90 + 5 = 495.\n" +
            "Example 2 — Read three digits. Draw 3 hundred blocks, 4 tens rods, and 2 ones. " +
            "The number is 342 because 300 + 40 + 2 = 342.\n" +
            "Example 3 — Equivalent models. Two hundreds, 14 tens, and 2 ones represent " +
            "200 + 140 + 2 = 342, the same value as 3 hundreds, 4 tens, and 2 ones.",
            "Step 1: Count how many hundreds, tens, and ones are in the starting model.\n" +
            "Step 2: Compute its total: for 2 hundreds + 28 tens + 15 ones, use 200 + 280 + 15 = 495.\n" +
            "Step 3: Group every 10 ones into a ten and every 10 tens into a hundred.\n" +
            "Step 4: Record the regrouped model as 4 hundreds, 9 tens, 5 ones; write 495.\n" +
            "Step 5: Check the value: 400 + 90 + 5 = 495, unchanged after the trades.",
            "Mistake: treating 28 tens as 28 ones. Twenty-eight tens have value 280.\n" +
            "Mistake: writing 2 hundreds + 28 tens + 15 ones as the digits 22815. " +
            "Regroup and find the actual total, 495.\n" +
            "Mistake: thinking exchanging blocks changes the number. Only the representation changes.")
    ];

    public static IReadOnlyCollection<string> TargetLessonCodes =>
        Repairs.Select(x => x.Code).ToArray();

    private static Repair? Find(CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        document.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode &&
        document.VersionCode == "CCSSM-2010" &&
        document.ContentVersion == "phase30-common-core-rich-content-v1"
            ? Repairs.SingleOrDefault(x =>
                string.Equals(x.Code, lesson.LessonCode, StringComparison.Ordinal))
            : null;

    public static bool IsTarget(CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) => Find(document, lesson) is not null;

    private static void GuardIdentity(CanonicalLessonContentPackLesson lesson, Repair repair)
    {
        if (!string.Equals(lesson.SourceUrl, repair.Url, StringComparison.Ordinal) ||
            !string.Equals(lesson.CanonicalBodySha256,
                repair.SourceContentSha256, StringComparison.OrdinalIgnoreCase) ||
            !lesson.OutcomeCodes.SequenceEqual(repair.Outcomes))
            throw new InvalidOperationException(
                $"G2 U05 lesson source/content identity changed: {lesson.LessonCode}");
    }

    private static bool MatchesCorrection(CanonicalLessonContentPackTranslation translation,
        Repair repair) =>
        translation.KeyConceptsAndRules == repair.Concepts &&
        translation.WorkedExamples == repair.Examples &&
        translation.StepByStepSolutions == repair.Steps &&
        translation.CommonMistakes == repair.Mistakes;

    public static void Apply(CanonicalLessonContentPackDocument document)
    {
        if (document.PackCode != MathematicsCurriculumPackRegistry.CommonCoreCode ||
            document.VersionCode != "CCSSM-2010")
            return;

        foreach (var lesson in document.Lessons)
        {
            var repair = Find(document, lesson);
            if (repair is null) continue;
            GuardIdentity(lesson, repair);
            var english = lesson.Translations.Single(t => t.CultureCode == "en");
            if (MatchesCorrection(english, repair)) continue;
            // A later fully source-guarded, six-field correction supersedes the
            // legacy hundreds teaching after its approved predecessor transition.
            if (CommonCoreG2Unit5OriginalRichContentCorrections.IsFinalApplied(document,lesson))
                continue;
            if (!string.Equals(english.StepByStepSolutions,
                    repair.PreviousSteps, StringComparison.Ordinal) ||
                !english.KeyConceptsAndRules.Contains(
                    "Measurement units must be equal in size", StringComparison.Ordinal))
                throw new InvalidOperationException(
                    $"Unexpected prior learner content for {lesson.LessonCode}; correction refused.");
            english.KeyConceptsAndRules = repair.Concepts;
            english.WorkedExamples = repair.Examples;
            english.StepByStepSolutions = repair.Steps;
            english.CommonMistakes = repair.Mistakes;
        }
    }

    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson, string previousVersion) =>
        IsTarget(document, lesson) ? CorrectedContentVersion : previousVersion;

    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson, string existingVersion) =>
        IsTarget(document, lesson) &&
        string.Equals(existingVersion,
            CommonCoreRichContentCorrections.CorrectionContentVersion, StringComparison.Ordinal);

    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingVersion,
        IReadOnlyCollection<CurriculumLessonContentTranslation> persistedTranslations)
    {
        var repair = Find(document, lesson)
            ?? throw new InvalidOperationException("Not a G2 U05 reviewed correction target");
        GuardIdentity(lesson, repair);
        if (!CanUpgradeExisting(document, lesson, existingVersion))
            throw new InvalidOperationException(
                $"Unexpected persisted content version for {lesson.LessonCode}");

        // Metadata-only seed stores no translations. If translations are
        // present, refuse a surprising or partially changed learner body.
        if (persistedTranslations.Count == 0) return;
        if (persistedTranslations.Count != 1)
            throw new InvalidOperationException("Unexpected G2 U05 translation count");
        var current = persistedTranslations.Single();
        if (current.CultureCode != "en" ||
            current.StepByStepSolutions != repair.PreviousSteps ||
            !current.KeyConceptsAndRules.Contains(
                "Measurement units must be equal in size", StringComparison.Ordinal))
            throw new InvalidOperationException(
                $"Unrecognized prior persisted G2 U05 learner content: {lesson.LessonCode}");
    }
}
