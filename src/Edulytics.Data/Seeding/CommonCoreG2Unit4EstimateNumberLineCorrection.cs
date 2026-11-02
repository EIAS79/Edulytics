using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;

/// <summary>Strict original-source SHA, CCSS alignment and six-field learner-content repair for PED:US-CCSS-MATH:G2:U04:L05.</summary>
public static class CommonCoreG2Unit4EstimateNumberLineCorrection
{
    public const string Version="usccss-g2-u04-l05-source-open-estimation-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U04:L05";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-5/preparation.html";
    private const string Sha="326ba1b74fa737e31a41d131bc0201009f2b33a2576c5fdcb288ce31cc9f80bb";
    private static readonly string[] Before=["Learning goal: You will use your understanding of length and the structure of a number line to estimate the location of a number on a number line.\nIn previous lessons, you estimated the length of objects using what you know about the size of standard length units and the tools used to measure them. You have located numbers on number lines using what you know about the structure of a number line and the labeled tick marks. The purpose of this lesson is for you to extend this understanding by estimating numbers on number lines that do not have tick marks to represent each consecutive whole number.",
        "Key ideas:\n- Use estimation to reason about the location of whole numbers on a number line\n- In previous lessons, you estimated the length of objects using what you know about the size of standard length units and the tools used to measure them\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
        "Example 1: What number could this be? Record an estimate that is: too low about right too high \\(\\phantom{\\hspace{2.3cm} \\\\ \\hspace{2.3cm}}\\) \\(\\phantom{\\hspace{2.3cm} \\\\ \\hspace{2.3cm}}\\) \\(\\phantom{\\hspace{2.3cm} \\\\ \\hspace{2.3cm}}\\) Record an estimate that is: too low about right too high \\(\\phantom{\\hspace{2.3cm} \\\\ \\hspace{2.3cm}}\\) \\(\\phantom{\\hspace{2.3cm}...\nApproach: “How did the second image help you revise your estimate?” (When I saw it was less than 20, but close to 20, I changed my about right estimate from 14 to 18.)\nExample 2: What number could this be? ___ What number could this be? ___ What number could this be? ___ What number could this be? ___ Which estimate are you most confident in? Why? Which estimate are you least confident in? Why?\nApproach: “What would help you to be more precise with your estimates?” (If I knew more numbers that were closer to the point. If the 10s and 5s were labeled.)",
        "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
        "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
        "You will use your understanding of length and the structure of a number line to estimate the location of a number on a number line. In previous lessons, you estimated the length of objects using what you know about the size of standard length units and the tools used to measure them."];
    private static readonly string[] After=["Original warm-up Estimation Exploration: teacher shows a SOURCE0..40 image with an unlabeled plotted point; students offer TOO LOW, ABOUT RIGHT, TOO HIGH guesses and revise after teacher marks midpoint20. The precise source point is deliberately NOT REVEALED, so there is no one required exact answer. Activity1 Estimate the Numbers: FOUR genuine graphical number lines, first30..60 by5 with dot BETWEEN50 and55; second0..30 by10 with dot BETWEEN20 and30; third0..100 with major50-step markers and unknown point; fourth0..60 with unknown point. Pupils reason about endpoints and intermediary benchmarks and discuss most/least confidence and why. Do not fabricate a unique publisher coordinate for any image. Activity2 Order the Numbers: a real group of THREE places TEN genuine publisher number cards on a shared chart-paper 0..40 axis with printed0,10,20,30,40; pupils revise approximate positions, then draw and label point locations and compare in an MLR7 gallery walk. Original card numbers are NOT present in public text, so do not invent them as publisher values. All actual source pictures, card positions and peer dialogue require teacher review; restricted Cool-down not copied.",
        "Estimation asks for a REASONABLE value or interval, not a manufactured exact original point. A dot between50 and55 on a numeric number line must be larger than50 and smaller than55; for a whole-number guess51..54. Between20 and30 allows21..29. A benchmark20 is exactly halfway from0 to40. Closer or additional benchmarks such as5,10 or20 help refine a reasonable range. If two values share equal physical intervals, number nearer right represents a larger value. An invented point at31 is not an asserted source image point. A properly scaled chart from0..40 has 40 equal unit intervals even when only multiples of10 printed. Real pupil-generated and publisher card points teacher-only.",
        "Warm-up public source intentionally leaves a plotted value0..40 OPEN; adding midpoint tick20 revises low/about-right/high estimates and DOES NOT reveal a unique exact answer. Activity1 source image descriptions: one point strictly BETWEEN50 and55, another strictly BETWEEN20 and30; images0..100 and0..60 have dots but printed text does not specify a justified exact value. Activity2 source task requires a chart0..40 labeled0,10,20,30,40 and TEN publisher number cards, whose contents cannot be filled from prose alone. FIRST-PARTY original example: point32 on newly authored0..40 graph is closer to30 than to40; point52 lies strictly between50 and55; 20 is midpoint between0 and40. These numbers refer ONLY to Edulytics problems, not source artwork.",
        "1. View authentic SOURCE Estimation Exploration0..40 figure with partner, choose too-low, about-right, too-high and JUSTIFY without asserting one correct numeric value. 2. After teacher draws a real tick20 at midpoint, revise estimates as evidence changes. 3. Analyze the authentic FOUR Activity1 publisher image scales:30..60 by5,0..30 by10,0..100 by50,0..60; for the first two dots use50<point<55 and20<point<30, and assess the other figures from true artwork rather than made-up pixel coordinates. 4. Explain which guess most/least confident and which added landmarks improve precision; human peer review required. 5. In real groups of3 use actual source TEN number cards to place and REVISE dots on chart0..40 printed at 0,10,20,30,40; complete actual MLR7 walk. 6. Independently solve Edulytics-created exact benchmark/midpoint/interval tasks with declared FIRST-PARTY coordinates, never claiming they are original hidden publisher dots.",
        "Treating every publisher estimate as a single exact graded point even though source leaves it open. Claiming52 or25 as definitely original image values when only bounded intervals are known. Assigning invented original TEN number-card values. Confusing an evenly scaled0..40 chart with just four gaps of one unit between labels0,10,20,30,40. Leaving a point to the left of its smaller benchmark, or refusing to revise with newly labelled midpoint20. Equating online exact benchmarks with actual drawing and teacher-reviewed group revision.",
        "IM2021 G2 U04 L05: original0..40 estimate/revise warm-up remains open-ended; four publisher plots with different benchmark granularity, first dot strictly between50/55 and second between20/30; group3 arranges genuine ten original source cards on0..40 chart, teacher reviews confidence and adjustments. Edulytics independently authored exact benchmark math is not a publisher image answer."];
    private static readonly string[] Standards=["CCSS:2.MD.B.6"];
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        d.VersionCode=="CCSSM-2010" &&
        d.ContentVersion=="phase30-common-core-rich-content-v1" && l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
           !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(Standards))
            throw new InvalidOperationException("G2 original 2.4.5 source identity drift");
    }
    private static bool Match(string[] current,bool updated)=>current.SequenceEqual(updated?After:Before);
    private static bool Same(CanonicalLessonContentPackTranslation t,bool updated)=>
        Match([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],updated);
    private static bool Same(CurriculumLessonContentTranslation t,bool updated)=>
        Match([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],updated);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Same(t,true))continue;
            if(!Same(t,false))throw new InvalidOperationException("G2 2.4.5 prior six fields drift");
            t.Explanation=After[0];t.KeyConceptsAndRules=After[1];
            t.WorkedExamples=After[2];t.StepByStepSolutions=After[3];
            t.CommonMistakes=After[4];t.QuickSummary=After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous)=>
        IsTarget(d,l)?Version:previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous)=>
        IsTarget(d,l)&&previous==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous,
        IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,previous))
            throw new InvalidOperationException("G2 2.4.5 persisted predecessor unknown");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 2.4.5 persisted language drift");
        var old=rows.Single();var current=l.Translations.Single(x=>x.CultureCode=="en");
        if(old.Title!=current.Title||!(Same(old,false)||Same(old,true)))
            throw new InvalidOperationException("G2 2.4.5 persisted prose conflict");
    }
}
