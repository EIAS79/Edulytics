using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Three 2021 original-source Grade 1 U02 pedagogical corrections.
/// Teacher preparation and full activities studied from on-device publisher archive.
/// Not an independent academic or 500-lesson release decision.
/// </summary>
public static class CommonCoreG1Unit2Part3SourceTeachingCorrections
{
    public const string CorrectedContentVersion="us-ccss-g1-u02-l08-l10-original-source-teaching-v1";
    private sealed record Repair(string Code,string SourceUrl,string SourceSha,string[] Outcomes,
        string PreviousStepPrefix,string PreviousExamplePrefix,string Explanation,
        string Concepts,string Examples,string Steps,string Mistakes,string Summary);
    private static readonly Repair[] Repairs=[
        new("PED:US-CCSS-MATH:G1:U02:L08","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-8/preparation.html","5bfd9d463b015a7b077d89764698402440a047d67ca10d5cc605d550814e525a",
            ["CCSS:1.NBT.A.1","CCSS:1.OA.A.1","CCSS:1.OA.C.6"],
            "Step 1: Represent the quantities or objects in the task.\nStep 2: ",
            "Example 1: “Today we played a game where we had to find out how m",
            "Today we play Shake, Spill, and Cover. We know how many two-color counters are in the whole collection and how many of one color we can see. Some counters of the other color are hidden under a cup. Find how many are hidden. The hidden amount is the missing addend, and you can check it by counting on or taking away.","Learning goal: Solve Put Together/Take Apart, Addend Unknown stories. Relate different equations to the same counters and circle the unknown part.\nWhen the total is 8 and 5 red counters show, we need 5 + [3] = 8. We can also write 8 = 5 + [3] or 8 - 5 = 3; all describe the same total and visible part.\nThe missing part answers 'How many are under the cup?' rather than 'How many in all?'.","Example 1 — Covered counters. You have 8 counters in all. Five red counters are showing. The rest are yellow under the cup. Use eight spaces on a 10-frame: fill five red and then three yellow. Write 5 + [3] = 8 and box 3.\nExample 2 — Count on. Ten counters altogether, six red showing. Count 7, 8, 9, 10: you counted four new counters. Six plus four equals ten: 6 + [4] = 10.\nExample 3 — Same picture, another equation. Nine counters total, four blue showing, five hidden. 9 = 4 + [5], or 9 - 4 = [5]. Point to 9 as the whole, 4 as the visible part, and 5 as the hidden part.",
            "Step 1: Count all counters before hiding any; that number is the total.\nStep 2: Observe the number of counters of the visible color and represent them on a 10-frame.\nStep 3: Count on from the visible amount to the whole to find the covered color.\nStep 4: Write total = visible + [hidden] or visible + [hidden] = total and box the hidden part.\nStep 5: Lift the imaginary cup to check the counts, then tell how a subtraction equation could show the same story.","Do not box the whole total when the question asks how many counters are covered.\nDo not start counting from one when counting on from the visible amount; start at that amount and count only extra counters.\nThe equals sign does not mean 'the answer comes next'; 9 = 4 + 5 is as true as 4 + 5 = 9.","I can find a hidden part of a known total, show it using counters or a 10-frame, and write matching addition or subtraction equations."),
        new("PED:US-CCSS-MATH:G1:U02:L09","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-9/preparation.html","c6c0cb894a46fdb2271e32e9c779755dbc09b058d66202632d0dfcc937e10169",
            ["CCSS:1.OA.A.1","CCSS:1.OA.B.3","CCSS:1.OA.D.7"],
            "Step 1: Identify the two quantities, 4 and 6.\nStep 2: Start at 4 ",
            "Example 1: The source task combines 4 and 6. Start at 4 and count",
            "We have learned several kinds of part-part-whole stories. Sometimes we know the two parts and need the total. Sometimes the total and one part are given, and sometimes both parts are unknown. Read the question to decide which number is missing. Work with a partner on a labelled drawing and equation, and compare methods as in a gallery walk.","Learning goal: Solve Put Together/Take Apart stories with the total, one addend, or both addends unknown. Write a matching equation and box the number that answers the question.\nFor 5 lizards and 3 snakes, the total pets is missing: 5 + 3 = [8]. For 8 pets including 5 lizards, the snakes are missing: 5 + [3] = 8.\nIf there are 8 pets with no given group sizes, many pairs work: 8 = 5 + 3, 8 = 6 + 2, 8 = 4 + 4. Explain more than one possibility.","Example 1 — Find the total. A child has 5 lizards and 3 snakes. Draw two labelled groups and write 5 + 3 = [8]. Box eight because the story asks how many pets there are.\nExample 2 — Find one part. There are 8 pets, and 5 are lizards. How many are snakes? Write 5 + [3] = 8. Box three because the story asks for the snakes, not for all the pets.\nExample 3 — Both parts unknown. Eight animals belong to two groups, but neither group size is given. Show 8 = [6] + [2] and 8 = [4] + [4]; explain why there is more than one answer. Share a drawing of one split and compare it with a partner's drawing.",
            "Step 1: Read the entire story and say what the question asks for: whole, a part, or two parts.\nStep 2: Draw and label the total and the known groups using counters, a bar, or pictures.\nStep 3: Write an addition equation, putting a box on what is unknown.\nStep 4: If both groups are unknown, find at least two different pairs with the same total.\nStep 5: Share the equation and drawing, compare a classmate's method, and justify the boxed number.","Do not automatically box the last number in an equation; locate the quantity the question asks for.\nDo not assume only one answer when both parts are unknown. A total of 8 can be split into 5+3 or 4+4.\nDo not leave your diagram unlabelled; each drawn group should match a number in the story.","I can decide whether a story hides the total, one group, or both groups, and support a correct equation with a labelled drawing."),
        new("PED:US-CCSS-MATH:G1:U02:L10","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-10/preparation.html","865a5e94280bd2f444d046eb63e0fb0149e9153abd8a6dc42e038100aa78be7b",
            ["CCSS:1.OA.A.1","CCSS:1.OA.C.6"],
            "Step 1: Identify the starting quantity 7 and the amount 2 being r",
            "Example 1: The source task compares or removes 2 from 7. The resu",
            "Today is Center Day 2. At Capture Squares, roll two number cubes, add the dots or the numbers, and find that sum on a game board. Connect a pair of adjacent dots beside the sum. If your line completes a square, capture it with your color. Then choose another familiar center to practice adding and subtracting within ten.","Learning goal: Add and subtract within 10 using strategies that make sense, and explain how you know an answer.\nFor Capture Squares, the sum of two number cubes is the target number; use a 10-frame, counters, or counting on. If the cubes show 4 and 3, count 4, 5, 6, 7 or combine 4 + 3 = 7. Find the board's 7 before drawing a connecting line.\nIn subtraction centers, relate a missing part to an addition fact: 7 - 3 = 4 because 3 + 4 = 7. The game board is a separate activity; finding an answer does not by itself create the visual square.","Example 1 — Roll and add. Two number cubes show 4 and 3. Put four counters and three counters in a 10-frame, or count on 5, 6, 7 from four. The sum is 7. Point to a 7 on Capture Squares and connect two adjacent dots.\nExample 2 — Another roll. The cubes show 2 and 6. Use 2 + 6 = 8 or 6 + 2 = 8. Explain that changing the order does not change the sum; identify an 8 on the game board.\nExample 3 — Switch centers. Seven counters make a whole, and three are yellow. The hidden blue part is four because 3 + 4 = 7, so 7 - 3 = 4. Use counters, a 10-frame, or a familiar center game to check.",
            "Step 1: Choose two number-cube values whose sum is at most 10; represent the dots or use counters.\nStep 2: Find the sum by counting on, combining groups, or using a known fact.\nStep 3: Find that sum on the Capture Squares game board and draw one allowed adjacent-dot line.\nStep 4: If a line completes a square, capture it with your color; then exchange turns.\nStep 5: Explain how you found a sum or difference and try a different strategy or center.","Do not subtract the cube values during Capture Squares: that center asks for their sum.\nDo not draw a line around an arbitrary number; point to the actual sum and follow the adjacent-dot rule.\nDo not count beyond ten when practicing this center; use a roll or representation appropriate to adding within ten.","I can add two cube values within ten, explain a strategy, use the correct sum on the Capture Squares board, and practice subtraction in a familiar center.")
    ];
    public static IReadOnlyCollection<string> TargetLessonCodes => Repairs.Select(r=>r.Code).ToArray();
    private static Repair? Find(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson) =>
        doc.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        doc.VersionCode=="CCSSM-2010" &&
        doc.ContentVersion=="phase29-source-intelligence-en-v2-g1"
          ? Repairs.SingleOrDefault(r=>r.Code==lesson.LessonCode) : null;

