using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Optional Grade 2 U01 L12: source-specific Sort and Display 20–30 real
/// objects in three categories, picture OR bar graph and center choice.
/// Learner prose and exact original SHA, URL and standards must match.
/// </summary>
public static class CommonCoreG2Unit1OptionalSortAndDisplaySourceCorrection
{
    public const string Version="us-ccss-g2-u01-l12-original-sort-graph-centers-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U01:L12";
    private const string SourceUrl="https://im.kendallhunt.com/k5/teachers/grade-2/unit-1/lesson-12/preparation.html";
    private const string Sha="199e272aed33bc0d089e20a04fdeebd19bb647f626833a9ff8a6797a1e6e690a";
    private sealed record Review(
        string BeforeExplanation,string BeforeConcepts,string BeforeSteps,
        string BeforeMistakes,string BeforeSummary,
        string AfterExplanation,string AfterConcepts,string AfterSteps,
        string AfterMistakes,string AfterSummary);
    private static readonly Review R=new(
        "Learning goal: You will practice organizing data and using graphs to represent it.\nThis lesson is optional because it is an opportunity for extra practice that not all classes may need. In this new stage, called Picture or Bar Graphs, you sort 20–30 objects into 3 categories and make a picture or bar graph that shows how you sorted. In Activity 2, you choose between two previously introduced centers focused addition and subtraction.",
        "Key ideas:\n- Practice adding and subtracting within 10 or 20\n- Practice adding within 50\n- Represent data using picture and bar graphs\n- Read category labels, values, and scales before comparing or calculating with a data display.",
        "Step 1: Read the title, labels, categories, and scale.\nStep 2: Locate the values needed for the question.\nStep 3: Compare, add, subtract, or interpret the values as required.\nStep 4: State the result in the context of the data.",
        "Common mistake: reading the wrong category or scale. Check the labels and scale before using the values.",
        "You will practice organizing data and using graphs to represent it. This lesson is optional because it is an opportunity for extra practice that not all classes may need.",
        "Grade 2 Unit 1 Lesson 12 is OPTIONAL center practice, not a compulsory worksheet or just four arithmetic drills. Warm-up Number Talk: mentally subtract 17−7, 17−9, 15−5, 15−8, explaining how the first difference helps find the second by using a ten. In Activity 1 (Sort and Display, Stage 2), bring or collect 20–30 REAL objects, sort them into exactly three meaningful categories and make a correctly titled and labeled picture OR bar graph, then ask a partner TWO answerable questions from that graph. In Activity 2, choose an already learned arithmetic center: What's Behind My Back?, How Close?, or Number Puzzles. Choose a stage already taught and practise with a partner. The teacher evaluates physical sorting, original graph representations, explanations, and mathematical community norms; this independent Edulytics companion checks the mathematical counts without pretending a real collection was observed through a form.",
        "A collection must contain between 20 and 30 actual items, counted exactly ONCE, sorted into THREE nonempty labeled categories whose counts sum to the original total. Labels must be different and describe the objects actually sorted.\nThe original Sort and Display Stage 2 center asks learners to represent the actual category counts using a picture graph or bar graph. A picture graph needs a title, labels and a clear symbol key, such as one symbol per object; a bar graph needs title, category labels and a numbered axis beginning at zero with equal increments.\nAsk a partner TWO answerable questions about the resulting representation. A category-count question asks how many of one type; a combine/compare question needs two categories. The teacher checks the student's authored wording and real partner interaction.\nUse within-ten or within-twenty subtract strategies in the Number Talk: 17−7=10, 17−9=8, 15−5=10, 15−8=7. Building a ten connects the related subtractions. Other previously learned centers can practise addition within 50.\nThis optional lesson has no separate Cool-down in the public source lesson. Edulytics may author an independent short check of the same mathematical skills, without claiming it is publisher material.",
        "Step 1: Count and record a REAL group of 20–30 classroom objects (not classmates' names or invented observations).\nStep 2: Decide on three clear, disjoint category labels and sort each object into exactly ONE category. Count the objects in each; verify the three counts sum to the observed collection total.\nStep 3: Choose picture graph OR bar graph, then add an informative title and the exact three category labels. For a picture graph write a one-object-per-symbol key; for a bar graph start a numbered axis at zero and use one object per equal tick.\nStep 4: Draw the symbols or bars using the THREE genuine category counts. Read the picture key/bar scale and confirm no objects were omitted or counted twice.\nStep 5: Ask your partner two meaningful questions the graph can answer, including at least one question that combines or compares categories when appropriate. Partner answers them using the graph's title and labels.\nStep 6: Reflect on the source Number Talk by comparing 17−7 with 17−9 and 15−5 with 15−8. Explain how subtracting to ten first can help.\nStep 7: Choose What's Behind My Back?, How Close?, or Number Puzzles for Activity 2; work with a partner and discuss what arithmetic strategy improved. Classroom observation and free student expression remain teacher assessed.",
        "Mistake: making up objects or category frequencies and presenting them as a class collection. Only actual items should be described as observed data; separate invented numbers clearly as practice examples.\nMistake: sorting one object into two categories or forgetting an item. The three category counts must total exactly the collection's true 20–30 items.\nMistake: recording four categories or 15 objects in the original 20–30 objects, THREE-category activity. Keep the source bounds.\nMistake: drawing a bar without a numbered equal-spacing scale or picture symbols without a key; labels and title must be clear.\nMistake: generating a fixed graph numerically and calling student-created drawings or the physical center finished. The teacher separately checks those authentic skills and genuine partner questions.\nMistake: skipping Number Talk or the source's optional choice of centers and treating this as only a categorical data task.",
        "Optional Center Day 2: mentally subtract to ten, sort a REAL 20–30-object collection into THREE categories, create a labeled pictograph OR bar graph from it, ask TWO questions, and then choose an earlier arithmetic center. Edulytics provides independent representations/checks while teacher assesses actual physical sorting and collaboration.");
    public static bool IsTarget(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        d.VersionCode=="CCSSM-2010" &&
        d.ContentVersion=="phase30-common-core-rich-content-v1" &&
        l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=SourceUrl||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase) ||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.D.10","CCSS:2.NBT.B.5","CCSS:2.OA.B.2"}))
            throw new InvalidOperationException("G2 U01 L12 optional center source or standards drift");
    }
    private static bool Match(CanonicalLessonContentPackTranslation t,bool after)=>
        t.Explanation==(after?R.AfterExplanation:R.BeforeExplanation)&&
        t.KeyConceptsAndRules==(after?R.AfterConcepts:R.BeforeConcepts)&&
        t.StepByStepSolutions==(after?R.AfterSteps:R.BeforeSteps)&&
        t.CommonMistakes==(after?R.AfterMistakes:R.BeforeMistakes)&&
        t.QuickSummary==(after?R.AfterSummary:R.BeforeSummary);
    private static bool Match(CurriculumLessonContentTranslation t,bool after)=>
        t.Explanation==(after?R.AfterExplanation:R.BeforeExplanation)&&
        t.KeyConceptsAndRules==(after?R.AfterConcepts:R.BeforeConcepts)&&
        t.StepByStepSolutions==(after?R.AfterSteps:R.BeforeSteps)&&
        t.CommonMistakes==(after?R.AfterMistakes:R.BeforeMistakes)&&
        t.QuickSummary==(after?R.AfterSummary:R.BeforeSummary);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;
            Guard(d,l);
            var en=l.Translations.Single(x=>x.CultureCode=="en");
            if(Match(en,true))continue;
            if(!Match(en,false))
                throw new InvalidOperationException("G2 U01 L12 source predecessor learner prose differs");
            en.Explanation=R.AfterExplanation;
            en.KeyConceptsAndRules=R.AfterConcepts;
            en.StepByStepSolutions=R.AfterSteps;
            en.CommonMistakes=R.AfterMistakes;
            en.QuickSummary=R.AfterSummary;
        }
    }
    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)?Version:before;
    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)&&before==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before,
        IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,before))
            throw new InvalidOperationException("G2 U01 L12 unexpected previous materialized version");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 U01 L12 unexpected original translation count");
        var old=rows.Single();var now=l.Translations.Single(x=>x.CultureCode=="en");
        if(old.Title!=now.Title||old.WorkedExamples!=now.WorkedExamples||
            !(Match(old,false)||Match(old,true)))
            throw new InvalidOperationException("G2 U01 L12 persisted source content tampered");
    }
}
