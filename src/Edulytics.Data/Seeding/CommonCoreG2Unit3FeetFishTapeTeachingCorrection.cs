using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>Original local IM2021 G2 U03 L09 fish tape and feet without mandated conversion algorithms.</summary>
public static class CommonCoreG2Unit3FeetFishTapeTeachingCorrection
{
    public const string Version="usccss-g2-u03-l09-one-foot12in-real-fish-tape-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U03:L09";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-3/lesson-9/preparation.html";
    private const string Sha="9dd9e384db73f5a4fc7dcc85599337d686d42f51dff96133b2b6640a993b5c06";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will learn about a larger unit of customary measurement—the foot.\nIn an earlier lesson, you were introduced to the inch as a length unit in the customary system. You developed a benchmark for an inch and measured objects with an inch ruler. In this lesson, you use the length of a 12-inch ruler to develop an understanding of the length of 1 foot .",
         "Key ideas:\n- Compare measurements in feet and inches and describe the relationship between different measurements and the size of length units\n- Use rulers to measure length in feet and inches\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
         "Example 1: How long is this Cobia fish in inches? Record an estimate that is: too low about right too high \\(\\phantom{\\hspace{2.5cm} \\\\ \\hspace{2.5cm}}\\) \\(\\phantom{\\hspace{2.5cm} \\\\ \\hspace{2.5cm}}\\) \\(\\phantom{\\hspace{2.5cm} \\\\ \\hspace{2.5cm}}\\) Record an estimate that is: too low about right too high \\(\\phantom{\\hspace{2.5cm} \\\\ \\hspace{2.5cm}}\\)...\nApproach: “How did you revise your estimate when you found out how old the boy was and his height?” (My first estimate was a lot higher, but it looks like if this fish was touching the ground too, you'd be closer to the same length. The fish looks about 20 inches higher or longer, but it isn’t touching the ground. I did 12 more inches, which is 60.)\nExample 2: Work with your group to measure the tape strips around the classroom in feet. Then measure the length in inches. Tape A largemouth bass Length in feet: ___ Length in inches: ___ Tape B spiny dogfish shark Length in feet: ___ Length in inches: ___ Tape C catfish Length in feet: ___ Length in inches: ___ Tape D koi Length in feet: ___ Length in inches: ___...\nApproach: “What did you notice about your measurements when you switched from measuring in feet to measuring in inches?” (The numbers are smaller for the number of feet. I had to count more inches than feet.) “Why are our measurements in feet so much smaller than our measurements in inches?” (Feet are longer, so you don't need as many to measure the length of the tape. It takes more inches to cover the same length.)",
         "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
         "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
         "You will learn about a larger unit of customary measurement—the foot. In an earlier lesson, you were introduced to the inch as a length unit in the customary system."],
        ["Grade2 Unit3 Lesson9 From Feet to Inches (IM K–5 2021). Warm-up Estimation Exploration Small Fry, Big Fish displays real source photo of a cobia and a boy; initially guess too low, about right, too high fish lengths in INCHES. Then the teacher reveals the boy is7 years old and48 inches tall, prompting revised estimates. Source discussion's estimate of60 inches is illustrative, NOT a verified fish length and original photograph is not recreated. Activity1 Measure the Length of Fish: educator constructs floor tape strips representing fish lengths: A largemouth bass2 feet, B spiny dogfish shark4 feet, C catfish3 feet and D koi2 feet6 inches. In groups3–4 measure FIRST in feet by carefully iterating the 0–12 segment of a REAL 12-inch ruler, then measure SAME strip in inches with rulers or inch tiles, compare unit counts. Correct target values if accurately prepared: A24in, B48in, C36in, D30in. Critical source constraint: the teacher expressly states Grade2 children are NOT EXPECTED TO CONVERT between feet and inches as an abstract algorithm; students notice 1 physical foot =12 inches by manipulating real rulers, not memorizing conversion tables. Activity2 Inches or Feet? pairs choose genuine classroom objects, ESTIMATE with a declared unit before measuring, select a sensible inch ruler or marked measuring tape, and record actual object length in inches, feet or both when appropriate; teacher checks MLR2 explanations and real measurements.",
         "Foot (ft) is a U.S. customary length unit with exactly the same PHYSICAL length as the 0–12 inch segment of a standard 12-inch ruler. If iterating ruler segments to measure actual tape on floor, start at zero, use only0-to12 as one foot (not the plastic stick's extra margins), move without gaps/overlaps, and compare inch tiles and ruler measures for the SAME strip. A has length2ft, B4ft, C3ft, D2ft6in in the publisher's floor tape prep. Illustrative inch target counts A24, B48, C36, D30 arise from physically repeating12 one-inch intervals for each full foot, plus6 more for D; these first-party numeric checks are NOT a required grade2 abstract unit conversion skill. A longer unit takes FEWER units than a shorter unit for the same object: a 2-foot tape can be covered by2 full-foot spans or24 one-inch intervals. In Activity2 choose inches for small objects and feet or mixed units for long ones, based on actual tool and object dimensions. The public warm-up boy is48in tall, age7; Cobia fish length is not given, so do not assign an exact fish length. Source lesson standards2.MD.A.1,2.MD.A.2,2.MD.A.3.",
         "PUBLIC source warm-up: boy age7 and height48 inches; fish measurement remains UNKNOWN, and one student's about60in estimate is not confirmed as actual. PUBLIC preparation floor tape: A largemouth bass2ft (24 one-inch intervals), B spiny dogfish shark4ft (48 one-inch intervals), C catfish3ft (36 one-inch intervals), D koi2ft6in (30 one-inch intervals). Source group tasks measure actual teacher-built tapes both in feet and inches. FIRST-PARTY numerical demonstrations represent laying one12-inch ruler segment end to end: three ruler spans give36 inch ticks, and a 2ft6in tape uses2 ruler spans plus6 inch tiles. These diagrams are NOT evidence that a child actually used a tool. Activity2 real object estimates, measurement tools and actual lengths vary by classroom; no publisher fixed numerical answers. Original independent Edulytics exit is numerical tool-comparison questions, not educator-only Measure in Inches and Feet Cool-down.",
         "1. Look at authentic source warm-up fish and boy, first record high/low/about-right estimates without known fish size. Use new source information that boy is48 inches tall to revise reasonable estimate; do not call60 the true fish length. 2. With a teacher and group, lay out REAL tape A2ft, B4ft, C3ft, D2ft6in as floor models, not real live fish. 3. Use genuine 12-inch ruler zero-to12 interval as ONE FOOT and repeat foot spans along tapes without counting plastic overhang before0 or after12. Measure feet first. 4. Measure the same tape in one-inch spans using inch rulers or inch tiles. Compare counts, for instance2 feet are covered by24 one-inch segments on an accurately prepared source tape. This repeated-unit count is NOT a requirement to perform generalized unit conversion by formula at grade2. 5. Explain why inch counts are larger for the exact same physical length and why a foot is an efficient unit for long objects. 6. For Activity2 independently choose true objects in the room, FIRST estimate and declare inches or feet, discuss with partner, THEN use a suitable real marked tool (including a tape measure labeled in feet) and record actual length and proper unit. 7. Teacher evaluates MLR2 comparison words and physical measurement; complete clearly separate Edulytics illustrative numeric exercises, not protected publisher assessment.",
         "Treating one foot as ten inches, or the plastic ruler's full body as exactly one foot rather than its marked0-to12 scale. Placing rulers successively with gaps or overlaps. Claiming warm-up Cobia fish is exactly60 inches because a pupil estimated that, despite no actual measurement provided. Requiring abstract feet-to-inches conversions as mandatory grade2 fluency even though teacher preparation expressly forbids that expectation. Pretending publisher's floor tape targets are verified physical student measurements. Choosing inches for very long objects without recognizing feet can give simpler counts, or confusing inch and foot labels. Recording real object measurements before actually estimating. Attributing independent Edulytics numerical examples to original fish image, real objects or protected publisher Cool-down.",
         "Grade2 Unit3 Lesson9 introduces FOOT, equal in physical length to the0–12 inch marks on a12-inch ruler. Teacher's actual floor tape models A bass2ft, B shark4ft, C catfish3ft, D koi2ft6in; measure REAL strips first by feet then by inches. Grade2 children observe repeated units and are NOT expected to master abstract conversions. Warm-up boy is48in tall, fish unknown. Activity2 requires genuine object estimates first, then real measurement in inches or feet, teacher observed."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&d.VersionCode=="CCSSM-2010"&&
        d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.A.1","CCSS:2.MD.A.2","CCSS:2.MD.A.3"}))
            throw new InvalidOperationException("G2 U03 L09 original source identity changed");
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
            if(!Same(t,false))throw new InvalidOperationException("G2 U03 L09 old six fields drift");
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
        if(!CanUpgradeExisting(d,l,before))throw new InvalidOperationException("G2 U03 L09 persisted predecessor version drift");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U03 L09 unexpected prior translations");
        var t=rows.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=incoming.Title||!(Same(t,false)||Same(t,true)))
            throw new InvalidOperationException("G2 U03 L09 persisted learner six fields changed");
    }
}
