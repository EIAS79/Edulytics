using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>Original 2021 Grade 2 U01 L17 optional 100–120 counting-center
/// source repair; guarded against unintended publisher/content drift.</summary>
public static class CommonCoreG2Unit1CountingCollectionsCenterTeachingCorrection
{
    public const string Version = "us-ccss-g2-u01-l17-counting-collections-centers-v1";
    private const string Code = "PED:US-CCSS-MATH:G2:U01:L17";
    private const string Url = "https://im.kendallhunt.com/k5/teachers/grade-2/unit-1/lesson-17/preparation.html";
    private const string Sha = "8578ce50b353f313353dcd8560935f23a9b72ee1204667fa99bc141c9f69f3f2";
    private sealed record Prose(string[] Before,string[] After);
    private static readonly Prose P = new(
        ["Learning goal: You will count large groups of objects by creating smaller groups of 5 or 10.\nThis lesson is optional because it is an opportunity for extra practice that not all classes may need. In Activity 1, you revisit stage 3 of the Counting Collections, which was first introduced in Kindergarten. In this stage, called Estimate and Count Up to 120, you count collections of up to 120 objects and represent how you counted.",
         "Key ideas:\n- Count large collections of objects\n- Practice adding within 50\n- Represent data using picture and bar graphs\n- Choose a representation that matches the quantities and relationship, then use it to justify and check the result.",
         "Example 1: Find the value of each sum mentally. \\(27 + 10\\) \\(27 + 10 + 10\\) \\(27 + 30\\) \\(37 + 30\\)\nApproach: “How did you use what you know about place value to find the sums?” (I knew I could start at 27 and count by ten. I knew 30 is the same as 3 tens. I know I could add the tens from each number together and the ones together.)",
         "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.",
         "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.",
         "You will count large groups of objects by creating smaller groups of 5 or 10. This lesson is optional because it is an opportunity for extra practice that not all classes may need."],
        ["OPTIONAL Grade 2 Center Day 3. We count and explain genuine collections by first estimating, then organizing 100–120 physical objects in equal groups of 5 or 10 and counting the leftovers. We revisit number talks based on adding tens, and choose two familiar centers: What's Behind My Back?, How Close?, Number Puzzles, or Sort and Display. A digital example is explicitly an Edulytics-created model, not evidence that a physical student collection was counted.",
         "A group of ten has exactly 10 items and a group of five has exactly 5; no item belongs to two groups. For 11 tens and 2 ones the total is 11×10+2=112. For 22 fives and 2 ones it is 22×5+2=112. A correct drawing, table or number sentence shows each group and any remaining items. Count by tens or fives, check against the real collection and record an estimate before counting. Adding a multiple of ten changes the tens while preserving the ones when no exchange is required: 27+10=37, 27+20=47, 27+30=57, 37+30=67. Familiar centers give choices to practise hidden parts, sums within 50, number puzzles, or honest picture/bar graphs with labeled categories and keys. This optional session is not a single generic worksheet.",
         "Number Talk: mentally find 27+10=37, 27+10+10=47, 27+30=57, 37+30=67 and explain how the tens change. Counting Collections Stage 3: a FIRST-PARTY VIRTUAL MODEL with eleven clearly labeled ten-object bundles and two singles represents 112, as 110+2 or 22 groups of five and two singles. For a genuine classroom task, partners estimate a real bag of 100–120 objects, count by physical fives or tens and individually record their actual result and diagram. Center examples: What's Behind My Back? 20 total and 12 visible leaves 8 hidden; How Close? 18+25=43; Number Puzzles 24+16=40; Sort and Display fictional 4 red, 7 blue and 6 yellow objects makes 17, with 3 more blue than red. These are independently authored illustrations, not publisher recording sheets.",
         "1. Number Talk: add tens to 27 using place value (37,47,57,67), then explain why the ones digit stays 7. 2. Before touching a real 100–120 object collection, write an ESTIMATE. 3. Count the actual objects with a partner, organizing into groups of FIVE or TEN, and keep any leftover objects visible. 4. Count groups and leftovers using 5×groups+remainder or 10×groups+remainder. Check that each physical object was counted once and the final total is 100–120; record your own number sentence or drawing. 5. Explain and compare both partners' representations. 6. Select an already taught center from four allowed choices, work with a partner, then choose another familiar center to practise addition within 50 or represent data on a labeled picture/bar graph. 7. Complete the independent Edulytics digital model and Cool-down; they do not certify that a physical collection or a classroom center was performed. Teacher observes authentic counting, diagrams, choice and peer work.",
         "A guess is an estimate, not the verified actual count. Do not mark a virtual 112-piece image as evidence of counting the real classroom collection. Eleven tens and two ones are 112, not 13 or 1102; twenty-two fives plus two ones are also 112. Ten-frames show the number of objects PER frame, not one object per frame. Do not double-count remaining objects, lose objects while regrouping or record a result outside the original 100–120 physical collection range. 27+30 is 57, not 30 or 47. A picture graph needs a key, a bar graph needs labels and a uniform numbered scale; arbitrary bars cannot certify real sorted data. Completing one numerical check never proves authentic partner discussion, center choice or a student's handwritten representation.",
         "Optional Center Day 3: 27+10, 27+10+10, 27+30 and 37+30 Number Talk; estimate then really count 100–120 items by groups of 5 or 10 and represent the count; choose familiar arithmetic or graph centers. The independently authored digital count and Cool-down check mathematics, while physical collection, drawings and collaboration require teacher review."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d, CanonicalLessonContentPackLesson l) =>
        d.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode &&
        d.VersionCode == "CCSSM-2010" &&
        d.ContentVersion == "phase30-common-core-rich-content-v1" &&
        l.LessonCode == Code;
    private static void Guard(CanonicalLessonContentPackDocument d, CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l) || l.SourceUrl != Url ||
           !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase) ||
           !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.D.10","CCSS:2.NBT.A.2","CCSS:2.NBT.B.5"}))
            throw new InvalidOperationException("Grade 2 U01 L17 source identity or outcome order changed");
    }
    private static bool Match(CanonicalLessonContentPackTranslation t,bool after) =>
        MatchFields([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Match(CurriculumLessonContentTranslation t,bool after) =>
        MatchFields([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool MatchFields(string[] actual,bool after) =>
        actual.SequenceEqual(after ? P.After : P.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;
            Guard(d,l);
            var t = l.Translations.Single(x=>x.CultureCode=="en");
            if(Match(t,true))continue;
            if(!Match(t,false))throw new InvalidOperationException("Grade 2 U01 L17 previous six learner fields changed");
            t.Explanation=P.After[0];t.KeyConceptsAndRules=P.After[1];
            t.WorkedExamples=P.After[2];t.StepByStepSolutions=P.After[3];
            t.CommonMistakes=P.After[4];t.QuickSummary=P.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)?Version:before;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)&&before==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,before))throw new InvalidOperationException("Grade 2 U01 L17 old persisted version mismatch");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("Grade 2 U01 L17 unexpected translations");
        var existing=rows.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(existing.Title!=incoming.Title || !(Match(existing,false)||Match(existing,true)))
            throw new InvalidOperationException("Grade 2 U01 L17 persisted source learner fields changed");
    }
}
