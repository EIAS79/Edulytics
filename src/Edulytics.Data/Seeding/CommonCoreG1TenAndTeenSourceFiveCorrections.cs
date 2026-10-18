using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>Original IM/Kendall Hunt 2021 source-reviewed grade 1 ordinals
/// 46-50, with hard source/outcome guards and no publisher-art parity claims.</summary>
public static class CommonCoreG1TenAndTeenSourceFiveCorrections
{
    public const string Version="us-ccss-g1-010-u03-l09-l13-original-source-v1";
    private sealed record Repair(string Code,string Url,string Sha,string[] Outcomes,
        string Title,string Explanation,string Concepts,string Examples,string Steps,
        string Mistakes,string Summary);
    private static readonly Repair[] Repairs=[
        new("PED:US-CCSS-MATH:G1:U03:L09","https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-9/preparation.html","55682ba3375384a72480cd2446387ff9923fcec7c66f91dc7c1f72252d28c709",new string[]{"CCSS:1.NBT.B.2","CCSS:1.OA.D.8"},"Addition With a Ten","Build teen numbers with one filled ten-frame and some extra counters on the second frame. Read a number card from 11 to 20, show it using two ten-frames, and write an equation with one addend equal to 10. For the boundary card 20, both frames fill: that is two tens and zero loose ones. Next find missing values in addition equations where the teen total, the ten, or the remaining ones may be unknown. Explain the model to a partner.","Goal: compose and decompose teen numbers as a ten and some ones, and solve equations with one addend 10 and an unknown in different positions. One complete frame means ten, not one counter. Numbers 11–19 are one ten and 1–9 extra ones, while the boundary card 20 has two tens and zero extra ones. The same number can be written 10+4=14, 14=10+4, or 14=4+10. Represent the quantity and the equation, not just an isolated fact.","Example 1: Fill the first ten-frame and put 4 counters in the next: 10+4=14.\nExample 2: 14=10+\u25a1, so \u25a1=4. One whole ten and four singles are fourteen.\nExample 3: 16=\u25a1+6, so \u25a1=10.\nExample 4: \u25a1+3=13 means \u25a1=10, while 10+5=\u25a1 means \u25a1=15.","Step 1: Read the teen number or the equation and find what is known.\nStep 2: Fill exactly one group of ten in a double 10-frame.\nStep 3: Count the remaining single counters.\nStep 4: Write a complete equation showing ten and those ones or solve the blank in its actual position.\nStep 5: Check the equality and explain why one full ten-frame stays filled.","Do not count the full ten-frame as one object or place all teen counters in one frame. A blank is not always the ones: in 16=\u25a1+6 the blank represents ten, and in 10+5=\u25a1 it represents fifteen.","I can represent teen numbers with a full ten-frame and some ones, and solve missing-number equations with a ten."),
        new("PED:US-CCSS-MATH:G1:U03:L10","https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-10/preparation.html","17863ba809bcb0482a24ef1fcca941d1cf4db54da1dfac22c0209af1746ee263",new string[]{"CCSS:1.NBT.B.2","CCSS:1.OA.A.1","CCSS:1.OA.B.4","CCSS:1.OA.C.6","CCSS:1.OA.D.8"},"Addition and Subtraction with a Ten","Use a full ten and some ones to solve related addition and subtraction equations and story problems with teen totals. Read the two collection stories carefully: Kiran starts with five caps and reaches fifteen (the CHANGE is unknown), while Priya receives three books and has thirteen (the START is unknown). Show both an addition and a subtraction equation that represent the same situation.","Goal: solve story problems with a teen total and a missing start or change, and find unknown values in related addition and subtraction equations with ten. Use the relationship 10+4=14, 14-4=10 and 14-10=4. Draw a full ten-frame plus ones when useful; arithmetic truth alone does not prove an equation matches the story.","Example 1: Five caps become fifteen after a gift: 5+\u25a1=15 and 15-5=\u25a1, so ten caps were added.\nExample 2: Three books are received and the collection becomes thirteen: \u25a1+3=13 and 13-3=\u25a1, so the start was ten books.\nExample 3: 14-4=10 because 10+4=14.\nExample 4: 15=10+\u25a1 gives five ones.","Step 1: Read each story and identify start, change and final total.\nStep 2: Place the unknown box in the correct position, not automatically after the equal sign.\nStep 3: Make a ten-frame model for the teen amount.\nStep 4: Write the matching addition and subtraction equations and solve the missing quantity.\nStep 5: Substitute your value into the ORIGINAL story and explain how the two equations are connected.","Do not confuse a missing change with a missing start. 5+\u25a1=15 asks how many arrived; \u25a1+3=13 asks how many were there first. Avoid treating 14-4 as fourteen minus ten.","I can solve missing-start and missing-change stories and use addition and subtraction facts involving a ten."),
        new("PED:US-CCSS-MATH:G1:U03:L11","https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-11/preparation.html","680f3a37749e9c58fc0a55d6e9c1d0bb8f9da73a53cc4e0a4e3c16b0d82a8c53",new string[]{"CCSS:1.OA.A.1","CCSS:1.OA.C.6","CCSS:1.OA.D.7","CCSS:1.OA.D.8"},"Add to a Teen Number","Add one digit to a teen number with sums at most 20. Keep the first ten, combine the ones, and when the ones total ten recognize a SECOND full ten, as in 18+2=20. Use double ten-frames or counters to show Kiran's 14 rocks and the three new rocks (14+3=17). Then solve addition equations in which the teen may appear on either side or an unknown appears before or after the equals sign. Finish by choosing a familiar center when working with a partner.","Goal: add one-digit numbers to teen numbers within 20, recognizing that the ones may complete another group of ten in a sum of 20. Represent and solve a story problem, and find missing totals or addends in teen-number addition equations. The warm-up True or False develops reasoning about equations; Compare, Number Puzzles and Find the Pair are familiar partner centers, not silently scored as digital free-form play.","Example 1: Kiran has 14 rocks and finds 3 more: 14+3=17. Ten stays ten; four ones plus three ones are seven.\nExample 2: 17+2=19: the ones give 7+2=9, then add the ten.\nExample 3: 6+11=17 has the same total as 11+6.\nExample 4: \u25a1=15+4 asks for nineteen, while 14+\u25a1=17 asks for three.\nExample 5: 18+2=20: eight ones plus two ones make a second complete ten. Two tens are twenty.","Step 1: Read the story or expression and identify the teen number and added ones.\nStep 2: Represent the teen as one ten and its single ones.\nStep 3: Add the one-digit group to the ones. For sums below twenty, one ten remains full; at twenty, ten additional ones complete a second full ten.\nStep 4: Complete the equation with the unknown in its actual location.\nStep 5: Check the result is at most twenty and explain your strategy to a partner or teacher.","Do not add the units to ten twice; 14+3 is 17, not 27. A blank at the beginning of an equation can be the total. A result of twenty has TWO full tens, not one ten and ten loose singles.","I can add a one-digit number to a teen number, combine the ones, and explain how a sum of twenty makes two tens."),
        new("PED:US-CCSS-MATH:G1:U03:L12","https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-12/preparation.html","c1987db2ca3f10d8b3716d584c4988240ec1147c3e05096974b6db74cdb1ca27",new string[]{"CCSS:1.NBT.A.1","CCSS:1.OA.A.1","CCSS:1.OA.B.4","CCSS:1.OA.C.6","CCSS:1.OA.D.8"},"Subtract From a Teen Number","Subtract a one-digit number from a teen number without taking apart the bundle of ten. Noah has 19 game pieces and removes 8: 19-8=11 because nine ones minus eight ones leaves one, along with the original ten. Next solve the original student Activity 12.2, Addition and Subtraction Equations with Teen Numbers: find missing values in equations where the unknown can occur in several positions. Shake and Spill, Cover is a separate teacher-facilitated center; its physical partner work is not automatically graded as this lesson's equation activity.","Goal: add and subtract within 20 without composing or decomposing a ten, explain a Take From, Result Unknown story and check missing-value equations. Use the ten-plus-ones structure and the inverse relationship between addition and subtraction. The warm-up counts by ten starting at two (2,12,22,32,42,52). The physical Shake and Spill, Cover center is teacher-facilitated, not a replacement for the source student equation activity.","Example 1: Noah starts with nineteen game pieces and takes out eight: 19-8=11, and 8+11=19 checks.\nExample 2: 14-2=12 because four ones minus two ones are two ones and ten remains.\nExample 3: 16+\u25a1=17 means the missing amount is one.\nExample 4: 16-\u25a1=12 means the unknown is four. Its matching addition fact is 12+4=16.","Step 1: Recognize the total as one group of ten plus single ones.\nStep 2: Name the visible, removed, or unknown part of the situation.\nStep 3: Subtract only available ones when solving these no-regrouping tasks.\nStep 4: Write a complete subtraction or related addition equation to check.\nStep 5: For each addition or subtraction equation, identify the blank's position, solve, and check the result with a related fact. Explain to a partner.","Do not take away from the bundled ten if the source task says no regrouping is needed. 19-8=11, not 1. A missing addend or subtrahend is not automatically the result; locate the blank in the ORIGINAL equation.","I can subtract within a teen number, solve missing-number addition and subtraction equations, and justify my solution."),
        new("PED:US-CCSS-MATH:G1:U03:L13","https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-13/preparation.html","fb42f31351fc7159587c49867bc5258565c0160fecf575494f0ff4c3f3c14b5b",new string[]{"CCSS:1.OA.A.1","CCSS:1.OA.C.5","CCSS:1.OA.C.6","CCSS:1.OA.D.8"},"More Story Problems with Teen Numbers PLC Activity","Use teen-number subtraction to solve two related Take From story types. In a Result Unknown story, seventeen students are standing and four leave: 17-4=13 remain. In a Change Unknown story, seventeen students were present and only four remain: 17-\u25a1=4, so thirteen went home. The stories may share numbers but ask DIFFERENT questions. Explain and compare methods and then choose from familiar centers.","Goal: identify the unknown in Take From, Result Unknown and Take From, Change Unknown stories, solve both using counting on, counting back, ten-and-ones or related equations, and explain how their representations differ. Study the original Sitting or Standing and Solve Story Problems and Compare Methods tasks. Partner Shake and Spill, Compare and Number Puzzles centers require separate classroom observations.","Example 1: 17 are standing; 4 sit down: 17-4=13 still stand. The UNKNOWN is the RESULT.\nExample 2: 17 are standing; some sit down; 4 remain: 17-\u25a1=4 and 4+\u25a1=17, so 13 sat down. The UNKNOWN is the CHANGE.\nExample 3: Fifteen were standing; five remain: 15-\u25a1=5, so ten sat down.\nExample 4: Two stories with 17 and 4 can have different correct answers because the unknown is in a different position.","Step 1: Read the story and find the number at the start and what happened after.\nStep 2: Identify whether the story asks for the RESULT or for the CHANGE.\nStep 3: Put an empty box in the correct place of the subtraction equation.\nStep 4: Solve using counting, ten-and-ones or inverse addition, and name what the answer counts.\nStep 5: Check against the exact story and compare it with a story that has a different unknown.","Do not use the same answer for 17-4=\u25a1 and 17-\u25a1=4. The first answer is thirteen remaining; the second is thirteen leaving, even though both numerical answers happen to be thirteen in this pair. In general the meanings and placement differ.","I can tell whether a story hides the amount left or the amount taken away, then solve its matching equation."),

    ];
    private static Repair? Find(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson)=>
        doc.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        doc.VersionCode=="CCSSM-2010"
        ?Repairs.SingleOrDefault(x=>x.Code==lesson.LessonCode):null;
    public static bool IsTarget(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson)=>Find(doc,lesson)!=null;
    private static Repair Checked(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson)
    {
        var r=Find(doc,lesson)??throw new InvalidOperationException("Not G1-010");
        if(lesson.SourceUrl!=r.Url||
           !string.Equals(lesson.CanonicalBodySha256,r.Sha,StringComparison.OrdinalIgnoreCase)||
           !lesson.OutcomeCodes.SequenceEqual(r.Outcomes))
            throw new InvalidOperationException("V1 G1-010 original 2021 source/identity/outcome drift: "+lesson.LessonCode);
        return r;
    }
    private static bool Same(CanonicalLessonContentPackTranslation e,Repair r)=>
        e.Title==r.Title&&e.Explanation==r.Explanation&&
        e.KeyConceptsAndRules==r.Concepts&&e.WorkedExamples==r.Examples&&
        e.StepByStepSolutions==r.Steps&&e.CommonMistakes==r.Mistakes&&
        e.QuickSummary==r.Summary;
    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        foreach(var lesson in doc.Lessons)
        {
            if(!IsTarget(doc,lesson))continue;
            var r=Checked(doc,lesson);
            var en=lesson.Translations.Single(x=>x.CultureCode=="en");
            if(Same(en,r))continue;
            if(!en.WorkedExamples.StartsWith("Example 1:",StringComparison.Ordinal)||
               !en.StepByStepSolutions.StartsWith("Step 1:",StringComparison.Ordinal))
                throw new InvalidOperationException("G1-010 pre-repair learner-body drift");
            en.Title=r.Title;en.Explanation=r.Explanation;en.KeyConceptsAndRules=r.Concepts;
            en.WorkedExamples=r.Examples;en.StepByStepSolutions=r.Steps;
            en.CommonMistakes=r.Mistakes;en.QuickSummary=r.Summary;
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previous)=>
        IsTarget(doc,lesson)?Version:previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previous)=>
        IsTarget(doc,lesson)&&previous==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previous,
        IReadOnlyCollection<CurriculumLessonContentTranslation> translations)
    {
        var r=Checked(doc,lesson);
        if(!CanUpgradeExisting(doc,lesson,previous))
            throw new InvalidOperationException("G1-010 persisted predecessor drift");
        if(translations.Count==0)return; // live JSON-only content storage
        if(translations.Count!=1)throw new InvalidOperationException("G1-010 culture drift");
        var en=translations.Single();
        if(en.CultureCode!="en"||
           !en.WorkedExamples.StartsWith("Example 1:",StringComparison.Ordinal)||
           !en.StepByStepSolutions.StartsWith("Step 1:",StringComparison.Ordinal))
            throw new InvalidOperationException("G1-010 persisted text drift");
    }
}
