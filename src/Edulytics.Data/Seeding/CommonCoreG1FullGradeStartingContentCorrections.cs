using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>Source-goal grounded learner-text corrections for G1 ordinals 51–55.
/// Teaching body only: this does NOT independently certify practice or classroom activities.</summary>
public static class CommonCoreG1FullGradeStartingContentCorrections
{
    public const string Version="us-ccss-g1-full-grade-u03-l14-l18-content-source-v1";
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
            @"I can match 10+n expressions to equivalent additions and explain patterns that preserve the total.")
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
