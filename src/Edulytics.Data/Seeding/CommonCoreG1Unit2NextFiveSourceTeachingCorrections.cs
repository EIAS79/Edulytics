using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Source-locked 2021 IM first-grade story and center instruction for V1 G1-005 lessons 21-25.
/// Original teacher prep, complete teacher/Student Facing activities, and
/// task-sheet PDF links were identified. Not independent academic signoff.
/// </summary>
public static class CommonCoreG1Unit2NextFiveSourceTeachingCorrections
{
    public const string CorrectedContentVersion="us-ccss-g1-u02-l06-l10-source-aligned-v1";
    private sealed record Repair(string Code,string Url,string Sha,string[] Outcomes,
        string OldFirstStep,string OldFirstExample,
        string Explanation,string Concepts,string Examples,string Steps,
        string Mistakes,string Summary);
    private static readonly Repair[] Repairs =
    [
        new("PED:US-CCSS-MATH:G1:U02:L06","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-6/preparation.html","0cd60addebdff2e502f0c58351933b6baf570a8bc3555b5de6ab1956efd71cef",["CCSS:1.OA.A.1","CCSS:1.OA.B.4","CCSS:1.OA.C.6"],
            "Step 1: Identify the two quantities, 4 and 2.",
            "Example 1: The source task combines 4 and 2. Start at 4 and count on 2 more to reach 6. Therefore, 4 + 2 = 6.",
            "A pet shelter has two groups, such as 4 cats and 2 dogs. Nobody arrives or leaves. To find the total, put the groups together. The order of the groups can change without changing the total. Draw the animals or use counters; write TWO matching addition equations and explain the quantities.",
            "Goal: solve Put Together, Total Unknown stories and explain matching equations.\nWhen no action happens, two groups still make one total. 4 + 2 = 6 and 2 + 4 = 6 show the same six pets. The related facts 6 - 4 = 2 and 6 - 2 = 4 show how the whole and parts fit.",
            "Example 1: There are four cats and two dogs. Count four counters and two counters: 4 + 2 = 6 pets.\nExample 2: Start with the dogs instead: 2 + 4 = 6. Both equations show six pets even though we named the groups in different orders.\nExample 3: A shelter has five rabbits and three guinea pigs. Draw the two groups, then write 5 + 3 = 8 and 3 + 5 = 8.",
            "Step 1: Read the pet story three times to identify the two groups and the question.\nStep 2: Draw or model the two groups without inventing an arrival or departure.\nStep 3: Combine and count all pets.\nStep 4: Write two full equations, one in each group order, and say why both show the same total.",
            "Do not add a leaving or joining action when the story describes two existing groups. A total of 6 is not itself an equation; write 4+2=6. Switching the addends cannot change the total.",
            "I can draw two groups of pets, find their total and write two addition equations with the groups in opposite orders."),
        new("PED:US-CCSS-MATH:G1:U02:L07","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-7/preparation.html","f3b8bdf030bd1f4bbe981e0867a5f048b3fc9d23c6fc75cd221547c767c0e7f9",["CCSS:1.OA.A.1","CCSS:1.OA.C.6"],
            "Step 1: Identify the two quantities, 8 and 2.",
            "Example 1: The source task combines 8 and 2. Start at 8 and count on 2 more to reach 10. Therefore, 8 + 2 = 10.",
            "Use two-color counters for Shake and Spill. Shake a known number, spill the counters, then sort them by color. The two parts always make the same whole, even when the split changes. Record different combinations as complete equations. Try to find all as an optional challenge.",
            "Goal: solve Both Addends Unknown stories. A whole of 6 can be 0+6, 1+5, 2+4 or 3+3 (and their reversed color order). Two different parts can make the same whole. List systematically so none is missed.",
            "Example 1: Shake six counters. If two land red and four land yellow, write 2+4=6.\nExample 2: With four counters, possible unordered groups are 0+4=4, 1+3=4, 2+2=4. You have listed all three ways.\nExample 3: For a whole of 7, begin 0+7=7, then 1+6=7, 2+5=7, 3+4=7.",
            "Step 1: Count the whole collection of counters before splitting it.\nStep 2: Put zero of the first color in a group; record the other color's count.\nStep 3: Move one counter from the second group to the first each time.\nStep 4: Write each full addition equation, check the same whole, and stop before reversing a split you already recorded.",
            "Do not skip the all-one-color arrangement; 0+6=6 is valid. Do not count 2+4 and 4+2 as two different unordered splits. An equation with the wrong total is not a valid shake result.",
            "I can use two-color counters to show every distinct pair that makes the same whole, and record each pair as a complete equation."),
        new("PED:US-CCSS-MATH:G1:U02:L08","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-8/preparation.html","5bfd9d463b015a7b077d89764698402440a047d67ca10d5cc605d550814e525a",["CCSS:1.NBT.A.1","CCSS:1.OA.A.1","CCSS:1.OA.C.6"],
            "Step 1: Represent the quantities or objects in the task.",
            "Example 1: \u201cToday we played a game where we had to find out how many counters were under our cup. We wrote equations to match the counters. How could this equation match the counters in a round of Shake and Spill, Cover?\u201d (There are 5 red counters showing. Some yellow counters are under the cup. There are 8 total counters.) \u201cFind the number that makes the equation true. Show how you know.\u201d (3. I counted on from 5: 6, 7, 8. I put down 5 red counters, then added yellow counters until I got to 8. There are 3 yellow counters.) \u201cWhen we know one of the addends in an equation, we can find the other by thinking about how many more we need to get to the total.\"",
            "Shake, Spill, and Cover hides some counters under a cup. You know the total and you can see only one part. Count on to find the hidden counters. Represent the unknown part with a question mark, then show the completed addition equation AND related subtraction equation.",
            "Goal: find a missing addend and relate two equations to one situation. If there are eight counters in all and five are visible, 5+?=8. Count on 6,7,8: three are hidden. So 5+3=8 and 8-5=3.",
            "Example 1: Eight counters total; five visible. Count 6,7,8 from five: the hidden part is three. Write 5+3=8.\nExample 2: Check by subtraction: 8-5=3. The same picture has a whole of eight and parts five and three.\nExample 3: Ten counters in total; four visible. The missing part is six because 4+6=10 and 10-4=6.",
            "Step 1: Count the total and the visible counters; do NOT pretend the hidden group is visible.\nStep 2: Put a question mark beside the covered part.\nStep 3: Count on from visible to total and identify how many are covered.\nStep 4: Write a completed addition equation or a related subtraction equation (both are valid), and optionally show both.\nStep 5: Check both equations represent the same cup-and-counters picture.",
            "Do not enter the total when asked for the hidden part. Do not count hidden objects as though you can see through the cup. Both equations must fit the SAME quantities, not different totals.",
            "I can find a hidden part using the whole and the visible part and write matching addition and subtraction equations."),
        new("PED:US-CCSS-MATH:G1:U02:L09","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-9/preparation.html","c6c0cb894a46fdb2271e32e9c779755dbc09b058d66202632d0dfcc937e10169",["CCSS:1.OA.A.1","CCSS:1.OA.B.3","CCSS:1.OA.D.7"],
            "Step 1: Identify the two quantities, 4 and 6.",
            "Example 1: The source task combines 4 and 6. Start at 4 and count on 6 more to reach 10. Therefore, 4 + 6 = 10.",
            "Three reptile stories can use the same numbers but have different unknowns. Sometimes you know two groups and need their total. Sometimes the total and one group are known. Sometimes only the total is known and many splits are possible. Draw or use counters and compare with classmates during a teacher-guided gallery walk.",
            "Goal: compare Total Unknown, One Part Unknown and Both Parts Unknown stories. Total unknown: 4+6=[10]. One part unknown: 4+[6]=10. Both unknown: [3]+[7]=10 or [5]+[5]=10. Box the number or part that answers the actual story question.",
            "Example 1: A class has four turtles and six lizards. Ten reptiles altogether: 4+6=10.\nExample 2: A class has ten reptiles, four are turtles. Six are lizards because 4+6=10 and 10-4=6.\nExample 3: There are ten reptiles, but their types are unknown. Both 3+7=10 and 5+5=10 are possible splits; one answer is not uniquely determined.",
            "Step 1: Read the story to find which quantity is unknown: total, one part or both parts.\nStep 2: Draw counters for the known quantities, leaving question marks where information is missing.\nStep 3: Write two full matching equations; for both-parts-unknown choose two different valid splits.\nStep 4: Box the quantity answering the question; explain what each number represents.\nStep 5: With a teacher or partner, compare pictures and equations during the gallery walk.",
            "Do not assume every problem has the total missing. If both parts are unknown, there are many correct splits, so don't pretend only one pair works. Do not count a reversed pair twice when asked for two different decompositions.",
            "I can tell whether a story asks for the total, one missing part, or two unknown parts, and write equations that fit each situation."),
        new("PED:US-CCSS-MATH:G1:U02:L10","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-10/preparation.html","865a5e94280bd2f444d046eb63e0fb0149e9153abd8a6dc42e038100aa78be7b",["CCSS:1.OA.A.1","CCSS:1.OA.C.6"],
            "Step 1: Identify the starting quantity 7 and the amount 2 being removed or compared.",
            "Example 1: The source task compares or removes 2 from 7. The resulting difference is 5. Therefore, 7 \u2212 2 = 5.",
            "Center Day 2 practices addition and subtraction within 10. In Capture Squares, roll two number cubes, add their faces, find that sum on the grid and draw ONE edge of the matching square. A square is captured only when the fourth side closes. You can also choose a Math Stories or Shake and Spill center with a teacher or partner.",
            "Goal: add and subtract within 10 using number cubes, number pairs and story problems. For Capture Squares, a sum identifies a board square; each player draws one legal edge. One edge is not a captured square, and an edge already drawn cannot be used again. A small first-party digital board demonstrates the rules; the original full-size classroom board is a separate partner activity.",
            "Example 1: The cubes show 3 and 4, so 3+4=7. On the sample board find the square labeled 7 (A2) and draw one edge.\nExample 2: Cubes show 5 and 3, so the sum is 8. Find square 8 (B1). Mark one legal side; do not claim a capture until the fourth side closes.\nExample 3: Mental subtraction warm-up: 8-3=5. Check using 5+3=8. This supports quick center calculations.",
            "Step 1: Choose Capture Squares, Math Stories, or Shake and Spill.\nStep 2: In Capture Squares, roll two cubes and add them within 10.\nStep 3: Find the matching number on the practice grid and mark a legal first side, or identify the final unused side when three sides are already marked.\nStep 4: With a teacher or partner, take turns marking unused edges; only the fourth edge completes a capture.\nStep 5: Check each sum or subtraction mentally, and explain the strategy you used.",
            "Do not mark a square with a different number from the dice sum. Drawing one side does not capture a whole square. Do not cross an already marked side in a real game; full turn history and classroom partner play need teacher supervision.",
            "I can add two number cubes within 10, find their sum on a board, describe a legal first move, and use related addition or subtraction facts."),
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
            throw new InvalidOperationException($"G1 U02 L06-L10 original source/standard drift: {lesson.LessonCode}");
    }

