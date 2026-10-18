using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>Source-goal grounded learner-text corrections for G1 ordinals 51–60.
/// Teaching body only: this does NOT independently certify practice or classroom activities.</summary>
public static class CommonCoreG1FullGradeStartingContentCorrections
{
    public const string Version="us-ccss-g1-full-grade-u03-l14-l23-content-source-v2";
    private sealed record Repair(string Code,string Url,string Sha,string[] Outcomes,
        string Title,string Explanation,string Concepts,string Examples,string Steps,
        string Mistakes,string Summary);
    private static readonly Repair[] Repairs =
    [
        new(@"PED:US-CCSS-MATH:G1:U03:L14",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-14/preparation.html",@"b439db9cd168fe54b5d04b664c77375b02d027fea69f6cef6664f7f486802b5a",new[] { @"CCSS:1.OA.C.6", @"CCSS:1.OA.D.7", @"CCSS:1.OA.D.8" },
            @"Center Day 2",
            @"Practice adding and subtracting within 20 through Number Puzzles, true-or-false equations, and center choices. First decide whether both sides of an equation represent the same number; then solve number puzzles using digit cards, each at most once per puzzle board. Practice both addition and subtraction, not just a count-on fact. A partner or teacher can lead the physical center games.",
            @"Learning goals: add AND subtract within 20; determine whether addition expressions on both sides of an equal sign are equivalent; find missing values in addition/subtraction number puzzles. Use one digit card only once on the same puzzle board. A balanced equation has equal values on both sides. The physical Number Puzzles, Shake and Spill and Compare centers are teacher-led, not simulated as completed.",
            @"Example 1: 4 + 6 = 3 + 7 is true because both sides equal 10.
Example 2: 5 + 1 = 5 + 2 is false because 6 is not 7.
Example 3: Complete 9 + □ = 15: the missing number is 6. Check: 9 + 6 = 15.
Example 4: Complete □ - 7 = 6: the missing number is 13. Check: 13 - 7 = 6.",
            @"Step 1: Read the expression or puzzle; identify which operation and unknown are present.
Step 2: Use counters, a drawing, count-on facts, or a known ten to reason within 20.
Step 3: For an equality, evaluate or compare BOTH sides. For a number puzzle, place digit cards so every equation is true and no card is reused.
Step 4: Check the addition or subtraction relation and explain why it works.",
            @"Do not mark an equation true merely because it has an equals sign. In 5+1=5+2, the sides have different values. Do not turn every subtraction puzzle into addition or reuse the same digit card in multiple blanks on one physical puzzle board.",
            @"I can add and subtract within 20, judge true and false equations, and explain a number puzzle. Partner center play remains a classroom activity."),
        new(@"PED:US-CCSS-MATH:G1:U03:L15",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-15/preparation.html",@"5841d00d899ca1a7452c1fd015f90a23c5bd134642f0c42ff1da9f8734388a24",new[] { @"CCSS:1.OA.A.2", @"CCSS:1.OA.B.3", @"CCSS:1.OA.C.5", @"CCSS:1.OA.C.6" },
            @"Solve Story Problems with Three Numbers",
            @"Solve a story with three groups of objects, where TWO of the addends combine to ten. Instead of adding only the first two numbers, identify all three groups, choose a pair that makes ten, then add the remaining number. Draw counters or a double ten-frame and write a three-addend equation.",
            @"Learning goal: solve story problems within 20 involving THREE addends, two making 10. The order and grouping of addends may change while the sum stays the same. For example, 3 + 8 + 7 = (3 + 7) + 8 = 10 + 8 = 18. Explain the story and write a matching equation; a two-addend total alone does not establish the goal.",
            @"Example 1: Three groups of birds contain 3, 8, and 7 birds. 3 and 7 make ten, so 3 + 8 + 7 = 10 + 8 = 18 birds.
Example 2: A card collection has piles of 5, 9, and 5 cards. Combine 5 + 5 first to make 10, then 10 + 9 = 19 cards.
Example 3: Three jars have 4, 6, and 7 beads. 4 + 6 = 10; 10 + 7 = 17 beads.",
            @"Step 1: Read the whole story and name ALL THREE quantities.
Step 2: Write an equation containing three addends, including the unknown total.
Step 3: Find a pair summing to 10, even if the pair is separated by another addend.
Step 4: Regroup as 10 plus the leftover amount, then compute the sum (at most 20).
Step 5: Check that the total and unit answer the original story; explain with a double ten-frame or drawing.",
            @"Do not ignore the third group, or assume only neighboring addends can form ten. 3+8+7 is 18, not 11; 3+7=10 and 10+8=18. Adding two numbers in isolation does not represent a three-part story.",
            @"I can write and solve three-addend story equations by pairing two groups into ten and then adding the remaining group."),
        new(@"PED:US-CCSS-MATH:G1:U03:L16",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-16/preparation.html",@"9636f47c3b2c14156f0974ae3b476bec15740b659111527850386b592335bcaa",new[] { @"CCSS:1.OA.B.3", @"CCSS:1.OA.C.6", @"CCSS:1.OA.D.7" },
            @"Add Three Numbers",
            @"Rewrite three-number additions as equivalent ten-plus-something expressions. Two numbers in each three-addend expression make ten. Also decide whether an equation with an expression on BOTH sides is true. Matching two expressions is more than finding one isolated sum.",
            @"Learning goals: use the addition properties to make ten with THREE addends; compare equivalent expressions 4+6+8 = 10+8; evaluate true/false equations with addition expressions on both sides. The equality sign asserts both expressions have the same total, not that the right side is a place to write an answer.",
            @"Example 1: 4 + 6 + 8 = 10 + 8 = 18. The 4 and 6 make ten.
Example 2: 3 + 6 + 7 = 10 + 6 = 16 because 3 + 7 = 10.
Example 3: 7 + 3 + 4 = 10 + 4 is TRUE; both sides equal 14.
Example 4: 6 + 5 + 4 = 15 + 10 is FALSE: left side is 15, right side is 25.
Example 5: 5 + 7 + 5 = 10 + 7 = 17.",
            @"Step 1: Inspect all three addends and identify which two make 10.
Step 2: Rewrite the full expression as 10 + the remaining addend.
Step 3: Match its value to an equivalent ten-plus expression.
Step 4: For a two-sided equation, compare BOTH totals or reason using regrouping; state true or false.
Step 5: Justify the match with counters, a double ten-frame, or mental grouping.",
            @"Do not treat = as an instruction to calculate only the left side. For 6+5+4=15+10, 6+4=10, so the left side is 15, NOT 25. Do not drop an addend when regrouping.",
            @"I can match three-addend expressions to 10+n expressions, and decide if both sides of an addition equation have equal values."),
        new(@"PED:US-CCSS-MATH:G1:U03:L17",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-17/preparation.html",@"f43bed16aeb8103c1cabc10b867c387b2fdb2a8482133ab1c9dac461d9553837",new[] { @"CCSS:1.OA.A.1", @"CCSS:1.OA.B.3", @"CCSS:1.OA.C.6" },
            @"Make 10 to Add PLC Activity",
            @"Use the make-ten strategy to add within 20. When one addend is near ten, move just enough counters from the other addend to complete ten, then add the leftovers. Represent the original addition and the equivalent 10+n expression; a 9-plus calculation is a special case, not a universal rule.",
            @"Learning goals: make a ten when adding two numbers within 20, analyze and explain equivalent methods. If the first addend is 9, take 1 from the second. If it is 8, take 2. Record equality between expressions, e.g. 9+5=10+4. Ten-frame manipulations and verbal explanations are meaningful even when not automatically graded.",
            @"Example 1: 9 + 5. Move 1 from the 5 to the 9: 9 + 5 = 10 + 4 = 14.
Example 2: 8 + 6. Move 2 from the 6 to the 8: 8 + 6 = 10 + 4 = 14.
Example 3: 7 + 5. Move 3 from the 5 to the 7: 7 + 5 = 10 + 2 = 12.
Example 4: 9 + 7 = 10 + 6 = 16. Both expressions have the same sum.",
            @"Step 1: Draw the first addend on a ten-frame and the second on another.
Step 2: Count how many are needed to fill the first frame to 10.
Step 3: Move that many counters FROM the second addend; do not change the total.
Step 4: Write the ORIGINAL addition and a 10+leftover expression showing their equality.
Step 5: Calculate the total within 20 and check both expressions agree.",
            @"Do not move a counter without reducing the other addend. 9+5 is not 10+5: both sides would have different totals. For 8+6 the leftover is 4 after moving 2, so 10+4, not 10+6.",
            @"I can make ten to add and use an equation to explain why my equivalent 10+n expression has the same sum."),
        new(@"PED:US-CCSS-MATH:G1:U03:L18",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-18/preparation.html",@"192643835e968a3be0239953140a275703903d9c8186362c4f85bfac50dbb6c1",new[] { @"CCSS:1.OA.C.6", @"CCSS:1.OA.D.8" },
            @"Patterns in Addition",
            @"Find and explain patterns among addition expressions with the same value. Match different two-addend expressions to 10+n by moving counters to make ten. Compare equivalent expressions and notice how raising one addend by one and lowering the other by one keeps the sum unchanged.",
            @"Learning goals: use number patterns to add within 20; make a ten to find sums; match equivalent expressions with 10+n; describe patterns between addend pairs. An equality such as 9+5=10+4 is a comparison of two equivalent expressions. The original Gallery Walk and paper posters are teacher-organized classroom activities.",
            @"Example 1: 9 + 5 = 10 + 4 because one counter moves from the group of five to fill ten.
Example 2: 8 + 6 = 9 + 5 = 10 + 4; each sum is 14.
Example 3: 3 + 9 = 10 + 2. Move one counter from 3 to the 9 to make 10.
Example 4: 9 + 4 = 8 + 5 = 7 + 6 = 13. When one addend drops by one, the other rises by one.",
            @"Step 1: Read the target 10+n expression and work out its total.
Step 2: Look at each pair of addends and decide which can be regrouped to make 10+n without changing the sum.
Step 3: Match or write several equivalent expressions.
Step 4: Compare the addends in your matched expressions and describe the pattern you notice.
Step 5: Check each proposed equation by calculating both sides or rearranging counters on a double ten-frame.",
            @"Do not assume any expression using the same digits is equivalent. For example, 8+4=12 is not equal to 10+3=13. Check both sides and keep the sum fixed while moving counters.",
            @"I can match 10+n expressions to equivalent additions and explain patterns that preserve the total."),
        new(@"PED:US-CCSS-MATH:G1:U03:L19",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-19/preparation.html",@"a133b4dd3e0a328f621dde57fbd3502262b47b6d41fa59ba3b8b8a145ffe85fc",new[] { @"CCSS:1.OA.B.3", @"CCSS:1.OA.C.6" },
            @"Methods for Addition Within 20",
            @"Add within 20 by choosing a useful method for the actual numbers. Compare counting on, making ten, using doubles, and breaking one addend into friendly parts. Different correct strategies can show the same sum. Explain why the selected method is efficient; partner discussion is led by a teacher.",
            @"The commutative property allows 8+5=5+8, and regrouping does not change a sum. For near-ten addition, 9+6 = 10+5. For a near-double, 7+8 = 7+7+1. Select a strategy based on the numbers, not just a memorized sequence of two-digit equations.",
            @"Example 1: 9+6=9+1+5=10+5=15 (make ten).
Example 2: 7+8=7+7+1=14+1=15 (near double).
Example 3: 6+8=8+2+4=10+4=14 (decompose six).
Example 4: 5+8=8+5=13. Swap the addends to count on fewer steps.",
            @"Step 1: Read the two addends and check the sum is within 20.
Step 2: Look for a ten, a double, or a known addition fact.
Step 3: Choose a method: count on, make ten, or build from a double.
Step 4: Write a correct chain of equal expressions, keeping all values equivalent.
Step 5: Confirm the total with counters or a second method and explain why your method works.",
            @"Do not write 9+6=10+6, because one must leave the six when the nine becomes ten. Do not claim every example requires making ten: near doubles may be easier for some sums.",
            @"I can choose a helpful, accurate addition strategy for two numbers within 20 and explain another possible method."),
        new(@"PED:US-CCSS-MATH:G1:U03:L20",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-20/preparation.html",@"b5be6aa0b9dc3671e4b3eb4907e6bdd1422d88405599558e2e910a37085d4779",new[] { @"CCSS:1.OA.A.2", @"CCSS:1.OA.C.6" },
            @"A Trip to the Zoo",
            @"Solve a zoo visit story by finding the total of THREE groups of animals or visitors. Read all quantities carefully, represent each group in a drawing or counters, and write a complete three-addend expression. Sometimes two groups make ten; other times count on or add a convenient pair. The source goal is three-addend story meaning, not only adding two numbers.",
            @"Learning goal: solve story problems with THREE addends flexibly, within 20. Count all three groups even if no pair makes exactly ten. You can rearrange or group addends without changing the total. Label the answer with the story unit (animals/visitors).",
            @"Example 1: At the zoo, 4 ducks, 6 geese and 3 swans swim: 4+6+3=10+3=13 birds.
Example 2: Five penguins, three seals and four otters are counted: 5+3+4=12 animals.
Example 3: The guide spots 2 lions, 7 zebras and 8 giraffes: 2+8+7=17 animals.
Example 4: Three groups contain 5, 4 and 6 visitors: 5+4+6=15 visitors.",
            @"Step 1: Read the zoo story and identify every group named.
Step 2: Write a three-addend equation, not a two-addend one.
Step 3: Draw or model the three groups with counters.
Step 4: Look for friendly pairs such as ten or a known sum, and calculate the total within 20.
Step 5: Check all three quantities were included and give an answer with a unit.",
            @"Do not ignore the third group. 5+3+4 is 12, not 8. Not every three-addend problem contains a ten-pair, so never invent one or force a fake ten.",
            @"I can solve a three-group story within 20 using a strategy that fits the numbers."),
        new(@"PED:US-CCSS-MATH:G1:U03:L21",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-21/preparation.html",@"bdda6d45824c14cdbbba931843195f9efc05abf0c4c45c174ba21cdf81fdbe29",new[] { @"CCSS:1.OA.C.5", @"CCSS:1.OA.C.6" },
            @"Center Day 3",
            @"Practice two-number and three-number addition within 20 in the How Close? and Five in a Row centers. Make a ten, count on or use known facts. Write an accurate three-addend equation for each three-group problem. Physical turn-taking, cards and winning rows remain class activities; digital Practice can check only numeric equations.",
            @"Learning goals: add two AND three numbers within 20; write equations with three addends. You may arrange addends to make ten or use a known double. The original How Close? and Five in a Row game rules, partners and materials require a real classroom.",
            @"Example 1: Three counters groups: 3+6+7=3+7+6=10+6=16.
Example 2: Two-number card: 8+5=10+3=13.
Example 3: Three-number card: 4+5+6=10+5=15.
Example 4: 2+8+9=10+9=19. Every group is counted once.",
            @"Step 1: Read the two or three visible card/group values.
Step 2: Decide whether to count on, make ten or regroup.
Step 3: For three groups, write a full equation containing three addends before simplifying.
Step 4: Solve and check the sum is no more than 20.
Step 5: Tell a partner which method you used if working in a supervised center.",
            @"Do not complete a three-addend task with a two-addend equation. For 3+6+7, adding only 3+7 gives ten but is not the final total; add the six to make sixteen.",
            @"I can add two and three numbers within 20 and write complete three-addend equations. Classroom games are teacher-led."),
        new(@"PED:US-CCSS-MATH:G1:U03:L22",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-22/preparation.html",@"ffd4ed62506326b6154d4231afe24476c91c6dd7553bb4840c317d4a43a1710d",new[] { @"CCSS:1.OA.A.1", @"CCSS:1.OA.B.4", @"CCSS:1.OA.C.5", @"CCSS:1.OA.C.6" },
            @"Subtract from Teen Numbers",
            @"Subtract a one-digit amount from a teen number, including when you must break apart a ten. Compare taking away counters with counting on from the subtracted amount to the starting amount. Use a full ten-frame and extra ones to see why removing more than the loose ones must cross ten.",
            @"Learning goal: subtract within 20 in a way that makes mathematical sense. For 14-6, remove four ones to reach ten, then remove two more to reach eight. Or count from six to fourteen: 6+8=14. Distinguish subtracting without breaking a ten (e.g., 17-4=13) from crossing ten (14-6=8).",
            @"Example 1: 14-6: first 14-4=10, then 10-2=8, so 14-6=8.
Example 2: 12-5: remove two to reach ten and three more to reach seven, so 12-5=7.
Example 3: 17-4=13: remove four ones; the full ten stays intact.
Example 4: 13-8=5 because 8+5=13. Count on to check.",
            @"Step 1: Show the teen starting value as ten and some ones.
Step 2: Find how many counters to remove and whether the loose ones are enough.
Step 3: If needed, subtract to ten first, then remove the remaining amount.
Step 4: Write accurate equations for each stage and find the final result.
Step 5: Verify with a related addition equation.",
            @"Do not pretend crossing ten is the same as removing only loose ones. 14-6 is not 14-4=10 as the final answer; two further counters must be removed. When using a chain, keep both sides equal: 14-6=14-4-2=8.",
            @"I can subtract from teen numbers by taking away, counting on, or using the structure of ten."),
        new(@"PED:US-CCSS-MATH:G1:U03:L23",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-23/preparation.html",@"56d2765a92f9b568461f2e650752f6cb7b95f275e1d8efe4a11a5e734bf3276f",new[] { @"CCSS:1.OA.C.6" },
            @"Use a Ten to Subtract",
            @"Use a full ten-frame as a landmark for subtraction within 20. Move down to ten first, then subtract the rest. Show each step with true equations. Compare this method with counting on to verify the difference, and explain why a subtraction across ten needs more than one removal.",
            @"Learning goal: use one ten to find differences within 20. For 13-6, split six into three and three, then 13-3-3=7. Another check: 6+7=13. Note the incorrect equality 13-3=10-3=7; 13-3 is 10 and 10-3 is 7, so the chain falsely equates different values.",
            @"Example 1: 13-6=13-3-3=10-3=7.
Example 2: 15-8=15-5-3=10-3=7.
Example 3: 12-7=12-2-5=10-5=5.
Example 4: 16-9=16-6-3=10-3=7.",
            @"Step 1: Show the first number as ten and some ones on a double ten-frame.
Step 2: Find how many ones to subtract to reach exactly ten.
Step 3: Split the amount being subtracted into that part and the remaining part.
Step 4: Subtract the remaining part from ten; keep every written equality true.
Step 5: Check with addition: the result plus the removed amount must equal the original teen number.",
            @"Do not write 13-3=10-3=7; 13-3 equals 10, not seven. Instead write 13-6=13-3-3=10-3=7. Do not remove the ten twice or assume a one-digit answer without checking.",
            @"I can use the group of ten to subtract and explain accurate intermediate steps within 20.")
    ];
    private static Repair? Find(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson) =>
        doc.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode &&
        doc.VersionCode == "CCSSM-2010"
        ? Repairs.SingleOrDefault(r=>r.Code==lesson.LessonCode) : null;
    public static bool IsTarget(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson)=>Find(doc,lesson)!=null;
    private static Repair Checked(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson)
    {
        var r=Find(doc,lesson) ?? throw new InvalidOperationException("Not a G1 full-grade start repair");
        if (lesson.SourceUrl!=r.Url ||
            !string.Equals(lesson.CanonicalBodySha256,r.Sha,StringComparison.OrdinalIgnoreCase)||
            !lesson.OutcomeCodes.SequenceEqual(r.Outcomes))
            throw new InvalidOperationException("G1 full-grade L51-L55 source/body/outcome drift: "+lesson.LessonCode);
        return r;
    }
    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        foreach(var lesson in doc.Lessons)
        {
            if(!IsTarget(doc,lesson)) continue;
            var r=Checked(doc,lesson);
            var en=lesson.Translations.Single(x=>x.CultureCode=="en");
            if(en.Title==r.Title&&en.Explanation==r.Explanation&&
                en.KeyConceptsAndRules==r.Concepts&&en.WorkedExamples==r.Examples&&
                en.StepByStepSolutions==r.Steps&&en.CommonMistakes==r.Mistakes&&
                en.QuickSummary==r.Summary) continue;
            if(!en.WorkedExamples.StartsWith("Example 1:",StringComparison.Ordinal)||
                !en.StepByStepSolutions.StartsWith("Step 1:",StringComparison.Ordinal))
                throw new InvalidOperationException("G1 full-grade L51-L55 unexpected pre-repair body");
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
        Checked(doc,lesson);
        if(!CanUpgradeExisting(doc,lesson,previous))
            throw new InvalidOperationException("G1 full-grade L51-L55 persisted predecessor drift");
        if(translations.Count==0)return; // JSON-only production storage
        if(translations.Count!=1 || translations.Single().CultureCode!="en" ||
           !translations.Single().WorkedExamples.StartsWith("Example 1:",StringComparison.Ordinal)||
           !translations.Single().StepByStepSolutions.StartsWith("Step 1:",StringComparison.Ordinal))
            throw new InvalidOperationException("G1 full-grade L51-L55 persisted language/body drift");
    }
}
