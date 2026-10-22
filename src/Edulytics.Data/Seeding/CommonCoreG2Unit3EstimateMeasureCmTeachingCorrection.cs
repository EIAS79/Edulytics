using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>IM2021 local source, G2 U03 L04: zero-tick ruler accuracy and estimates.</summary>
public static class CommonCoreG2Unit3EstimateMeasureCmTeachingCorrection
{
    public const string Version="usccss-g2-u03-l04-estimate-and-measure-centimeters-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U03:L04";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-3/lesson-4/preparation.html";
    private const string Sha="655f031b245a477ee25ae17c5172a3b2bef9484e673b5cc74de6a04791e77cd3";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will estimate and measure lengths in centimeters.",
         "Key ideas:\n- Estimate lengths in centimeters\n- Use standard rulers to measure length in centimeters\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
         "Example 1: Which one doesn't belong? A Caption: A B Caption: B C Caption: C D Caption: D\nApproach: “If the length unit on the tools should be centimeters, which images show shapes that have a length of 4 centimeters? Explain.” (A shows a length of 4 centimeters because the side starts at 0 and ends at 4. B shows 4 cm because I can count 4 cm lengths starting at the beginning of the ruler or counting back from the 4. C does not show 4 cm because each length between the numbers is not a cm. D shows 4 cm because...\nExample 2: Record an estimate that is: too low about right too high \\(\\phantom{\\hspace{2.1cm} \\\\ \\hspace{2.1cm}}\\) \\(\\phantom{\\hspace{2.1cm} \\\\ \\hspace{2.1cm}}\\) \\(\\phantom{\\hspace{2.31cm} \\\\ \\hspace{2.1cm}}\\) Record an estimate that is: too low about right too high \\(\\phantom{\\hspace{2.1cm} \\\\ \\hspace{2.1cm}}\\) \\(\\phantom{\\hspace{2.1cm} \\\\ \\hspace{2.1cm}}\\)...\nApproach: “How did you make your estimates? What did you think about?” (I thought about the size of a 10-centimeter tool and imagined measuring with it. I thought about the length of the ruler we made.)",
         "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
         "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
         "You will estimate and measure lengths in centimeters. In previous lessons, you measured lengths in centimeters with physical units and the rulers you constructed."],
        ["Grade 2 Unit 3 Lesson 4, Measure and Estimate in Centimeters, IM K–5 2021. This lesson introduces a standard centimeter ruler after students have measured with centimeter cubes, groups of ten and rulers they constructed. Warm-up: Which One Doesn't Belong? Length compares FOUR original source images A–D of rulers and a shape. The public teacher synthesis says A, B and D correctly show 4 centimeters, while C does NOT: the spacing between consecutive tick labels is not a real centimeter. A begins at zero and ends at four; B can count four real centimeter intervals starting at the tool's edge or counting back; D goes from the 1 mark to the 5 mark, four equal intervals. Because these are publisher images, first-party Edulytics number-line examples illustrate the mathematics without copying them. Activity1 Estimate Length in Centimeters: pairs estimate actual classroom items mostly 5–30 cm, including at least one 50–90 cm item. Begin with notebook estimation, collect a TOO LOW estimate, an ABOUT RIGHT estimate, and a TOO HIGH estimate, and revise against a 10-cm reference or adjacent folder. Activity2 Measure and Compare: use ACTUAL centimeter rulers to measure those SAME objects, accurately align the ZERO tick rather than the outer edge when appropriate, or align the edge if the unlabelled zero is genuinely at the edge of an alternative ruler. Record actual lengths with cm, compare to each estimate, and iterate the ruler along an item longer than the ruler. Genuine objects, ruler calibration and partner conversation need educator observation; numerical first-party examples do not assert exact real-world classroom-object lengths.",
         "A centimeter is the distance between adjacent genuine 1-cm ruler marks, NOT any interval between arbitrary numbers on a poorly spaced diagram. A ruler with zero labeled away from the physical edge must start measuring AT the zero tick, not the outer edge; a ruler whose unlabeled zero is truly at the edge begins there. The length of an object starting at label 1 and ending at label 5 is 5−1=4 cm, not5 cm. Equivalent shift: from 3 to 13 on a centimeter scale measures13−3=10 cm. Real objects are not necessarily whole centimeters; a measurement is approximate to instrument precision. Estimate BEFORE measuring: too low < actual or plausible benchmark < too high; about right is a plausible intermediate estimate, not necessarily identical to a real measured value. Source Activity1 classroom objects mostly5–30 cm and at least one 50–90 cm (physical lengths not predetermined). Activity2 uses physical centimeter rulers and base-ten blocks as optional checks; place the ruler flat and aligned with the object's edge, without shifting, gaps or overlaps. To measure something longer than a 30-cm ruler, measure consecutive full30-cm spans plus the remaining partial span, with no skipped or double-counted segments. Example full30+30+12=72 cm. Addressing standards CCSS2.MD.A.1 and2.MD.A.3.",
         "SOURCE Which One Doesn't Belong? four original pictures A–D; teacher says A, B and D represent a true 4-cm segment, while C does not because intervals are not genuine centimeters. D is from the 1 tick to the 5 tick, so5−1=4 centimeters. SOURCE Activity1: estimate a notebook first; make one too-low, one about-right and one too-high value, then revise after seeing a 10-cm comparison tool. The teacher must collect actual object data; the public source does not specify a fixed notebook length. SOURCE Activity2: measure each selected real classroom object with a standard centimeter ruler, record units and compare with earlier estimates; include a 50–90 cm object to practise iterating ruler placements. FIRST-PARTY Edulytics digital example of a calibrated scale: object from labeled tick2 to tick17 measures15 cm (17−2). FIRST-PARTY illustration of ruler iteration: two30-cm full ruler spans and remainder8 cm are30+30+8=68 cm. FIRST-PARTY independent values only: tick3 to14 =>11 cm, tick5 to23 =>18 cm, full spans30+30+9=>69 cm. These are not publisher diagrams or claims about actual classroom objects.",
         "1. Compare the source's authentic A–D ruler images with a real partner. Check whether each numbered interval represents one real centimeter; A/B/D can show4 cm, while C does not have correct centimeter spacing. Explain why D spans1 to5 =4 real intervals. 2. Before using any measuring tool, estimate lengths for real classroom objects: most between5–30 cm, with at least one between50–90 cm. For the warm-up notebook estimate supply a too-low, about-right and too-high guess. 3. Use a genuine10-cm reference or a ruler built in the previous lesson to revise estimates, recording reasons and pairing explanations. 4. Use an actual standard ruler. If its printed zero tick is inset from the physical edge, align the tick mark with the object's left edge. If zero is genuinely an unlabeled physical edge, align that edge instead. Read the OTHER endpoint and interpret centimeters correctly. 5. If the object does not start on zero, subtract the two labeled readings: from3 to13 measures10, NOT13. 6. If the object is longer than the ruler, place the ruler successively without gaps or overlaps, record complete lengths and final remainder; two30-cm placements and12 more centimeters give72 cm. 7. Record each real measured length with cm and compare with original estimates; teacher confirms actual measurements, object identities and MLR2 discussion. 8. Complete separate first-party number-line and iteration arithmetic with the correct cm label, without copying protected publisher Cool-down The Pencil.",
         "Measuring from the physical edge even when the standard ruler's zero tick is set in from that edge. Assuming the first printed label1 must always be the starting point; some rulers have unlabeled zero at the edge. Saying an object from1 to5 is5 cm (the actual interval is4 cm). Treating arbitrary unequal tick spacing in source image C as centimeters. Confusing numeric position at endpoint with length of a segment starting away from zero. Claiming a specific pencil, notebook or folder is a fixed length despite no real measurement. Making a longer measurement by repeating ruler placements with gaps/overlaps or starting at inconsistent endpoints. Writing69 without 'cm' when the task expects length. Claiming original A–D diagrams, real object measurements, MLR2 talk or protected The Pencil cool-down were completed merely because a digital numeric answer is correct.",
         "IM2021 G2 U03 L04: true centimeter intervals on ruler; A/B/D ruler pictures show4 cm but C has invalid interval spacing, and1→5 is4 cm. First estimate authentic items, mostly5–30 cm plus one50–90 cm; use 10-cm reference and too-low/about-right/too-high estimates. Then align physical ruler at the TRUE zero, measure real objects, iterate along long objects and compare lengths and estimates. Digital examples are first-party and teacher verifies real measuring."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        d.VersionCode=="CCSSM-2010"&&d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
          !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
          !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.A.1","CCSS:2.MD.A.3"}))
            throw new InvalidOperationException("G2 U03 L04 source identity, body SHA or standards changed");
    }
    private static bool Same(CanonicalLessonContentPackTranslation t,bool newer)=>
        Match([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],newer);
    private static bool Same(CurriculumLessonContentTranslation t,bool newer)=>
        Match([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],newer);
    private static bool Match(string[] x,bool newer)=>x.SequenceEqual(newer?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Same(t,true))continue;
            if(!Same(t,false))throw new InvalidOperationException("G2 U03 L04 prior six learner fields changed");
            t.Explanation=S.After[0];t.KeyConceptsAndRules=S.After[1];
            t.WorkedExamples=S.After[2];t.StepByStepSolutions=S.After[3];
            t.CommonMistakes=S.After[4];t.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prev)=>
        IsTarget(d,l)?Version:prev;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prev)=>
        IsTarget(d,l)&&prev==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prev,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,prev))throw new InvalidOperationException("G2 U03 L04 unapproved prior version");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U03 L04 unexpected persisted languages");
        var old=rows.Single();var next=l.Translations.Single(x=>x.CultureCode=="en");
        if(old.Title!=next.Title||!(Same(old,false)||Same(old,true)))
            throw new InvalidOperationException("G2 U03 L04 prior persisted fields changed");
    }
}
