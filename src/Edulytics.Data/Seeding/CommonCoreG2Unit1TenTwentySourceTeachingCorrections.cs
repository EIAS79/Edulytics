using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// IM 2021 Grade 2 Unit 1 Lessons 2-3: source-specific ten- and twenty-cube
/// teaching corrections. No generic target matching, no arbitrary body overwrite.
/// Existing pupil/teacher prose not related to these exact defects is preserved.
/// </summary>
public static class CommonCoreG2Unit1TenTwentySourceTeachingCorrections
{
    public const string CorrectedContentVersion = "us-ccss-g2-u01-l02-l03-ten-twenty-v1";

    private sealed record Repair(
        string LessonCode, string SourceUrl, string CanonicalSourceBodySha,
        string OriginalConcepts, string OriginalSteps, string OriginalMistakes,
        string Concepts, string Steps, string Mistakes);

    private static readonly Repair[] Repairs =
    [
        new(
            "PED:US-CCSS-MATH:G2:U01:L02",
            "https://im.kendallhunt.com/k5/teachers/grade-2/unit-1/lesson-2/preparation.html",
            "6ebcd498b5dd493c5d40e1b523e1347aa1686f9a3e0e31894607a931c878e05a",
            "Key ideas:\n- Find the number that makes equations within 20 true\n- Write equations with unknown addends and sums of 10 and your related subtraction equations\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
            "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.",
            "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.",
            "The whole tower has exactly 10 connecting cubes. If 6 cubes are visible, 4 are hidden: 6 + 4 = 10.\nThe same two parts make related subtraction facts: 10 − 6 = 4 and 10 − 4 = 6.\nA ten-cube diagram can show a pair of differently colored parts. Match the missing addend and related subtraction equation to the two visible parts, not merely the broad topic name.",
            "Step 1: Identify the entire ten-cube tower as the whole; label visible cubes and hidden cubes as its two parts.\nStep 2: Record an unknown-addend equation (for example, 6 + ? = 10) BEFORE working out the hidden part.\nStep 3: Count on to ten or subtract the visible part: 10 − 6 = 4.\nStep 4: Check with the related addition fact 6 + 4 = 10; identify the same two parts in the cube tower.\nStep 5: For a tower-matching question, compare the actual part quantities, including their colors or positions, to the target equation.\nStep 6: With a partner, write more than one valid addition or subtraction equation for the same ten-cube tower; this authored work requires teacher observation.",
            "Mistake: using a total of 20 for the ten-cube activity. This lesson uses 10; extending to 20 is a later lesson.\nMistake: counting the visible cubes as the unknown hidden cubes. Find the complement to 10.\nMistake: assuming the equation 10 − 7 = 3 describes a tower with 7 blue cubes when the pictured tower labels the 7 cubes yellow. Keep the representation and part labels aligned."),
        new(
            "PED:US-CCSS-MATH:G2:U01:L03",
            "https://im.kendallhunt.com/k5/teachers/grade-2/unit-1/lesson-3/preparation.html",
            "d5112c52440a0b679890f5b03ab1e85ae54eb6f21d1877dfa95ea6215c8749bd",
            "Key ideas:\n- Find the number that makes equations within 20 true using the relationship between addition and subtraction\n- In this lesson, you apply the reasoning you used in previous lessons with addends that make 10 to find the number that makes an equations true that include the number 20\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
            "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.",
            "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.",
            "Start with two complete towers of ten, a total of 20 cubes. If 8 are shown, 12 are hidden: 8 + 12 = 20.\nAn unknown can occur in either addend or after a subtraction sign: 20 − ? = 4 requires ? = 16.\nMaking ten first is a useful strategy: 8 needs 2 to make 10, then one more ten gives 12 missing cubes.\nThe optional source challenge ? − 5 = 20 asks for 25 as the STARTING number; it is not a missing part of a total of 20.",
            "Step 1: Build or imagine two ten-cube towers and record the known part before solving (for example, 8 + ? = 20).\nStep 2: Use the relation between addition and subtraction to find the complement, or add to ten first then account for the second ten.\nStep 3: Check the result against the full 20 cubes: 8 + 12 = 20 and 20 − 8 = 12.\nStep 4: For 20 − ? = 4, find the unknown amount removed, 16; for 20 − 15 = ?, find the remaining amount, 5. Do not interchange positions of the blank.\nStep 5: Treat the optional extension ? − 5 = 20 differently: find a start of 25, then verify 25 − 5 = 20.\nStep 6: Write an addition equation with a blank for each physical partner round and explain the hidden quantity; original student writing and oral reasoning require teacher observation.",
            "Mistake: thinking two ten-cube towers still total 10. They contain 20 cubes.\nMistake: answering 4 in 20 − ? = 4 instead of the required unknown 16.\nMistake: rejecting ? − 5 = 20 because the correct start, 25, exceeds 20; the source identifies it as an optional challenge.")
    ];