    public static bool IsTarget(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson)=>Find(doc,lesson)!=null;

    private static void CheckIdentity(CanonicalLessonContentPackLesson lesson,Repair r)
    {
        if(lesson.SourceUrl!=r.SourceUrl||
           !string.Equals(lesson.CanonicalBodySha256,r.SourceSha,StringComparison.OrdinalIgnoreCase)||
           !lesson.OutcomeCodes.SequenceEqual(r.Outcomes))
            throw new InvalidOperationException($"Original G1 U02 lesson source drift: {lesson.LessonCode}");
    }
    private static bool Matches(CanonicalLessonContentPackTranslation t,Repair r)=>
        t.Explanation==r.Explanation&&t.KeyConceptsAndRules==r.Concepts&&
        t.WorkedExamples==r.Examples&&t.StepByStepSolutions==r.Steps&&
        t.CommonMistakes==r.Mistakes&&t.QuickSummary==r.Summary;

    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        if(doc.PackCode!=MathematicsCurriculumPackRegistry.CommonCoreCode||
           doc.VersionCode!="CCSSM-2010")return;
        foreach(var lesson in doc.Lessons)
        {
            var r=Find(doc,lesson);
            if(r is null)continue;
            CheckIdentity(lesson,r);
            var en=lesson.Translations.Single(x=>x.CultureCode=="en");
            if(Matches(en,r))continue;
            if(!en.StepByStepSolutions.StartsWith(r.PreviousStepPrefix,StringComparison.Ordinal)||
               !en.WorkedExamples.StartsWith(r.PreviousExamplePrefix,StringComparison.Ordinal))
                throw new InvalidOperationException($"Unrecognized old G1 U02 lesson content: {lesson.LessonCode}");
            en.Explanation=r.Explanation;
            en.KeyConceptsAndRules=r.Concepts;
            en.WorkedExamples=r.Examples;
            en.StepByStepSolutions=r.Steps;
            en.CommonMistakes=r.Mistakes;
            en.QuickSummary=r.Summary;
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string prior)=>
        IsTarget(doc,lesson)?CorrectedContentVersion:prior;

    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string prior)=>
        IsTarget(doc,lesson)&&prior==CommonCoreRichContentCorrections.CorrectionContentVersion;

    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string prior,
        IReadOnlyCollection<CurriculumLessonContentTranslation> existing)
    {
        var r=Find(doc,lesson)??throw new InvalidOperationException("Not an authorized G1 L08-L10 correction");
        CheckIdentity(lesson,r);
        if(!CanUpgradeExisting(doc,lesson,prior))
            throw new InvalidOperationException($"Unsafe previous G1 L08-L10 version: {lesson.LessonCode}");
        if(existing.Count==0)return;
        if(existing.Count!=1)throw new InvalidOperationException("Unexpected stored culture count");
        var en=existing.Single();
        if(en.CultureCode!="en"||
           !en.StepByStepSolutions.StartsWith(r.PreviousStepPrefix,StringComparison.Ordinal)||
           !en.WorkedExamples.StartsWith(r.PreviousExamplePrefix,StringComparison.Ordinal))
            throw new InvalidOperationException($"Persisted learner prose differs: {lesson.LessonCode}");
    }
}
