using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Source-locked G1 U01 L05 repair: original goal requires both addition
/// and subtraction within 10, but the original body taught subtraction only.
/// Preserve the previously reviewed learner-voice correction. Never certify.
/// </summary>
public static class CommonCoreT001MixedAddSubtractContentCorrection
{
    public const string LessonCode="PED:US-CCSS-MATH:G1:U01:L05";
    public const string CorrectedVersion="usccss-t001-g1-u01-l05-add-and-subtract-mixed-v1";
    private const string SourceUrl=
        "https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-5/preparation.html";
    private const string OriginalSha=
        "cbf2eb665cc716f0363a75a9b7e9ab4b67fa223a9f5d852f008bb3b9ef1248ce";
    private const string OldExplanation=
        "In this lesson, you will add and subtract within 10. You add and subtract in a way that makes sense to you as they learn a new stage of the center, Check it Off, and then choose from other center activities introduced in previous lessons. You may use your knowledge of the counting sequence, count all, count on, or count back to find the sum or difference.";
    private const string NewExplanation=
        "In this lesson, you will add and subtract within 10. You will explore a new stage of the Check It Off center, then choose from familiar center activities. You can use the counting sequence, count all, count on, or count back to find the sum or difference.";
    private const string OldConcepts=
        "Learning goals:\n• Add and subtract within 10 in a way that makes sense to you.\n\nKey idea: Subtraction can describe taking away, comparing, or finding a missing part. Check the result by relating it back to addition when that relationship fits.";
    private const string OldExamples=
        "Example 1: The source task compares or removes 1 from 6. The resulting difference is 5. Therefore, 6 − 1 = 5.\n\nExample 2: The source task compares or removes 2 from 6. The resulting difference is 4. Therefore, 6 − 2 = 4.\n\nExample 3: The source task compares or removes 1 from 8. The resulting difference is 7. Therefore, 8 − 1 = 7.";
    private const string OldSteps=
        "Step 1: Identify the starting quantity 6 and the amount 1 being removed or compared.\nStep 2: Use subtraction to find the difference.\nStep 3: 6 − 1 = 5.\nStep 4: Check that the answer fits the original situation.";
    private const string OldMistakes=
        "Do not choose subtraction only because two numbers appear. Identify whether the situation is taking away, comparing, or finding a missing part.";
    private const string OldSummary=
        "In this lesson, you will add and subtract within 10. Key idea: Subtraction can describe taking away, comparing, or finding a missing part. Check the result by relating it back to addition when that relationship fits.";

    public const string Concepts=
        "Learning goal: Add AND subtract numbers within 10 using a ten-frame, number path, or counting strategy. " +
        "When two groups are combined, add to find the total. When a group is removed, subtract to find the amount left. " +
        "You may count all, count on, or count back; use the relationship between addition and subtraction to check.";
    public const string Examples=
        "Example 1 — Addition: Put out 3 counters and then 2 more. Count on: 4, 5. Write 3 + 2 = 5.\n" +
        "Example 2 — Subtraction: Put out 6 counters; remove 2. Count back: 5, 4. Write 6 - 2 = 4.\n" +
        "Example 3 — Addition with another total: Put out 7 counters, then 1. 7 + 1 = 8.\n" +
        "Example 4 — Related subtraction: From 8 counters remove 1. 8 - 1 = 7. Check 7 + 1 = 8.";
    public const string Steps=
        "Step 1: Decide whether objects are being combined (add) or removed (subtract).\n" +
        "Step 2: For addition 3 + 2, show 3 and 2 counters; count on to 5.\n" +
        "Step 3: For subtraction 6 - 2, show 6 counters and remove 2; count back to 4.\n" +
        "Step 4: Write the answer in the Check It Off record or number sentence.\n" +
        "Step 5: Check each result with objects or the related inverse fact.";
    public const string Mistakes=
        "Mistake: always subtracting just because two numbers are given. First determine whether you combine or take away.\n" +
        "Mistake: counting from zero instead of counting on from the starting addend. Count exactly the added steps.\n" +
        "Mistake: taking away one object but recording two removed. Use counters to check each subtraction step.";
    public const string Summary=
        "Add when groups combine, subtract when objects are removed. " +
        "Within 10, 3 + 2 = 5 and 6 - 2 = 4. Use a ten-frame and check with the inverse fact.";