    private static Repair? Find(CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        document.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode &&
        document.VersionCode == "CCSSM-2010" &&
        document.ContentVersion == "phase30-common-core-rich-content-v1"
            ? Repairs.SingleOrDefault(row => row.LessonCode == lesson.LessonCode)
            : null;

    public static bool IsTarget(CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        Find(document, lesson) is not null;

    private static Repair GetVerifiedRepair(CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson)
    {
        var repair = Find(document, lesson) ??
            throw new InvalidOperationException("Not a Grade 2 U01 L02/L03 repair");
        if (lesson.SourceUrl != repair.SourceUrl ||
            !string.Equals(lesson.CanonicalBodySha256, repair.CanonicalSourceBodySha,
                StringComparison.OrdinalIgnoreCase) ||
            !lesson.OutcomeCodes.SequenceEqual(new[] { "CCSS:2.OA.B.2" }))
            throw new InvalidOperationException("Grade 2 source identity / mapping drift: " +
                lesson.LessonCode);
        return repair;
    }

    private static bool IsOriginal(CurriculumLessonContentTranslation row, Repair repair) =>
        row.KeyConceptsAndRules == repair.OriginalConcepts &&
        row.StepByStepSolutions == repair.OriginalSteps &&
        row.CommonMistakes == repair.OriginalMistakes;

    private static bool IsCorrected(CurriculumLessonContentTranslation row, Repair repair) =>
        row.KeyConceptsAndRules == repair.Concepts &&
        row.StepByStepSolutions == repair.Steps &&
        row.CommonMistakes == repair.Mistakes;

    private static bool IsOriginal(CanonicalLessonContentPackTranslation row, Repair repair) =>
        row.KeyConceptsAndRules == repair.OriginalConcepts &&
        row.StepByStepSolutions == repair.OriginalSteps &&
        row.CommonMistakes == repair.OriginalMistakes;

    private static bool IsCorrected(CanonicalLessonContentPackTranslation row, Repair repair) =>
        row.KeyConceptsAndRules == repair.Concepts &&
        row.StepByStepSolutions == repair.Steps &&
        row.CommonMistakes == repair.Mistakes;

    public static void Apply(CanonicalLessonContentPackDocument document)
    {
        foreach (var lesson in document.Lessons)
        {
            var repair = Find(document, lesson);
            if (repair is null) continue;
            GetVerifiedRepair(document, lesson);
            var en = lesson.Translations.Single(x => x.CultureCode == "en");
            if (IsCorrected(en, repair)) continue;
            if (!IsOriginal(en, repair))
                throw new InvalidOperationException(
                    "Unexpected previous G2 U01 lesson prose: " + lesson.LessonCode);
            en.KeyConceptsAndRules = repair.Concepts;
            en.StepByStepSolutions = repair.Steps;
            en.CommonMistakes = repair.Mistakes;
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
        existingVersion == CommonCoreRichContentCorrections.CorrectionContentVersion;

    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingVersion,
        IReadOnlyCollection<CurriculumLessonContentTranslation> persistedTranslations)
    {
        var repair = GetVerifiedRepair(document, lesson);
        if (!CanUpgradeExisting(document, lesson, existingVersion))
            throw new InvalidOperationException(
                "G2 U01 ten/twenty repair requires exact predecessor version");
        if (persistedTranslations.Count == 0) return; // JSON-backed metadata only.

        if (persistedTranslations.Count != 1 ||
            persistedTranslations.Single().CultureCode != "en")
            throw new InvalidOperationException("Unexpected persisted G2 language count");

        var before = persistedTranslations.Single();
        var after = lesson.Translations.Single(x => x.CultureCode == "en");
        if (!(IsOriginal(before, repair) || IsCorrected(before, repair)) ||
            before.Title != after.Title ||
            before.Explanation != after.Explanation ||
            before.WorkedExamples != after.WorkedExamples ||
            before.QuickSummary != after.QuickSummary)
            throw new InvalidOperationException(
                "Persisted Grade 2 lesson body differs from verified predecessor: " +
                lesson.LessonCode);
    }
}
