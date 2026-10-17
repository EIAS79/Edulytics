using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Source-locked 2021 IM first-grade story and center instruction for B001 first 20.
/// Original teacher prep, complete teacher/Student Facing activities, and
/// task-sheet PDF links were identified. Not independent academic signoff.
/// </summary>
public static class CommonCoreG1Unit2Part2SourceTeachingCorrections
{
    public const string CorrectedContentVersion="us-ccss-g1-u02-part2-original-source-teaching-v1";
    private sealed record Repair(string Code,string Url,string Sha,string[] Outcomes,
        string OldFirstStep,string OldFirstExample,
        string Explanation,string Concepts,string Examples,string Steps,
        string Mistakes,string Summary);
    private static readonly Repair[] Repairs =
    [
        new("PED:US-CCSS-MATH:G1:U02:L04","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-4/preparation.html","b1a39087b6ca3f653862d2323314037283a6ef101234a0d8d05de8eea9518592",["CCSS:1.OA.A.1","CCSS:1.OA.D.7"],
            "Step 1: Identify the two quantities, 5 and 4.","Example 1: The source task combines 5 and 4.",
            "Two stories can use the same numbers but ask different questions. Listen for whether more things join or some things leave, and decide if the unknown is the final amount or how many were added. Show each situation with counters, a drawing or a 10-frame, then write an equation that matches the story.","Learning goal: Solve Add To, Result Unknown; Add To, Change Unknown; and Take From, Result Unknown stories and explain each equation.\nResult Unknown asks, 'How many are there now?' Change Unknown asks, 'How many more joined?' The number that answers the story is not necessarily after the equals sign. The equals sign means both sides have the same value.","Example 1 — Find the result. Mia had 4 books and was given 3 more. Draw four squares and add three. There are 7 books now, so write 4 + 3 = [7].\nExample 2 — Find the change. Mia had 4 books and later had 7. How many new books did she get? Put down 4 counters and add until you reach 7. You added 3, so 4 + [3] = 7. Box the 3 because the question asks how many joined, not how many are there now.\nExample 3 — Find what remains. Nine books were on a table and two were borrowed. Show nine dots and cross out two. Seven remain, so 9 - 2 = [7]. Explain why this story uses subtraction.",
            "Step 1: Read the story and say what changes, or what amount is being asked for.\nStep 2: Act it out with counters, dots, or a 10-frame.\nStep 3: Decide whether the action is adding more or taking some away.\nStep 4: Write a matching complete equation, with a box around the number that answers the question.\nStep 5: Compare the equation with the picture and tell your partner why each number belongs there.","Do not always box the number after the equals sign. In 4 + [3] = 7, the answer is 3 because that was the change.\nDo not use addition when the story says objects leave. Nine books with two borrowed leaves seven: 9 - 2 = 7.\nDo not read only the numbers. Compare what happened in the stories to decide which number is missing.","I can compare stories that ask for a result or a missing change, draw a model, write an equation, and box the correct answer."),
        new("PED:US-CCSS-MATH:G1:U02:L05","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-5/preparation.html","f271d39546769d8fd1822c3d605e1e16b23ea2070dcbd5c8eee070a2a0517dc1",["CCSS:1.MD.C.4","CCSS:1.OA.A.1","CCSS:1.OA.C.5","CCSS:1.OA.C.6"],
            "Step 1: Represent the quantities or objects in the task.","Example 1: “What story could we tell about this picture?”",
            "Today is a Math Stories center day. Look at a picture and invent a math story about what happens. Tell your partner the story, listen to their answer, and then write a matching equation. You can also choose a familiar center to practice making pairs, sorting objects into categories or adding and subtracting within 10.","Learning goal: Tell and solve an addition or subtraction story from a picture, and write an equation with quantities up to 10.\nIn Math Stories, the picture gives an idea for the story; you choose an action that matches what is visible. In Find the Pair, practice making number partners that total the target. In Sort and Display, organize a collection and explain your categories. Your equation must match the story you told.","Example 1 — Make a subtraction story. Imagine a picture with 9 books and a child taking 4 away. Tell: 'There were 9 books. Four were borrowed. How many remain?' Show nine dots, cross out four, then write 9 - 4 = 5.\nExample 2 — Make an addition story. Imagine 3 children playing and 2 friends joining. Tell: 'Three children were playing. Two joined. How many play now?' Show two groups and write 3 + 2 = 5.\nExample 3 — Find the Pair. Show 7 with two groups of counters. If 4 are yellow, the other 3 are blue: 4 + 3 = 7. Ask your partner to find another pair for 7.",
            "Step 1: Choose Math Stories, Find the Pair, or Sort and Display.\nStep 2: In Math Stories, notice the people and objects in the picture; decide whether the groups join or separate.\nStep 3: Tell a complete math story with a question, and ask a partner to model and solve it.\nStep 4: Write a matching + or - equation within 10 and explain each number.\nStep 5: Swap roles. Check that the story, model and equation agree, then reflect on what you practiced.","A picture alone does not tell you to add or subtract. Your spoken story needs a clear action that the equation represents.\nDo not write 9 + 4 when four of nine books leave; the group becomes smaller.\nDo not ignore the center purpose: a sort needs categories, a pair game needs number partners, and a story needs a question and matching equation.","I can use pictures to invent and solve a math story, write a matching equation, and explain how I practiced addition, subtraction, pairs or sorting.")
    ];

