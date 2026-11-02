using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Source-locked original IM 2021 Grade 2 Unit 1 Lesson 10: students author
/// picture AND bar graphs from a data table. Removes broken publisher
/// caption/placeholders in the raw learner examples using Edulytics prose.
/// Exact source SHA/URL/standard and prior persisted fields are mandatory.
/// </summary>
public static class CommonCoreG2Unit1ConstructGraphsTeachingCorrection
{
    public const string Version="us-ccss-g2-u01-l10-construct-two-graphs-v1";
    private const string LessonCode="PED:US-CCSS-MATH:G2:U01:L10";
    private const string SourceUrl="https://im.kendallhunt.com/k5/teachers/grade-2/unit-1/lesson-10/preparation.html";
    private const string CanonicalSha="80647b37a9dfdbb65c88ac871446a0d8e57088195237188dd0c24b5179be6fc2";
    private sealed record Repair(
        string OldExplanation,string OldConcepts,string OldExamples,
        string OldSteps,string OldMistakes,string OldSummary,
        string NewExplanation,string NewConcepts,string NewExamples,
        string NewSteps,string NewMistakes,string NewSummary);
    private static readonly Repair Values = new(
        "Learning goal: You will attend to the features of picture graphs and bar graphs as you represent a given set of categorical data in picture graphs and bar graphs.\nIn the first activity, you are introduced to the idea of using one symbol in a picture graph rather than a picture based on the category. You may find it easier and more efficient to use a symbol when drawing your graphs. In the second activity, you attend to the features of bar graphs as you draw a bar graph to represent a given set of categorical data.",
        "Key ideas:\n- Explain how picture graphs, bar graphs, and tables represent the same data\n- Represent data using a picture graph and a bar graph\n- Read category labels, values, and scales before comparing or calculating with a data display.",
        "Example 1: Which one doesn’t belong? A Caption: A B Caption: B C Caption: C D Caption: D\nExample 2: A group of 2nd graders were asked, “What is your favorite sport?” Their responses are shown in this picture graph. Represent the data shown in your table in a picture graph. Table # ___",
        "Step 1: Read the title, labels, categories, and scale.\nStep 2: Locate the values needed for the question.\nStep 3: Compare, add, subtract, or interpret the values as required.\nStep 4: State the result in the context of the data.",
        "Common mistake: reading the wrong category or scale. Check the labels and scale before using the values.",
        "You will attend to the features of picture graphs and bar graphs as you represent a given set of categorical data in picture graphs and bar graphs. In the first activity, you are introduced to the idea of using one symbol in a picture graph rather than a picture based on the category.",
        "Goal: create a picture graph AND a bar graph from the same four-category data table, using accurately labelled categories, a title, and a clear key or numbered scale. Warm-up: examine four data displays including a horizontal bar graph, vertical bar graph, pictograph and table; give a mathematical reason why more than one could be 'the one that does not belong.' Activity 1: choose one consistent symbol for one response and create your own picture graph from a table. Activity 2: switch data tables with a partner, draw a bar graph with a unit-one numeric scale and one bar per category, and compare the two forms. The partner and teacher review clarity and mathematics; digital counts alone do not substitute for drawing or explanation.",
        "A DATA TABLE lists the count in each named category. A correct pictograph, bar graph or table of the SAME data must keep ALL category totals unchanged.\nA picture graph needs a relevant title, labeled categories, and a key stating how many responses each drawn symbol represents. Choosing an easy-to-draw invented symbol is valid; in this Edulytics lesson the key is one symbol per response.\nA bar graph needs title, category labels, a numbered axis beginning at zero and equally spaced scale ticks. A vertical or horizontal bar represents a category's quantity by its height or length.\nWhen several displays differ, Which One Doesn't Belong can admit several valid explanations; do not force a single picture to be incorrect when more than one attribute distinguishes a display.\nThe original 2021 second-grade sports example has Gymnastics 5, Basketball 4, Hockey 2 and Soccer 6. An independent Edulytics table with genres Comics 6, Science 3, Animals 5, Poetry 2 can be used to author separate pictures and bars.",
        "Original mathematical idea: Use a table with Gymnastics 5, Basketball 4, Hockey 2, Soccer 6; make a picture graph by drawing exactly the right number of repeated symbols in each category, with title, labels and key 'one symbol = one learner'.\nOriginal Edulytics graphing activity: A small survey table lists Comics 6, Science 3, Animals 5, Poetry 2. Draw 6, 3, 5 and 2 original symbols with the key one symbol equals one response. Then draw four bars at heights 6, 3, 5 and 2 on a numbered 0–10 unit-one axis. Explain why the table, pictures and bars all have the same 16 responses even though they look different.",
        "Step 1: Read the assigned data table and check the categories and counts. Identify the population and give the new display an informative title.\nStep 2: Choose an easy-to-draw symbol; write a key such as '● = 1 response'.\nStep 3: For each category, draw EXACTLY as many symbols as the table count. Write category labels and compare each row with the original table.\nStep 4: Trade data tables with your partner for the bar-graph activity or use a different independently authored data table. Write the class/table title and the category names.\nStep 5: Mark equally spaced numbered ticks, including zero and the largest required value. Draw each bar upward from zero to its category's value (or rightward for an accurately labelled horizontal graph).\nStep 6: Check every picture count and bar height against its own table. If two graphs use the SAME table, every category must have the same count; if different tables, do not accidentally blend groups.\nStep 7: Share the two student-created graphs with a partner, identify one clear feature and one possible improvement, and explain in words why table, picture graph and bar graph preserve the category totals.",
        "Mistake: copying blank publisher template placeholders ('Table # ___' or 'Caption') into the student's explanation; create meaningful Edulytics titles, labels and diagrams instead.\nMistake: using different symbol meanings across picture-graph categories without updating the key. This version uses ONE response per symbol.\nMistake: drawing a taller bar because a category name is longer, rather than plotting its count on an equally spaced numbered scale that starts at zero.\nMistake: losing a category or swapping data tables when making two representations; verify all category totals separately.\nMistake: presenting a single 'doesn't belong' answer as uniquely correct when another student can justify a different feature of the four displays.\nMistake: awarding a machine-computed category score as proof that the student actually drew, labeled or explained a picture and bar graph.",
        "Make a correctly titled and labeled picture graph AND a correctly scaled bar graph from original four-category table data. Both must preserve every category count, use a one-response picture key or numbered axis, and be explained and revised with a partner.");

