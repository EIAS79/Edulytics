using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// IM 2021 Grade2 U01 L11: prior lesson learner-made graphs are used to
/// author FOUR questions and exchange answers, with at least one two-category
/// question. Original source identity and old published prose are locked.
/// </summary>
public static class CommonCoreG2Unit1QuestionsAboutDataTeachingCorrection
{
    public const string Version="us-ccss-g2-u01-l11-question-authoring-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U01:L11";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-1/lesson-11/preparation.html";
    private const string Sha="bb48fc1b9a5b06c4543d4bec2ae474da67edf6f7402c454ac0d04b6f9cc7bffd";
    private sealed record Review(
        string BeforeExplanation,string BeforeConcepts,string BeforeExamples,
        string BeforeSteps,string BeforeMistakes,string BeforeSummary,
        string AfterExplanation,string AfterConcepts,string AfterExamples,
        string AfterSteps,string AfterMistakes,string AfterSummary);
    private static readonly Review R=new(
        "Learning goal: You will ask and answer questions about a given set of categorical data represented in picture and bar graphs.\nIn previous lessons, you interpreted picture graphs and bar graphs and answered questions using the data presented in graphs. In this lessons, you generate your own questions about the data represented in the graphs you created in the previous lesson. You use your classmates’ picture graphs and bar graphs to answer student-generated questions about the data.",
        "Key ideas:\n- Generate and answer questions about data represented in picture graphs and bar graphs\n- In previous lessons, you interpreted picture graphs and bar graphs and answered questions using the data presented in graphs\n- Read category labels, values, and scales before comparing or calculating with a data display.",
        "Example 1: Find the value of each sum mentally. \\(3 + 7\\) \\(3 + 7 + 2\\) \\(5 + 7\\) \\(2 + 4 + 8\\)\nExample 2: Write 4 questions you could ask about the data in your graphs. Make sure you have 2 for each graph. \\(\\underline{\\hspace{25 cm}}\\) \\(\\underline{\\hspace{25 cm}}\\) \\(\\underline{\\hspace{25 cm}}\\) \\(\\underline{\\hspace{25 cm}}\\) \\(\\underline{\\hspace{25 cm}}\\) \\(\\underline{\\hspace{25 cm}}\\) \\(\\underline{\\hspace{25 cm}}\\) \\(\\underline{\\hspace{25 cm}}\\) Bonus...\nApproach: \"How do you know these questions will require someone to combine or compare the totals of two or more categories?\" (The questions ask about two or more categories. It doesn't just ask to tell how many are in one category or which category had the most. I see words like \"how many more\" or \"how many less.\")",
        "Step 1: Read the title, labels, categories, and scale.\nStep 2: Locate the values needed for the question.\nStep 3: Compare, add, subtract, or interpret the values as required.\nStep 4: State the result in the context of the data.",
        "Common mistake: reading the wrong category or scale. Check the labels and scale before using the values.",
        "You will ask and answer questions about a given set of categorical data represented in picture and bar graphs. In previous lessons, you interpreted picture graphs and bar graphs and answered questions using the data presented in graphs.",
        "Learning goal: WRITE four new questions for two previously created categorical graphs, two questions about each graph, then have a new partner choose the correct graph to find and explain each answer. Warm-up Number Talk adds within twenty by making ten in 3+7, 3+7+2, 5+7 and 2+4+8. Activity 1: use the learner-authored picture graph and bar graph from the PREVIOUS lesson, write two valid mathematical questions for each, mix the order, and ensure at least ONE question combines or compares two categories. Activity 2: exchange those graph questions with a different partner, answer using category labels, scales and data rather than guessing, compare answers, and revise unclear questions. This lesson builds directly on the original Grade 2 U01 L10 construction task; if saved pupil drawings are unavailable, bring the paper drawing or reconstruct your data table rather than pretending digital persistence.",
        "Each authored question must identify a graph and a specific thing the graph can actually tell us. Write TWO questions about a picture graph and TWO about a bar graph, in mixed order, and ask at least ONE multi-category combination/comparison question.\nA category COUNT asks how many in one group; a COMBINED TOTAL asks how many in two categories together; a COMPARISON asks how many more or fewer one group has than another. The exact answer comes from the graph category values and numbered key or axis.\nQuestions about why learners liked a category or what another population preferred are NOT answerable if those facts were never collected.\nStudent-created questions and partner explanations need educator review; a number checker can validate selected graph arithmetic but cannot determine whether an open-ended question is clear.\nThis Edulytics companion reuses the independently authored previous-lesson tables A (Comics 6, Science 3, Animals 5, Poetry 2) and B (Music 4, Drawing 6, Soccer 3, Reading 7) when saved learner drawings are not available. These fictional examples are not actual student votes.",
        "Warm-up: 3+7=10, 3+7+2=12, 5+7=12 (or 5+5+2=12), 2+4+8=14 (2+8+4=14).\nFrom Edulytics picture graph Table A (Comics 6, Science 3, Animals 5, Poetry 2), an authored category question is 'How many selected Comics?' Answer 6. A comparison question is 'How many more selected Comics than Poetry?' Answer 4.\nFrom Edulytics bar graph Table B (Music 4, Drawing 6, Soccer 3, Reading 7), two possible questions are 'How many selected Drawing?' Answer 6, and 'How many selected Music and Reading altogether?' Answer 11. All original questions must refer to the source student graphs and their labeled categories, not to the publisher's hidden response sheet.",
        "Step 1: Retrieve the TWO graphs you constructed in the previous lesson or enter their corresponding original table data again. Check titles, category labels and picture symbol key/bar scale.\nStep 2: Write ONE original answerable question about the picture graph, recording which category/graph supplies its answer. Write a SECOND different picture graph question.\nStep 3: Write TWO different questions about the bar graph, mixing their order rather than grouping them all by display type. Include at least ONE question asking for a combination of two categories or a numerical comparison between categories.\nStep 4: For each question, locate the named category or categories and calculate the answer. Do not invent information absent from either graph.\nStep 5: Exchange the four questions and graph data with a new partner, who identifies which graph can answer each question. Record answers and compare them with the writer's solutions.\nStep 6: Use group feedback to revise questions that are ambiguous, refer to the wrong graph or cannot be answered. Explain which title, label or graph key helped find each solution. Teacher checks the real questioning and collaboration.",
        "Mistake: treating this as a fourth graph-reading drill and never authoring any questions. Original Activity 1 requires the learner to write four QUESTIONS, two for each graph.\nMistake: asking four one-category counts. At least one authored question must require combining or comparing two categories.\nMistake: making up values or responses not present in either graph. Questions and solutions must use the existing graph/table from the previous lesson.\nMistake: using the wrong partner's bar values for the picture graph or mixing the fictional example tables. Every question must be answerable from the named graph.\nMistake: copying blank underlined publisher worksheets into lesson prose instead of supplying clear question-writing prompts and teacher answer rubrics.\nMistake: marking the learner's open question-writing, graph selection or partner discussions complete based only on a numerical result.",
        "From the TWO graphs drawn in Grade 2 Lesson 10, write four different answerable questions (two per graph), including at least one combine/compare problem. Exchange them with another learner, locate the relevant graph/category labels, answer each, revise unclear questions, and explain the reasoning.");
    public static bool IsTarget(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l) =>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        d.VersionCode=="CCSSM-2010" &&
        d.ContentVersion=="phase30-common-core-rich-content-v1" &&
        l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase) ||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.D.10","CCSS:2.OA.B.2"}))
            throw new InvalidOperationException("Grade2 U01 L11 source identity or official outcome order changed");
    }
    private static bool Match(CanonicalLessonContentPackTranslation t,bool updated)=>
        t.Explanation==(updated?R.AfterExplanation:R.BeforeExplanation) &&
        t.KeyConceptsAndRules==(updated?R.AfterConcepts:R.BeforeConcepts) &&
        t.WorkedExamples==(updated?R.AfterExamples:R.BeforeExamples) &&
        t.StepByStepSolutions==(updated?R.AfterSteps:R.BeforeSteps) &&
        t.CommonMistakes==(updated?R.AfterMistakes:R.BeforeMistakes) &&
        t.QuickSummary==(updated?R.AfterSummary:R.BeforeSummary);
    private static bool Match(CurriculumLessonContentTranslation t,bool updated)=>
        t.Explanation==(updated?R.AfterExplanation:R.BeforeExplanation) &&
        t.KeyConceptsAndRules==(updated?R.AfterConcepts:R.BeforeConcepts) &&
        t.WorkedExamples==(updated?R.AfterExamples:R.BeforeExamples) &&
        t.StepByStepSolutions==(updated?R.AfterSteps:R.BeforeSteps) &&
        t.CommonMistakes==(updated?R.AfterMistakes:R.BeforeMistakes) &&
        t.QuickSummary==(updated?R.AfterSummary:R.BeforeSummary);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;
            Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Match(t,true))continue;
            if(!Match(t,false))
                throw new InvalidOperationException("G2 U01 L11 unexpected learner prose; refusing overwrite");
            t.Explanation=R.AfterExplanation;
            t.KeyConceptsAndRules=R.AfterConcepts;
            t.WorkedExamples=R.AfterExamples;
            t.StepByStepSolutions=R.AfterSteps;
            t.CommonMistakes=R.AfterMistakes;
            t.QuickSummary=R.AfterSummary;
        }
    }
    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)?Version:before;
    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l) && before==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before,
        IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,before))
            throw new InvalidOperationException("L11 requires original rich content predecessor");
        if(rows.Count==0)return;
        if(rows.Count!=1 || rows.Single().CultureCode!="en")
            throw new InvalidOperationException("L11 unexpected old language rows");
        var old=rows.Single();var now=l.Translations.Single(x=>x.CultureCode=="en");
        if(old.Title!=now.Title || !(Match(old,false)||Match(old,true)))
            throw new InvalidOperationException("L11 persisted learner content differs from reviewed predecessor");
    }
}
