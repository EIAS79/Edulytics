using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>Source-locked G2 U03 L03 handmade ruler tick marks 0 through25; no fabricated printed A–F lengths.</summary>
public static class CommonCoreG2Unit3BuildRulerTeachingCorrection
{
    public const string Version="usccss-g2-u03-l03-ruler-ticks25-rectangles-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U03:L03";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-3/lesson-3/preparation.html";
    private const string Sha="26e47ae63c5720a631ebca53fffdd3edcc2cb1e371960b8da8868fb53010bc70";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will use a ruler to measure and compare lengths in centimeters.\nIn earlier lessons, you measured with centimeters using centimeter cubes and 10-centimeter tools (base-ten blocks). In this lesson, you transition from measuring length with physical objects to measuring length with more abstract tools such as rulers, measuring tapes, and meter sticks. You create rulers and describe how a centimeter is represented on the tool (MP2).",
         "Key ideas:\n- Create and use a ruler with centimeter units\n- Measure to determine how much longer one object is than another\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
         "Example 1: Find the value of each expression mentally. \\(63 - 3\\) \\(63 - 20\\) \\(63 - 23\\) \\(63 - 24\\)\nApproach: Choose the required unit, align equal units from the endpoint, measure without gaps or overlaps, and record the result with its unit.\nExample 2: Use your tools to make a ruler that shows lengths in centimeters. Compare your ruler with your partner’s ruler.\nApproach: “How can you use your ruler to show someone how long 1 centimeter is?” (It’s the length from 0 to 1. It’s the length between any two numbers.) “How can you use the ruler to show someone how long 10 centimeters is?” (It’s the length from 0 to 10.) “What other lengths could you use your ruler to show?” (Answers vary between 0 and 25 centimeters)",
         "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
         "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
         "You will use a ruler to measure and compare lengths in centimeters. In earlier lessons, you measured with centimeters using centimeter cubes and 10-centimeter tools (base-ten blocks)."],
        ["IM K–5 2021 Grade 2 Unit3 Lesson3 Create and Use a Ruler. Students turn real1cm cubes and10cm rods into an abstract centimeter ruler. Warm-up Number Talk: 63−3=60;63−20=43;63−23=40;63−24=39. Activity1 Create a Ruler: each learner gets authentic printed centimeter ruler template, scissors and centimeter blocks, starting from ZERO. Use each real1cm cube to space successive tick marks exactly1cm apart; label the TICK MARKS 0,1,2,...25 (NOT the spaces), so the mark n indicates distance n cm from zero. Use the real25cm hand-made ruler to explain1cm as0→1 and10cm as0→10. Activity2 Measure and Compare Lengths: measure original printed rectangles labeled A–F using rulers students created, check with blocks, record lengths in centimeters, compare how much longer A is than B and F than D, find two longest, and explain their joined total. The public original teacher page contains rectangle drawings but NOT authoritative numeric lengths in plain text; teacher must check genuine printouts rather than Edulytics fabricating measurements. Source synthesis asks how to prove longest rectangle is10cm longer than shortest, a relative difference without giving either exact length. MLR8 physical work and partner checks are teacher-scored.",
         "On a centimeter ruler the TICK MARK at n reports the distance in centimeters from the mark at 0. The interval between adjacent tick marks is ONE centimeter. The numeral should label the MARK, not a gap, and tick marks should be equal distances; gaps and overlap distort measurement. A real ruler built in this source activity begins at0 and continues to25cm. Length of an object aligned at0 is the terminal tick label. For an object aligned at nonzero start mark a and end mark b with b≥a, length is b−a cm, not b. To compare objects, ensure same standard unit, then find positive difference longer−shorter; if joining two lengths end-to-end with no overlap, total is sum of individual lengths. The SOURCE rectangle A–F picture measurements are not numerically specified by the readable teacher text, so actual original length values and first/second longest rectangle IDs need teacher inspection. Source warm-up subtraction63−24=39 can use63−23=40 then subtract1 more. Outcome standards2.MD.A.1,2.MD.A.4,2.MD.B.6,2.NBT.B.5.",
         "SOURCE warm-up:63−3=60,63−20=43,63−23=40,63−24=39. SOURCE Activity1 homemade ruler markings at0,1,...25 cm; 0→1 demonstrates1cm and0→10 demonstrates10cm, using1cm cubes to space marks. SOURCE Activity2 A–F real printed rectangles: questions include difference A−B and F−D, identifying longest two, and sum joined; values require physically reading printed diagrams; no numeric claim about original A–F lengths is made. Source lesson synthesis: longest rectangle10cm longer than shortest (no exact pictured lengths in public text). FIRST-PARTY Edulytics virtual ruler example: marks3 to18 correspond to15cm, not18cm; a separate invented rectangle A is23cm, B is17cm, longer by6cm; a joined 11cm and8cm gives19cm. Independent exit: marks4 to21 show17cm.",
         "1. Solve source mental subtractions60,43,40,39, discussing how each previous relation simplifies the next. 2. Receive the real teacher-provided centimeter ruler template, real centimeter cubes and scissors. Draw/label tick mark0 first; place a1cm cube beginning at0, draw a new tick precisely at its end and label that mark1. 3. Continue with touching equal-length1cm units until25 is labeled. DO NOT label intervals instead of marks. Compare accuracy with a genuine partner and keep your ruler for future lessons. 4. Demonstrate that0→1 measures1cm and0→10 measures10cm; a label indicates distance from zero. 5. Measure each source rectangle A–F on the AUTHENTIC printed student sheet; set ruler0 at each start, record endpoint in cm, and use correct units. 6. Compare real rectangle A versus B, F versus D, and the two longest, using longer−shorter and end-to-end joining. If measurements differ from partner, remeasure and examine tick spacing. 7. In source synthesis discuss why a correctly made ruler is easier than individually placing cubes. Teacher reviews actual printed rectangles, genuine ruler marks and MLR8 oral explanations. 8. Use separately authored Edulytics interval arithmetic prompts for practice, without presenting them as actual source A–F numbers or protected 'Use a Ruler' Cool-down.",
         "Beginning a homemade ruler at1 instead of0; putting numerals in spaces between marks, not at tick marks. Drawing uneven intervals or measuring from the tool's physical edge if0 is not at that edge. Reading an object between marks3 and18 as18cm, rather than18−3=15cm. Reporting lengths without cm units. Calling original rectangles A–F a specific length merely because arbitrary Edulytics virtual diagrams use letter labels. Assuming a source synthesis difference of10cm gives exact identities and absolute lengths for the original longest/shortest rectangles. Claiming student-made ruler accuracy, physical measurement and real peer comparison are verified from numeric answers alone.",
         "Grade2 U03 L03 original Create and Use a Ruler: source Number Talk60,43,40,39; mark ruler0→25 with equal1cm intervals and labels on TICK MARKS; a tick n is n cm FROM ZERO. Physically measure authentic rectangle A–F printout, compare differences and combine lengths with teacher review. First-party virtual examples 3→18=15cm,23−17=6cm and11+8=19cm are not source A–F lengths."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        d.VersionCode=="CCSSM-2010"&&d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.A.1","CCSS:2.MD.A.4","CCSS:2.MD.B.6","CCSS:2.NBT.B.5"}))
            throw new InvalidOperationException("G2 U03 L03 source and outcomes changed");
    }
    private static bool Equal(CanonicalLessonContentPackTranslation t,bool after)=>
        Same([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Equal(CurriculumLessonContentTranslation t,bool after)=>
        Same([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Same(string[] a,bool after)=>a.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Equal(t,true))continue;
            if(!Equal(t,false))throw new InvalidOperationException("G2 U03 L03 original six rich fields changed");
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
        if(!CanUpgradeExisting(d,l,before))throw new InvalidOperationException("G2 U03 L03 persisted content version changed");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 U03 L03 prior translations changed");
        var t=rows.Single();var source=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=source.Title||!(Equal(t,false)||Equal(t,true)))
            throw new InvalidOperationException("G2 U03 L03 previous persisted six fields changed");
    }
}
