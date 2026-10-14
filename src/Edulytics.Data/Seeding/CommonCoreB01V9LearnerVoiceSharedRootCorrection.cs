using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// V9 one shared learner-voice error family. Original teacher preparations
/// distinguish teacher-facing "them" from student-facing "you": the imported
/// explanatory bridge accidentally mixes both. Correct all two members as
/// a single source-guarded semantic group. This is not academic certification.
/// </summary>
public static class CommonCoreB01V9LearnerVoiceSharedRootCorrection
{
    public const string CorrectedVersion = "usccss-b01-v9-learner-voice-shared-root-v1";
    private sealed record Repair(string Code,string SourceUrl,string OriginalCanonicalSha,
        string OldExplanation,string NewExplanation,string[] Outcomes,string PriorVersion);
    private static readonly Repair[] Repairs = [
        new Repair("PED:US-CCSS-MATH:G1:U01:L04", "https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-4/preparation.html", "8806fa7a4977ea12d0c5bb3fe2dd20d8f04e440f4c64ca2ac4b4849234c7cbd4", "In this lesson, you will subtract 1 or 2 within 10. This builds on the previous lesson where you considered the number sequence to support them in adding 1 or 2. When you see that subtracting 1 or 2 from any number is the same as counting one or two less, or counting back one or two, they are observing an important structure of the count sequence (MP7, MP8).", "In this lesson, you will subtract 1 or 2 within 10. In the previous lesson, you used the number sequence to add 1 or 2. Now you can count back one or two steps to subtract 1 or 2, and notice how numbers change in the counting sequence (MP7, MP8).", new[] { "CCSS:1.OA.C.5", "CCSS:1.OA.C.6" }, CommonCoreG1SourceGroundedInstructionalCorrections.CountBackOneOrTwoVersion),
        new Repair("PED:US-CCSS-MATH:G1:U01:L05", "https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-5/preparation.html", "cbf2eb665cc716f0363a75a9b7e9ab4b67fa223a9f5d852f008bb3b9ef1248ce", "In this lesson, you will add and subtract within 10. You add and subtract in a way that makes sense to you as they learn a new stage of the center, Check it Off, and then choose from other center activities introduced in previous lessons. You may use your knowledge of the counting sequence, count all, count on, or count back to find the sum or difference.", "In this lesson, you will add and subtract within 10. You will explore a new stage of the Check It Off center, then choose from familiar center activities. You can use the counting sequence, count all, count on, or count back to find the sum or difference.", new[] { "CCSS:1.OA.C.5", "CCSS:1.OA.C.6" }, CommonCoreRichContentCorrections.CorrectionContentVersion)
    ];
    private static readonly IReadOnlyDictionary<string,Repair> ByCode =
        Repairs.ToDictionary(x=>x.Code,StringComparer.Ordinal);

    public static bool IsTarget(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson) =>
        doc.PackCode=="US-CCSS-MATH"&&doc.VersionCode=="CCSSM-2010"&&
        ByCode.ContainsKey(lesson.LessonCode);

    private static Repair VerifySource(CanonicalLessonContentPackLesson lesson)
    {
        var r=ByCode[lesson.LessonCode];
        if(lesson.SourceUrl!=r.SourceUrl ||
           !string.Equals(lesson.CanonicalBodySha256,r.OriginalCanonicalSha,
                StringComparison.OrdinalIgnoreCase) ||
           !lesson.OutcomeCodes.SequenceEqual(r.Outcomes) ||
           lesson.Translations.Count!=1 || lesson.Translations[0].CultureCode!="en")
            throw new InvalidOperationException("V9 learner voice original-source identity/SHA/outcome drift: "+lesson.LessonCode);
        return r;
    }
    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        if(doc.PackCode!="US-CCSS-MATH"||doc.VersionCode!="CCSSM-2010")return;
        foreach(var lesson in doc.Lessons.Where(l=>ByCode.ContainsKey(l.LessonCode)))
        {
            var r=VerifySource(lesson);
            var t=lesson.Translations[0];
            if(t.Explanation==r.NewExplanation)continue;
            if(t.Explanation!=r.OldExplanation)
                throw new InvalidOperationException("V9 learner voice partial/unreviewed learner text drift: "+lesson.LessonCode);
            t.Explanation=r.NewExplanation;
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string prior) =>
        IsTarget(doc,lesson)?CorrectedVersion:prior;

    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string version) =>
        IsTarget(doc,lesson)&&(version==ByCode[lesson.LessonCode].PriorVersion||
                               version==CorrectedVersion);

    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string version,
        IReadOnlyCollection<CurriculumLessonContentTranslation> persisted)
    {
        if(!CanUpgradeExisting(doc,lesson,version))
            throw new InvalidOperationException("V9 learner voice predecessor rejected");
        var r=VerifySource(lesson);
        if(persisted.Count==0)return;
        if(persisted.Count!=1||persisted.Single().CultureCode!="en")
            throw new InvalidOperationException("V9 learner voice persisted culture mismatch");
        var p=persisted.Single();
        var effective=lesson.Translations.Single();
        if(p.Explanation!=r.OldExplanation&&p.Explanation!=r.NewExplanation)
            throw new InvalidOperationException("V9 learner voice old/new persisted prose drift: "+lesson.LessonCode);
        if(p.Title!=effective.Title||p.WorkedExamples!=effective.WorkedExamples||
           p.StepByStepSolutions!=effective.StepByStepSolutions||
           p.CommonMistakes!=effective.CommonMistakes||
           p.KeyConceptsAndRules!=effective.KeyConceptsAndRules||
           p.QuickSummary!=effective.QuickSummary)
            throw new InvalidOperationException("V9 learner voice non-narrative persisted body drift: "+lesson.LessonCode);
    }
}