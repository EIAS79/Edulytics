using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>Source-faithful locally archived Grade2 U03 L02 measuring bearded dragon24cm.</summary>
public static class CommonCoreG2Unit3BeardedDragonCmTeachingCorrection
{
    public const string Version="usccss-g2-u03-l02-bearded-dragon-24cm-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U03:L02";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-3/lesson-2/preparation.html";
    private const string Sha="19079895438ac31d574e8a5169b2a4c872b7352af5cc93ab7a5b3823e02547c5";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will measure in centimeters.\nIn an earlier lesson, you learned the importance of using a standard unit to ensure that you can communicate clearly about your measurements. You learned that a single base-ten block is a centimeter cube because each edge of the cube is 1 centimeter long. You use tools (base-ten blocks) that have lengths of 1 centimeter and 10 centimeters to measure the length of objects.",
         "Key ideas:\n- Measure length in centimeters\n- In an earlier lesson, you learned the importance of using a standard unit to ensure that you can communicate clearly about your measurements\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
         "Example 1: Measure the length of the bearded dragon using the single centimeter cubes. The bearded dragon is ___ centimeter cubes long. Measure the bearded dragon using the 10-centimeter tool. The bearded dragon is ___ 10-centimeter tools long. How many centimeters long is the bearded dragon? The bearded dragon is ___ centimeters long.\nApproach: “How can you prove that the bearded dragon is 24 centimeters long with only centimeter cubes?” (Start at one end and line up the cubes until you reach the other end. You can count the cubes because each cube is 1 centimeter long.) “How can you prove the bearded dragon is 24 cm long with 10-centimeter tools?” (Start at one end with one 10-centimeter tool, then keep lining up more 10-centimeter tools until you go...\nExample 2: Measure the length of each reptile in centimeters. ___ cm ___ cm ___ cm ___ cm Compare your measurements with your partner’s measurements. How much longer is the day gecko than the threadsnake?\nApproach: Choose the required unit, align equal units from the endpoint, measure without gaps or overlaps, and record the result with its unit.",
         "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
         "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
         "You will measure in centimeters. In an earlier lesson, you learned the importance of using a standard unit to ensure that you can communicate clearly about your measurements."],
        ["IM K–5 2021 Grade2 Unit3 Lesson2 Measure in Centimeters PLC Activity: move from saying an object measures so many blocks to expressing a precise STANDARD length in centimeters. Warm-up Notice and Wonder: original teacher images A and B both label ONE centimeter, with 'centimeter' spelled out and abbreviation 'cm'. A one-centimeter unit cube has an edge1cm; a ten-block tool is10cm along its long side, not just 'ten cubes' of unspecified length. Activity1 Length in Centimeters: children use REAL centimeter cubes and authentic10-centimeter base-ten rods to measure a printed bearded-dragon drawing; source teacher synthesis states its length is24 centimeters. Accurate representation is24 individual1cm cube edges, or two complete10cm tools PLUS4 more centimeters. An exact count of full10cm rods alone cannot express24cm; two rods cover20cm and 4 more individual cubes fill the remainder. Activity2 Measure with10-centimeter Tools uses the authentic Reptile Length printout; students choose cube/rod combinations for various pictured reptiles, then compare the day gecko length with the threadsnake. The actual lengths of those pictured reptiles are NOT present as numeric values in the public teacher text, so they must be measured with printed materials by the teacher; Edulytics never fabricates those measurements as source data. MLR8 actual pair explanation and physical alignment require teacher review.",
         "One centimeter =1cm in symbol and written word; a true centimeter cube has an edge exactly1cm. A matching10-centimeter base-ten tool represents exactly TEN successive centimeters along its length. To measure any length N cm with rods and centimeter cubes: use q = floor(N/10) full10cm rods and r = N mod10 centimeter cubes; N=10q+r, with0≤r≤9. For24cm, q=2 rods (20cm), r=4 additional centimeters. The count of complete rods alone does NOT equal the whole measurement. To compare two measured lengths, ensure both have the SAME centimeter unit and proper endpoints, then subtract shorter from longer; gecko vs threadsnake actual values require the original printed Reptile Length sheet, not invented numbers. Lay measuring edges end-to-end from correct origin with no gaps or overlaps. The lesson's addressing standards are2.MD.A.1 and2.MD.A.4. Real pupils' measurement and tool choices require direct teacher observation.",
         "SOURCE original warm-up A and B: labels 'centimeter' and 'cm' both mean one unit length. SOURCE Activity1 bearded dragon drawing:24cm measured by twenty-four true1cm cube edges, or two COMPLETE10cm rods plus four 1cm cubes; explain why two rods alone show20cm, not24cm. SOURCE Activity2 original Reptile Length BLM: four reptile diagram-length answers represented as blanks in teacher lesson (A–F are mentioned in synthesis); EXACT pictured values of day gecko and threadsnake are not supplied in accessible public text. These are teacher-measured, not digitally inferred. FIRST-PARTY Edulytics illustration: a new line28cm=2 ten-cm tools+8 centimeter cubes; a fictional gecko length29cm and threadsnake length14cm differ by15cm, explicitly NOT the source animals' actual worksheet lengths. Independent digital exit: length36cm requires3 rods and6 cube edges. Publisher 'Measure with Centimeters' cool-down restricted, not reproduced.",
         "1. Observe two authentic teacher warm-up images; describe why '1 centimeter' and '1 cm' label equal lengths. 2. Handle a true1cm base-ten unit cube and a genuine10cm rod, checking where the unit starts and ends. 3. Use centimeter cubes to measure the actual printed bearded-dragon drawing; count24 equal cube edges, NO gaps or overlaps. 4. With10cm rods measure the SAME printed length: two complete rods cover20cm; add4 true centimeter cubes for the remaining4cm, so total24cm. 5. In Activity2 measure actual Reptile Length sheet pictures using centimeter cubes,10cm rods or a justified mixture. Students compare their own four printed reptile lengths and remeasure if partners disagree. 6. Calculate how much longer the real day gecko is than the real threadsnake using THEIR physically measured source lengths; teacher checks real values from the sheet, no source numeric values invented. 7. Use MLR8 speaking, student choice of measurement tools and the source synthesis examples: five1cm cubes show5cm; five10cm tools show50cm. 8. Complete independent Edulytics virtual integer arithmetic questions distinct from protected teacher cool-down.",
         "Mistaking a 10cm rod for1cm because it is one object, or reporting two10cm rods represent24cm without the four remaining centimeters. Saying 24 complete 10cm rods (240cm) instead of 24 centimeter units. Treating 'cm' and 'centimeter' as two different unit sizes. Leaving gaps/overlaps or starting from a shifted endpoint. Comparing length counts made with DIFFERENT units, rather than converting both to centimeters first. Fabricating lengths of source day gecko/threadsnake printed pictures when the teacher-only numeric measurement sheet is not provided in the archived text. Claiming independently invented 29cm vs14cm animals are the publisher's actual values. Typed arithmetic answers cannot demonstrate physically aligned cubes, accurate original source picture measures or genuine partner conversation.",
         "Grade2 U03 L02: centimeter and cm mean same1cm length; real 10cm rod spans TEN centimeter edges. Source bearded dragon24cm =24 centimeter cube edges =2 ten-cm rods plus4 more centimeters. Use original printed reptile images for true gecko/threadsnake comparison; source worksheet lengths not specified in public lesson text. Teacher reviews authentic measurement; Edulytics independent28cm and36cm examples are separate."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        d.VersionCode=="CCSSM-2010"&&d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.A.1","CCSS:2.MD.A.4"}))
            throw new InvalidOperationException("G2 U03 L02 source URL/hash/outcome identity changed");
    }
    private static bool Match(CanonicalLessonContentPackTranslation x,bool after)=>
        Same([x.Explanation,x.KeyConceptsAndRules,x.WorkedExamples,x.StepByStepSolutions,x.CommonMistakes,x.QuickSummary],after);
    private static bool Match(CurriculumLessonContentTranslation x,bool after)=>
        Same([x.Explanation,x.KeyConceptsAndRules,x.WorkedExamples,x.StepByStepSolutions,x.CommonMistakes,x.QuickSummary],after);
    private static bool Same(string[] x,bool after)=>x.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var en=l.Translations.Single(x=>x.CultureCode=="en");
            if(Match(en,true))continue;
            if(!Match(en,false))throw new InvalidOperationException("G2 U03 L02 prior six content fields changed");
            en.Explanation=S.After[0];en.KeyConceptsAndRules=S.After[1];
            en.WorkedExamples=S.After[2];en.StepByStepSolutions=S.After[3];
            en.CommonMistakes=S.After[4];en.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string old)=>
        IsTarget(d,l)?Version:old;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string old)=>
        IsTarget(d,l)&&old==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string old,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,old))throw new InvalidOperationException("G2 U03 L02 persisted previous content version mismatch");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U03 L02 previous translation rows mismatch");
        var t=rows.Single();var source=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=source.Title||!(Match(t,false)||Match(t,true)))
            throw new InvalidOperationException("G2 U03 L02 previous stored content fields drifted");
    }
}