    // Official teacher/student lesson headings from locally archived 2021 IM K-5 source.
    // The earlier catalogue uses activity names for L07-L09; preserve lesson IDs.
    private static (string Previous,string Verified) SourceTitle(string code)=>code switch
    {
        "PED:US-CCSS-MATH:G1:U02:L07"=>("Shake and Spill","One Part Red, One Part Yellow"),
        "PED:US-CCSS-MATH:G1:U02:L08"=>("Shake, Spill, and Cover","One Part Red, How Many Yellow?"),
        "PED:US-CCSS-MATH:G1:U02:L09"=>("Compare Story Problems PLC Activity","What Is the Unknown?"),
        "PED:US-CCSS-MATH:G1:U02:L14"=>("Compare with Addition and Subtraction PLC Activity","Compare with Addition and Subtraction"),
        _=>("", "")
    };
    // Keep the immutable historical blueprint/title alias for traceability.
    // Only the three local teacher/student-page-verified lesson headings may
    // replace those aliases in the live canonical grade catalogue.
    public static bool TryGetVerifiedPublisherTitle(
        string code, out string historical, out string publisher)
    {
        (historical, publisher)=SourceTitle(code);
        return publisher.Length>0;
    }

    public static string ProjectVerifiedPublisherTitle(string code,string blueprintTitle)
    {
        if(!TryGetVerifiedPublisherTitle(code,out var historical,out var publisher))
            return blueprintTitle;
        if(blueprintTitle!=historical && blueprintTitle!=publisher)
            throw new InvalidOperationException(
                $"G1 U02 unexpected baseline catalogue title: {code}");
        return publisher;
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
            var (previousTitle,verifiedTitle)=SourceTitle(r.Code);
            if(verifiedTitle.Length>0 &&
               en.Title!=previousTitle && en.Title!=verifiedTitle)
                throw new InvalidOperationException($"G1 U02 unknown existing publisher title: {r.Code}");
            if(Matches(en,r))
            {
                if(verifiedTitle.Length>0)en.Title=verifiedTitle;
                continue;
            }
            if(!en.StepByStepSolutions.StartsWith(r.OldFirstStep,StringComparison.Ordinal) ||
               !en.WorkedExamples.StartsWith(r.OldFirstExample,StringComparison.Ordinal))
                throw new InvalidOperationException($"G1 U02 L06-L10 unknown old learner body: {lesson.LessonCode}");
            if(verifiedTitle.Length>0)en.Title=verifiedTitle;
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
        var r=Find(doc,lesson) ?? throw new InvalidOperationException("Not G1 U02 L06-L10 correction target");
        Check(lesson,r);
        if(!CanUpgradeExisting(doc,lesson,existingVersion))
            throw new InvalidOperationException($"G1 U02 L06-L10 unexpected persisted version: {lesson.LessonCode}");
        if(existing.Count==0)return; // deployed JSON-only lesson mode
        if(existing.Count!=1)
            throw new InvalidOperationException("G1 U02 L06-L10 unexpected persisted culture rows");
        var t=existing.Single();
        if(t.CultureCode!="en" || !t.StepByStepSolutions.StartsWith(r.OldFirstStep,StringComparison.Ordinal) ||
           !t.WorkedExamples.StartsWith(r.OldFirstExample,StringComparison.Ordinal))
            throw new InvalidOperationException($"G1 U02 L06-L10 persisted learner text differs: {lesson.LessonCode}");
    }
}
