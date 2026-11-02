using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;

/// <summary>Strict original-source SHA, CCSS alignment and six-field learner-content repair for PED:US-CCSS-MATH:G2:U04:L04.</summary>
public static class CommonCoreG2Unit4CompareNumberLinesCorrection
{
    public const string Version="usccss-g2-u04-l04-source-line-comparison-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U04:L04";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-4/preparation.html";
    private const string Sha="d2482530d3b7c83eb48bb27c7e6f95cd438ce7f6d8e077089a7c7fb33b6ee11c";
    private static readonly string[] Before=["Learning goal: You will recognize that on a number line the numbers increase to the right and decrease to the left.\nIn previous lessons, you learned how to locate a number on the number line and represent numbers with labeled tick marks and points. You used multiples of 5 and 10 to help them locate numbers up to 100 on a number line. In this lesson, you recognize that as you move to the right on the number line, numbers increase in value because you are a greater distance from 0.",
        "Key ideas:\n- Recognize that on a number line, the numbers increase to the right and decrease to the left\n- In previous lessons, you learned how to locate a number on the number line and represent numbers with labeled tick marks and points\n- Compare actual values or measured attributes, not the visual size or position of a drawing.",
        "Example 1: Find the value of each expression mentally. \\(35 - 5\\) \\(35 - 10\\) \\(35 - 15\\) \\(35 - 25\\)\nApproach: “How did the first 2 expressions help you think about the last 2?” (I know that \\(5 - 5 =0\\) because all of the ones are being taken away. Then I just take the tens from the tens.)\nExample 2: Partner A: Roll 3 number cubes and find the sum. Put a counter on the location of the sum. Partner B: Roll 3 number cubes and find the sum. Put a counter on the location of the sum on the same number line. Decide which number is greater and explain. Use \\(<\\) , \\(>\\) , or \\(=\\) to compare the 2 numbers represented on your number line. Partner A \\(<\\) ,...\nApproach: “What do you notice about the numbers that are farther to the right?” (You were greater. You represent a greater length from zero.)",
        "Step 1: Identify the quantities or expressions to compare.\nStep 2: Represent or calculate each value accurately.\nStep 3: Decide whether one is greater, less, or equal.\nStep 4: Write or explain the comparison.\nStep 5: Recheck both values before finalizing.",
        "Common mistake: deciding from appearance instead of actual values. Represent or calculate both quantities before comparing.",
        "You will recognize that on a number line the numbers increase to the right and decrease to the left. In previous lessons, you learned how to locate a number on the number line and represent numbers with labeled tick marks and points."];
    private static readonly string[] After=["Number Talk Subtract Fives: 35−5=30,35−10=25,35−15=20,35−25=10. Activity1 each pair uses the ACTUAL 0..20 equally spaced line made in Lesson1, rolls THREE number cubes for partner A and THREE for partner B; put both actual sums as counters on the SAME paper line and compare using <,> or =. Numbers to right represent larger lengths from zero, and equal sums can coincide. Activity2 each partner rolls TWO cubes to MAKE a two-digit number on a new0..100 line with only multiples of5 pre-labeled; compare near pairs (within10) and distant pairs (over30) and explain exact physical point placement. Cube outcomes are random, not source fixed answers. Teacher evaluates physical counters, actual rolls, comparison statements and MLR8 peer dialogue. Cool-down restricted.",
        "Each whole-number line shows a nonnegative length from zero; the greater number is farther RIGHT. The values can be equal and then use '='. A three-cube roll of standard 1..6 number cubes gives sums3..18, which fit a0..20 line; pupils may roll identical sums. For Activity2 two drawn digits can form a two-digit number, starting on0..100 with only labels0,5,10..100 printed: hidden ones are still unit steps. The physical positions are not inferred from a numeric comparison. Subtract fives warm-up:30,25,20,10. Explanations connect both the numerical relation and longer distance-from-zero.",
        "Publisher Number Talk (attributed) four expressions35−5=30,35−10=25,35−15=20,35−25=10. Original Activity1 pairs each produce true dice rolls, put 2 markers on their actual prior 0..20 number line and write comparison (<,> or =). Source Activity2 two-digit roll numbers from two cubes are put on original0..100 with labels every5; NO original outcomes supplied. Separately authored Edulytics FICTIONAL rolls (3,4,6) sum13 versus (2,3,4) sum9: 13>9 and gap4; invented two-digit values52 and26 give 52>26 and gap26. These made-up values are NOT publisher observed student dice.",
        "1. Compute original four Subtract Fives Number Talk answers30,25,20,10 using jumps5,10,15,25 left from35. 2. Prepare the real previously-made0..20 equal-unit number line and two physical counters. 3. In a real pair each player rolls3 number cubes, adds the actual values, and places a counter at the correct numeric point on the SAME line. 4. Decide <,> or = by left/right positions and equal zero-based distances, then explain aloud and document the true expression; use '<' if first less. 5. Move to0..100 line labelled in multiples of5, each player rolls two cubes and builds a two-digit number, places and labels both points, and explains comparison and distance. 6. Teacher sees original individual pair work, physical intervals and MLR8 discussion. 7. Separately solve exact Edulytics-made arithmetic without treating its fictional rolls as real source results.",
        "Calling a larger right-side point smaller; confusing physical mark spacing with the actual numerical value. Failing to consider equality (both partners can roll the same sum). Rolling three dice then comparing a single cube rather than their SUM. Interpreting the0..100 label increments5 as five-unit intervals without locating ones. Making the two-digit number with imaginary unrecorded original dice or representing numeric practice as evidence of original MLR8 partner work. Changing the same-line physical scale between partners or moving wrong direction for subtraction.",
        "IM2021 G2 U04 L04 compare lengths from zero: subtract fives (30,25,20,10), two real three-cube sum markers on a shared0..20 line, two-digit dice values on an original 0..100 line with multiples-of5 printed, compare <,> or =, physical reasoning teacher-only; separate authored digital numerical practice."];
    private static readonly string[] Standards=["CCSS:2.MD.B.6","CCSS:2.NBT.B.5"];
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        d.VersionCode=="CCSSM-2010" &&
        d.ContentVersion=="phase30-common-core-rich-content-v1" && l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
           !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(Standards))
            throw new InvalidOperationException("G2 original 2.4.4 source identity drift");
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
            if(!Same(t,false))throw new InvalidOperationException("G2 2.4.4 prior six fields drift");
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
            throw new InvalidOperationException("G2 2.4.4 persisted predecessor unknown");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 2.4.4 persisted language drift");
        var old=rows.Single();var current=l.Translations.Single(x=>x.CultureCode=="en");
        if(old.Title!=current.Title||!(Same(old,false)||Same(old,true)))
            throw new InvalidOperationException("G2 2.4.4 persisted prose conflict");
    }
}
