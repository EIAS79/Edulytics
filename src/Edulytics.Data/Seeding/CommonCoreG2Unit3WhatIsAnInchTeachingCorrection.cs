using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>IM K–5 2021 G2 U03 L08 local original inch tile/ruler source, no invented lengths.</summary>
public static class CommonCoreG2Unit3WhatIsAnInchTeachingCorrection
{
    public const string Version="usccss-g2-u03-l08-inch-tile-whole-inch-source-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U03:L08";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-3/lesson-8/preparation.html";
    private const string Sha="c1891c5171ac4d25f4188c110885e8c17c777e017830a8b0269261b2bd6ef5f5";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will learn that an inch is a standard length unit in the U.\nIn previous lessons, you learned the importance of using standard length units to measure and compare lengths. You learned about length units from the metric system and measured the length of objects in centimeters and meters with different tools, including rulers and meter sticks. In this lesson, you learn that an inch is a standard unit of measure in the U.S. customary system.",
         "Key ideas:\n- Define an inch as a unit of measure\n- Use a ruler to measure length in inches\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
         "Example 1: Find 2 items that are about an inch long. ___ ___ Measure the length of each object. object to measure length in inches marker \\(\\phantom{\\hspace{2.5cm} \\\\ \\hspace{2.5cm}}\\) colored pencil \\(\\phantom{\\hspace{2.5cm} \\\\ \\hspace{2.5cm}}\\) 11 connecting cubes \\(\\phantom{\\hspace{2.5cm} \\\\ \\hspace{2.5cm}}\\) a book \\(\\phantom{\\hspace{2.5cm} \\\\ \\hspace{2.5cm}}\\)...\nApproach: “How can we describe the length of the ___ when its end doesn’t line up with one of the inch marks on the ruler?” (If it is close to 3 inches, I can write “about 3 inches.”)\nExample 2: Here is a rectangle. How long is the long side of the rectangle in inches? Estimate: ___ Measure the long side of the rectangle. Actual length: ___ Here is a square. How long is a side of the square in inches? Estimate: ___ Measure one side of the square. Actual length: ___ Here is a triangle. How long is the longest side of the triangle in inches?...\nApproach: “I noticed that some you had different measurements for the longest side of the triangle. Why do you think that is?” (It was less than 6, but more than 5. It was in between 5 and 6 inches.)",
         "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
         "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
         "You will learn that an inch is a standard length unit in the U. In previous lessons, you learned the importance of using standard length units to measure and compare lengths."],
        ["G2 Unit3 Lesson8 What is an Inch? (IM K–5 2021). The INCH is a U.S. customary unit of length, longer than a centimeter but shorter than a meter. Original warm-up Notice and Wonder compares two ruler images with DIFFERENT-sized units: both show starting at0, but distance from0 to1 differs. Teacher displays authentic images and confirms one ruler counts cm and another counts inches. Activity1 in groups of four uses physical inch tiles: each square tile SIDE measures exactly1 inch, matching0-to-1 on an inch ruler. Pupils find TWO real objects about1 inch long; then estimate/measure genuine marker, colored pencil, line of11 connecting cubes, book and two self-chosen real items shorter than12 inches using 12-inch rulers or inch tiles. Actual lengths vary and cannot be supplied by Edulytics. If endpoint lies between marks, say about the NEAREST whole inch; do not record halves at this grade stage. Activity2 uses actual student book geometry artwork: estimate FIRST and measure with inch rulers the rectangle LONG side, square side and triangle LONGEST side. The public source only says the triangle side lies between5 and6 inches; no exact hidden dimension is asserted. MLR8 student discussion, original physical measurements and ruler alignment require teacher review; protected Measure a Rectangle cooldown is not reproduced.",
         "An inch (in) is a STANDARD U.S. customary length unit. Inch TILE square SIDE corresponds to length from0 to1 on a standard inch ruler. To measure real objects, align zero with the actual starting edge, use evenly-spaced inch intervals end to end with no gaps/overlaps and read the endpoint. The number of inch units to cover a fixed length can differ from the number of centimeters, since 1 inch is longer than1 cm; no precise decimal cm conversion is required here. For objects between whole-inch marks, the PUBLIC teacher instruction is choose the closest WHOLE-number inch and say 'about N inches'; do not require half-inch or decimal recorded measurements. For a length more than5 but less than6, students may appropriately explain 'between5 and6' and choose the nearest whole based on the actual physical endpoint; without original image measurements, a numerical answer cannot be fixed in advance. Source preparation groups of4 have physical objects between1 and11 inches; source Activity2 shapes rectangle, square, triangle have no fixed universally accessible measurement from text alone. Teacher separately assesses real inch tile/ruler work. Addressing standards 2.MD.A.1 and2.MD.A.3 ONLY.",
         "SOURCE warm-up: authentic image of inch ruler and centimeter ruler; both count from0, but physical intervals0–1 DIFFER in length. SOURCE Activity1: tile's side is1 inch, find two classroom objects approximately1 inch, physically measure a marker, colored pencil, 11 connecting cubes, book, and two objects of your choice; written lengths vary. SOURCE Activity2: estimate and physically measure rectangle's long side, one square side and a triangle's longest side in inches; source triangle discussion notes a value GREATER than5 and LESS than6 inches, and nearest whole inch depends on endpoint. FIRST-PARTY Edulytics virtual rulers (not source printed images): length from0 to4 means4 inch intervals; 7 adjacent inch tiles make7 inches; virtual endpoint5.2 in is about5 whole inches, while5.8 in is about6; digital decimals are internal practice representations, NOT required student notation or authentic printed shape readings.",
         "1. Look at original source warm-up rulers and discuss why identical number labels do not imply equal physical centimeter and inch units. 2. With four classmates handle a real inch tile; align one side with the0 and1 marks of a real inch ruler, notice they correspond. 3. Choose two genuine classroom objects about one inch long and name them based on physical comparison, not invented standard lengths. 4. Measure each source-listed classroom item with an inch ruler or real repeated tiles, aligning zero and avoiding overlaps; record inch units and say 'about' when not exactly on a whole mark. 5. For source Activity2 FIRST estimate long rectangle side, square side and longest triangle side from the actual original student book artwork. THEN measure with a real ruler and record the observed values in whole inches, not required fractions. 6. Teacher leads MLR8 partner comparison and discusses triangle whose side lies somewhere between5 and6 inches; select nearest whole based on actual endpoint. 7. Separate original Edulytics numeric virtual illustrations may count ruler tick intervals and determine nearest integer from explicitly invented sub-inch endpoints; do not assert these were source art measurements or physical classroom actions. 8. Original Measure a Rectangle Cool-down is protected, not copied.",
         "Treating inch and centimeter tick intervals as physically identical because both ruler pictures show0 and1. Assuming a tile face AREA is1 inch rather than ONE SIDE LENGTH is1 inch. Recording all markers or books as a fixed given number of inches; real items vary. Starting a ruler at1 instead of0, leaving gaps between tiles, overlapping tiles or choosing the wrong endpoint. Recording half inches or decimal final measures for tasks that ask pupils to report about the nearest whole inch. Claiming the source triangle's longest side measures exactly6 inches: public text says only it is between5 and6, and genuine endpoint/ruler are required to choose the nearest. Confusing a virtual first-party ruler example with the publisher's actual geometric shape or a real pupil measurement. Teacher must observe physical manipulation and speaking.",
         "Unit3 Lesson8 introduces INCH tiles and inch rulers, and explains that an inch is longer than a centimeter. Real group-of-four measurement of classroom objects shorter than12 inches uses physical0-aligned rulers and approximately whole-inch estimates. Source rectangle, square and triangle sides are estimated BEFORE real measurement; triangle longest side falls between5 and6 inches, no invented exact dimension. First-party Edulytics virtual ruler checks stay separate from authentic classroom measurements."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&d.VersionCode=="CCSSM-2010"&&
        d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.A.1","CCSS:2.MD.A.3"}))
            throw new InvalidOperationException("G2 U03 L08 source URL/hash/standard drifted");
    }
    private static bool Same(CanonicalLessonContentPackTranslation t,bool after)=>
        Equal([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Same(CurriculumLessonContentTranslation t,bool after)=>
        Equal([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Equal(string[] a,bool after)=>a.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Same(t,true))continue;
            if(!Same(t,false))throw new InvalidOperationException("G2 U03 L08 old six rich fields changed");
            t.Explanation=S.After[0];t.KeyConceptsAndRules=S.After[1];
            t.WorkedExamples=S.After[2];t.StepByStepSolutions=S.After[3];
            t.CommonMistakes=S.After[4];t.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prior)=>
        IsTarget(d,l)?Version:prior;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prior)=>
        IsTarget(d,l)&&prior==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prior,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,prior))throw new InvalidOperationException("G2 U03 L08 predecessor rich version unexpected");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 U03 L08 persisted language rows changed");
        var t=rows.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=incoming.Title||!(Same(t,false)||Same(t,true)))
            throw new InvalidOperationException("G2 U03 L08 predecessor six learner text fields drifted");
    }
}
