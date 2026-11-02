using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Original IM K5 2021 Grade 2 U01 L09 source-specific bar graph pedagogy.
/// Strict canonical SHA/URL/outcome, exact prior learner fields and persisted
/// predecessor checks refuse unrelated or edited lessons.
/// </summary>
public static class CommonCoreG2Unit1BarGraphsSourceTeachingCorrection
{
    public const string Version="us-ccss-g2-u01-l09-bar-graph-source-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U01:L09";
    private const string SourceUrl="https://im.kendallhunt.com/k5/teachers/grade-2/unit-1/lesson-9/preparation.html";
    private const string SourceSha="edb682adef41d4f2852cdb8213de75dd9787f8421b26cf0e7b20bd6625ef7e54";

    private sealed record Review(
        string BeforeExplanation,string BeforeConcepts,string BeforeSteps,
        string BeforeMistakes,string BeforeSummary,
        string AfterExplanation,string AfterConcepts,string AfterSteps,
        string AfterMistakes,string AfterSummary);
    private static readonly Review Record = new(
        "Learning goal: You will interpret data in a bar graph in order to answer questions.\nIn previous lessons, you learned how to interpret categorical data represented in picture graphs and other displays. In this lesson, bar graphs are introduced as another way to represent categorical data. You compare the features of bar graphs to picture graphs and use the unique features of bar graphs to interpret data and answer questions.",
        "Key ideas:\n- Answer questions based on a bar graph\n- Interpret data represented in a bar graph\n- Read category labels, values, and scales before comparing or calculating with a data display.",
        "Step 1: Read the title, labels, categories, and scale.\nStep 2: Locate the values needed for the question.\nStep 3: Compare, add, subtract, or interpret the values as required.\nStep 4: State the result in the context of the data.",
        "Common mistake: reading the wrong category or scale. Check the labels and scale before using the values.",
        "You will interpret data in a bar graph in order to answer questions. In previous lessons, you learned how to interpret categorical data represented in picture graphs and other displays.",
        "Goal: interpret a bar graph by first reading the title, category labels and the numbered vertical scale. In a bar graph, each solid rectangle's height matches a category's count; compare it with a picture graph containing the same data. Warm-up: notice the counts of cats, dogs, fish and lizards in two kinds of displays, and explain how a bar's height replaces the repeated picture symbols. In Activity 1, write several different true statements about a field-trip-choice bar graph assigned to your group. The data vary among classes, so identify the class and graph before making a statement. In Activity 2, interpret a favorite-season bar graph, find a category count, combine two categories, find more than one pair totaling ten when possible, and calculate the number of all responses.",
        "A bar graph uses numbered ticks and bars to represent how many responses belong to each named category. Read the axis SCALE, including the starting value and tick increments, before interpreting the height of a bar.\nThe same categorical data can appear as a picture graph or a bar graph. In a picture graph, read the image key; in a bar graph, read each bar's height using the numbered scale.\nThe original source warm-up illustrates cat 6, dog 10, fish 5, lizard 3. Different graphs represent the same numerical counts, not different surveyed groups.\nFor a class field-trip-choice graph, write multiple mathematically true statements, including comparisons, combined totals and category counts, not merely a single number.\nFor the source favorite-season graph, winter 7, spring 3, summer 13, fall 3: summer is 13; fall plus spring is 6; winter and spring or winter and fall each total 10; the total class responses are 26.",
        "Step 1: Name which population and category the bar graph describes; read the title, horizontal labels and vertical numeric scale.\nStep 2: Match each bar top to a numerical tick. The bar's height is the CATEGORY COUNT; do not mistake the number of bar segments for a separate key.\nStep 3: For a category question, report that bar's height. For two categories combined, add their bar heights. For how many more, subtract the smaller height from the larger.\nStep 4: For the three source field-trip graphs, decide WHICH classroom's graph is being described and write several original true statements; use details from its categories and scale to justify each statement.\nStep 5: For the original season graph (7 winter, 3 spring, 13 summer, 3 fall), answer all categories and combinations. Both winter+spring and winter+fall make 10; add all four heights to obtain 26.\nStep 6: Explain to a partner why the picture graph and bar graph may represent the same data, and why reading the bar graph scale often avoids counting every item individually. Teacher evaluates student-generated statements and reasoning.",
        "Mistake: counting printed graph grid lines instead of reading the height against its numbered scale; always inspect the tick increments.\nMistake: mixing different classrooms' bar graphs and claiming they show the same population as the source warm-up. Check the chart's title and group.\nMistake: reporting just one pair of seasons totaling 10 and rejecting winter+fall when winter+spring was used. Both satisfy the original 7+3 relationship.\nMistake: counting summer's bar as 14 just because the axis reaches 14; its bar height is 13.\nMistake: adding the favorite-season totals incorrectly: 7+3+13+3=26.\nMistake: treating student-authored group statements or partner reasoning as automatically proven by an integer score.",
        "Read bar graph titles, category labels and number scales. Explain how the same data differ in a picture graph; write multiple true category/combination/comparison statements and use bar heights to answer favorite-season questions.");