    private static readonly string[] Previous=[OldConcepts,OldExamples,OldSteps,OldMistakes,OldSummary];
    private static readonly string[] Corrected=[Concepts,Examples,Steps,Mistakes,Summary];

    public static bool IsTarget(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson) =>
        doc.PackCode=="US-CCSS-MATH" && doc.VersionCode=="CCSSM-2010" &&
        lesson.LessonCode==LessonCode;

    private static void Verify(CanonicalLessonContentPackLesson lesson)
    {
        if(lesson.SourceUrl!=SourceUrl ||
           !string.Equals(lesson.CanonicalBodySha256,OriginalSha,StringComparison.OrdinalIgnoreCase) ||
           string.Join("|",lesson.OutcomeCodes)!="CCSS:1.OA.C.5|CCSS:1.OA.C.6" ||
           lesson.Translations.Count!=1 || lesson.Translations[0].CultureCode!="en")
            throw new InvalidOperationException("T001 L05 publisher source or content SHA drift.");
    }

    private static string[] Fields(string concepts,string examples,string steps,
        string mistakes,string summary) => [concepts,examples,steps,mistakes,summary];

    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        if(doc.PackCode!="US-CCSS-MATH"||doc.VersionCode!="CCSSM-2010")return;
        foreach(var lesson in doc.Lessons)
        {
            if(!IsTarget(doc,lesson))continue;
            Verify(lesson);
            var t=lesson.Translations.Single();
            if(t.Explanation!=NewExplanation)
                throw new InvalidOperationException("T001 L05 earlier learner-voice repair missing.");
            var original=Fields(t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,
                t.CommonMistakes,t.QuickSummary);
            if(original.SequenceEqual(Corrected,StringComparer.Ordinal))continue;
            if(!original.SequenceEqual(Previous,StringComparer.Ordinal))
                throw new InvalidOperationException("T001 L05 partial or unrelated content drift.");
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
        IsTarget(doc,lesson)?CorrectedVersion:prior;

    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string version) =>
        IsTarget(doc,lesson) &&
        (version==CorrectedVersion ||
         version==CommonCoreB01V9LearnerVoiceSharedRootCorrection.CorrectedVersion ||
         version==CommonCoreRichContentCorrections.CorrectionContentVersion);

    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string version,
        IReadOnlyCollection<CurriculumLessonContentTranslation> persisted)
    {
        if(!CanUpgradeExisting(doc,lesson,version))
            throw new InvalidOperationException("T001 L05 unknown earlier content version");
        Verify(lesson);
        if(persisted.Count==0)return;
        if(persisted.Count!=1||persisted.Single().CultureCode!="en")
            throw new InvalidOperationException("T001 L05 stored translation drift");
        var prior=persisted.Single();
        var expected=lesson.Translations.Single();
        if(prior.Title!=expected.Title ||
           (prior.Explanation!=OldExplanation && prior.Explanation!=NewExplanation))
            throw new InvalidOperationException("T001 L05 unreviewed lesson identity/explanation");
        var state=Fields(prior.KeyConceptsAndRules,prior.WorkedExamples,
            prior.StepByStepSolutions,prior.CommonMistakes,prior.QuickSummary);
        if(!state.SequenceEqual(Previous,StringComparer.Ordinal) &&
           !state.SequenceEqual(Corrected,StringComparer.Ordinal))
            throw new InvalidOperationException("T001 L05 stored content not approved old/new state");
    }
}
