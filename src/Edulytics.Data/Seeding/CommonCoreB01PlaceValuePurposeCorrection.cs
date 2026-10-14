using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// V9 source-verified B01 G2 U05 L06 correction: the original lesson goal is
/// number representation in unit form, expanded form and words, not ruler
/// measurement. Guarded against source/translation drift and idempotent.
/// Does not grant L1-L6 certification or source rights approval.
/// </summary>
public static class CommonCoreB01PlaceValuePurposeCorrection
{
    public const string LessonCode = "PED:US-CCSS-MATH:G2:U05:L06";
    public const string CorrectedVersion = "usccss-b01-g2-u05-l06-placevalue-purpose-independent-v2";
    private const string PreviousVersion = "usccss-b01-g2-u05-l06-placevalue-purpose-v1";
    private const string SourceUrl = "https://im.kendallhunt.com/k5/teachers/grade-2/unit-5/lesson-6/preparation.html";
    private const string SourceSha = "9d63ab9c21dc75ff605e8defc2f51386bd99ed8b3bce8c1f25af0319cd0f5e5e";
    private const string Title = "Represent Numbers in Different Ways";
    private const string Explanation = "Learning goal: You will represent numbers using unit form, base-ten numerals, expanded form, and words.\nIn previous lessons, you learned different ways that can be used to represent numbers. In this lesson, you are introduced to one more way to represent numbers, that is, using words. You gain more experience with numbers represented in all the different ways you have worked with so far.";
    private const string IndependentExplanation = "Learning goal: Show the same three-digit number in four ways. First build it from hundreds, tens, and ones. Write the base-ten numeral, separate each place into an addition expression, and say the number in words. Check all forms describe an identical quantity, including when one place has zero units.";
    private const string IndependentSummary = "Use hundreds, tens, and ones to connect a numeral, an expanded sum, unit form, and the number name. Check that the four forms all name the same value.";
    private const string Summary = "You will represent numbers using unit form, base-ten numerals, expanded form, and words. In previous lessons, you learned different ways that can be used to represent numbers.";
    private const string OldConcepts = "Key ideas:\n- Read, write, and represent three-digit numbers, including number names\n- In previous lessons, you learned different ways that can be used to represent numbers\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.";
    private const string OldExamples = "Example 1: Which one doesn’t belong? A Caption: A B Caption: B C Caption: C D Caption: D\nApproach: “Which representations best help you understand the value of the number? Explain.” (A has a lot of words to read, so I prefer C. D tells you how many of each. With B you just have to count the blocks.)\nExample 2: Fill in the blanks to represent 248 with words. two ___ forty- ___ Fill in the blanks to represent 562 with words. ___ hundred ___ - ___ Represent this number with words. Represent 627 with words. Represent \\(900 + 50 + 1\\) with words. Represent three hundred eighteen in two different ways.\nApproach: Choose the required unit, align equal units from the endpoint, measure without gaps or overlaps, and record the result with its unit.";
    private const string OldSteps = "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.";
    private const string OldMistakes = "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.";
    private const string NewConcepts = "Key ideas:\n- A three-digit numeral shows hundreds, tens, and ones by place value.\n- The same quantity can be written as unit form, base-ten numerals, expanded form, or words.\n- Example: 248 = 2 hundreds + 4 tens + 8 ones = 200 + 40 + 8 = two hundred forty-eight. Place-value regrouping preserves the quantity.";
    private const string NewExamples = "Example 1: Which one doesn’t belong? A Caption: A B Caption: B C Caption: C D Caption: D\nApproach: “Which representations best help you understand the value of the number? Explain.” (A has a lot of words to read, so I prefer C. D tells you how many of each. With B you just have to count the blocks.)\nExample 2: Fill in the blanks to represent 248 with words. two ___ forty- ___ Fill in the blanks to represent 562 with words. ___ hundred ___ - ___ Represent this number with words. Represent 627 with words. Represent \\(900 + 50 + 1\\) with words. Represent three hundred eighteen in two different ways.\nApproach: Read the hundreds, tens, and ones digits in each numeral. Build the expanded form from the place values, then write the same number in words. For example, 562 is five hundred sixty-two; 900 + 50 + 1 is nine hundred fifty-one.";
    private const string NewSteps = "Step 1: Read the three-digit numeral and identify its hundreds, tens, and ones digits.\nStep 2: Name the unit form (for 248: 2 hundreds, 4 tens, 8 ones).\nStep 3: Write its expanded form (200 + 40 + 8).\nStep 4: Write the number in words (two hundred forty-eight).\nStep 5: Check that each notation represents the same quantity, including any zero place-value digits.";
    private const string IndependentConcepts = "Key ideas:\n- A three-digit numeral records hundreds, tens, and ones.\n- Unit form counts the number of hundreds, tens, and ones; expanded form adds their values.\n- For example, 436 = 400 + 30 + 6 = four hundred thirty-six. All four representations describe the same amount.";
    private const string IndependentExamples = "Example 1: A collection contains 4 bundles of 100 straws, 3 bundles of 10 straws, and 6 single straws. Write its numeral, expanded form, and name.\nSolution: 436 = 400 + 30 + 6 = four hundred thirty-six. Unit form: 4 hundreds, 3 tens, 6 ones.\nExample 2: Represent 705 in words and unit form.\nSolution: Seven hundred five; 7 hundreds, 0 tens, 5 ones; 700 + 0 + 5 = 705. The zero tens must not be mistaken for 7 tens.";
    private const string IndependentSteps = "Step 1: Separate the given quantity into hundreds, tens, and ones.\nStep 2: Record each count in unit form, such as 4 hundreds, 3 tens, and 6 ones.\nStep 3: Write the standard numeral from those digits.\nStep 4: Add the place values for expanded form, and write the corresponding number name.\nStep 5: Confirm each form represents the same quantity; use 0 for a missing place-value digit.";
    private const string IndependentMistakes = "Mistake: writing 436 as 4 + 3 + 6. The expanded form is 400 + 30 + 6 because each digit has a place value.\nMistake: omitting the 0 in 705. There are no tens, but the numeral must still show the tens place.";
    private const string NewMistakes = "Mistake: ignoring place values when writing number words or expanded form (248 is not 20 + 40 + 8). Read and represent 2 hundreds, 4 tens, and 8 ones.\nMistake: omitting a zero place holder in a numeral such as 405. The four represents 400 and the five represents 5; there are zero tens.";

