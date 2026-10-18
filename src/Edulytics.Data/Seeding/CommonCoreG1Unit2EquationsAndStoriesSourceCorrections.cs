using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>2021 original teacher preparation + full Kendall Hunt teacher
/// lesson activities G1 U02 L16–20; no claim of independent original student PDF
/// image parity or live teacher-facilitated center completion.</summary>
public static class CommonCoreG1Unit2EquationsAndStoriesSourceCorrections
{
    public const string CorrectedContentVersion="us-ccss-g1-u02-l16-l20-source-aligned-v1";
    private sealed record Repair(string Code,string Url,string Sha,string[] Outcomes,
        string OldStep,string OldExample,string Title,string Explanation,
        string Concepts,string Examples,string Steps,string Mistakes,string Summary);
    private static readonly Repair[] Repairs=
    [
        new("PED:US-CCSS-MATH:G1:U02:L16","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-16/preparation.html","11a4d97d030bc7e24025edd2f660084cc57e91acd2e858b8a200cc320cfc5c5a",new string[]{"CCSS:1.OA.A.1","CCSS:1.OA.C.6"},
            "Step 1: Identify the starting quantity 9 and the amount 7 being removed or compa","Example 1: The source task compares or removes 7 from 9. The resulting differenc",
            "Center Day 3","Center Day 3 is not a collection of unrelated subtraction facts: the original 2021 lesson teaches Capture Squares Stage 2, where partners subtract two drawn number cards (within ten) and use their difference to choose a legal gameboard edge. If you complete a square, you shade it. Practice number-talk related subtraction facts such as 9−7 and 9−2, then choose a familiar center. Our digital Capture Squares activity simulates legal edge selection but does not certify real center collaboration.","Goal: add and subtract within 10 using the Number Talk and Capture Squares Stage 2. To take your turn, draw two number cards from 0–10, subtract the smaller from the larger, and connect a legal adjacent pair of dots around that answer on the gameboard. Closing a square captures it. Centers also let learners practice Math Stories, Shake and Spill, and What's Behind My Back. The gameboard and partner play are teacher-facilitated.","Example 1: Number Talk 9−7=2 and 9−2=7. They are related facts because 7+2=9.\nExample 2: 8−6=2 and 8−2=6. Count on to check each difference.\nExample 3: In Capture Squares choose cards 8 and 3; the difference is 5. On the gameboard connect two adjacent dots bordering the square marked 5. A line not adjacent to 5 is not a legal move.",
            "Step 1: Draw two number cards and order them so the larger comes first.\nStep 2: Subtract to find the difference (0 through 10), using counters or a number talk strategy.\nStep 3: Find the gameboard square labeled by that difference and select an available adjacent edge.\nStep 4: Check the edge is unoccupied; if it completes a square, mark that square in your color.\nStep 5: Choose an optional previously introduced center with a partner and explain your strategy.","A Capture Squares answer is not the subtraction number alone: legal placement on the gameboard matters. Never subtract to obtain a negative number in this Grade 1 center. Center choice and real partner negotiations must not be claimed as completed by a solo digital response.","I can subtract within ten and use the result to make a legal Capture Squares move."),
        new("PED:US-CCSS-MATH:G1:U02:L17","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-17/preparation.html","a0f7510e878a91d467029d7df540b368d76e3326970fbac5bb87c890eb41a94e",new string[]{"CCSS:1.OA.A.1","CCSS:1.OA.C.6","CCSS:1.OA.D.7"},
            "Step 1: Identify the two quantities, 6 and 4.\nStep 2: Start at 6 and count on 4 ","Example 1: The source task combines 6 and 4. Start at 6 and count on 4 more to r",
            "How Do the Stories Compare?","In the original 2021 activity pupils compare several story problem structures rather than simply calculate isolated sums. Ask: what happened in the story, what is already known, and what is being asked? Add To change unknown and Put Together addend unknown are related but not identical: one describes a joining action and the other describes two parts. In an outdoor-games activity, pupils solve a story and defend why their equation represents it.","Goal: identify how Add To (change unknown), Take From (result unknown), and Put Together/Take Apart (addend unknown) stories are alike and different, and write an equation with a box around the answer. One story might ask for a total, a missing change, or a remaining part even when some numbers repeat.","Example 1: Six children are jumping rope and four join. The number playing now is unknown: 6+4=10 (total unknown).\nExample 2: Ten children play; six started and some others joined. Find the change: 6+4=10 or 10−6=4 (change unknown).\nExample 3: Nine children jump Double Dutch; four jump alone. Five fewer children jump alone: 9−4=5 or 5+4=9 (difference unknown). The unit and story matter.",
            "Step 1: Read the entire story and underline the question.\nStep 2: Mark its quantities as starting group, change, total, or two compared groups.\nStep 3: Say how the story differs from a related story that uses similar numbers.\nStep 4: Draw or use counters to represent the story, not just its numbers.\nStep 5: Write a complete addition/subtraction equation and put a box around the number answering the question; explain the match.","Do not assume a change-unknown story is the same as a total-unknown one. Picking addition because the word 'more' appears can misread a comparison. A mathematically true equation about unrelated numbers does not represent the story. Group posters and discussion remain teacher-led.","I can compare the structures of different story types and explain an equation that matches each one."),
        new("PED:US-CCSS-MATH:G1:U02:L18","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-18/preparation.html","2e0473e7b39174ac445be31797739e205f4ba91e1d1ade66b89764ad30dff7a0",new string[]{"CCSS:1.OA.A.1","CCSS:1.OA.B.4","CCSS:1.OA.D.8"},
            "Step 1: Identify the starting quantity 9 and the amount 3 being removed or compa","Example 1: The source task compares or removes 3 from 9. The resulting differenc",
            "Equations with Unknowns","The original 2021 warm-up shows 4+□=10 and asks what the unknown box means in a picture. Lesson activities match story cards to equation cards with boxed unknowns, then discuss which of two different equations fits a Put Together, Addend Unknown situation. Keep the box in the story-to-equation representation; when you solve it, replace the box with the value that makes the equation TRUE.","Goal: interpret a square symbol □ as an unknown value in an equation and connect its position to the unknown in a story problem (1.OA.D.8). An empty box is not zero. An equation with a box can represent a missing addend, missing part, or unknown result. Related addition and subtraction equations can both match one story.","Example 1: Four basketballs are visible, and ten in all: 4+□=10. The hidden amount is six, so 4+6=10.\nExample 2: Nine blocks in all, three inside the box: 9−□=3; the unknown removed part is six, so 9−6=3.\nExample 3: Five counters and some more make eight: 5+□=8 and 8−5=□ both refer to the missing three; 5+3=8.",
            "Step 1: Read the story and identify what quantity is unknown.\nStep 2: Place □ at the position of that quantity in an addition or subtraction equation.\nStep 3: Use counters, counting on or a related fact to solve for the box.\nStep 4: Replace □ with the number and check that BOTH sides of the equation are equal.\nStep 5: Explain how the symbols and each quantity match the original story.","Do not treat □ as the number 0, and don't move the box to a different unknown without changing the story. An equation such as 4+□=10 cannot be solved by simply adding 4 and 10. Writing only 6 omits the connection to the unknown-box equation.","I can interpret, locate and solve a boxed unknown in an equation that matches a story."),
        new("PED:US-CCSS-MATH:G1:U02:L19","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-19/preparation.html","68d764f69636ee52c8ece8d919026198acf3971fb754512f686c5d0879faf7c2",new string[]{"CCSS:1.NBT.A.1","CCSS:1.OA.A.1","CCSS:1.OA.B.4"},
            "Step 1: Identify the starting quantity 9 and the amount 3 being removed or compa","Example 1: The source task compares or removes 3 from 9. The resulting differenc",
            "Story Problems and Equations","In the original Kendall Hunt 2021 lesson, children solve Lotería stories and write two equations for each, then discuss what question a story without a stated question could ask. If ten pictures are called and seven are on Mai's board, three are not on her board. Two matching equations might be 10−7=3 and 7+3=10; 10=7+3 and 3+7=10 are also related true statements. A student may use an unknown box; it is not compulsory.","Goal: write TWO different true equations matching the SAME story problem (1.OA.A.1, 1.OA.B.4). The original 2021 Lotería task accepts addition/subtraction related facts, reversing the order of addends, or writing the total on the left of '='; use a box for an unknown if useful. The warm-up separately practices choral counting by tens to 100 (1.NBT.A.1).","Example 1: Ten picture cards were called, seven match the board, three are missing: 10−7=3; 7+3=10.\nExample 2: Ten beans, three fall off. Seven remain: 10−3=7; 3+7=10.\nExample 3: Four playing Lotería and three more join makes seven: 4+3=7 and 3+4=7. The rearrangement demonstrates the addends' order property.",
            "Step 1: Read the story and identify its question (or identify a possible meaningful question if none was stated).\nStep 2: Represent the starting amount and other known quantities.\nStep 3: Solve and write the first COMPLETE true equation.\nStep 4: Write a second DIFFERENT complete matching equation using a related fact, reordered addends or equal-sign orientation.\nStep 5: Check BOTH equations describe the same story; a teacher can discuss other valid equations using a box.","Two unrelated true equations do not both match a story; do not count a bare result as an equation. An equation with its total to the left of '=' is valid when its sides are equal. The 0–100 choral count is a separate warm-up, not permission to enlarge the within-10 story-problem number domain.","I can write and explain two different true equations for one story and understand the equal sign."),
        new("PED:US-CCSS-MATH:G1:U02:L20","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-20/preparation.html","4be2011a4fd08dc71e8894bb9f778b8c1f2773ed6b3ea17fe79dc543cdb52571",new string[]{"CCSS:1.OA.A.1","CCSS:1.OA.B.4","CCSS:1.OA.C.6"},
            "Step 1: Identify the two quantities, 3 and 5.\nStep 2: Start at 3 and count on 5 ","Example 1: The source task combines 3 and 5. Start at 3 and count on 5 more to r",
            "What's the Story?","The original 2021 lesson reverses the usual direction of work: rather than finding an equation for a story, children WRITE a story for an equation such as 3+□=8. In the next activity they choose an answer, produce an equation containing that number, and invent a matching story. A teacher/partner should check that the invented story has the correct quantitative meaning. Our bounded digital story scaffold accepts a simple join/take-away narrative; this does NOT certify every possible free-form story.","Goal: AUTHOR a story that matches a provided addition/subtraction equation or a student-chosen answer (1.OA.A.1, 1.OA.B.4; building toward 1.OA.D.8). The student must describe the quantities and how they change, not merely solve an isolated number fact. The original 2021 lesson also offers student-selected center practice within 10.","Example 1: For 3+□=8: 'I had 3 shells. I got 5 more shells and now I have 8 shells.' The box represents the five new shells.\nExample 2: For 10−4=6: 'I had 10 shells. I gave away 4 shells and now I have 6 shells.' The subtraction action and outcome match the equation.\nExample 3: Choose answer 7. Write 4+3=7 and a story: 'I had 4 shells. I got 3 more shells and now I have 7 shells.' Share it and have a partner check it.",
            "Step 1: Read the GIVEN equation, including any □, and identify what each number means.\nStep 2: Choose a joining or taking-away action that matches the equation's operation.\nStep 3: Invent a starting situation and write a complete sentence showing the first quantity.\nStep 4: Add a sentence with the change and result (or a question for the missing part).\nStep 5: Compare your story with the original equation and check it actually represents that operation; ask a teacher/partner to review free-form wording.","Do not only calculate 3+5=8: the goal is to WRITE a story. A subtraction equation cannot be matched with an unrelated adding action. Avoid changing a given quantity while inventing the narrative. Automated scaffold checks are not a replacement for reviewing open-ended stories.","I can invent a joining or taking-away story that matches an equation, and explain the unknown.")
    ];
    private static Repair? Find(CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson)=>
        doc.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        doc.VersionCode=="CCSSM-2010"
            ?Repairs.SingleOrDefault(x=>x.Code==lesson.LessonCode)
            :null;
    public static bool IsTarget(CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson)=>
        Find(doc,lesson)!=null;
    private static Repair Checked(CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson)
    {
        var r=Find(doc,lesson)??throw new InvalidOperationException("Not G1-007 target");
        if(lesson.SourceUrl!=r.Url||
            !string.Equals(lesson.CanonicalBodySha256,r.Sha,StringComparison.OrdinalIgnoreCase)||
            !lesson.OutcomeCodes.SequenceEqual(r.Outcomes))
            throw new InvalidOperationException("V1 G1-007 source/identity/outcome drift: "+lesson.LessonCode);
        return r;
    }
    private static bool Matches(CanonicalLessonContentPackTranslation t,Repair r)=>
        t.Title==r.Title&&t.Explanation==r.Explanation&&t.KeyConceptsAndRules==r.Concepts&&
        t.WorkedExamples==r.Examples&&t.StepByStepSolutions==r.Steps&&
        t.CommonMistakes==r.Mistakes&&t.QuickSummary==r.Summary;
    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        foreach(var lesson in doc.Lessons)
        {
            if(!IsTarget(doc,lesson))continue;
            var r=Checked(doc,lesson);
            var en=lesson.Translations.Single(x=>x.CultureCode=="en");
            if(Matches(en,r))continue;
            if(!en.StepByStepSolutions.StartsWith(r.OldStep,StringComparison.Ordinal)||
               !en.WorkedExamples.StartsWith(r.OldExample,StringComparison.Ordinal))
                throw new InvalidOperationException("V1 G1-007 unexpected original learner body: "+lesson.LessonCode);
            en.Title=r.Title;en.Explanation=r.Explanation;
            en.KeyConceptsAndRules=r.Concepts;en.WorkedExamples=r.Examples;
            en.StepByStepSolutions=r.Steps;en.CommonMistakes=r.Mistakes;
            en.QuickSummary=r.Summary;
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previous)=>
        IsTarget(doc,lesson)?CorrectedContentVersion:previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string version)=>
        IsTarget(doc,lesson)&&version==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson,
        string existingVersion,IReadOnlyCollection<CurriculumLessonContentTranslation> existing)
    {
        var r=Checked(doc,lesson);
        if(!CanUpgradeExisting(doc,lesson,existingVersion))
            throw new InvalidOperationException("V1 G1-007 unexpected persisted version");
        if(existing.Count==0)return; // JSON-only live lesson mode
        if(existing.Count!=1)throw new InvalidOperationException("V1 G1-007 culture drift");
        var en=existing.Single();
        if(en.CultureCode!="en"||!en.StepByStepSolutions.StartsWith(r.OldStep,StringComparison.Ordinal)||
           !en.WorkedExamples.StartsWith(r.OldExample,StringComparison.Ordinal))
            throw new InvalidOperationException("V1 G1-007 persisted content drift");
    }
}
