using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>G2 U03 L16 local IM2021 source, real Group B plant heights and Han image evidence boundary.</summary>
public static class CommonCoreG2Unit3PlantHeightsTeachingCorrection
{
    public const string Version="usccss-g2-u03-l16-groupb-plant-height-lineplot-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U03:L16";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-3/lesson-16/preparation.html";
    private const string Sha="0f959a4fa8961fbaa8ecee7e105bb6a71b9cc631563ff637c2f39a25c9fad9b8";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will create and interpret a line plot.\nYou will use data presented in a table to determine the longest and shortest length and then create a line plot and interpret the data to answer questions. When working with measurements with larger numbers, you learn to set up your number line to start with the shortest length.",
         "Key ideas:\n- Interpret data presented in a line plot\n- Represent numerical data in a line plot using an appropriate scale\n- Read category labels, values, and scales before comparing or calculating with a data display.",
         "Example 1: Find the value of each expression mentally. \\(15 + 5 + 1\\) \\(25 + 6\\) \\(16 + 7\\) \\(37 + 6\\)\nApproach: \"How do you think the third expression could help with finding the value of the fourth one?\" (In the ones place there was a 6 and 7, so I knew that \\(6 + 4 = 10\\) and there were 3 left over. In the fourth expression the ones were switched, but it was still 10 with 3 left over.)\nExample 2: Use the data in this table to create a line plot. Group B plant heights (centimeters) Andre 33 Clare 25 Diego 27 Elena 25 Han 35 Jada 33 Kiran 26 Noah 30 Priya 26 Tyler 33\nApproach: “How did you decide which numbers to start and end your line plot with?” (I looked at the table to find the shortest and longest lengths.) “What can you say about the height of your plants by looking at the line plot?” (There are 3 Xs over 33, so those plants all measured 33 cm. Only 1 plant was 27 cm.) “How many you had a plant that measured more than 30 cm?” (4 because 3 had 33 and 1 had 35).",
         "Step 1: Read the title, labels, categories, and scale.\nStep 2: Locate the values needed for the question.\nStep 3: Compare, add, subtract, or interpret the values as required.\nStep 4: State the result in the context of the data.",
         "Common mistake: reading the wrong category or scale. Check the labels and scale before using the values.",
         "You will create and interpret a line plot. You will use data presented in a table to determine the longest and shortest length and then create a line plot and interpret the data to answer questions."],
        ["IM K–5 2021 Grade 2 Unit 3 Lesson 16 Interpret Measurement Data. Warm-up Number Talk mentally evaluates15+5+1=21;25+6=31;16+7=23;37+6=43, using composing a ten. Activity1 The Plant Project: in real pairs, create a centimeter line plot for the PUBLIC Group B plant table Andre33, Clare25, Diego27, Elena25, Han35, Jada33, Kiran26, Noah30, Priya26, Tyler33. Decide scale from the minimum25cm to maximum35cm rather than starting at0. One X represents exactly one plant; show EVERY consecutive tick25 through35 (including empty lengths28,29,31,32,34), an appropriate title, uniform Xs, and centimeter units. Activity2 Interpret Measurement Data on a Line Plot: use the line plot made in Activity1 to identify minimum25, maximum35, difference35−25=10cm, ten total plants and FOUR plants greater than30cm. Students also interpret a DIFFERENT line plot created by Han, asking whether its tallest plant is29cm, total count, and a data-based statement; this additional original image's actual counts and marks are not in available archive text, so they CANNOT be certified digitally without the image. MLR8 teacher listens for comparative language during real partner dialogue. Restricted Diego's Art Project Cool-down is not copied; original Edulytics exit is separate.",
         "A line plot has a numerical horizontal scale whose consecutive whole-centimeter labels are equally spaced. A plot may START at the shortest recorded measurement instead of zero, but the numbers always denote genuine lengths from a ruler's zero point. For SOURCE Group B lengths25,25,26,26,27,30,33,33,33,35cm, X frequency:25→2;26→2;27→1;28→0;29→0;30→1;31→0;32→0;33→3;34→0;35→1. Total number of Xs10. Shortest25cm, longest35cm, range35−25=10cm. Count heights GREATER than30: three33cm plus one35cm =4; 30 itself is NOT greater than30. Tallest and most frequent/mode are different ideas: tallest35cm, most common33cm. On public Group B plot all measurements are whole centimeters. Original Han's separate line plot is image-specific: never invent if tallest29 or number of plants without reading the actual graphical marks. The independently authored first-party plot and actual real class partner work are not source data. CCSS addressing 2.MD.D.9 and2.NBT.B.5.",
         "SOURCE warm-up15+5+1=21;25+6=31;16+7=23;37+6=43. ORIGINAL teacher Group B table exactly: Andre33cm, Clare25cm, Diego27cm, Elena25cm, Han35cm, Jada33cm, Kiran26cm, Noah30cm, Priya26cm, Tyler33cm. Sorted25,25,26,26,27,30,33,33,33,35. Complete plot25:XX 26:XX 27:X 28:none 29:none 30:X 31:none 32:none 33:XXX 34:none 35:X. Answer source question shortest25cm, tallest35cm, difference10cm; four plants taller than30cm. A distinct source 'Han's line plot' asks if tallest29cm, but the image is not represented by numerical text; leave this question for teacher visually reviewing actual original figure. FIRST-PARTY Edulytics fictional heights:24,25,25,26,26,26,27,28,28cm (nine plants). Fictional plot minimum24, maximum28, modal height26, spread4, three plants taller than26; never describe these as Group B.",
         "1. Perform Number Talk 21,31,23,43, relating16+7 and37+6 by composing a ten from the ones. 2. Read original ten-person Group B table; note heights are CENTIMETERS and lowest25, highest35. 3. Create a personal line plot on a REAL template using a scale of whole-centimeter ticks from25 to35 inclusive. Mark labels at28,29,31,32 and34 even though their X count is zero. 4. Place two identical-sized Xs over25, two over26, one over27, one over30, three over33 and one over35, vertically aligned, and label plot and unit. 5. Use that source plot to answer minimum25, maximum35, difference35−25=10cm, number10, and number ABOVE30 =4 (do not count30). 6. Show your real plot to a partner; compare tick starts, how two same datasets look and explanations using MLR8 frames with shortest, tallest and greater. Teacher checks actual plot and pair work. 7. For the independent publisher graphic drawn by Han, examine that ACTUAL published plot before deciding whether its tallest is29, how many Xs exist or what assertion can be made; values cannot be inferred from text alone. 8. Complete Edulytics fictional data interpretation and separate first-party Cool-down; do not claim either represents the restricted source assessment.",
         "Starting scale at0 and not leaving sufficient room for heights25–35; choosing a shorter nonzero-start scale is allowed. Skipping blank tick labels28,29,31,32 or34 distorts the unit intervals. Treating an X stack height as centimeters (instead, stack counts frequency; the horizontal position indicates actual height). Counting 30cm as GREATER than30, giving5 rather than4. Confusing commonest33cm with tallest35cm. Forgetting to count all10 individual students. Guessing how many measurements or maximum height appear in Han's different image-only line plot (the source archive text does not supply its marks). Attributing independently invented fictional sample data to real Group B children. Inferring real paper plots, MLR8 conversation or teacher signoff from digital arithmetic.",
         "Group B source plants25,25,26,26,27,30,33,33,33,35cm. Build plot ticks25..35 including empty labels, frequencies2,2,1,0,0,1,0,0,3,0,1. Shortest25, tallest35, difference10cm, total10 plants and4 plants taller than30. Number Talk21,31,23,43. Han's additional image-specific line plot requires the actual figure; independently authored nine-plant Edulytics plot and teacher-reviewed authentic work are separate."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        d.VersionCode=="CCSSM-2010"&&d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
           !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.D.9","CCSS:2.NBT.B.5"}))
           throw new InvalidOperationException("G2 U03 L16 original body/standards drifted");
    }
    private static bool Same(CanonicalLessonContentPackTranslation t,bool after)=>
        Match([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Same(CurriculumLessonContentTranslation t,bool after)=>
        Match([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Match(string[] x,bool after)=>x.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Same(t,true))continue;
            if(!Same(t,false))throw new InvalidOperationException("G2 U03 L16 prior six learner fields changed");
            t.Explanation=S.After[0];t.KeyConceptsAndRules=S.After[1];
            t.WorkedExamples=S.After[2];t.StepByStepSolutions=S.After[3];
            t.CommonMistakes=S.After[4];t.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous)=>
        IsTarget(d,l)?Version:previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string old)=>
        IsTarget(d,l)&&old==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string old,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,old))throw new InvalidOperationException("G2 U03 L16 persistence predecessor drift");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 U03 L16 prior language row mismatch");
        var actual=rows.Single();var expected=l.Translations.Single(x=>x.CultureCode=="en");
        if(actual.Title!=expected.Title||!(Same(actual,false)||Same(actual,true)))
            throw new InvalidOperationException("G2 U03 L16 persisted learner text drift");
    }
}
