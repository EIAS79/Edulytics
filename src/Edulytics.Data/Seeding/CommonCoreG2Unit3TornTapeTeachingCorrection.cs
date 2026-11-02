using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>Locally archived 2021 G2 U03 L10 torn-tape original source,
/// SHA-guarded and no fabricated unpublished long-side endpoint.</summary>
public static class CommonCoreG2Unit3TornTapeTeachingCorrection
{
    public const string Version="usccss-g2-u03-l10-torn-tape-nonzero-source-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U03:L10";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-3/lesson-10/preparation.html";
    private const string Sha="6ca8d276669032508fc1394ce25af9047aefb865dc9bc6d321699290513823dd";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will measure objects without lining up the end with the 0 mark.\nIn earlier lessons, you used rulers, measuring tapes, yardsticks, and meter sticks to measure lengths in inches, feet, centimeters, and meters. You learned that length units are represented on these tools as the length between tick marks. You learned that if you line up the edge of an object with zero on your tools, you can easily read the measurement from the tool without counting each length unit.",
         "Key ideas:\n- Determine the measurement of an object with a measuring tool when the endpoint does not line up with 0\n- In earlier lessons, you used rulers, measuring tapes, yardsticks, and meter sticks to measure lengths in inches, feet, centimeters, and meters\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
         "Example 1: Decide if each statement is true or false. Be prepared to explain your reasoning. \\(10 - 0 = 12 - 2\\) \\(8 - 4 = 10 - 6\\) \\(12 - 4 = 10 - 3\\) \\(15 - 2 = 13 - 0\\)\nApproach: “How could we change the third equation to make it true?” (We could change the 3 to a 2 because the 12 went down by 2 and 4 also went down by 2. The difference would stay the same.)\nExample 2: Jada and Han used an inch ruler to measure the short side of a notebook. Han says 8 inches. Jada says 8 inches. How did Han and Jada get the same measurement? Write an equation that could show Jada’s thinking. Measure an object using Jada’s method. I measured a ___ . I started with the number ___ . I ended with the number ___ . Equation: ___ . The length...\nApproach: “Why do you think you get the same measurement each time, even if you measure starting from a different place?” (The object didn't change, so the length has to stay the same. The total length in inches is still the same between the number that you start with and end with.)",
         "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
         "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
         "You will measure objects without lining up the end with the 0 mark. In earlier lessons, you used rulers, measuring tapes, yardsticks, and meter sticks to measure lengths in inches, feet, centimeters, and meters."],
        ["Grade 2 Unit3 Lesson10 Measure with a Torn Tape (IM K–5 2021). The actual lesson is about determining the length of an object without aligning its start with zero on a ruler; moving the start mark doesn't change its length. Warm-up True or False: Constant Difference: 10−0=12−2 is TRUE; 8−4=10−6 TRUE; 12−4=10−3 FALSE because8 differs from7; 15−2=13−0 TRUE. Activity1 The Notebook Problem (15 min): Han and Jada BOTH measured the short side of a notebook to be8 inches; Jada's tape was torn, so she could measure from2 to10 and compute10−2=8, or2+?=10. Pupils work in pairs and measure REAL classroom objects2–10 inches long by aligning each real object's endpoints to different NONZERO marks and explaining their methods. Activity2 A Desktop to Measure (20 min): Lin's tape is tangled; she starts at16 inches. The original source synthesis explicitly gives SHORT side34−16=18 inches and16+?=34. The public lesson text DOES NOT reveal the long-side ending tick from original illustration, so Edulytics doesn't claim its fixed actual length. Student chooses subtraction or missing-addend addition to solve. Teacher observes physical ruler work, genuine desk diagram, partner MLR2 language and actual endpoints. Original Torn Tape Cool-down is restricted; independent first-party digital illustrations are not attributed to the publisher.",
         "The measurement is the NUMBER OF EQUAL INTERVALS between an object's start and finish, NOT the printed number at the finish mark. On an evenly spaced inch ruler, length = end tick − start tick when end≥start. Equivalently start + ? = end; count on to find the missing number. A ripped tape can still measure the same length using its remaining labeled portion because each unit stays the same length. Source Jada from2 to10:10−2=8,2+8=10. Source Lin short desktop side from16 to34:34−16=18,16+18=34. Warm-up shows subtracting the same amount from both operands preserves difference:12−2=10−0=10,10−6=8−4=4, but12−4=8 and10−3=7 not equal. Do NOT claim to know the long side's fixed endpoint without the original illustration. Unit labels must be written as inches. Source preparation Addressing CCSS2.OA.B.2; Building Towards2.MD.B.6 and cooldown2.MD.B.5 are distinct, so they must not be silently substituted for the official outcome code.",
         "PUBLIC source Number Talk:10−0=12−2 TRUE (10),8−4=10−6 TRUE (4),12−4=10−3 FALSE (8≠7),15−2=13−0 TRUE (13). PUBLIC Activity1 notebook short side: Han8 inches and Jada8 inches, Jada used torn ruler from2 to10;10−2=8 and2+8=10. PUBLIC Activity2 short desktop side: start16 inch tick, finish34 inch tick,34−16=18 inches and16+18=34. The original source's long-side drawing exists, but its ending measurement isn't specified in accessible text; it requires genuine illustration/teacher review. INDEPENDENT first-party examples: ruler from3 to11 measures8in, from4 to13 measures9in, and from6 to18 measures12in. These invented virtual rulers are NOT source pictures. First-party Cool-down:7 to16 measures9in;13 to26 measures13in.",
         "1. Discuss each of four original True/False equations and justify using equal differences; correct12−4=10−2 not10−3. 2. Look at the teacher's authentic notebook drawing. Explain Han measuring8in from0 to8 and Jada measuring the SAME8in from2 to10 on a torn ruler; a constant length is conserved when a ruler mark changes. 3. Write BOTH10−2=8 and2+?=10. Count actual inch intervals; don't confuse end tick10 with length10. 4. With a REAL partner use actual rulers to measure one classroom object2–10 inches long starting at different nonzero marks; record BOTH start and end and write a matching subtraction or missing-addend equation. 5. In Activity2 study Lin's actual illustrated DESKTOP; the source states she starts at16in. The public discussion supplies a short side ending34in, so34−16=18; demonstrate also16+18=34. Teacher checks the long side directly on source illustration; its numeric length cannot be safely filled from accessible teacher text. 6. Compare two actual students' different strategies and MLR2 speech, explicitly preserve unit inches and equality of length. 7. Separately complete original Edulytics invented ruler intervals and exit questions; the publisher Torn Tape Cool-down remains protected.",
         "Reporting10 inches for Jada's notebook merely because the torn ruler ends at tick10: her start tick is2, so the physical length is8. Starting ruler calculation at0 when the object begins elsewhere. Counting printed TICK MARKS instead of spaces BETWEEN marks. Calling Lin's short desk side34 inches rather than34−16=18 inches. Misclassifying original 12−4=10−3 as true; the sides are8 and7. Assuming every two shifted subtraction expressions have the same value without checking that both numbers shift equally. Inventing Lin's long desktop length from text that supplies no long-end tick. Presenting independent digital picture measurements as teacher's actual source images, or treating a typed answer as evidence of genuine ruler/peer participation.",
         "Torn tape measures length between labels: end − start = length; or start + ? = end. Original warm-up T,T,F,T; Han/Jada's notebook8in from2→10; Lin's short desktop side18in from16→34. Real ruler measurements, original long-side diagram and pair MLR2 dialogue need teacher observation. Independent Edulytics examples from3→11=8,4→13=9,6→18=12 use invented, clearly labeled virtual rulers."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&d.VersionCode=="CCSSM-2010"&&
        d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.OA.B.2"}))
            throw new InvalidOperationException("G2 U03 L10 source URL/hash/outcomes changed");
    }
    private static bool Matches(CanonicalLessonContentPackTranslation t,bool after)=>
        Equal([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Matches(CurriculumLessonContentTranslation t,bool after)=>
        Equal([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Equal(string[] x,bool after)=>x.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Matches(t,true))continue;
            if(!Matches(t,false))throw new InvalidOperationException("G2 U03 L10 previous six source fields changed");
            t.Explanation=S.After[0];t.KeyConceptsAndRules=S.After[1];t.WorkedExamples=S.After[2];
            t.StepByStepSolutions=S.After[3];t.CommonMistakes=S.After[4];t.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)?Version:before;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)&&before==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,before))throw new InvalidOperationException("G2 U03 L10 previous persisted rich version changed");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U03 L10 previous translation rows changed");
        var t=rows.Single();var inc=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=inc.Title||!(Matches(t,false)||Matches(t,true)))
            throw new InvalidOperationException("G2 U03 L10 previous six persisted fields changed");
    }
}
