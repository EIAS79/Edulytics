using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>G2 U03 L17 optional original Stage1 line plot and source-less true classroom measurements.</summary>
public static class CommonCoreG2Unit3CenterDay3LinePlotsTeachingCorrection
{
    public const string Version="usccss-g2-u03-l17-optional-stage1-real-eight-objects-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U03:L17";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-3/lesson-17/preparation.html";
    private const string Sha="6325945a7675a6b84e32f9b97b7d3da28669a09cd10a70266b31a04df607cd92";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will practice adding, subtracting, and measuring length to create line plots.\nThis lesson is optional because it is an opportunity for extra practice that not all classes may need. In Activity 1, you learn stage 1 of the Creating Line Plots center. In this stage, called Inches and Centimeters, you You measure up to 8 objects to the nearest centimeter or inch.",
         "Key ideas:\n- Add and subtract within 100\n- Estimate and measure length of objects\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
         "Example 1: Find the value of each expression mentally. \\(60 - 10\\) \\(60 - 11\\) \\(60 - 21\\) \\(70 - 32\\)\nApproach: Choose the required unit, align equal units from the endpoint, measure without gaps or overlaps, and record the result with its unit.",
         "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
         "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
         "You will practice adding, subtracting, and measuring length to create line plots. This lesson is optional because it is an opportunity for extra practice that not all classes may need."],
        ["IM K–5 2021 Grade2 Unit3 Lesson17 Center Day3 is OPTIONAL, not a mandatory new measured dataset. Original Number Talk mentally subtracts60−10=50,60−11=49,60−21=39 and70−32=38; explain how known multiples-of-ten differences support further decomposition. Activity1 Introduce Creating Line Plots Stage1, Inches and Centimeters: in genuine PAIRS choose UP TO EIGHT actual objects, use centimeter or inch rulers from the zero mark, record each authentic measurement rounded to the nearest centimeter or inch, and construct an original line plot with a clear title, an accurate equally spaced single-unit numerical scale, the unit label and ONE X per real object. Partners take turns asking EACH OTHER TWO questions answerable from the graph; teacher checks their questions and responses. Actual classroom object lengths are not supplied by the source, so no invented sample must be represented as source measurements. Activity2 Centers Choice Time: learners choose from previously introduced Estimate and Measure (Stage2), Number Puzzles (Stages2–4) and Target Measurements (Stage1); partner may reuse REAL measured values in a Creating Line Plots follow-up. The optional nature and student choice are preserved. Lesson synthesis includes real partner feedback on something done well. FIRST-PARTY Edulytics demonstration with eight fictional objects is separate from real measured student data; any authentic participation requires educator observation.",
         "Measuring a physical object correctly starts at the ruler ZERO mark (not its outer edge), with no gaps, overlaps or inconsistent units. Use the nearest whole centimeter (cm) OR whole inch (in) for a single sample group; avoid mixing units on the same plot. A line plot's Xs show NUMBER OF OBJECTS per length value; the horizontal tick labels show length in the chosen unit. Consecutive whole-number tick labels must remain evenly spaced, including empty measurements. Use 1 X per object, so sum of all Xs equals total objects actually measured, at most EIGHT in the source Stage1 center. A plot can start at shortest measured length; it need not start at0 if every tick still denotes a true length from ruler zero. Teacher observes two peer-generated questions based on the real plot. First-party fictional Edulytics values9,10,10,11,12,12,13,14cm show8 items; min9, max14, range5cm; 4 items greater than11cm (12,12,13,14), frequency at10cm=2. This sample was NOT measured by actual source pupils. Source Number Talk50,49,39,38 illustrates tens-first subtraction. Official addressing 2.MD.A.1,2.MD.A.3,2.MD.A.4,2.MD.D.9,2.NBT.B.5,2.OA.B.2.",
         "PUBLIC source Number Talk:60−10=50;60−11=49;60−21=39;70−32=38. PUBLIC center rules: measure UP TO8 authentic objects, choose centimeters or inches, create a line plot, ask a real partner TWO questions using the Xs, and switch to a previously introduced center if desired. There is NO prescribed source table of eight classroom measurements. FIRST-PARTY Edulytics FICTIONAL sample in centimeters:9,10,10,11,12,12,13,14. Labels9–14 include all equal interval ticks. Frequencies9:1,10:2,11:1,12:2,13:1,14:1; total8, shortest9cm, longest14cm, range14−9=5cm, four objects longer than11cm. Original independent number exercise:80−23=57,91−35=56; not a publisher Cool-down.",
         "1. Do the source Number Talk50,49,39,38 and explain when a full ten is decomposed. 2. In a REAL pair select up to8 objects such as school pencils/crayons/math tools of differing lengths; choose an approved physical inch or centimeter ruler. 3. Hold the zero mark at each object's endpoint and measure to the nearest WHOLE selected unit; record actual item and unit values without guessing. 4. Sort the TRUE measurement values and plot a single equal-sized X per measured object, using an accurately scaled horizontal sequence of consecutive whole-number labels and a title and chosen unit. 5. Compare the plotted total of Xs with the true number of measured objects (≤8); do not add unmeasured fictional items. 6. Ask your real partner TWO plot questions that can be answered by counting specific X stacks or finding largest, smallest, or differences. Partner responds and alternates asking. Teacher checks honest original questions and authentic graph. 7. In source Choice Time choose real Estimate and Measure, Number Puzzles or Target Measurements; if appropriate reuse measurements for the Creating Line Plots center. 8. State something the real partner did well in lesson synthesis. 9. Separately complete Edulytics first-party fictional8-object numerical practice and independent exit; these are NOT the actual source measured dataset or evidence of participation.",
         "Using an inch value on a plot labeled centimeters, silently mixing units within a single dataset. Misreading the ruler by starting at its plastic END rather than the zero tick. Measuring more than eight objects and claiming the original Stage1 task required that count. Omitting zero-frequency tick numbers or stacking Xs of different dimensions, distorting the plot. Thinking tallest X STACK means longest physical object rather than greatest frequency, which is not the same as rightmost X. Counting two partners' copies of the SAME physical object twice. Claiming invented Edulytics sample numbers are source student measurements; the original source provides no fixed item lengths. Fabricating partner-generated questions, teacher verification, actual choice-time participation or positive peer feedback from typed numeric answers.",
         "Optional G2 U03 L17 Center Day3: Number Talk60−10=50,60−11=49,60−21=39,70−32=38. Creating Line Plots Stage1: with a real partner measure up to8 objects to the nearest whole cm OR inch from ruler zero, plot consecutive measurement labels with equal Xs and ask each other TWO true graph questions. Choice-time real Estimate and Measure, Number Puzzles or Target Measurements. Fictional eight-object Edulytics sample is clearly not real classroom data."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        d.VersionCode=="CCSSM-2010"&&d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
           !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.A.1","CCSS:2.MD.A.3","CCSS:2.MD.A.4","CCSS:2.MD.D.9","CCSS:2.NBT.B.5","CCSS:2.OA.B.2"}))
            throw new InvalidOperationException("G2 U03 L17 source identity changed");
    }
    private static bool Matches(CanonicalLessonContentPackTranslation t,bool newer)=>
        Match([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],newer);
    private static bool Matches(CurriculumLessonContentTranslation t,bool newer)=>
        Match([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],newer);
    private static bool Match(string[] a,bool after)=>a.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Matches(t,true))continue;
            if(!Matches(t,false))throw new InvalidOperationException("G2 U03 L17 prior learner fields changed");
            t.Explanation=S.After[0];t.KeyConceptsAndRules=S.After[1];
            t.WorkedExamples=S.After[2];t.StepByStepSolutions=S.After[3];
            t.CommonMistakes=S.After[4];t.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)?Version:before;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string old)=>
        IsTarget(d,l)&&old==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string old,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,old))throw new InvalidOperationException("G2 U03 L17 wrong prior version");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U03 L17 previous rows changed");
        var t=rows.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=incoming.Title||!(Matches(t,false)||Matches(t,true)))
            throw new InvalidOperationException("G2 U03 L17 persisted learner fields drifted");
    }
}
