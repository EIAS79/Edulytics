using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>Locally archived IM2021 G2 U03 L07 stage2 estimates vs REAL cm measurements.</summary>
public static class CommonCoreG2Unit3EstimateMeasureStage2TeachingCorrection
{
    public const string Version="usccss-g2-u03-l07-stage2-estimate-before-cm-measure-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U03:L07";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-3/lesson-7/preparation.html";
    private const string Sha="89bf5ec0017086d9aaa8716d9bb43eaa2a70295ac7442d792c1503e6231e3091";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will practice estimating and measuring length as well as telling and solving math stories.\nThis lesson is optional because it is an opportunity for extra practice that not all classes may need. In the first activity, you learn stage 2 of the Estimate and Measure center, which was first introduced in grade 1. You estimate and measure the length of objects and find the difference between your estimate and the actual measurement.",
         "Key ideas:\n- Estimate and measure objects and find the difference between your estimate and the actual measurement\n- Tell and solve story problems\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
         "Example 1: Find the value of each expression mentally. \\(37 - 20\\) \\(37 - 21\\) \\(37 - 17\\) \\(37 - 16\\)\nApproach: “How could you use \\(37-20\\) to help you with \\(37 - 21\\) ?”",
         "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
         "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
         "You will practice estimating and measuring length as well as telling and solving math stories. This lesson is optional because it is an opportunity for extra practice that not all classes may need."],
        ["Grade2 Unit3 Lesson7 Center Day 1 (optional), IM K–5 2021. Warm-up Number Talk:37−20=17,37−21=16,37−17=20,37−16=21. Students explain the relationships (37−21 is one less than37−20). Activity1 introduces Estimate and Measure Stage2, in pairs with actual rulers showing CENTIMETERS and meter sticks. Students pick real classroom objects such as a pencil, glue, notebook, desk side or bulletin board, FIRST record a personal estimate and THEN use a centimeter tool aligned at zero to measure the real object. Partner estimates may differ. They record the named object, estimated cm, actually observed cm and, IF time, their numeric difference. The published center stage includes inch and foot materials in its broader design but teacher lesson states measure only in CENTIMETERS at this point; do not switch the unit without approval. Teacher synthesis example: notebook estimated20 cm, actual31 cm, so distance between them11 cm; this is a SOURCE illustrative data row, not a verified measurement made by any pupil. Activity2 optional choice time: Estimate and Measure Stage2 or previously introduced Math Stories Stage5; students choose twice and teacher observes authentic hands-on work. No source Cool-down is included in the public lesson; first-party Edulytics numerical check is independent.",
         "Every measurement is tied to an actual object, correct unit (centimeters in this lesson), a zero-aligned centimeter ruler or meterstick and proper endpoint. A numerical estimate is a prediction BEFORE physically measuring, not a score to match. If estimate e cm and actual m cm, distance or unsigned difference = |m−e| cm, regardless of which is larger. Source sample notebook estimate20 cm, measured31 cm gives31−20=11 cm. When actual is smaller than estimate, for example an ORIGINAL illustrative estimate45 cm and measured38 cm, the difference is45−38=7 cm; the negative number −7 is not a positive length difference. Do not automatically assign generic object lengths without measuring: real pencils and notebooks vary. Activity2 real center options are Estimate and Measure Stage2 and Math Stories Stage5, not a guessing-game that digitally certifies participation. The first-party numeric practice uses made-up but explicitly labeled example records, not source physical measurements. Addressing standards 2.MD.A.1,2.MD.A.3,2.NBT.B.5,2.OA.A.1.",
         "SOURCE Number Talk:37−20=17,37−21=16,37−17=20,37−16=21. SOURCE activity synthesis notebook: estimate20 cm, actual measurement31 cm, discrepancy11 cm. Actual source procedure: in PAIRS choose classroom object, tool in centimeters, make and record separate estimates, use proper measurement starting point and record actual value, compute gap when time. SOURCE choice center: Estimate and Measure Stage2 or Math Stories Stage5. FIRST-PARTY Edulytics illustrative data, not a classroom reading: notebook estimate45 cm, actual38 cm, gap7 cm; paper roll estimated24 cm, actual29 cm, gap5 cm; toy car estimated34 cm, actual32 cm, gap2 cm. The arbitrary first-party object-label values are not publisher source results.",
         "1. Mentally compute source subtraction17,16,20,21 and discuss how changing subtrahend by one changes the difference by one. 2. In real PAIRS choose a classroom object from genuine available items, such as a pencil, book or desktop; write its actual identity and select a centimeter ruler or meter stick. 3. BEFORE using the tool each partner states and records an independent estimate in centimeters, potentially different from their partner's. 4. Align the physical ruler's0 at the beginning of the object; ensure equal cm increments, no gaps/overlaps and record observed endpoint and actual measurement in cm. Teacher confirms placement and reading. 5. Compute gap by subtracting smaller of estimate and actual from larger. Source notebook20 estimate/31 actual ->11 cm; first-party illustrative45/38 ->7 cm. 6. Return the object and choose another if time, completing original stage2 sheet; teacher validates real rows, not digital numeric arithmetic. 7. During choice time select actual Estimate and Measure Stage2 or Math Stories Stage5, work with genuine partner and discuss choice; teacher observes center participation. 8. Finish independent Edulytics numeric gap examples; they must never masquerade as original pupil object measurements or a protected publisher Cool-down.",
         "Calculating37−21 as17 instead of16; it is one less than37−20. Estimating AFTER reading a ruler (not an estimate). Treating source notebook20 and31 as mandatory sizes for all real notebooks. Starting at ruler1 instead of0, confusing millimeters with centimeters, or leaving the unit blank. Reporting a negative value for absolute gap when actual is shorter than estimate. Measuring in inches or feet even though the Stage2 public teacher note explicitly restricts this session to centimeters. Inventing actual classroom participation based only on a typed illustrative arithmetic result. Claiming a fake numeric worksheet proves partner chose the Math Stories center.",
         "Optional Center Day1: Number Talk 37−20=17,37−21=16,37−17=20,37−16=21. Real Stage2 Estimate and Measure pairs select physical objects, estimate BEFORE measuring in CENTIMETERS, use correct zero-aligned ruler/meterstick, record actual cm and optional unsigned gap. Published example notebook20 cm estimated,31 cm measured, gap11 cm. Choice time: Estimate and Measure Stage2 or Math Stories Stage5. Independent digital checks do not certify actual physical measurement."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&d.VersionCode=="CCSSM-2010"&&
        d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.A.1","CCSS:2.MD.A.3","CCSS:2.NBT.B.5","CCSS:2.OA.A.1"}))
            throw new InvalidOperationException("G2 U03 L07 original source identity changed");
    }
    private static bool Same(CanonicalLessonContentPackTranslation t,bool after)=>
        Check([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Same(CurriculumLessonContentTranslation t,bool after)=>
        Check([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Check(string[] a,bool after)=>a.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Same(t,true))continue;
            if(!Same(t,false))throw new InvalidOperationException("G2 U03 L07 previous six fields changed");
            t.Explanation=S.After[0];t.KeyConceptsAndRules=S.After[1];
            t.WorkedExamples=S.After[2];t.StepByStepSolutions=S.After[3];
            t.CommonMistakes=S.After[4];t.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)?Version:before;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)&&before==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,before))throw new InvalidOperationException("G2 U03 L07 prior persistence version changed");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U03 L07 persisted translations changed");
        var t=rows.Single();var expected=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=expected.Title||!(Same(t,false)||Same(t,true)))
            throw new InvalidOperationException("G2 U03 L07 persisted prior fields changed");
    }
}
