using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>IM2021 G2 U03 L14 source-locked hand-span numerical line plot,
/// separate original fictional practice from live class data.</summary>
public static class CommonCoreG2Unit3HandSpanLinePlotTeachingCorrection
{
    public const string Version="usccss-g2-u03-l14-hand-span-numerical-line-plot-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U03:L14";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-3/lesson-14/preparation.html";
    private const string Sha="45ef6983eab08969e03c2238a1d8b9fc742779b1ac784ac47f56f0ea0801b060";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will learn about the ways a line plot can be used to represent data collected from measuring objects.\nIn a previous unit, you created and interpreted two representations of categorical data, bar graphs and picture graphs. In a previous lesson, you measured length using metric and customary units. In this lesson, a line plot is defined as a way to show how many of each measurement using an x for each measurement.",
         "Key ideas:\n- Interpret a line plot\n- Understand that a line plot is used to represent and interpret numerical data\n- Read category labels, values, and scales before comparing or calculating with a data display.",
         "Example 1: Trace your hand. (Spread your fingers wide.) Draw a line from your thumb to your pinky. This line represents your hand span. Measure the length of your hand span in inches. My hand span is ___ inches.\nApproach: “What does the total number of Xs represent?” (Everybody's measurement. Its the same number as the number of people in our class today.)\nExample 2: What was the longest hand span? What was the shortest hand span? Write another statement about our class’ hand spans based on the line plot.\nApproach: “Which hand span length was most common?” (The measurement that has the most Xs represents the length that was most common in our class. This is different from having the most votes. ___ inches was the most common measurement.)",
         "Step 1: Read the title, labels, categories, and scale.\nStep 2: Locate the values needed for the question.\nStep 3: Compare, add, subtract, or interpret the values as required.\nStep 4: State the result in the context of the data.",
         "Common mistake: reading the wrong category or scale. Check the labels and scale before using the values.",
         "You will learn about the ways a line plot can be used to represent data collected from measuring objects. In a previous unit, you created and interpreted two representations of categorical data, bar graphs and picture graphs."],
        ["IM K–5 2021 Grade2 Unit3 Lesson14 What is a Line Plot? Warm-up Notice and Wonder, “Handy” Graphs, compares an authentic categorical glove-size bar graph and a numerical hand-span line plot; teacher displays the original pictures. Activity1 Measure Our Hand Spans: learners spread thumb and pinky, trace their hand, draw from thumb tip to pinky tip, and measure the resulting HAND SPAN to the nearest whole INCH using a real 12-inch ruler, checking with a partner. Every student puts one equally-sized sticky-note X above that measurement on a class line plot with a prepared scale from 0 to 10 inches. Each X represents ONE measurement, and the total X count equals the total number of measurements (normally class attendance). Activity2 Interpret Our Numerical Data: add a title and a measurement-in-inches axis, then answer the authentic questions 'What was the longest hand span?', 'What was the shortest?' and write another justified statement (for example most common hand span). The plot's equal-spaced horizontal ticks resemble a ruler's NUMBER SCALE, but physical distances on the plotted axis need not equal actual inches; LABELS indicate the unit. Teacher checks actual hand measurements, class plot, real pair talk and MLR8. Digital Edulytics demonstrates a separate FICTIONAL hand-span data set only and never asserts class data or official Cool-down Hand Spans.",
         "A LINE PLOT shows how often each NUMERICAL measured length occurs. The horizontal number line has ordered equally-spaced labels with the UNIT 'inches'. Place ONE X directly above the correct length for each student measurement, in aligned rows; more Xs at a position means more students measured THAT LENGTH, not a longer hand span. The longest/shortest value is the largest/smallest HORIZONTAL x-axis length with at least one X, not the highest stack. The most common length is the x-axis value with the most Xs; the total count is all Xs combined. A bar graph groups categorical glove sizes while a line plot groups numerical hand-span lengths, though both summarize counts. REAL class: hand trace from thumb to pinky, measure nearest whole inch from ruler zero, partners check; one sticky-note X per measurement on 0–10 teacher class number line. First-party example has fictional counts at 4in:1,5in:2,6in:3,7in:2,8in:1; total9, shortest4in, longest8in, mode6in, frequency of7in=2. Original class values unknown; do not substitute invented data for class measurements. CCSS addressing 2.MD.A.1,2.MD.D.9.",
         "AUTHENTIC SOURCE: original warm-up two separate displays labeled glove sizes (categorical bar graph) and hand spans (numerical line plot). Students trace spread hands, measure tip-of-thumb to tip-of-pinky to nearest whole inch, attach one equal-sized X sticky note over correct length on the 0–10 class scale. Actual class shortest, longest, most common and attendance count depend on live student measurements and CANNOT be known ahead of time. FIRST-PARTY fictional line plot: hand span (in)4: X;5: XX;6: XXX;7: XX;8: X. Thus shortest4in, longest8in, most common6in, total9 measurements, 7in frequency2, range8−4=4in. The fictional diagram is separately invented to demonstrate the source skill; it is NOT a source image or a real class dataset.",
         "1. With an authentic partner view the original 'Handy' categorical bar and hand-span line plot pictures; explain similarities and differences in axis labels, numerical measurements and counts. 2. Spread fingers wide, trace your actual hand, draw a segment from tip of pinky to tip of thumb, then measure this span to nearest whole inch with a real ruler placed from zero. 3. Have your partner independently confirm the measurement. 4. Write one X on one equal-sized sticky note and place it directly ABOVE that numerical hand-span length on the teacher-prepared 0–10 class chart; stack notes neatly and keep vertical X sizes aligned. 5. Title the completed source chart 'Class hand spans', mark the horizontal label 'Hand span (inches)' and check scale values increase equally. 6. For original class questions, locate leftmost and rightmost measured numbers for shortest/longest; count Xs at each value for frequency and most common; count ALL Xs for total measurements. Teacher validates real numbers and participation. 7. Compare a glove-size categorical bar graph to the new NUMERIC line plot; MLR8 student conversations about class measurements need direct classroom observation. 8. Separately complete Edulytics invented fictional data analysis (4in one,5in two,6in three,7in two,8in one) without representing it as measured class data or the educator-only Cool-down.",
         "Reading the height of a pile of Xs as the LENGTH of the hand span instead of the FREQUENCY of that length. Treating the rightmost X as the most common rather than the LONGEST measurement. Making different-sized Xs or shifting them sideways so they cannot be counted. Omitting the 'inches' axis unit or equating the number-line graphic distance to a real 1-inch segment. Measuring hand length from wrist to fingertip instead of the source-defined thumb-to-pinky spread. Adding an X per person AND another X for each inch, so total Xs no longer equals number of measurements. Replacing unknown authentic class data with fabricated fictional counts. Claiming that arithmetic demonstrates real paper hand tracing, partner checks, class discussion or teacher Cool-down completion.",
         "IM2021 G2 U03 L14: compare glove-size CATEGORY bar graph with NUMERIC hand-span line plot. Measure real spread thumb-to-pinky hand span to nearest inch, partner check, add one X per measurement on 0–10 equally-spaced inch scale, title and label it. Find longest, shortest, most common lengths from horizontal NUMBERS, not height of stack; total Xs is total measured hands. Teacher certifies authentic class plot; first-party fictional plot demonstrations are distinct."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&d.VersionCode=="CCSSM-2010"&&
        d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.A.1","CCSS:2.MD.D.9"}))
            throw new InvalidOperationException("G2 U03 L14 source SHA/standard drift");
    }
    private static bool Same(CanonicalLessonContentPackTranslation t,bool after)=>
        Equal([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Same(CurriculumLessonContentTranslation t,bool after)=>
        Equal([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Equal(string[] v,bool after)=>v.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var e=l.Translations.Single(x=>x.CultureCode=="en");
            if(Same(e,true))continue;
            if(!Same(e,false))throw new InvalidOperationException("G2 U03 L14 learner predecessor drift");
            e.Explanation=S.After[0];e.KeyConceptsAndRules=S.After[1];
            e.WorkedExamples=S.After[2];e.StepByStepSolutions=S.After[3];
            e.CommonMistakes=S.After[4];e.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous)=>
        IsTarget(d,l)?Version:previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous)=>
        IsTarget(d,l)&&previous==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,previous))throw new InvalidOperationException("G2 U03 L14 unauthorized persisted upgrade");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U03 L14 translation rows changed");
        var t=rows.Single();var source=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=source.Title||!(Same(t,false)||Same(t,true)))
            throw new InvalidOperationException("G2 U03 L14 six previous learner fields changed");
    }
}
