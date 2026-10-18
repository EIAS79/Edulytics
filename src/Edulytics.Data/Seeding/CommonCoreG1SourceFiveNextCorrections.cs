using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>Source-reviewed first-edition IM/Kendall Hunt 2021 G1 ordinal
/// lessons 41–45. The student activity PDF and local teacher pages are
/// independently referenced; no publisher diagram pixel parity is claimed.</summary>
public static class CommonCoreG1SourceFiveNextCorrections
{
    public const string Version="us-ccss-g1-009-u03-l04-l08-original-2021-source-v1";
    private sealed record Repair(string Code,string Url,string Sha,string[] Outcomes,
        string Title,string Explanation,string Concepts,string Examples,string Steps,
        string Mistakes,string Summary);

    private static readonly Repair[] Repairs=
    [
        new("PED:US-CCSS-MATH:G1:U03:L04",
            "https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-4/preparation.html",
            "204c4950598a8d1099c5bf4756f729e9d095ff1bdeee199469f1abfeac4f4e04",
            ["CCSS:1.OA.B.3","CCSS:1.OA.C.5","CCSS:1.OA.C.6","CCSS:1.OA.D.7","CCSS:1.OA.D.8"],
            "Sums of 10",
            "How many ways can you split TEN counters into two groups? Use a ten-frame with red and yellow counters to show pairs whose sum is 10. Turn one red counter yellow at a time. Record a different equation each time, and explain why you found every combination. A classmate or teacher can listen to your argument.",
            "Goal: find ALL the ways to make 10, understand that reversed addends make the same total, and look for a systematic pattern. A complete ordered list runs from 0+10=10 to 10+0=10: eleven ordered expressions and six unordered pairs. Show ten using two-color counters or a ten-frame. A physical Shake-and-Spill center is teacher-facilitated.",
            "Example 1: Ten counters show 6 red and 4 yellow, so 6+4=10 and 4+6=10.\nExample 2: Change one red counter to yellow: 6+4=10, then 5+5=10, then 4+6=10. The total stays ten.\nExample 3: To prove you have ALL the ordered ways, start at 0 red and 10 yellow and change exactly one at a time until 10 red and 0 yellow. No value from 0 through 10 has been skipped.",
            "Step 1: Lay out ten counters or fill a ten-frame.\nStep 2: Choose the number of red counters and count the yellow ones.\nStep 3: Write a complete equation with a total of ten.\nStep 4: Change exactly one counter from yellow to red and record the next pair.\nStep 5: Continue until you reach ten red and zero yellow; explain why each red count 0 through 10 is present exactly once.",
            "Do not omit 0+10 or 10+0 when claiming all ordered ways. Reversing addends does not create a new unordered pair, although it is a different ordered expression. The warm-up asks if an equation is true; it does not replace the all-ways task.",
            "I can show all the decompositions of ten with counters, write true equations and explain my systematic method."),
        new("PED:US-CCSS-MATH:G1:U03:L05",
            "https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-5/preparation.html",
            "e8d1c99ea297888a428c2ce8204a2e60cd31e19445a59c04c141b8c41b3ef70e",
            ["CCSS:1.OA.B.4","CCSS:1.OA.C.5","CCSS:1.OA.C.6","CCSS:1.OA.D.8"],
            "Find the Difference",
            "Subtraction within 10 can be solved by taking away, counting on from the smaller number, or remembering a related addition fact. Each approach must agree. Then compare a string of subtraction equations: when you take away more from the same starting number, fewer are left.",
            "Goal: use the relationship between addition and subtraction to find differences within ten; explain three strategies and analyze a subtraction number-string pattern. Write a missing-value fact and its inverse addition fact; concrete cubes support the explanation.",
            "Example 1: 10−6=4. Take six counters away from ten and four remain.\nExample 2: Count on from 6: 7, 8, 9, 10. Four steps, so 10−6=4 and 6+4=10.\nExample 3: In 6−1=5, 6−2=4, 6−3=3, 6−4=2, the difference decreases by one whenever one more is subtracted.",
            "Step 1: Read the starting number and the number to take away.\nStep 2: Model take-away with counters, count on, or recall the addition fact.\nStep 3: Record the correct difference and check that subtrahend+difference=starting number.\nStep 4: For a number string, calculate each difference in order.\nStep 5: Compare answers and explain how changing the number subtracted changes the difference.",
            "Counting on from six to ten is four steps, not ten or six. With a fixed starting number, subtracting more gives a smaller difference, not a larger one. Never switch the subtraction operands without changing the problem.",
            "I can use addition to find a difference and explain the pattern in related subtraction facts."),
        new("PED:US-CCSS-MATH:G1:U03:L06",
            "https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-6/preparation.html",
            "e2c0628cfcd071e0763800268579daee4dd410ed52cf64613c0d791c8d8d7ca9",
            ["CCSS:1.OA.A.1","CCSS:1.OA.C.5","CCSS:1.OA.C.6"],
            "Story Problems within 10 PLC Activity",
            "Some addition stories tell the amount added and the final total but not the beginning. In an Add To, Start Unknown story, draw a box BEFORE the plus sign: □+3=10. Find the starting number by counting on, subtraction or a known addition fact. Next solve different Add To, Take From and Put Together stories with unknowns in different positions.",
            "Goal: solve Add To and Put Together story problems with the unknown in all positions, including a NEW Add To, Start Unknown case. Match each quantity to the correct place in an equation. Some stories ask for the start, others the change, the result or one part. Explain diagrams and different solution strategies to a teacher or classmate.",
            "Example 1: A cup held some counters. Three more were added, and there are ten now: □+3=10, so the start is 7. Check 7+3=10.\nExample 2: A cup held four counters, and more were added until there were seven: 4+□=7, so three were added.\nExample 3: Ten counters were in a cup; four fell out: 10−4=6 remain.\nExample 4: Two red and eight yellow counters are ten altogether: 2+8=10.",
            "Step 1: Read the story and name the start, change and final quantity (or the two parts and whole).\nStep 2: Identify which amount is UNKNOWN and place □ in that position, even at the start.\nStep 3: Draw counters or a ten-frame matching the story and write its equation.\nStep 4: Solve by counting on, subtraction or a known fact; write the answer for the question.\nStep 5: Check the answer in the ORIGINAL story and compare the start-unknown story with a change-unknown story.",
            "Do not assume the blank is always the result. In □+3=10, 3 is the change and ten the ending total: the start is seven, not thirteen. Taking away and putting together describe different relationships.",
            "I can solve stories within ten when any part is missing and write the equation matching the story."),
        new("PED:US-CCSS-MATH:G1:U03:L07",
            "https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-7/preparation.html",
            "0927f7600983dd94440646ecf86925d1f7fb5b90eefee62f67e866677cfde711",
            ["CCSS:1.OA.C.6","CCSS:1.OA.D.8"],
            "Center Day 1",
            "Practice addition and subtraction within ten with the new Compare center. Two players each flip an expression card. Find each value: the larger value wins both cards. If both values are equal, it is a tie, and the players flip more cards. Then choose from Number Puzzles, Find the Pair and Compare. Digital practice tests the mathematics; actual turn taking and collaboration remain classroom work.",
            "Goal: calculate and COMPARE values of addition and subtraction expressions within 10; recognize expressions with equal values and use = correctly. Model the Find the Pair center by finding a missing part that makes ten. The choral counting from 40 to 62 is a warm-up toward future number sense, not a substitute for addition practice.",
            "Example 1: First card 7−2=5 and second card 2+3=5. This is a tie because 7−2=2+3.\nExample 2: First card 9−2=7 and second card 3+2=5, so the first card has the greater value.\nExample 3: In Find the Pair, a card showing six pairs with four because 6+4=10.",
            "Step 1: Flip or read two expression cards.\nStep 2: Calculate the value of EACH card using counters or number facts.\nStep 3: Decide which is greater, or explain why the values are equal.\nStep 4: Check the comparison by writing the values or a true equal-expression equation.\nStep 5: Choose another familiar center and practice addition or subtraction within ten; share what improved with a partner.",
            "Do not choose a card just because it contains a larger written number: evaluate the whole expression. Equal-value cards tie, even if their addition and subtraction signs are different.",
            "I can find the value of two cards, compare their sums or differences and explain an equal-value tie."),
        new("PED:US-CCSS-MATH:G1:U03:L08",
            "https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-8/preparation.html",
            "1fe57481c2dde70456184c216e7ac2fa9d5a2d642256ebd27ffb214dfaa2b219",
            ["CCSS:1.NBT.B.2"],
            "Ten as a Unit",
            "Ten single connecting cubes can be bundled to make ONE unit called a ten. A teen number is one ten and some extra ones. Count a collection of sixteen cubes and represent it first as individual ones and then as a group of ten with six left over. Use a ten-frame, a connecting-cube tower or a drawing to show the same quantity. Next choose four numbers from 10 to 19 and build each with one ten.",
            "Goal: understand 10 ones as a unit called ONE ten; compose and decompose teen numbers as 1 ten and 0 to 9 ones. Ten is a unit, not ten tens. A classroom partner explains and compares representations, while digital Practice checks the unit counts.",
            "Example 1: Ten ones connected into a tower are ONE ten, not one hundred.\nExample 2: Sixteen cubes can be shown as 16 separate ones, or a tower of 10 and 6 single cubes: 16=10+6.\nExample 3: Fourteen is one group of ten and four ones. Nineteen is one group of ten and nine ones.",
            "Step 1: Count the original objects, making sure none is missed or counted twice.\nStep 2: Group EXACTLY ten of the objects; name the bundle ONE ten.\nStep 3: Count all leftover single ones.\nStep 4: Represent the count as one ten and some ones with a labeled drawing, cubes or a ten-frame.\nStep 5: Choose four numbers from 10 through 19, show the same ten-group structure, and tell a partner what stayed the same.",
            "One ten is ten ones, not ten tens. Do not write 16 as sixteen tens or as 1 ten and sixteen ones. A teen number from 10 to 19 contains exactly one ten and 0–9 extra ones.",
            "I can bundle ten ones, show a teen number as one ten and some ones, and explain the representation.")
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
        var r=Find(doc,lesson)??throw new InvalidOperationException("Not G1-009");
        if(lesson.SourceUrl!=r.Url||
           !string.Equals(lesson.CanonicalBodySha256,r.Sha,StringComparison.OrdinalIgnoreCase)||
           !lesson.OutcomeCodes.SequenceEqual(r.Outcomes))
            throw new InvalidOperationException("V1 G1-009 original source/outcome/identity drift: "+lesson.LessonCode);
        return r;
    }
    private static bool Same(CanonicalLessonContentPackTranslation en,Repair r)=>
        en.Title==r.Title&&en.Explanation==r.Explanation&&
        en.KeyConceptsAndRules==r.Concepts&&en.WorkedExamples==r.Examples&&
        en.StepByStepSolutions==r.Steps&&en.CommonMistakes==r.Mistakes&&
        en.QuickSummary==r.Summary;
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
                throw new InvalidOperationException("Unexpected G1-009 pre-repair source body: "+lesson.LessonCode);
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
            throw new InvalidOperationException("G1-009 persisted predecessor drift");
        if(translations.Count==0)return; // live JSON-only content storage
        if(translations.Count!=1)throw new InvalidOperationException("G1-009 culture drift");
        var en=translations.Single();
        if(en.CultureCode!="en"||
           !en.WorkedExamples.StartsWith("Example 1:",StringComparison.Ordinal)||
           !en.StepByStepSolutions.StartsWith("Step 1:",StringComparison.Ordinal))
            throw new InvalidOperationException("G1-009 persisted text drift");
    }
}
