using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Exact 2021 IM Grade 2 Unit 1 Lesson 8 source-specific graph comprehension:
/// category counts, combined/difference questions and whether a graph supplies
/// the requested facts. Original source URL/body/outcome and original
/// published learner prose are preserved as guarded predecessors.
/// </summary>
public static class CommonCoreG2Unit1PictureGraphSourceTeachingCorrection
{
    public const string Version="us-ccss-g2-u01-l08-picture-graph-original-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U01:L08";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-1/lesson-8/preparation.html";
    private const string CanonicalSha="60b3c0825b8e7e2bbf2ad4318612e4fe19f2e8bc767f61e66085d02b6fb11ce6";
    private sealed record Review(string BeforeExplanation,string BeforeConcepts,string BeforeSteps,
        string BeforeMistakes,string BeforeSummary,
        string AfterExplanation,string AfterConcepts,string AfterSteps,
        string AfterMistakes,string AfterSummary);
    private static readonly Review Record = new(
        "Learning goal: You will read and interpret a picture graph , which is a standard way of representing data.\nIn the previous lesson, you looked at different ways to organize data to make it easier to ask and answer questions about data. In this lesson, you are introduced to a picture graph as a way to represent data.",
        "Key ideas:\n- Answer questions based on a picture graph\n- Determine whether a question can be answered by a given picture graph\n- Interpret data represented in a picture graph\n- Read category labels, values, and scales before comparing or calculating with a data display.",
        "Step 1: Read the title, labels, categories, and scale.\nStep 2: Locate the values needed for the question.\nStep 3: Compare, add, subtract, or interpret the values as required.\nStep 4: State the result in the context of the data.",
        "Common mistake: reading the wrong category or scale. Check the labels and scale before using the values.",
        "You will read and interpret a picture graph , which is a standard way of representing data. In the previous lesson, you looked at different ways to organize data to make it easier to ask and answer questions about data.",
        "Goal: Read and interpret one-symbol picture graphs. For each picture graph, name what the graph represents, read the category labels and key, count the picture symbols, and answer questions about counts, combinations, and differences. Ask whether a question can truly be answered using only the categories and population recorded in this graph. A picture graph cannot supply unrecorded information.",
        "A picture graph uses repeated pictures or symbols to show how many responses belong to each named category. In this lesson, the key identifies one symbol as one response.\nTo answer 'how many', count the symbols in the matching category. To answer 'how many altogether', combine category counts. To answer 'how many more', subtract the smaller count from the larger one.\nA graph showing children's vegetable preferences does not automatically tell you what adults prefer. Distinguish answerable and unanswerable questions from the actual graph evidence.",
        "Step 1: Read the picture graph's title, category names, population being asked, and its key.\nStep 2: Count the pictures in each needed category using the key, recording values from the actual diagram.\nStep 3: For one category, report its count; for two categories together, add their counts; for 'how many more', subtract the smaller count from the larger.\nStep 4: Before answering a question, ask whether the requested population, category and period appear in this graph. Say 'cannot determine' rather than inventing counts for absent data.\nStep 5: Explain the mathematical evidence in the picture graph and check whether the result matches the original question.",
        "Mistake: counting images without reading the key or the category labels. A symbol's value comes from the graph key.\nMistake: adding two categories when the question asks how many MORE; compare them by subtraction instead.\nMistake: answering a question about unrecorded adults, dates, or categories using a graph of children's preferences. Such information cannot be recovered from this display.\nMistake: assuming every question can be answered simply because a graph is shown; the lesson explicitly assesses answerability.",
        "Interpret single-symbol picture graphs by reading the key, category labels and response counts. Answer category, combined-total and compare questions, and explain which questions the graph does not support.");
    public static bool IsTarget(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson) =>
        doc.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode &&
        doc.VersionCode == "CCSSM-2010" &&
        doc.ContentVersion == "phase30-common-core-rich-content-v1" &&
        lesson.LessonCode == Code;
    private static void Guard(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson)
    {
        if (!IsTarget(doc,lesson) || lesson.SourceUrl!=Url ||
            !string.Equals(lesson.CanonicalBodySha256,CanonicalSha,
                StringComparison.OrdinalIgnoreCase) ||
            !lesson.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.D.10"}))
            throw new InvalidOperationException("G2 U01 L08 IM source identity drift");
    }
    private static bool Match(CanonicalLessonContentPackTranslation t,bool newer) =>
        t.Explanation == (newer?Record.AfterExplanation:Record.BeforeExplanation) &&
        t.KeyConceptsAndRules == (newer?Record.AfterConcepts:Record.BeforeConcepts) &&
        t.StepByStepSolutions == (newer?Record.AfterSteps:Record.BeforeSteps) &&
        t.CommonMistakes == (newer?Record.AfterMistakes:Record.BeforeMistakes) &&
        t.QuickSummary == (newer?Record.AfterSummary:Record.BeforeSummary);
    private static bool Match(CurriculumLessonContentTranslation t,bool newer) =>
        t.Explanation == (newer?Record.AfterExplanation:Record.BeforeExplanation) &&
        t.KeyConceptsAndRules == (newer?Record.AfterConcepts:Record.BeforeConcepts) &&
        t.StepByStepSolutions == (newer?Record.AfterSteps:Record.BeforeSteps) &&
        t.CommonMistakes == (newer?Record.AfterMistakes:Record.BeforeMistakes) &&
        t.QuickSummary == (newer?Record.AfterSummary:Record.BeforeSummary);
    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        foreach(var lesson in doc.Lessons)
        {
            if(!IsTarget(doc,lesson))continue;
            Guard(doc,lesson);
            var t=lesson.Translations.Single(x=>x.CultureCode=="en");
            if(Match(t,true))continue;
            if(!Match(t,false))throw new InvalidOperationException(
                "G2 U01 L08 cannot overwrite unreviewed learner prose");
            t.Explanation=Record.AfterExplanation;
            t.KeyConceptsAndRules=Record.AfterConcepts;
            t.StepByStepSolutions=Record.AfterSteps;
            t.CommonMistakes=Record.AfterMistakes;
            t.QuickSummary=Record.AfterSummary;
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previous)=>
        IsTarget(doc,lesson)?Version:previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previous)=>
        IsTarget(doc,lesson) &&
        previous==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson,
        string previous,IReadOnlyCollection<CurriculumLessonContentTranslation> persisted)
    {
        Guard(doc,lesson);
        if(!CanUpgradeExisting(doc,lesson,previous))
            throw new InvalidOperationException("G2 U01 L08 requires exact rich predecessor");
        if(persisted.Count==0)return;
        if(persisted.Count!=1 || persisted.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 U01 L08 unexpected persisted translation count");
        var old=persisted.Single();
        var now=lesson.Translations.Single(t=>t.CultureCode=="en");
        if(old.Title!=now.Title || old.WorkedExamples!=now.WorkedExamples ||
            !(Match(old,false)||Match(old,true)))
            throw new InvalidOperationException("G2 U01 L08 persisted learner fields differ");
    }
}
