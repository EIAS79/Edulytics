using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>2021 G2 U01 L18 student-created CLASS survey, real data,
/// source-predecessor-guarded body correction; fictitious examples distinct.</summary>
public static class CommonCoreG2Unit1ClassSurveysTeachingCorrection
{
    public const string Version="us-ccss-g2-u01-l18-authentic-class-surveys-graphs-tapes-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U01:L18";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-1/lesson-18/preparation.html";
    private const string Sha="14b2f5aec126889e5c0013fe2f6855f618f95b55bb47475b5de1e8b207f5eb20";
    private sealed record Fields(string[] Before,string[] After);
    private static readonly Fields F=new(
        ["Learning goal: You will use what you have learned about data, bar graphs, and tape diagrams to create a survey and to organize, collect, and represent data.\nThis lesson is optional because it does not address any new mathematical content standards. This lesson does provide you with an opportunity to apply precursor skills of mathematical modeling. In previous lessons, you analyzed and represented categorical data using picture and bar graphs.",
         "Key ideas:\n- Collect, organize, and represent data from survey questions\n- Create questions related to survey data\n- Interpret results of a survey and represent findings\n- Read category labels, values, and scales before comparing or calculating with a data display.",
         "Example 1: What is your survey question? What are your categories? Category 1: ___ Category 2: ___ Category 3: ___ Category 4: ___ Record the data. Organize and represent the data in a picture graph or bar graph.\nApproach: “How are these graphs the same? How are you different?” (The bar graph has numbers on it, but the picture graphs don’t. Some have 3 and some have 4 different responses. Some are up and down and some are side-to-side.) “What choices did these mathematicians make with your graphs?” (You had to decide what kind of graph to make, a title, which direction to show the bars in how to represent your data so it is clear...\nExample 2: Trade graphs. Create questions about the other group’s graph. Sentence stems: How many students picked ___ and ___ all together? How many more students picked ___ than ___ ? Take turns asking and answering questions.",
         "Step 1: Read the title, labels, categories, and scale.\nStep 2: Locate the values needed for the question.\nStep 3: Compare, add, subtract, or interpret the values as required.\nStep 4: State the result in the context of the data.",
         "Common mistake: reading the wrong category or scale. Check the labels and scale before using the values.",
         "You will use what you have learned about data, bar graphs, and tape diagrams to create a survey and to organize, collect, and represent data. This lesson is optional because it does not address any new mathematical content standards."],
        ["Grade 2 Unit 1 Lesson 18 is an OPTIONAL mathematics modelling session, not new grade-level content. Warm-up: explain what you know about bar graphs using your own mathematical example and discuss it with a partner. Activity 1: with a partner, invent ONE meaningful anonymous classroom survey question with exactly 3 or 4 distinct answer categories. Ask real classmates, count one choice per respondent, and organize the genuine responses in a picture or bar graph with a meaningful title. Activity 2: exchange graphs with another pair, each person AUTHOR questions that can be answered from the other's graph and use addition/subtraction equations to combine or compare named categories. Activity 3: independently draw a TWO-category tape diagram of two facts learned from your survey, then write the corresponding compare equation and discuss with a partner. The Edulytics independent fictional practice graph and Cool-down are demonstrations, never evidence of surveys actually conducted.",
         "A classroom survey asks a precise neutral question; its 3 or 4 response categories must be distinct and exhaustive for participants. Every respondent contributes exactly ONE tally; counts must come from a REAL collected classroom survey. A graph has a helpful title, accurate category labels and quantity representation. For a pictograph, state the key (for this independent Edulytics demonstration, 1 symbol = 1 response); for a bar graph, use a numbered zero baseline and equally spaced ticks. Combining two categories is ADDITION (red 6 plus blue 9 =15); comparison gap is SUBTRACTION (blue9 minus red6=3). A tape diagram shows exactly two selected factual category quantities, aligned and labeled, and a correctly positioned difference. Exchanging a real graph with peers, authoring genuine original questions and explanations need teacher review. Modelled first-party examples are not observations from a real class.",
         "WARM-UP: Describe when a bar graph helps compare class choices and why the categories and numbered equal-interval axis matter. FIRST-PARTY FICTIONAL SURVEY: Four categories for favorite reading activities: Stories9, Comics6, Poetry4, Nonfiction3 (22 invented responses total). Picture graph key ●=1 imagined response; bar heights 9,6,4,3 on a shared 0-to-10 scale. Example partner question: How many picked Comics and Poetry together? 6+4=10. Comparison question: How many MORE picked Stories than Nonfiction? 9−3=6. Tape diagram: Stories9 aligned above Comics6, difference3, with equation 6+3=9. The 22 responses are fictional and NOT a class count. Students must separately gather their OWN original actual survey.",
         "1. Discuss the true/false features of a bar graph: categories must be clearly named, scale increments equal, bars start at zero and taller/longer bars show greater quantities. 2. With your partner, write your real survey question and 3 or 4 response categories; give each classmate just ONE choice and do not collect names or other identifying details. 3. Ask your class, count genuine anonymous responses and record actual tallies/total. Check the sum equals the number of valid responses you collected, not an invented target. 4. Make your own picture OR bar graph, labeled with the survey question or relevant title; use 1 picture per response or a clear numbered 0-based uniform bar scale. 5. Trade that real graph with another pair; independently write your own combine and compare questions about named graph categories, then answer a partner's question using an addition or subtraction equation. 6. Choose two REAL category facts, draw aligned labeled tape bars with the smaller part and the missing gap clearly distinguished, then write the matching comparison equation. 7. Explain what the whole picture/bar graph shows that the two-part tape does not, and what difference the tape makes easier to see. 8. Complete the separate, explicitly FICTIONAL Edulytics Cool-down only as additional numerical assessment; teacher checks real survey integrity, peer questioning, original representation and discussion.",
         "Do not put made-up counts into the real class survey and pretend classmates answered. Do not record classmates' names or other identifying personal information; collect anonymous category totals only. Do not place one response in two categories. Do not silently use only two or five categories when the original activity requires THREE or FOUR choices. A picture graph without a symbol key or a bar graph with arbitrary ticks hides the quantities. Do not add when asked how many MORE, or subtract when asked how many ALL TOGETHER. Do not swap the larger and smaller bars or label a subtraction gap as a category total. Do not claim that an automatically checked equation proves a student genuinely asked classmates, authored questions, created a drawing or talked with a peer; those tasks require teacher review.",
         "Optional student-authored class survey: discuss bar graphs, create a real anonymous question with 3–4 categories, collect and graph actual class answers, exchange diagrams and write original combine/compare questions, draw a labeled two-category tape and explain the comparison. Independent Edulytics fictional examples and Cool-down verify numerical skills only; teacher reviews real survey and peer work."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        d.VersionCode=="CCSSM-2010" &&
        d.ContentVersion=="phase30-common-core-rich-content-v1" &&
        l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
           !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.D.10","CCSS:2.OA.A.1","CCSS:2.OA.B.2"}))
            throw new InvalidOperationException("G2 U01 L18 original source identity changed");
    }
    private static bool Match(CanonicalLessonContentPackTranslation t,bool newer)=>
        MatchFields([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],newer);
    private static bool Match(CurriculumLessonContentTranslation t,bool newer)=>
        MatchFields([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],newer);
    private static bool MatchFields(string[] a,bool newer)=>a.SequenceEqual(newer?F.After:F.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;
            Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Match(t,true))continue;
            if(!Match(t,false))throw new InvalidOperationException("G2 U01 L18 learner predecessor mismatch");
            t.Explanation=F.After[0];t.KeyConceptsAndRules=F.After[1];
            t.WorkedExamples=F.After[2];t.StepByStepSolutions=F.After[3];
            t.CommonMistakes=F.After[4];t.QuickSummary=F.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)?Version:before;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)&&before==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,before))throw new InvalidOperationException("G2 U01 L18 unknown persisted prior version");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 U01 L18 unexpected persisted translation");
        var old=rows.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(old.Title!=incoming.Title||!(Match(old,false)||Match(old,true)))
            throw new InvalidOperationException("G2 U01 L18 persisted source text changed");
    }
}