    public static IReadOnlyCollection<string> TargetLessonCodes=>Repairs.Select(x=>x.Code).ToArray();
    private static Repair? Find(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson)=>
        doc.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        doc.VersionCode=="CCSSM-2010" &&
        doc.ContentVersion=="phase29-source-intelligence-en-v2-g1"
            ? Repairs.SingleOrDefault(x=>x.Code==lesson.LessonCode)
            : null;

    public static bool IsTarget(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson)=>Find(doc,lesson)!=null;

    private static void Check(CanonicalLessonContentPackLesson lesson,Repair r)
    {
        if(lesson.SourceUrl!=r.Url ||
           !string.Equals(lesson.CanonicalBodySha256,r.Sha,StringComparison.OrdinalIgnoreCase) ||
           !lesson.OutcomeCodes.SequenceEqual(r.Outcomes))
            throw new InvalidOperationException($"G1 U02 Part2 original source/standard drift: {lesson.LessonCode}");
    }

    private static bool Matches(CanonicalLessonContentPackTranslation en,Repair r)=>
        en.Explanation==r.Explanation && en.KeyConceptsAndRules==r.Concepts &&
        en.WorkedExamples==r.Examples && en.StepByStepSolutions==r.Steps &&
        en.CommonMistakes==r.Mistakes && en.QuickSummary==r.Summary;

    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        if(doc.PackCode!=MathematicsCurriculumPackRegistry.CommonCoreCode ||
           doc.VersionCode!="CCSSM-2010")return;
        foreach(var lesson in doc.Lessons)
        {
            var r=Find(doc,lesson);
            if(r is null)continue;
            Check(lesson,r);
            var en=lesson.Translations.Single(x=>x.CultureCode=="en");
            if(Matches(en,r))continue;
            if(!en.StepByStepSolutions.StartsWith(r.OldFirstStep,StringComparison.Ordinal) ||
               !en.WorkedExamples.StartsWith(r.OldFirstExample,StringComparison.Ordinal))
                throw new InvalidOperationException($"G1 U02 Part2 unknown old learner body: {lesson.LessonCode}");
            en.Explanation=r.Explanation;
            en.KeyConceptsAndRules=r.Concepts;
            en.WorkedExamples=r.Examples;
            en.StepByStepSolutions=r.Steps;
            en.CommonMistakes=r.Mistakes;
            en.QuickSummary=r.Summary;
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previous)=>
        IsTarget(doc,lesson)?CorrectedContentVersion:previous;

    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string existingVersion)=>
        IsTarget(doc,lesson) &&
        existingVersion==CommonCoreRichContentCorrections.CorrectionContentVersion;

    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson,
        string existingVersion,IReadOnlyCollection<CurriculumLessonContentTranslation> existing)
    {
        var r=Find(doc,lesson) ?? throw new InvalidOperationException("Not G1 U02 Part2 correction target");
        Check(lesson,r);
        if(!CanUpgradeExisting(doc,lesson,existingVersion))
            throw new InvalidOperationException($"G1 U02 Part2 unexpected persisted version: {lesson.LessonCode}");
        if(existing.Count==0)return; // deployed JSON-only lesson mode
        if(existing.Count!=1)
            throw new InvalidOperationException("G1 U02 Part2 unexpected persisted culture rows");
        var t=existing.Single();
        if(t.CultureCode!="en" || !t.StepByStepSolutions.StartsWith(r.OldFirstStep,StringComparison.Ordinal) ||
           !t.WorkedExamples.StartsWith(r.OldFirstExample,StringComparison.Ordinal))
            throw new InvalidOperationException($"G1 U02 Part2 persisted learner text differs: {lesson.LessonCode}");
    }
}