    public static bool IsTarget(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson) =>
        doc.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode &&
        doc.VersionCode == "CCSSM-2010" &&
        doc.ContentVersion == "phase30-common-core-rich-content-v1" &&
        lesson.LessonCode == Code;

    private static void Guard(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson)
    {
        if(!IsTarget(doc,lesson) || lesson.SourceUrl!=SourceUrl ||
            !string.Equals(lesson.CanonicalBodySha256,SourceSha,
                StringComparison.OrdinalIgnoreCase) ||
            !lesson.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.D.10"}))
            throw new InvalidOperationException("G2 U01 L09 bar graph source identity or standard drift");
    }
    private static bool Matches(CanonicalLessonContentPackTranslation t,bool newer) =>
        t.Explanation==(newer?Record.AfterExplanation:Record.BeforeExplanation) &&
        t.KeyConceptsAndRules==(newer?Record.AfterConcepts:Record.BeforeConcepts) &&
        t.StepByStepSolutions==(newer?Record.AfterSteps:Record.BeforeSteps) &&
        t.CommonMistakes==(newer?Record.AfterMistakes:Record.BeforeMistakes) &&
        t.QuickSummary==(newer?Record.AfterSummary:Record.BeforeSummary);

    private static bool Matches(CurriculumLessonContentTranslation t,bool newer) =>
        t.Explanation==(newer?Record.AfterExplanation:Record.BeforeExplanation) &&
        t.KeyConceptsAndRules==(newer?Record.AfterConcepts:Record.BeforeConcepts) &&
        t.StepByStepSolutions==(newer?Record.AfterSteps:Record.BeforeSteps) &&
        t.CommonMistakes==(newer?Record.AfterMistakes:Record.BeforeMistakes) &&
        t.QuickSummary==(newer?Record.AfterSummary:Record.BeforeSummary);

    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        foreach(var lesson in doc.Lessons)
        {
            if(!IsTarget(doc,lesson))continue;
            Guard(doc,lesson);
            var en=lesson.Translations.Single(t=>t.CultureCode=="en");
            if(Matches(en,true))continue;
            if(!Matches(en,false))
                throw new InvalidOperationException("G2 U01 L09 unexpected preexisting teacher/learner prose");
            en.Explanation=Record.AfterExplanation;
            en.KeyConceptsAndRules=Record.AfterConcepts;
            en.StepByStepSolutions=Record.AfterSteps;
            en.CommonMistakes=Record.AfterMistakes;
            en.QuickSummary=Record.AfterSummary;
        }
    }
    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string before) =>
        IsTarget(doc,lesson)?Version:before;
    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string before) =>
        IsTarget(doc,lesson) &&
        before==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string before,
        IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(doc,lesson);
        if(!CanUpgradeExisting(doc,lesson,before))
            throw new InvalidOperationException("G2 U01 L09 requires exact rich-content predecessor version");
        if(rows.Count==0)return;
        if(rows.Count!=1 || rows.Single().CultureCode!="en")
            throw new InvalidOperationException("Unexpected grade2 source translation set");
        var old=rows.Single();var after=lesson.Translations.Single(t=>t.CultureCode=="en");
        if(old.Title!=after.Title || old.WorkedExamples!=after.WorkedExamples ||
            !(Matches(old,false)||Matches(old,true)))
            throw new InvalidOperationException("G2 U01 L09 persisted learner prose differs from verified prior");
    }
}
