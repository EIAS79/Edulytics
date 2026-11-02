using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>Original IM2021 optional G2 U03 L18: 3x5 card, real 36in tool, desk and arm measurement.</summary>
public static class CommonCoreG2Unit3YardstickTeachingCorrection
{
    public const string Version="usccss-g2-u03-l18-optional-3x5-yardstick-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U03:L18";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-3/lesson-18/preparation.html";
    private const string Sha="19028c1fad43d97892d7bc067214e8abcf7110e68d0130f550975d96235c5588";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will apply your understanding of addition, subtraction, and measurement to build your own yardstick.\nThis lesson is optional because it does not address any new mathematical content standards. This lesson does provide you with an opportunity to apply precursor skills of mathematical modeling. In previous lessons, you made your own centimeter ruler and used different measuring tools such as rulers, measuring tapes, and yardsticks to measure objects.",
         "Key ideas:\n- Use addition and subtraction to mark the inch marks on a yardstick\n- Use a yardstick to measure objects and compare measurements\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
         "Example 1: Find the value of each expression mentally. \\(3 + 5\\) \\(5 - 3\\) \\(5 - 3 + 5\\) \\(3 + 5 + 3 + 3\\)\nApproach: Represent the whole and the part, write a matching subtraction relationship, then find and check the difference.\nExample 2: Measure the height of your desk using your yardstick. Draw an x on your sticky note and add your measurement to the class line plot. Use the completed line plot to answer the following questions: What do you notice? What do you wonder? What is the tallest measurement for the desks in the room? What is the shortest measurement? Find the difference between...\nApproach: Represent the whole and the part, write a matching subtraction relationship, then find and check the difference.",
         "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.",
         "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.",
         "You will apply your understanding of addition, subtraction, and measurement to build your own yardstick. This lesson is optional because it does not address any new mathematical content standards."],
        ["IM K–5 2021 Grade2 Unit3 Lesson18 Make a Yard Stick (OPTIONAL) uses mathematical MODELING, not new standard content. Warm-up Number Talk using card sides of3 and5 inches:3+5=8,5−3=2,5−3+5=7,3+5+3+3=14. Activity1 Make a Yardstick: in real groups of TWO, obtain a paper strip at least36 inches long (~3 inches wide) and a REAL 3-by-5 inch index card. With ONLY the card's long side5in, short side3in and sums/differences of measured card lengths, physically mark consecutive inch boundaries from1 all the way to36, switching partner holding/marking roles midway. For example 5−3=2in, 3+3−5=1in and6×5+2×3=36in; these are arithmetic demonstrations, and actual ruler precision requires teacher. Activity2 Compare Yardsticks: physically measure SAME classroom desk's HEIGHT in inches, each partner agrees, marks an X on a sticky note and contributes real measurements to a CLASS line plot. Use observed range (largest minus smallest) and discuss disagreement due to shifted zero, slanted measurement, inconsistent units or gaps; compare and REVISE actual paper sticks. No genuine class desk measurements provided in source; none invented. Activity3 Measure Your Arms: OPTIONAL hand lengths wrist to middle fingertip and forearm lengths outer elbow to wrist for both partners, then write TWO equations and their real-world descriptions using honest actual measurements. The user-created tool, desk line plot and arm values are teacher-reviewed; independent Edulytics numeric illustrations are clearly not real physical measurements.",
         "One yard equals exactly THREE feet or36 inches. In this source activity one genuine3×5-inch index card provides only its edge lengths3in and5in for marking a homemade36-inch paper yardstick. Using add/subtract of exact edge lengths can locate points at1in (3+3−5),2in (5−3),3in,5in,8in,11in (5+3+3),14in (3+5+3+3), and36in (six5-inch segments and two3-inch segments). Every adjacent tick must correspond to the SAME1-inch length: no gaps or overlaps, no shifted zero, no uneven spacing. Physical card positions and marks require actual alignment, not arithmetic alone. For a class desk height plot each sticky-note X represents one genuine measurement; if different pupils measure the same desk differently, check inconsistent zero location, skew/slant, slack and intervals before revising. A range is longest−shortest in INCHES. For hands, measure from WRIST to top of middle finger; forearm from OUTSIDE elbow to wrist, each partner's authentic values; comparing equation must refer to REAL endpoints and subject. Warm-up with3 and5:8,2,7,14. CCSS addressing 2.MD.A.1,2.MD.A.4,2.MD.B.5,2.MD.B.6,2.MD.D.9,2.OA.B.2; 2.MD.A.1 also Building On.",
         "PUBLIC source Number Talk3+5=8,5−3=2,5−3+5=7,3+5+3+3=14. Original tool source: three-by-five inch rectangle used to construct precisely marked1–36in on at least36in strip. FIRST-PARTY pure maths:5−3=2in,3+3−5=1in,5+3+3=11in,5+5+5+5+5+5+3+3=36in. Independently authored illustration of TWO fictional desk measurements28in and30in gives difference30−28=2in; these numbers are NOT actual class desk readings or source results. FIRST-PARTY optional arm illustration only: imaginary forearm16in and imaginary hand7in, difference9in; never present as pupil hand/forearm data. Original source asks actual desk heights, class plot, REAL two-partner hand/forearm data and TWO student-generated equations; values are not present in public text. Independent Edulytics cooldown:36−17=19 inches remaining,25−8=17 and3+5+3=11 inches.",
         "1. Mentally solve the four source Number Talk expressions:8,2,7,14. Discuss combining 3 and5 and subtraction5−3 to produce useful increments. 2. In a REAL pair obtain a true3×5-inch index card and at least36in by ~3in paper strip. Mark zero at one paper endpoint; measure/mark card 3- and 5-inch sides with NO shifted zero or slack. 3. Combine/subtract known lengths to get each inch from1 to36, e.g. 3+3−5 produces1, 5−3 produces2,3 then5, and six5s plus two3s yield36. Check the actual POSITIONS of all inch tick marks and equal1-inch intervals; exchange card-holder/marker roles halfway. Teacher checks physical precision. 4. Measure SAME classroom DESK height with this homemade ruler in inches. Partners reach honest consensus for each measurement, put real X sticky notes onto a class line plot and calculate observed maximum−minimum. There is no fixed publisher desk-height dataset. 5. Compare actual rulers directly, discuss different results, fix inaccuracies caused by unequal spaces, incorrect zero, slant or gaps, then physically REVISE markings. 6. In OPTIONAL activity3 measure both partners' actual HAND length wrist to middle finger and FOREARM outside elbow to wrist. Write TWO distinct equations using these measurements, plus TWO contextual statements, and discuss them with a teacher. 7. Independently solve Edulytics numeric practice with explicitly fictional desk measurements. Numeric correct answers do not prove a usable physical yardstick or peer activity.",
         "Believing a yard is three INCHES rather than3 FEET=36 inches. Using 3 and5 as NUMBERS but not aligning the corresponding actual card EDGE lengths, so inch marks on the paper are not equally spaced. Starting with1 at card edge rather than zero causing every mark to shift. Creating overlaps or gaps between card positions, slanting the yardstick, or not measuring from the same desk height endpoints. Publishing arbitrary illustrative desk-height observations as source real classroom values; the teacher page supplies none. Treating total Xs as tallest desk rather than frequency. Forgetting difference is MAX−MIN. Confusing hand length and forearm endpoints or fabricating actual personal body measurements. Recording two hypothetical arithmetic equations as evidence students physically built and revised a precision measuring instrument is unwarranted.",
         "OPTIONAL source G2 U03 L18: Number Talk8,2,7,14; use a real3×5in index card and36in strip to build a 1–36in yardstick with uniform inch marks, measure a common desk with classmates, plot actual sticky-note Xs and revise inaccurate marks. Optional hand/forearm measurements with two genuine equations. Real measurements and modeling need teacher assessment; independent invented Edulytics arithmetic is not original classroom data."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&d.VersionCode=="CCSSM-2010"&&
        d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
           !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.A.1","CCSS:2.MD.A.4","CCSS:2.MD.B.5","CCSS:2.MD.B.6","CCSS:2.MD.D.9","CCSS:2.OA.B.2"}))
           throw new InvalidOperationException("G2 U03 L18 original source changed");
    }
    private static bool Same(CanonicalLessonContentPackTranslation t,bool after)=>
        Matches([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Same(CurriculumLessonContentTranslation t,bool after)=>
        Matches([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Matches(string[] a,bool after)=>a.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Same(t,true))continue;
            if(!Same(t,false))throw new InvalidOperationException("G2 U03 L18 previous learner text changed");
            t.Explanation=S.After[0];t.KeyConceptsAndRules=S.After[1];
            t.WorkedExamples=S.After[2];t.StepByStepSolutions=S.After[3];
            t.CommonMistakes=S.After[4];t.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prev)=>
        IsTarget(d,l)?Version:prev;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string old)=>
        IsTarget(d,l)&&old==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,before))throw new InvalidOperationException("G2 U03 L18 predecessor unknown");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U03 L18 prior language mismatch");
        var t=rows.Single();var inc=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=inc.Title||!(Same(t,false)||Same(t,true)))
            throw new InvalidOperationException("G2 U03 L18 prior persisted learner text changed");
    }
}