    public static bool IsTarget(CanonicalLessonContentPackDocument d, CanonicalLessonContentPackLesson l) =>
        d.PackCode == "US-CCSS-MATH" && d.VersionCode == "CCSSM-2010" && l.LessonCode == LessonCode;

    private static void Guard(CanonicalLessonContentPackLesson l)
    {
        if (l.SourceUrl != SourceUrl || !string.Equals(l.CanonicalBodySha256,SourceSha,StringComparison.OrdinalIgnoreCase) ||
            !l.OutcomeCodes.SequenceEqual(["CCSS:2.NBT.A.3"]) || l.Translations.Count != 1 ||
            l.Translations[0].CultureCode != "en")
            throw new InvalidOperationException("B01 G2 U05 L06 source, CCSS, edition or culture drift.");
    }
    // Stage 0 = uncorrected baseline, 1 = prior purpose-corrected body,
    // 2 = independently authored first-party learner explanation and examples.
    private static bool Matches(string title,string explanation,string summary,string concepts,
        string examples,string steps,string mistakes,int stage) =>
        title == Title &&
        explanation == (stage == 2 ? IndependentExplanation : Explanation) &&
        summary == (stage == 2 ? IndependentSummary : Summary) &&
        concepts == (stage == 2 ? IndependentConcepts : stage == 1 ? NewConcepts : OldConcepts) &&
        examples == (stage == 2 ? IndependentExamples : stage == 1 ? NewExamples : OldExamples) &&
        steps == (stage == 2 ? IndependentSteps : stage == 1 ? NewSteps : OldSteps) &&
        mistakes == (stage == 2 ? IndependentMistakes : stage == 1 ? NewMistakes : OldMistakes);

    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        if (d.PackCode != "US-CCSS-MATH" || d.VersionCode != "CCSSM-2010") return;
        foreach (var l in d.Lessons.Where(x => x.LessonCode == LessonCode))
        {
            Guard(l);
            var t = l.Translations[0];
            if (Matches(t.Title,t.Explanation,t.QuickSummary,t.KeyConceptsAndRules,
                t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,2)) continue;
            if (!Matches(t.Title,t.Explanation,t.QuickSummary,t.KeyConceptsAndRules,
                t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,0) &&
                !Matches(t.Title,t.Explanation,t.QuickSummary,t.KeyConceptsAndRules,
                t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,1))
                throw new InvalidOperationException("Refusing unreviewed partial place-value content rewrite.");
            t.Explanation = IndependentExplanation;
            t.QuickSummary = IndependentSummary;
            t.KeyConceptsAndRules = IndependentConcepts;
            t.WorkedExamples = IndependentExamples;
            t.StepByStepSolutions = IndependentSteps;
            t.CommonMistakes = IndependentMistakes;
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l,string previous) =>
        IsTarget(d,l) ? CorrectedVersion : previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l,string existing) =>
        IsTarget(d,l) && (existing == CommonCoreRichContentCorrections.CorrectionContentVersion ||
            existing == PreviousVersion || existing == CorrectedVersion);
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l,string prior,IReadOnlyCollection<CurriculumLessonContentTranslation> persisted)
    {
        if (!CanUpgradeExisting(d,l,prior))
            throw new InvalidOperationException("B01 place-value unexpected predecessor.");
        Guard(l);
        if (persisted.Count == 0) return;
        if (persisted.Count != 1 || persisted.Single().CultureCode != "en")
            throw new InvalidOperationException("B01 place-value unexpected persisted cultures.");
        var t = persisted.Single();
        if (!Matches(t.Title,t.Explanation,t.QuickSummary,t.KeyConceptsAndRules,t.WorkedExamples,
            t.StepByStepSolutions,t.CommonMistakes,0) &&
            !Matches(t.Title,t.Explanation,t.QuickSummary,t.KeyConceptsAndRules,t.WorkedExamples,
            t.StepByStepSolutions,t.CommonMistakes,1) &&
            !Matches(t.Title,t.Explanation,t.QuickSummary,t.KeyConceptsAndRules,t.WorkedExamples,
            t.StepByStepSolutions,t.CommonMistakes,2))
            throw new InvalidOperationException("B01 place-value persisted body drift.");
    }
}
