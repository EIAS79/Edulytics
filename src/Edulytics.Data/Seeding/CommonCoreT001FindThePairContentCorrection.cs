using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// T001/G1/U01/L06: source-checked repair of teacher discussion text
/// misused as a learner's worked arithmetic example.
/// The original source remains immutable. No certification is granted.
/// </summary>
public static class CommonCoreT001FindThePairContentCorrection
{
    public const string LessonCode = "PED:US-CCSS-MATH:G1:U01:L06";
    public const string CorrectedVersion = "usccss-t001-g1-u01-l06-find-pair-make-ten-v1";
    private const string SourceUrl =
        "https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-6/preparation.html";
    private const string OriginalSha =
        "cc81eae102a5baa7ae9e514546a024c319eecd4b705e4593d57947a1f3208a35";

    private const string OldConcepts =
        "Learning goals:\n• Add and subtract within 10 in a way that makes sense to you.\n\n" +
        "Key idea: Subtraction can describe taking away, comparing, or finding a missing part. " +
        "Check the result by relating it back to addition when that relationship fits.";
    private const string OldExamples =
        "Example 1: “We have been playing a lot of games to help us build fluency with addition and subtraction within 10. How can you continue working on addition and subtraction at home?” (I could use flash cards. I could play these games with my family.)";
    private const string OldSteps =
        "Step 1: Represent the quantities or objects in the task.\n" +
        "Step 2: Describe the relationship or pattern you notice.\n" +
        "Step 3: Use the lesson key idea to reach a conclusion.\n" +
        "Step 4: Check the conclusion against the original task.";
    private const string OldMistakes =
        "Do not choose subtraction only because two numbers appear. Identify whether the situation is taking away, comparing, or finding a missing part.";
    private const string OldSummary =
        "In this lesson, you will practice adding and subtracting within 10. Key idea: Subtraction can describe taking away, comparing, or finding a missing part. Check the result by relating it back to addition when that relationship fits.";

    public const string Concepts =
        "Learning goal: Add and subtract within 10 by finding pairs that make ten and explaining your strategy.\n" +
        "If you have 6, you need 4 more to make 10: 6 + 4 = 10. " +
        "The same relationship can be written 10 - 6 = 4. " +
        "Use two-color counters, a ten-frame, or number cards to check each pair.";
    public const string Examples =
        "Example 1 — Find the Pair: You draw card 6. Which card makes 10? " +
        "Four empty spaces on a ten-frame show 6 + 4 = 10.\n" +
        "Example 2 — Complete the pair: You have card 7. Three counters are needed: 7 + 3 = 10.\n" +
        "Example 3 — Use subtraction: All 10 counters are shown. Hide 2. " +
        "Eight remain: 10 - 2 = 8, so 8 + 2 = 10.\n" +
        "Example 4 — Check the pair: A card shows 0. " +
        "The matching card is 10 because 0 + 10 = 10.";
    public const string Steps =
        "Step 1: Read the number card; for example, 6.\n" +
        "Step 2: Put 6 counters on a ten-frame and count the empty spaces: 4.\n" +
        "Step 3: Record the pair as 6 + 4 = 10.\n" +
        "Step 4: Check the related subtraction equation 10 - 6 = 4.\n" +
        "Step 5: Repeat with a different card, explaining how you know the pair totals ten.";
    public const string Mistakes =
        "Mistake: choosing 3 as the matching card for 6. Check: 6 + 3 = 9, not 10.\n" +
        "Mistake: treating a classroom reflection question as a worked math example. " +
        "Instead show the two card quantities, their addition equation, and an independent check.";
    public const string Summary =
        "Find two numbers that make ten. For card 6, the missing part is 4: " +
        "6 + 4 = 10 and 10 - 6 = 4. Check with counters or a ten-frame.";

    private static readonly string[] OriginalFields =
        [OldConcepts, OldExamples, OldSteps, OldMistakes, OldSummary];
    private static readonly string[] CorrectedFields =
        [Concepts, Examples, Steps, Mistakes, Summary];

    public static bool IsTarget(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson) =>
        doc.PackCode == "US-CCSS-MATH" &&
        doc.VersionCode == "CCSSM-2010" &&
        lesson.LessonCode == LessonCode;

    private static void Verify(CanonicalLessonContentPackLesson lesson)
    {
        if(lesson.SourceUrl != SourceUrl ||
           !string.Equals(lesson.CanonicalBodySha256,OriginalSha,
               StringComparison.OrdinalIgnoreCase) ||
           string.Join("|",lesson.OutcomeCodes) != "CCSS:1.OA.C.5|CCSS:1.OA.C.6" ||
           lesson.Translations.Count != 1 ||
           lesson.Translations[0].CultureCode != "en")
            throw new InvalidOperationException(
                "T001 source/standard/body SHA mismatch: " + lesson.LessonCode);
    }

    private static string[] Fields(CurriculumLessonContentTranslation row) =>
        [row.KeyConceptsAndRules,row.WorkedExamples,row.StepByStepSolutions,
         row.CommonMistakes,row.QuickSummary];

    private static string[] Fields(CanonicalLessonContentPackTranslation row) =>
        [row.KeyConceptsAndRules,row.WorkedExamples,row.StepByStepSolutions,
         row.CommonMistakes,row.QuickSummary];

    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        if(doc.PackCode != "US-CCSS-MATH" || doc.VersionCode != "CCSSM-2010")return;
        foreach(var lesson in doc.Lessons)
        {
            if(!IsTarget(doc,lesson))continue;
            Verify(lesson);
            var t=lesson.Translations.Single();
            var values=Fields(t);
            if(values.SequenceEqual(CorrectedFields,StringComparer.Ordinal))continue;
            if(!values.SequenceEqual(OriginalFields,StringComparer.Ordinal))
                throw new InvalidOperationException(
                    "T001 find-pair correction refused partial/unreviewed content drift");
            t.KeyConceptsAndRules=Concepts;
            t.WorkedExamples=Examples;
            t.StepByStepSolutions=Steps;
            t.CommonMistakes=Mistakes;
            t.QuickSummary=Summary;
        }
    }

    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string prior) =>
        IsTarget(doc,lesson) ? CorrectedVersion : prior;

    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string version) =>
        IsTarget(doc,lesson) &&
        (version==CorrectedVersion ||
         version==CommonCoreRichContentCorrections.CorrectionContentVersion);

    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string version,
        IReadOnlyCollection<CurriculumLessonContentTranslation> persisted)
    {
        if(!CanUpgradeExisting(doc,lesson,version))
            throw new InvalidOperationException("T001 find-pair predecessor not approved");
        Verify(lesson);
        if(persisted.Count==0)return;
        if(persisted.Count!=1 || persisted.Single().CultureCode!="en")
            throw new InvalidOperationException("T001 find-pair stored translation drift");
        var actual=persisted.Single();
        var expected=lesson.Translations.Single();
        if(actual.Title!=expected.Title || actual.Explanation!=expected.Explanation)
            throw new InvalidOperationException("T001 unrelated stored content drift");
        var values=Fields(actual);
        if(!values.SequenceEqual(OriginalFields,StringComparer.Ordinal) &&
           !values.SequenceEqual(CorrectedFields,StringComparer.Ordinal))
            throw new InvalidOperationException("T001 stored content not an approved predecessor");
    }
}
