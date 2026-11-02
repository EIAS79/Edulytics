using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>IM2021 locally archived G2 U03 L16 source Group B plant height numerical plot;
/// Han's inaccessible original plot never replaced with invented data.</summary>
public static class CommonCoreG2Unit3PlantMeasurementPlotTeachingCorrection
{
    public const string Version="usccss-g2-u03-l16-groupb-plant-lineplot-han-image-v1";
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
        ["IM K–5 2021 Grade2 Unit3 Lesson16 Interpret Measurement Data. Original warm-up Number Talk:15+5+1=21,25+6=31,16+7=23,37+6=43; making ten explains the latter additions. Activity1 The Plant Project: construct a numerical line plot from the PUBLISHED Group B plant-height table (centimeters): Andre33, Clare25, Diego27, Elena25, Han35, Jada33, Kiran26, Noah30, Priya26, Tyler33. Students label a suitable scale from shortest25 to tallest35 (it is NOT necessary to start at zero), show one X per plant at its TRUE height, and include EVERY tick 25–35 including zero-frequency positions. Data counts25:2,26:2,27:1,28:0,29:0,30:1,31:0,32:0,33:3,34:0,35:1; total10 plants. Activity2 Interpret Measurement Data on a Line Plot: ask shortest25cm, tallest35cm and how much taller35−25=10cm, plus questions about Han's OTHER displayed line plot. Han claims his tallest plant29cm, but the original Han plot is an IMAGE without accessible values in locally archived public lesson text; Edulytics must not invent whether his claim is true or its total count. Teacher displays actual Han plot and checks student evidence. Source synthesis: 4 teachers had a hand span of8 inches on a DIFFERENT section-summary sample; do NOT mix inches with plant cm. Original protected Diego’s Art Project Cool-down not copied. First-party independent numerical plot is separate.",
         "A line plot displays counts of numeric measured lengths, ONE equal-size X per item. Each labeled x-axis tick represents a true centimeter measurement regardless of the visual length of drawn tick spacing, just like a ruler with its zero portion hidden; plot axis MAY START at shortest25 rather than0 and MUST include whole numbers even where count is zero. Source Group B sorted heights25,25,26,26,27,30,33,33,33,35cm. Therefore smallest25cm, largest35cm, range10cm, total10, mode33cm (three plants), heights MORE than30cm count4 (three33 and one35), height of30 count1. Source Number Talk21,31,23,43. In Han's distinct visual plot, students must base any claim about maximum29cm or total on ACTUAL image positions; those image counts are not in readable original text. Do not treat 'many Xs' as taller HEIGHT; horizontal axis value gives HEIGHT, vertical stack X count gives FREQUENCY. Units: plant heights in centimeters; independent teacher hand-span summary in inches (4 teachers at8inches) cannot be combined. Outcomes CCSS2.MD.D.9 and2.NBT.B.5.",
         "SOURCE Number Talk:15+5+1=21;25+6=31;16+7=23;37+6=43. SOURCE Group B: Andre33,Clare25,Diego27,Elena25,Han35,Jada33,Kiran26,Noah30,Priya26,Tyler33 (centimeters). SOURCE data display25:XX,26:XX,27:X,28:0,29:0,30:X,31:0,32:0,33:XXX,34:0,35:X. Shortest25cm, tallest35cm, difference35−25=10cm. More-than30cm=4 and most common33cm. SOURCE separate Han line plot asks whether tallest29cm and its total; NOT answerable from text-only archive, requires actual displayed image. SOURCE section summary distinct teacher hand-span plot: 4 Xs at8 inches. FIRST-PARTY Edulytics fictional plant heights21,21,22,24,24,24,26,27cm; total8, mode24cm (3), range27−21=6cm, longer than24cm count2.",
         "1. Solve warm-up sums21,31,23,43 using forming ten and inverse checks. 2. Read original Group B named centimeter measurements and identify minimum25, maximum35 before deciding on the number line; choose labels25 through35 even though the axis does not begin at0. 3. Draw each tick25,26,27,28,29,30,31,32,33,34,35 equally spaced; annotate 'Plant height (cm)' and title 'Group B plant heights'. 4. For the ten original numbers, draw two Xs at25, two at26, one at27, one at30, three at33 and one at35. Keep ticks28,29,31,32,34 as ZERO-count positions rather than removing those labels. 5. Answer source shortest25cm and tallest35cm from horizontal positions, and difference35−25=10cm; most common33cm and four plants taller than30cm can be checked against X counts. 6. Teacher displays the actual separate Han line plot. Independently determine whether its largest x-axis label with an X is29cm and count all plotted Xs, describing evidence; no invented numerical answer without source graphic. 7. Discuss with a real partner using MLR8 why x-axis separation represents length difference and stacked Xs show count; teacher examines actual student-drawn plots. 8. Solve a clearly independent Edulytics fictional-data Cool-down without claiming source Diego’s Art Project restricted assessment.",
         "Confusing the mode33cm (most Xs) with tallest35cm (rightmost X). Guessing a verdict about Han's separate line plot's claimed maximum29cm or its number of plants without seeing the graphic. Treating plot start25 as meaning the plants are0cm tall or omitting skipped-number labels28,29,31,32,34. Counting three Xs above33 as a plant33+3=36cm tall. Subtracting25−35 instead of35−25 for nonnegative height difference. Treating plants' cm and sample teacher handspans' inches as the same data unit or combined dataset. Adding heights to get total number of plants instead of counting Xs. Claiming source teacher-only cool-down was completed based on first-party digital arithmetic.",
         "G2 U03 L16 Interpret Measurement Data: mental additions21,31,23,43; original Group B heights25,25,26,26,27,30,33,33,33,35cm; make line plot from25–35 with ALL labels, including empty ticks. Shortest25, tallest35, difference10cm, total10, most common33, four heights exceed30. Actual separate Han picture needed before verifying his29cm claim. Teacher checks drawings and discussion; Edulytics fictional data are separate."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&d.VersionCode=="CCSSM-2010"&&
        d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.D.9","CCSS:2.NBT.B.5"}))
            throw new InvalidOperationException("G2 U03 L16 original source identity drift");
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
            if(!Same(t,false))throw new InvalidOperationException("G2 U03 L16 source predecessor six fields changed");
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
        if(!CanUpgradeExisting(d,l,prior))throw new InvalidOperationException("G2 U03 L16 unauthorized previous content version");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U03 L16 stored language rows drift");
        var old=rows.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(old.Title!=incoming.Title||!(Same(old,false)||Same(old,true)))
            throw new InvalidOperationException("G2 U03 L16 stored predecessor learner text drifted");
    }
}
