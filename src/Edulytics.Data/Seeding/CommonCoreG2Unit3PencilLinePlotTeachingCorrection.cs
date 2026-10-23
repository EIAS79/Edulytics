using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>IM2021 locally archived G2 U03 L15 source Group C line plot with nonzero axis.</summary>
public static class CommonCoreG2Unit3PencilLinePlotTeachingCorrection
{
    public const string Version="usccss-g2-u03-l15-pencil-lineplot-nonzero-axis-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U03:L15";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-3/lesson-15/preparation.html";
    private const string Sha="bc28fa5862d09ad831fb3aeaa740f1871152cd7df821d348ebd157c4858ac2d9";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will create line plots to represent numerical data.\nIn an earlier lesson, you learned about the features of a line plot. You interpreted a line plot that represented data from measurements you made in inches. In this lesson, you measure the lengths of pencils in centimeters and represent the data on your own in a line plot.",
         "Key ideas:\n- Represent numerical data in a line plot\n- In an earlier lesson, you learned about the features of a line plot\n- Read category labels, values, and scales before comparing or calculating with a data display.",
         "Example 1: Find the value of each expression mentally. \\(47 - 20\\) \\(47 - 24\\) \\(36 - 10\\) \\(36 - 15\\)\nApproach: “How did the first expression help you find the value of the second expression?” (I already knew that \\(47 - 20 = 27\\) , so I just took away 4 more.)\nExample 2: Measure the pencils in centimeters. Work with a partner and check each other’s measurements. Record each measurement in the table. pencil length (cm) Create a line plot to represent the lengths of all the pencils in your group.\nApproach: “Why is it important to keep the Xs the same size and in rows?” (It can make it look like some measurements have more because the tower looks taller.)",
         "Step 1: Read the title, labels, categories, and scale.\nStep 2: Locate the values needed for the question.\nStep 3: Compare, add, subtract, or interpret the values as required.\nStep 4: State the result in the context of the data.",
         "Common mistake: reading the wrong category or scale. Check the labels and scale before using the values.",
         "You will create line plots to represent numerical data. In an earlier lesson, you learned about the features of a line plot."],
        ["IM K–5 2021 Grade 2 Unit3 Lesson15 Create Line Plots PLC Activity. Warm-up Number Talk: 47−20=27,47−24=23,36−10=26,36−15=21, highlighting how the first known subtraction supports the next. Activity1 Measure and Plot Pencil Lengths: groups of FOUR measure 10–12 real pencils with centimeters rulers, two DIFFERENT children checking each actual length; record each unique measured pencil in a table and construct a numerical line plot with TITLE, unit label 'pencil length (cm)', equal-size Xs and aligned vertical rows. True pencil lengths are not included in public lesson; never substitute invented lengths as measured real group results. Activity2 Plot Pencil Lengths: study two source line plots representing SAME dataset but with different starting tick labels; a numerical line plot MAY START ABOVE ZERO but the measurements still represent actual zero-based lengths. Consecutive whole-centimeter tick marks must be kept, INCLUDING labels with zero Xs. Source Group C pencil data: Andre12, Clare10, Diego10, Elena10, Han13, Jada12, Kiran14, Noah16, Priya14, Tyler13 (all centimeters). Line plot range10–16 must show blank 11 and15 ticks; counts10:3,11:0,12:2,13:2,14:2,15:0,16:1. Total10 pencils; mode10cm; longest16cm; shortest10cm. Use MLR8 to compare two correct plots with different axis starting points and explain why a zero-origin is NOT required. Source protected Cool-down Hand Spans of Mai’s Group not copied; separate Edulytics numerical exit.",
         "Numerical line plots show individual measurement counts: one X for ONE object measured, and one equal-size equally-spaced horizontal tick for EACH successive whole centimeter value in plotted span. You CAN start axis at10 (not at0) because tick labels still denote true ruler measurements from zero; never redefine10 as0. Source Group C data sorted:10,10,10,12,12,13,13,14,14,16cm. All consecutive ticks10,11,12,13,14,15,16 must appear; zeros at11 and15 mean no measured pencils, not missing labels. Largest STACK = most COMMON length10cm, whereas rightmost X = longest length16cm; total count all Xs10. To find how many pencils were MORE than12cm:2 at13+2 at14+0 at15+1 at16=5. Source Number Talk47−24=23 after47−20=27 then subtract4, and36−15=21 after36−10=26 then subtract5. Physical centimeters measuring from ruler zero, peer checks, authentic group dataset and original drawings require teacher confirmation. CCSS addressing2.MD.A.1,2.MD.D.9,2.NBT.B.5.",
         "PUBLIC SOURCE warm-up47−20=27;47−24=23;36−10=26;36−15=21. PUBLIC source Group C table: Andre12cm, Clare10cm, Diego10cm, Elena10cm, Han13cm, Jada12cm, Kiran14cm, Noah16cm, Priya14cm, Tyler13cm. Exact distribution10:XXX,11:(none),12:XX,13:XX,14:XX,15:(none),16:X. Source Lesson Synthesis: Diego is correct that MOST pencils were10cm long (3 Xs), and Kiran is correct that LONGEST pencil is16cm (rightmost X). Total X count10; shortest10cm; range16−10=6cm. FIRST-PARTY independent data, NOT authentic pencil group:11cm:2,12cm:1,13cm:3,14cm:2,15cm:1,16cm:1, total10, mode13cm, greatest16cm.",
         "1. Calculate source Number Talk27,23,26,21, explaining a second subtraction as ten subtraction plus another few ones. 2. In real groups of4 select10–12 genuine different pencils; at least two students measure EACH pencil to nearest centimeter from zero and agree on a true record table. 3. Make an authentic line plot with title, unit '(cm)', consecutive whole-centimeter ticks from a suitable start at or below the shortest data to at least the longest, and one equal-size aligned X for each measurement. 4. Study original display pair of same data on axes starting at different points; values retain their TRUE ruler lengths, so a line plot need not begin at0. 5. For PUBLIC original Group C data, organize 10,10,10,12,12,13,13,14,14,16 and label every tick10 through16, including EMPTY ticks11 and15. Stack Xs3 over10,2 over12,2 over13,2 over14 and1 over16. 6. Answer source synthesis: most common10cm; longest16cm; shortest10cm; total10 pencils, and5 were longer than12cm. 7. Use real MLR8 partner discussion to compare plots with different x-axis starts and why equal-size Xs matter; teacher reviews authentic explanations and actual group materials. 8. Work independently on first-party Edulytics sample measurement data, not the protected cool-down.",
         "Starting a line plot from0 despite having too few ticks to display the whole dataset; an appropriate NONZERO axis start is valid. Omitting 11 and15 labels because neither has an X; that distorts the whole-centimeter scale. Stacking unequal-sized X marks or using irregular spacing, making frequency appear falsely different. Confusing the modal length10cm (greatest X stack) with the longest pencil16cm (rightmost). Mistaking zero Xs at11cm as meaning zero centimeters of length. Asserting authentic class pencil measurements are the source Group C public table (Group C is provided to all students, real group pencils vary). Measuring in inches when source asks centimeters or measuring from ruler edge instead of zero. Treating numeric data correctness as proof that two children measured each pencil or MLR8 discussion occurred.",
         "G2 U03 L15: original mental subtraction27,23,26,21; real group of four measures10–12 pencils with centimeter rulers and two measurers per pencil; create equal-sized X line plot with title and units. Group C public table 10,10,10,12,12,13,13,14,14,16cm; line plot may start10 (not necessarily0), but must label missing-data ticks11 and15. Mode10cm, longest16cm, total10. Separate Edulytics fictional practice never replaces group measurements."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&d.VersionCode=="CCSSM-2010"&&
        d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.A.1","CCSS:2.MD.D.9","CCSS:2.NBT.B.5"}))
            throw new InvalidOperationException("G2 U03 L15 source identity drift");
    }
    private static bool Same(CanonicalLessonContentPackTranslation x,bool after)=>
        Equal([x.Explanation,x.KeyConceptsAndRules,x.WorkedExamples,x.StepByStepSolutions,x.CommonMistakes,x.QuickSummary],after);
    private static bool Same(CurriculumLessonContentTranslation x,bool after)=>
        Equal([x.Explanation,x.KeyConceptsAndRules,x.WorkedExamples,x.StepByStepSolutions,x.CommonMistakes,x.QuickSummary],after);
    private static bool Equal(string[] v,bool after)=>v.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var x=l.Translations.Single(t=>t.CultureCode=="en");
            if(Same(x,true))continue;
            if(!Same(x,false))throw new InvalidOperationException("G2 U03 L15 prior six learner fields changed");
            x.Explanation=S.After[0];x.KeyConceptsAndRules=S.After[1];
            x.WorkedExamples=S.After[2];x.StepByStepSolutions=S.After[3];
            x.CommonMistakes=S.After[4];x.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string old)=>
        IsTarget(d,l)?Version:old;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string old)=>
        IsTarget(d,l)&&old==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string old,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,old))throw new InvalidOperationException("G2 U03 L15 unapproved previous version");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U03 L15 previous translation rows");
        var t=rows.Single();var inc=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=inc.Title||!(Same(t,false)||Same(t,true)))
            throw new InvalidOperationException("G2 U03 L15 previous persisted learner text changed");
    }
}
