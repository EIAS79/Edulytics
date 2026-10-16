using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Source-locked 2021 IM first-grade story and center instruction.
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
            "Today is a center day. Look at a picture and invent a math story about what happens. Tell your partner the story, listen to their answer, and then write a matching equation. You can also choose a familiar center to practice making pairs, sorting objects into categories or adding and subtracting within 10.","Learning goal: Tell and solve an addition or subtraction story from a picture, and write an equation with quantities up to 10.\nIn Math Stories, the picture gives an idea for the story; you choose an action that matches what is visible. In Find the Pair, practice making number partners that total the target. In Sort and Display, organize a collection and explain your categories. Your equation must match the story you told.","Example 1 — Make a subtraction story. Imagine a picture with 9 books and a child taking 4 away. Tell: 'There were 9 books. Four were borrowed. How many remain?' Show nine dots, cross out four, then write 9 - 4 = 5.\nExample 2 — Make an addition story. Imagine 3 children playing and 2 friends joining. Tell: 'Three children were playing. Two joined. How many play now?' Show two groups and write 3 + 2 = 5.\nExample 3 — Find the Pair. Show 7 with two groups of counters. If 4 are yellow, the other 3 are blue: 4 + 3 = 7. Ask your partner to find another pair for 7.",
            "Step 1: Choose Math Stories, Find the Pair, or Sort and Display.\nStep 2: In Math Stories, notice the people and objects in the picture; decide whether the groups join or separate.\nStep 3: Tell a complete math story with a question, and ask a partner to model and solve it.\nStep 4: Write a matching + or - equation within 10 and explain each number.\nStep 5: Swap roles. Check that the story, model and equation agree, then reflect on what you practiced.","A picture alone does not tell you to add or subtract. Your spoken story needs a clear action that the equation represents.\nDo not write 9 + 4 when four of nine books leave; the group becomes smaller.\nDo not ignore the center purpose: a sort needs categories, a pair game needs number partners, and a story needs a question and matching equation.","I can use pictures to invent and solve a math story, write a matching equation, and explain how I practiced addition, subtraction, pairs or sorting."),
        new("PED:US-CCSS-MATH:G1:U02:L06","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-6/preparation.html","0cd60addebdff2e502f0c58351933b6baf570a8bc3555b5de6ab1956efd71cef",["CCSS:1.OA.A.1","CCSS:1.OA.B.4","CCSS:1.OA.C.6"],
            "Step 1: Identify the two quantities, 4 and 2.","Example 1: The source task combines 4 and 2.",
            "Some problems tell us about two groups that are already there, without anyone joining or leaving. Today we use the parts to find the total. Read the story carefully, draw each part, and write an equation. You can add the parts in either order without changing the total.","Learning goal: Solve Put Together, Total Unknown stories by combining two existing parts and explaining a matching addition equation.\nThere is no joining or leaving action: for example, some fish are red and some are blue, and you want to know how many fish there are in all. The parts can be written in either order: 4 + 3 = 7 and 3 + 4 = 7. Each number still represents a real group.","Example 1 — Kiran's fish. In one tank there are 4 red fish and 3 blue fish. How many fish are in the tank altogether? Draw four red circles and three blue circles. Count the whole group: 4 + 3 = 7 fish.\nExample 2 — Same pets, another order. You can write 3 + 4 = 7 for the same tank. Explain that 3 still counts blue fish and 4 still counts red fish, even though they switched order.\nExample 3 — Pets at home. A child has 2 cats and 5 birds. How many pets does the child have? Draw two cat symbols and five bird symbols, write 2 + 5 = 7 and, if helpful, 7 = 5 + 2. Both sides of the equals sign describe seven pets.",
            "Step 1: Read the situation three times: understand the story, find the quantities, and identify the question.\nStep 2: Name the two existing parts and the whole you want to find.\nStep 3: Draw the parts as different groups or use two-color counters.\nStep 4: Write an addition equation that describes both groups making one whole.\nStep 5: Check the total by counting, then swap the addends and explain why the total does not change.","Do not look for an action that is not in the story. Two kinds of fish are already in one tank; nobody needs to arrive.\nDo not subtract 4 - 3 if you are asked how many fish are there altogether. Combine both parts.\nDo not think 4 + 3 and 3 + 4 are different totals. The groups are unchanged; only the written order changes.","I can find the total of two groups even when nothing joins or leaves. I can explain why adding the parts in either order gives the same total."),
        new("PED:US-CCSS-MATH:G1:U02:L07","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-7/preparation.html","f3b8bdf030bd1f4bbe981e0867a5f048b3fc9d23c6fc75cd221547c767c0e7f9",["CCSS:1.OA.A.1","CCSS:1.OA.C.6"],
            "Step 1: Identify the two quantities, 8 and 2.","Example 1: The source task combines 8 and 2.",
            "Shake and Spill starts with a known number of two-color counters. After you spill them, you can see the same total split into two parts in different ways. Today the total is known but both parts are not known before you spill. Record several ways to make the total and explain an equation for each split.","Learning goal: Solve Put Together/Take Apart, Both Addends Unknown stories by finding different decompositions and writing equal equations.\nIf there are 9 counters in all, there can be 7 red and 2 yellow, or 5 red and 4 yellow. Record 9 = 7 + 2 and 9 = 5 + 4. The total can appear before the equals sign because the expressions on both sides have the same value.","Example 1 — Two colors, one total. Shake 9 counters. Suppose 7 land red and 2 land yellow. Show two groups; record 7 + 2 = 9 and 9 = 7 + 2. Explain what 7, 2 and 9 count.\nExample 2 — Shake again. The same 9 counters may show 5 red and 4 yellow. Record 9 = 5 + 4. Explain why the total stayed nine even though the split changed.\nExample 3 — Another decomposition. A total of 10 could be 6 red and 4 yellow, or 8 red and 2 yellow. Write 10 = 6 + 4 and 10 = 8 + 2. Both equations are true and describe different possible spills.",
            "Step 1: Count the whole set of counters before shaking and spilling.\nStep 2: Split the total into two colors and count each color separately.\nStep 3: Record the split with an equation, such as 9 = 7 + 2, with the total before or after the equals sign.\nStep 4: Try another split using the same total and write a different true equation.\nStep 5: Show both arrangements with a drawing and explain why the total is unchanged.","Do not assume 9 = 7 + 2 is false just because nine is written first; each side has value nine.\nDo not count the same counter in both color groups; the groups together must equal the whole.\nDo not change the total when you change the split. Nine counters can be partitioned in several ways but remain nine.","I can break the same number into two unknown parts in more than one way, write true equations, and explain how the parts equal the whole.")
    ];

    public static IReadOnlyCollection<string> TargetLessonCodes=>Repairs.Select(x=>x.Code).ToArray();
    private static Repair? Find(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson)=>
        doc.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        doc.VersionCode=="CCSSM-2010" &&
        doc.ContentVersion=="phase30-common-core-rich-content-v1"
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
