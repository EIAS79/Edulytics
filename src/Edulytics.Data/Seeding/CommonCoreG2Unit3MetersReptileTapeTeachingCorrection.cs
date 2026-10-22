using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>IM K5 2021 G2 U03 L05 teacher source exact A-G tape lengths.</summary>
public static class CommonCoreG2Unit3MetersReptileTapeTeachingCorrection
{
    public const string Version="usccss-g2-u03-l05-reptile-tapes-meter-source-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U03:L05";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-3/lesson-5/preparation.html";
    private const string Sha="a23f00bf366db1a1c13b1bed0ab1f70daa96d07b7225742cc61a3512b8653fb6";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will learn that the meter is a larger unit of metric length measurement.\nIn previous lessons, you measured and estimated varied lengths in centimeters. You measured using centimeter cubes and centimeter rulers. In this lesson, you measure longer lengths and identify the need for a more appropriate length unit and tool for these measurements.",
         "Key ideas:\n- Use standard rulers and meter sticks to measure length in centimeters and meters\n- In previous lessons, you measured and estimated varied lengths in centimeters\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
         "Example 1: Each length of tape on the floor represents the length of a reptile. A: gila monster ​​​ ​​​ B: baby alligator C: baby cobra D: komodo dragon Measure to find the length of each reptile. Don’t forget the unit. What is the length of a gila monster? What is the length of a baby alligator? What is the length of a baby cobra? What is the length of a komodo...\nApproach: Choose the required unit, align equal units from the endpoint, measure without gaps or overlaps, and record the result with its unit.\nExample 2: D: komodo dragon E: adult alligator F: adult cobra G: ribbon snake Measure the length of the komodo dragon in meters. What is the same or different about your measurements for the komodo dragon from Activity 1 and Activity 2? Measure each reptile in centimeters or meters. Don’t forget the units. How long is an adult alligator? How long is an adult cobra?...\nApproach: “Which measuring tool did you choose to use? Why?” (We used the 10-centimeter tools and cubes because you were easier to count tens and ones. We used the rulers because we didn’t have to use as many blocks. It was faster.) “What was challenging about measuring longer lengths in centimeters?” (We had to line up our tools and count the centimeters. Centimeters are small so there were a lot of them to count.) “What...",
         "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
         "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
         "You will learn that the meter is a larger unit of metric length measurement. In previous lessons, you measured and estimated varied lengths in centimeters."],
        ["G2 Unit3 Lesson5 Measure in Meters (IM K–5 2021): The meter is a longer standard length unit and one meter stick represents exactly100 centimeters. Warm-up Choral Count: count 20–30 by ones then120–130, align the two rows and notice each number in the second row is100 greater. Do not require extensive abstract hundreds-place work yet. Activity1 Reptiles to Measure: teacher makes exact FLOOR TAPE STRIPS (not actual reptiles) A gila monster58cm, B baby alligator72cm, C baby cobra44cm, D komodo dragon180cm. In actual groups of3–4 use existing centimeter cubes, 10-cm rods, handmade or standard rulers, accurately iterating tools to measure strips A–D and reflect on why very long measuring is cumbersome. Activity2 Measure with a Meter Stick: introduce real meter stick and measure D again, then longer floor strips E adult alligator3m36cm=336cm, F adult cobra1m90cm=190cm and G ribbon snake2m82cm=282cm. Meterstick zero must be aligned; place consecutively without gaps and record units in m and cm. Komodo D is1m80cm=180cm, approximately2 meters, not exactly2m. Student chooses appropriate tool and explains, educator monitors MLR2 vocabulary and real physical readings. Publisher cool-down Measure in Meters login-required and not copied; first-party independent numeric conversion practice separate.",
         "1 meter=100 centimeters; centimeter ruler suits short lengths whereas meter stick makes repeating a long unit easier. When lengths are written m and cm, convert by multiplying whole meters by100 and adding remaining centimeters: 3m36cm=336cm,1m90cm=190cm,2m82cm=282cm,1m80cm=180cm. In exact standard mixed-unit decomposition, centimeters remainder is from0 to99: 180cm=1m80cm, NOT1m180cm. For a meter stick used three full times plus36cm, length is336cm. The classroom tape strip is a MODEL of an animal length and must be physically laid to its source-provided number; no student is touching or measuring live reptiles. Source A58cm, B72cm, C44cm, D180cm; source E336cm, F190cm, G282cm. The komodo D is measured with a centimeter tool and with meterstick, giving the SAME length, but measurement strategy differs. Warm-up count rows20..30 and120..130 differ by100. Addressing standards CCSS 2.MD.A.1 and2.NBT.A.2; source Activity2 builds toward2.MD.A.2 without claiming it is an Addressing outcome.",
         "PUBLIC EXACT SOURCE TEACHER PREPARATION: tape A gila monster58cm, B baby alligator72cm, C baby cobra44cm, D komodo dragon180cm; E adult alligator3m36cm or336cm; F adult cobra1m90cm or190cm; G ribbon snake2m82cm or282cm. Warm-up counts20,21,...30 and120,121,...130. Source synthesis says the D komodo dragon can be estimated as about2m, while measured length1m80cm equals180cm: approximate2m≠exact2m. Meterstick repeated 3 times plus36cm measures E=336cm. Source ribbon snake G=282cm and adult cobra F=190cm; difference92cm. Independent first-party Edulytics numbers not represented as original tape:2m47cm=247cm;365cm=3m65cm;4 full meter lengths plus12cm=412cm.",
         "1. Source choral count20 to30 then120 to130; compare aligned numbers, each second row number100 greater. 2. With the teacher construct authentic tape strips of EXACT specified floor lengths and label A–G with reptile names; measure tape, NOT animals. 3. In actual groups of3–4 use centimeter cubes,10cm tools, handmade/standard centimeter rulers to measure A=58,B=72,C=44,D=180cm. Keep tools aligned end-to-end with no gaps or overlap. 4. Discuss difficulty of many ruler iterations for D and why a longer unit or meter stick helps. 5. Examine actual meter stick with equal markings0..100 centimeters; 100 centimeters is1 meter, find50cm and identify zero correctly. 6. Measure D AGAIN as1 meter and80cm=180cm (about2 meters), then E3m36cm=336cm, F1m90cm=190cm, G2m82cm=282cm. Use full meter stick placements plus remainder to record each length with units. 7. Compare first and second measurement of D: same physical strip length, fewer tool repositionings with the longer meterstick. Real tool choice, individual measurements and MLR2 talk assessed by teacher. 8. Complete separate Edulytics first-party exact metric-conversion questions; these do not copy publisher login-required Cool-down.",
         "Treating 1 meter as10cm rather than100cm. Saying the 180cm komodo tape is EXACTLY2 meters because it's 'about2 meters' in synthesis; exact1m80cm. Losing the cm remainder in E=3m36cm, erroneously writing3036cm instead of336cm. Comparing tape lengths with different units without conversion. Leaving gaps or overlaps between meterstick placements, or starting at the ruler's wrong edge. Fabricating measurements of real reptiles instead of tape models with teacher-provided floor strip lengths. Treating source 2.NBT.A.2 choral count as a full hundreds-place conceptual assessment. Claiming the partner group's real measurement, instrument alignment or MLR2 discussion verified solely from a digital answer.",
         "IM2021 G2 U03 L05 introduces meters,1m=100cm. Choral count20–30 vs120–130. Teacher lays source tape strips A58cm, B72cm, C44cm, D180cm; pairs/groups measure first four with cm tools. Measure D again with meterstick, then E3m36cm, F1m90cm and G2m82cm; discuss same length, easier iteration and tool choice. Exact values are SOURCE tape lengths, not student-real-measurement claims. Independent conversions and teacher-observed physical work stay separate."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        d.VersionCode=="CCSSM-2010"&&d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
           !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.A.1","CCSS:2.NBT.A.2"}))
            throw new InvalidOperationException("G2 U03 L05 source SHA/URL/outcomes changed");
    }
    private static bool Same(CanonicalLessonContentPackTranslation t,bool after)=>
        Match([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Same(CurriculumLessonContentTranslation t,bool after)=>
        Match([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Match(string[] v,bool after)=>v.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var en=l.Translations.Single(x=>x.CultureCode=="en");
            if(Same(en,true))continue;
            if(!Same(en,false))throw new InvalidOperationException("G2 U03 L05 old six fields changed");
            en.Explanation=S.After[0];en.KeyConceptsAndRules=S.After[1];
            en.WorkedExamples=S.After[2];en.StepByStepSolutions=S.After[3];
            en.CommonMistakes=S.After[4];en.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prev)=>
        IsTarget(d,l)?Version:prev;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prev)=>
        IsTarget(d,l)&&prev==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prev,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,prev))throw new InvalidOperationException("G2 U03 L05 prior persisted version invalid");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U03 L05 prior row mismatch");
        var t=rows.Single();var en=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=en.Title||!(Same(t,false)||Same(t,true)))
            throw new InvalidOperationException("G2 U03 L05 persisted prior fields changed");
    }
}