    public static bool IsTarget(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson) =>
        doc.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        doc.VersionCode=="CCSSM-2010" &&
        doc.ContentVersion=="phase30-common-core-rich-content-v1" &&
        lesson.LessonCode==LessonCode;

    private static void Guard(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson)
    {
        if(!IsTarget(doc,lesson) || lesson.SourceUrl!=SourceUrl ||
            !string.Equals(lesson.CanonicalBodySha256,CanonicalSha,
                StringComparison.OrdinalIgnoreCase) ||
            !lesson.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.D.10"}))
            throw new InvalidOperationException("G2 U01 L10 original source identity changed");
    }

    private static bool Match(CanonicalLessonContentPackTranslation t,bool newFields) =>
        t.Explanation==(newFields?Values.NewExplanation:Values.OldExplanation) &&
        t.KeyConceptsAndRules==(newFields?Values.NewConcepts:Values.OldConcepts) &&
        t.WorkedExamples==(newFields?Values.NewExamples:Values.OldExamples) &&
        t.StepByStepSolutions==(newFields?Values.NewSteps:Values.OldSteps) &&
        t.CommonMistakes==(newFields?Values.NewMistakes:Values.OldMistakes) &&
        t.QuickSummary==(newFields?Values.NewSummary:Values.OldSummary);

    private static bool Match(CurriculumLessonContentTranslation t,bool newFields) =>
        t.Explanation==(newFields?Values.NewExplanation:Values.OldExplanation) &&
        t.KeyConceptsAndRules==(newFields?Values.NewConcepts:Values.OldConcepts) &&
        t.WorkedExamples==(newFields?Values.NewExamples:Values.OldExamples) &&
        t.StepByStepSolutions==(newFields?Values.NewSteps:Values.OldSteps) &&
        t.CommonMistakes==(newFields?Values.NewMistakes:Values.OldMistakes) &&
        t.QuickSummary==(newFields?Values.NewSummary:Values.OldSummary);

    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        foreach(var lesson in doc.Lessons)
        {
            if(!IsTarget(doc,lesson))continue;
            Guard(doc,lesson);
            var en=lesson.Translations.Single(t=>t.CultureCode=="en");
            if(Match(en,true))continue;
            if(!Match(en,false))
                throw new InvalidOperationException("G2 U01 L10 original learner prose does not match expected source predecessor");
            en.Explanation=Values.NewExplanation;
            en.KeyConceptsAndRules=Values.NewConcepts;
            en.WorkedExamples=Values.NewExamples;
            en.StepByStepSolutions=Values.NewSteps;
            en.CommonMistakes=Values.NewMistakes;
            en.QuickSummary=Values.NewSummary;
        }
    }
    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string prior) =>
        IsTarget(doc,lesson)?Version:prior;
    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string prior) =>
        IsTarget(doc,lesson) &&
        prior==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string prior,
        IReadOnlyCollection<CurriculumLessonContentTranslation> previous)
    {
        Guard(doc,lesson);
        if(!CanUpgradeExisting(doc,lesson,prior))
            throw new InvalidOperationException("G2 U01 L10 requires exact previous rich-content version");
        if(previous.Count==0)return;
        if(previous.Count!=1 || previous.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 U01 L10 persisted languages unexpected");
        var old=previous.Single();
        var now=lesson.Translations.Single(t=>t.CultureCode=="en");
        if(old.Title!=now.Title || !(Match(old,false)||Match(old,true)))
            throw new InvalidOperationException("G2 U01 L10 previous student text changed; fail closed");
    }
}
