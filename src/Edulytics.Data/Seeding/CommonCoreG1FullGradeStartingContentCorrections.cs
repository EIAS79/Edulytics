using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>Source-goal grounded learner-text corrections for G1 ordinals 51–80.
/// Teaching body only: this does NOT independently certify practice or classroom activities.</summary>
public static class CommonCoreG1FullGradeStartingContentCorrections
{
    public const string Version="us-ccss-g1-full-grade-u03-l14-to-u04-l15-source-v5";
    private const string ZooLessonCode="PED:US-CCSS-MATH:G1:U03:L20";
    public const string ZooSourceContentVersion="us-ccss-g1-u03-l20-zoo-2021-source-v8";
    private const string CenterLessonCode="PED:US-CCSS-MATH:G1:U03:L21";
    private const string TeenSubtractLessonCode="PED:US-CCSS-MATH:G1:U03:L22";
    private const string TenSubtractLessonCode="PED:US-CCSS-MATH:G1:U03:L23";
    public const string CenterSourceContentVersion="us-ccss-g1-u03-l21-centers-2021-source-v7";
    public const string TeenSubtractSourceContentVersion="us-ccss-g1-u03-l22-teen-2021-source-v7";
    public const string TenSubtractSourceContentVersion="us-ccss-g1-u03-l23-ten-2021-source-v8";
    private const string CountRelationLessonCode="PED:US-CCSS-MATH:G1:U03:L24";
    private const string ChooseSubtractLessonCode="PED:US-CCSS-MATH:G1:U03:L25";
    private const string StoryLessonCode="PED:US-CCSS-MATH:G1:U03:L26";
    private const string CenterFourLessonCode="PED:US-CCSS-MATH:G1:U03:L27";
    public const string CountRelationSourceContentVersion="us-ccss-g1-u03-l24-original-activities-v7";
    public const string ChooseSubtractSourceContentVersion="us-ccss-g1-u03-l25-original-activities-v7";
    public const string StorySourceContentVersion="us-ccss-g1-u03-l26-original-activities-v7";
    public const string CenterFourSourceContentVersion="us-ccss-g1-u03-l27-original-activities-v6";
    private const string UnitFourCollectionLessonCode="PED:US-CCSS-MATH:G1:U04:L01";
    private const string UnitFourTensMatchLessonCode="PED:US-CCSS-MATH:G1:U04:L02";
    private const string UnitFourMultipleTensLessonCode="PED:US-CCSS-MATH:G1:U04:L04";
    private const string UnitFourCenterLessonCode="PED:US-CCSS-MATH:G1:U04:L05";
    public const string UnitFourCollectionSourceContentVersion="us-ccss-g1-u04-l01-original-activities-v6";
    public const string UnitFourTensMatchSourceContentVersion="us-ccss-g1-u04-l02-original-activities-v6";
    public const string UnitFourMultipleTensSourceContentVersion="us-ccss-g1-u04-l04-original-activities-v6";
    public const string UnitFourCenterSourceContentVersion="us-ccss-g1-u04-l05-original-activities-v6";
    private static readonly HashSet<string> UnitFourLaterLessonCodes =
        Enumerable.Range(6,10)
            .Select(n=>$"PED:US-CCSS-MATH:G1:U04:L{n:00}")
            .ToHashSet(StringComparer.Ordinal);
    public const string UnitFourLaterSourceContentVersion="us-ccss-g1-u04-l06-l15-original-student-activities-v6";
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
            @"Solve a story with three groups of objects, where TWO of the addends combine to ten. Instead of adding only the first two numbers, identify all three groups, choose a pair that makes ten, then add the remaining number. Draw counters or a double ten-frame and write a three-addend equation. The original 2021 student activity ALSO asks learners to WRITE THEIR OWN bird story with 9 birds, 8 birds, and 1 bird; students author the story and show a matching equation, reviewed by the teacher rather than fabricated by digital Practice.",
            @"Learning goal: solve story problems within 20 involving THREE addends, two making 10. The order and grouping of addends may change while the sum stays the same. For example, 3 + 8 + 7 = (3 + 7) + 8 = 10 + 8 = 18. Explain the story and write a matching equation; a two-addend total alone does not establish the goal.",
            @"Example 1: Three groups of birds contain 3, 8, and 7 birds. 3 and 7 make ten, so 3 + 8 + 7 = 10 + 8 = 18 birds.
Example 2: A card collection has piles of 5, 9, and 5 cards. Combine 5 + 5 first to make 10, then 10 + 9 = 19 cards.
Example 3: Three jars have 4, 6, and 7 beads. 4 + 6 = 10; 10 + 7 = 17 beads.",
            @"Step 1: Read the whole story and name ALL THREE quantities.
Step 2: Write an equation containing three addends, including the unknown total.
Step 3: Find a pair summing to 10, even if the pair is separated by another addend.
Step 4: Regroup as 10 plus the leftover amount, then compute the sum (at most 20).
Step 5: Check that the total and unit answer the original story; explain with a double ten-frame or drawing.
Step 6: ORIGINAL STUDENT-AUTHORED TASK: write your own three-group bird story using 9 birds, 8 birds and 1 bird, draw or describe all groups and write a matching equation. A teacher reviews the student's actual wording and representation; auto-graded Practice does not claim the pupil wrote a story.",
            @"Do not ignore the third group, or assume only neighboring addends can form ten. 3+8+7 is 18, not 11; 3+7=10 and 10+8=18. Adding two numbers in isolation does not represent a three-part story.",
            @"I can write and solve three-addend story equations by pairing two groups into ten and then adding the remaining group. I can also write my own three-group bird story and show its equation; this authored classroom task needs teacher review."),
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
            @"Learning goal: solve story problems with THREE addends flexibly, within 20. ORIGINAL student-facing reptile story: Jada saw 8 snakes, 7 iguanas and 5 frogs, for a total of 20 animals across all three groups. Count all three groups even if no pair makes exactly ten. You can rearrange or group addends without changing the total. Label the answer with the story unit (animals/visitors).",
            @"Example 1: At the zoo, 4 ducks, 6 geese and 3 swans swim: 4+6+3=10+3=13 birds.
Example 2: Five penguins, three seals and four otters are counted: 5+3+4=12 animals.
Example 3: The guide spots 2 lions, 7 zebras and 8 giraffes: 2+8+7=17 animals.
Example 4: Three groups contain 5, 4 and 6 visitors: 5+4+6=15 visitors.
Original 2021 mental warm-up: 8+2+4=14 and 8+6=14; 7+3+6=16 and 7+9=16. Each pair has the same value: splitting an addend or regrouping does not change the total.
Original 2021 reptile story: Jada sees 8 snakes, 7 iguanas and 5 frogs: 8+7+5=20 animals across THREE groups. One valid method splits 7 into 2 and 5: 8+2+5+5=20. A double ten-frame also shows twenty counters.
Original 2021 zoo bird exhibit: 3 herons, 6 hawks and 7 hummingbirds: 3+7+6=10+6=16 birds.
Original 2021 large cat exhibit: 8 lions, 4 tigers and 3 cheetahs: 8+4+3=15 large cats.
Original 2021 petting zoo: 8 goats, 7 sheep and 4 pigs: 8+7+4=19 animals. These original source quantities are represented with first-party teaching text rather than copied publisher illustrations.
Original 2021 STUDENT SECTION SUMMARY: make ten with THREE addends: 4+8+6=4+6+8=10+8=18. A first-party textual double ten-frame explanation places four red and six yellow counters together in one filled frame, with eight blue counters in the other, representing 18 counters total. Make ten with TWO addends: 4+8=4+6+2=10+2=12. In a first-party textual frame, four and six fill one ten and two remain. These are mathematics descriptions, not the original publisher artwork; a student can draw the counter frames and discuss with a teacher.
Original 2021 Kendall Hunt primate cool-down (the exact student task is reproduced in the 2021 EdReports review): Jada sees 8 monkeys, 4 gorillas, and 7 orangutans. 8+4+7=8+2+2+7=10+9=19 primates. Explain with drawings, numbers, or words. The original cached 2021 teacher lesson confirms the five-minute primates cool-down title, and the 2021 EdReports Kendall Hunt Grade 1 review independently transcribes the primates and all three quantities. The actual 2021 login-protected cool-down attachment was not opened; no source illustration was copied.",
            @"Step 1: Read the zoo story and identify every group named.
Step 2: Write a three-addend equation, not a two-addend one.
Step 3: Draw or model the three groups with counters.
Step 4: Look for friendly pairs such as ten or a known sum, and calculate the total within 20.
Step 5: Check all three quantities were included and give an answer with a unit.
Step 6: In the original zoo activities, show your thinking with drawings, numbers, or words and discuss different valid strategies. The digital equation check verifies arithmetic, not an individual pupil's drawing or classroom explanation.",
            @"Do not ignore the third group. 5+3+4 is 12, not 8. Not every three-addend problem contains a ten-pair, so never invent one or force a fake ten.",
            @"I can solve a three-group story within 20 using a strategy that fits the numbers."),
        new(@"PED:US-CCSS-MATH:G1:U03:L21",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-21/preparation.html",@"bdda6d45824c14cdbbba931843195f9efc05abf0c4c45c174ba21cdf81fdbe29",new[] { @"CCSS:1.OA.C.5", @"CCSS:1.OA.C.6" },
            @"Center Day 3",
            @"Practice two-number and three-number addition within 20 in the How Close? and Five in a Row centers. Make a ten, count on or use known facts. Write an accurate three-addend equation for each three-group problem. Physical turn-taking, cards and winning rows remain class activities; digital Practice can check only numeric equations.",
            @"Learning goals: add two AND three numbers within 20; write equations with three addends. You may arrange addends to make ten or use a known double. The original How Close? and Five in a Row game rules, partners and materials require a real classroom. The original 2021 warm-up also asks What do you know about 20? Learners may show 20 as two filled ten-frames, 10+10, 13+7, or twenty counters, and discuss how representations are connected; this open response remains teacher-facilitated.",
            @"Example 1: Three counters groups: 3+6+7=3+7+6=10+6=16.
Example 2: Two-number card: 8+5=10+3=13.
Example 3: Three-number card: 4+5+6=10+5=15.
Example 4: 2+8+9=10+9=19. Every group is counted once.
Original 2021 warm-up representation: 10+10=13+7=20. Two full ten-frames also represent 20; pupils can suggest and explain other mathematically correct representations.",
            @"Step 1: Read the two or three visible card/group values.
Step 2: Decide whether to count on, make ten or regroup.
Step 3: For three groups, write a full equation containing three addends before simplifying.
Step 4: Solve and check the sum is no more than 20.
Step 5: Tell a partner which method you used if working in a supervised center.
Original source HOW CLOSE? center (teacher-led): the number-card set is 0 through 10; remove any ten-value cards so the permitted cards are 0 through 9; display five numeral cards; select two or three cards and write a true addition equation whose sum is nearest to 20. Compare how close the sums are, replenish the hand to five cards, and award a point to the closer result. The game winner has more round points.
Original source FIVE IN A ROW center (teacher-led): pick a number card from the actual 2021 0-through-10 set (no 11 card), choose to add 7, 8 or 9, calculate the sum, and place a marker on that sum on the gameboard. The first partner with five markers consecutively in a row wins. This digital Practice tests the underlying arithmetic ONLY; it does not claim students physically played a card round, followed partner rules, or won a board.",
            @"Do not complete a three-addend task with a two-addend equation. For 3+6+7, adding only 3+7 gives ten but is not the final total; add the six to make sixteen.",
            @"I can add two and three numbers within 20 and write complete three-addend equations. Classroom games are teacher-led."),
        new(@"PED:US-CCSS-MATH:G1:U03:L22",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-22/preparation.html",@"ffd4ed62506326b6154d4231afe24476c91c6dd7553bb4840c317d4a43a1710d",new[] { @"CCSS:1.OA.A.1", @"CCSS:1.OA.B.4", @"CCSS:1.OA.C.5", @"CCSS:1.OA.C.6" },
            @"Subtract from Teen Numbers",
            @"Subtract a one-digit amount from a teen number, including when you must break apart a ten. Compare taking away counters with counting on from the subtracted amount to the starting amount. Use a full ten-frame and extra ones to see why removing more than the loose ones must cross ten.",
            @"Learning goal: subtract within 20 in a way that makes mathematical sense. For 14-6, remove four ones to reach ten, then remove two more to reach eight. Or count from six to fourteen: 6+8=14. Distinguish subtracting without breaking a ten (e.g., 17-4=13) from crossing ten (14-6=8). In the original 2021 source warm-up, 14-4=10 helps solve 14-5=9 and 17-7=10 helps solve 17-9=8. The original source story also asks how many crayons Elena has left after giving 7 away from 16.",
            @"Example 1: 14-6: first 14-4=10, then 10-2=8, so 14-6=8.
Example 2: 12-5: remove two to reach ten and three more to reach seven, so 12-5=7.
Example 3: 17-4=13: remove four ones; the full ten stays intact.
Example 4: 13-8=5 because 8+5=13. Count on to check.
Original 2021 Student Facing: Elena has 16 crayons and gives 7 to Diego. She has 16-7=9 crayons left. One method uses 16-6=10, then 10-1=9; another counts from 7 up to 16. The original Number Card Subtraction uses teen cards 11–19 and subtractor cards 0–9 (the material deck includes 11–20, but the teacher expressly instructs students to choose a teen number). Some legal rounds never cross ten, for example 17-4=13 and 19-0=19; never invent a negative remainder for a cross-ten method. A related addition fact, including an addition equation with a missing addend, may be used to explain the result. The classroom poster and explanation are not automatically verified by numeric Practice.",
            @"Step 1: Show the teen starting value as ten and some ones.
Step 2: Find how many counters to remove and whether the loose ones are enough.
Step 3: If needed, subtract to ten first, then remove the remaining amount.
Step 4: Write accurate equations for each stage and find the final result.
Step 5: Verify with a related addition equation.
Step 6: For the original Elena story, represent the 16 crayons and 7 given away, show your own thinking with drawings, numbers, or words, and explain the method to a teacher or partner.",
            @"Do not pretend crossing ten is the same as removing only loose ones. 14-6 is not 14-4=10 as the final answer; two further counters must be removed. When using a chain, keep both sides equal: 14-6=14-4-2=8.",
            @"I can subtract from teen numbers by taking away, counting on, or using the structure of ten."),
        new(@"PED:US-CCSS-MATH:G1:U03:L23",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-23/preparation.html",@"56d2765a92f9b568461f2e650752f6cb7b95f275e1d8efe4a11a5e734bf3276f",new[] { @"CCSS:1.OA.C.6" },
            @"Use a Ten to Subtract",
            @"Use a full ten-frame as a landmark for subtraction within 20. When the amount removed crosses ten, move down to ten first then subtract the rest. When the available extra ones are sufficient, subtract only from the ones without inventing a negative remainder. Show each step with true equations; compare Diego's and Andre's ten-frame methods and counting on.",
            @"Learning goal: use one ten to find differences within 20. For 13-6, split six into three and three, then 13-3-3=7. Another check: 6+7=13. Note the incorrect equality 13-3=10-3=7; 13-3 is 10 and 10-3 is 7, so the chain falsely equates different values. Original 2021 students compare Diego's and Andre's different ten-frame methods for 15-8 and then solve 14-5 and 13-6 by a chosen valid method; drawing and method discussion require teacher review.",
            @"Example 1: 13-6=13-3-3=10-3=7.
Example 2: 15-8=15-5-3=10-3=7.
Example 3: 12-7=12-2-5=10-5=5.
Example 4: 16-9=16-6-3=10-3=7.
Original 2021 mental warm-up: 17-7=10, 17-7-1=9, 17-8=9, and 17-9=8.
Original 2021 Diego and Andre task: 15-8=7; Diego uses 15-5=10 then 10-3=7. Andre takes 8 from the full ten, leaving 2, then adds the 5 extra ones, making 7. Additional student tasks: 14-5=9 and 13-6=7, with drawings, numbers, or words to explain the chosen method.
The original Number Card Subtraction with double ten-frames reuses cards from the previous lesson: a chosen teen card 11–19, with a digit subtractor 0–9. For 17-4=13, only loose ones are removed; for 19-0=19, none are removed. These legal source-card rounds must not be forced into a cross-ten calculation.",
            @"Step 1: Show the first number as ten and some ones on a double ten-frame.
Step 2: Compare the number being subtracted with the available ones. If it fits, remove only those ones; crossing ten is not required.
Step 3: If it does not fit, remove those extra ones to reach ten, then subtract the remainder from ten.
Step 4: Show accurate equations for the chosen method, whether counting on, subtracting from the full ten, or splitting at ten; keep every written equality true.
Step 5: Check with addition: the result plus the removed amount must equal the original teen number.
Step 6: EDULYTICS ORIGINAL COOL-DOWN (independently authored, not a reconstruction of the protected 2021 publisher assessment): You have 14 counters. Remove 8, show how you reach ten and subtract the rest. Write the result and explain your strategy with a ten-frame or accurate intermediate equations. Digital Practice checks the two stages TO10 and REST as well as the difference; handwriting and personal explanations require teacher review.",
            @"Do not write 13-3=10-3=7; 13-3 equals 10, not seven. Instead write 13-6=13-3-3=10-3=7. Do not remove the ten twice or assume a one-digit answer without checking.",
            @"I can use the group of ten to subtract and explain accurate intermediate steps within 20."),
        new(@"PED:US-CCSS-MATH:G1:U03:L24",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-24/preparation.html",@"3fd5d2c854c79d76d2678582f6cf6d3285a8004ed7e608a62fe54d2777be9761",new[] { @"CCSS:1.NBT.A.1", @"CCSS:1.OA.B.4", @"CCSS:1.OA.C.5", @"CCSS:1.OA.C.6", @"CCSS:1.OA.D.8" },
            @"Relate Counting to Addition and Subtraction",
            @"Compare subtracting by counting ON from the smaller number and taking AWAY from the larger number; explain which route is shorter. Students compare each actual subtraction by counting ON and taking AWAY; the original lesson involves teacher-observed partner discussion and does not automatically grade pupil explanations.",
            @"Learning goal: analyze and use counting-on and taking-away strategies to subtract within 20. For 15-8, one strategy takes away 5 to reach 10, then 3 more; another counts on 2 to reach 10 and 5 more to reach 15. Both produce 7, though their steps differ. Original 2021 choral warm-up counts BACKWARD by one from 50 to 20, noticing place-value patterns. Original Diego-versus-Tyler tasks include 16-3, 17-8 and 18-15 using both count-on and take-away methods.",
            @"Example 1: 15-8=7. Take away: 15-5=10, 10-3=7. Count on: 8+2=10, 10+5=15, and 2+5=7.
Example 2: 13-9=4. Count on 9+1=10, then 10+3=13, so 1+3=4.
Example 3: 18-6=12; removing six ones is direct and does not need crossing ten.
Original 2021 Different Ways to Subtract: 16-3=13, 17-8=9 and 18-15=3. For 18-15, take away fifteen to leave three, or count on 15+3=18.
Original 2021 Set A missing values: 12-7=5; 18-5=13; 14-6=8; 15-12=3; 13-4=9.
Original 2021 Set B missing addends: 5+13=18; 13=7+6; 20=15+5. These answers are worked-example explanations; original tasks retain blanks for students.
Original Set A has the larger subtractor 15-12=3 and reversed equality □=14-6; original Set B keeps three DIFFERENT blank positions: 5+□=18, 13=□+6, 20=15+□. Edulytics digitally checks the missing values as well as each full equation.",
            @"Step 1: Identify starting amount, removed amount and difference.
Step 2: Solve by physically taking away or by subtracting in stages.
Step 3: Solve again by counting ON from the smaller number until you reach the start.
Step 4: Check both methods give the same difference; discuss which is easier for these numbers.
Step 5: Use a true equation and a drawing, not a false chain.
Step 6: ORIGINAL Set A: fill 12-7=□, 18-5=□, □=14-6, 15-12=□, 13-4=□. ORIGINAL Set B: fill 5+□=18, 13=□+6, 20=15+□. Explain how the selected strategy makes each equation true; do not pre-fill pupil blanks.
Step 7: Compare Diego's take-away and Tyler's count-on methods for 16-3, 17-8 and 18-15 using teacher-supported drawings, numbers or words.
Step 8: Review all eight original Set A and Set B equation structures and verify the missing-value position. The original Unit 3 Section D Checkpoint is a teacher observation checklist, not a five-minute copied pupil Cool-down.",
            @"Do not subtract 8-15 for a 15-8 story. Do not count only the jump to ten: 8 to 10 is 2, then to 15 is 5 more, so the difference is 7.",
            @"I can compare counting-on and taking-away subtraction methods and explain my choice; teacher-led PLC discussion remains a class activity."),
        new(@"PED:US-CCSS-MATH:G1:U03:L25",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-25/preparation.html",@"a6cb06045a13078308b3af728d4251a25a747c461a7199d6ca82d9d29e3ab104",new[] { @"CCSS:1.NBT.A.1", @"CCSS:1.OA.A.1", @"CCSS:1.OA.B.4", @"CCSS:1.OA.C.5", @"CCSS:1.OA.C.6" },
            @"How Do You Want to Subtract?",
            @"Choose an efficient subtraction strategy for each pair of numbers within 20. You can take away, count on using related addition, or break a teen number at ten. Do not force the same method on every question.",
            @"Learning goal: flexibly choose a mathematically correct subtraction method based on operands. For 20-15, counting on from 15 to 20 is a quick five-step jump. For 17-4, taking away four ones is direct. For 14-8, subtract to ten and then the rest. The original 2021 choral warm-up counts FORWARD from 50 through 100, noticing patterns; this is teacher facilitated.",
            @"Example 1: 20-15=5 because 15+5=20.
Example 2: 17-4=13 by removing four ones.
Example 3: 14-8=14-4-4=10-4=6.
Example 4: 19-9=10, subtracting the nine extra ones.
Original 2021 pencil story: 12 pencils are on a table; a teacher picks up 7, leaving 12-7=5 pencils.
Original unknown-change story: Clare starts with 8 glue sticks and finishes with 15; 8+7=15, so the unknown additional amount is 7.
Original missing-subtrahend story: Kiran has 17 crayons and 9 after sharing; 17-8=9, so he shared 8 crayons.
Original 2021 student expressions: 20-15=5, 19-3=16, 13-5=8, 18-9=9 and 17-15=2. Select a suitable method for each, explaining why.
Original 2021 full story trio: 12-7=5 pencils; Clare 8+□=15 glue sticks gives □=7; Kiran 17-□=9 crayons gives □=8. Preserve each unknown's position.
EDULYTICS ORIGINAL EXIT CHECK (first-party, not publisher 2021 Cool-down): Solve 16-7 by a suitable strategy, then check that 9+7=16. The numeric fields verify the difference and addition check; a teacher reviews explanations and drawings.",
            @"Step 1: Read the starting and removed amounts.
Step 2: Ask whether taking away, counting on or crossing ten will be simplest.
Step 3: Represent the steps with counters, a ten-frame or a true subtraction/addition equation.
Step 4: Find the difference without reversing the order.
Step 5: Check the answer using the related addition fact.
Step 6: EDULYTICS exit check: choose a valid method for 16-7=9 and confirm 9+7=16. Counting on from 7 by 3 to 10 then 6 more gives nine; taking away seven is also valid. Discuss why your chosen method makes sense instead of claiming an automatic score proves a drawing.",
            @"Do not confuse a short count-on jump with the total number: 20-15 is 5, not 15. Avoid false equality chains: 14-4=10-4=6 is invalid, but 14-8=14-4-4=6 is correct.",
            @"I can select and explain a helpful subtraction method within 20."),
        new(@"PED:US-CCSS-MATH:G1:U03:L26",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-26/preparation.html",@"66dfe676b9c7e8a0d7d9efdda13be6a4dceaa7c8d3108fb6dd9e3d400fe5c4b3",new[] { @"CCSS:1.OA.A.1", @"CCSS:1.OA.B.4", @"CCSS:1.OA.C.6" },
            @"What’s the Story?",
            @"Solve addition AND subtraction stories where the unknown may be the start, the change or the result. Interpret the event before writing an equation: an amount may be added, taken away or compared. Match your equation to the story rather than assuming the final amount is always unknown.",
            @"Learning goal: solve addition and subtraction story problems with unknown values in different positions. Result unknown: 7+5=?. Change unknown: 7+?=12. Start unknown: ?+5=12. For subtraction, 14-?=8 and ?-6=8 require different reasoning. ORIGINAL Student Facing Elena story: Elena has 6 counters; some more join; now she has 18 counters. The missing change is 12 because 6+12=18. A second original Elena task starts with 18, removes an unknown number, and ends at 6: 18-□=6. Both stories have unknown change 12 but distinct events.",
            @"Example 1: Seven children are playing; five join. Total 7+5=12.
Example 2: Seven are playing; some join, now twelve. 7+?=12, so ?=5.
Example 3: Some children were playing; five joined and now twelve play. ?+5=12, so ?=7.
Example 4: Fourteen counters; some leave, eight remain. 14-?=8, so six leave.
Example 5: Some counters are present; six leave, eight remain. ?-6=8, so fourteen were there.
Original 2021 warm-up: 15-10=5, 15-12=3, 16-10=6, 16-13=3.
Original 2021 Elena change stories: 6+12=18 and 18-12=6.
Original 2021 Han story 1: □+9=15, so Han began with 6 pencils.
Original 2021 Han story 2: 15-□=9, so Han gave away 6 pencils.
Original 2021 Han story 3: 9+□=15, so Han received 6 pencils.
Original 2021 Han story 4: 15-9=6, so Han has 6 pencils left.
Teacher-facilitated classroom posters and original pupil representations are not electronically certified.
For the four original Han stories the total is 15 in every case, but the UNKNOWN changes: 6+9=15 (missing start), 15-6=9 (missing removed amount), 9+6=15 (missing new amount), and 15-9=6 (missing result). Edulytics checks both the missing value and the equation with the unknown in the source-appropriate position.
Original 2021 unit 3 Section D Checkpoint is identified as a 0-minute teacher checkpoint; its protected publisher text has NOT been inspected or copied.",
            @"Step 1: Identify the story event: join, leave, compare or change.
Step 2: Draw a bar, counters or a change diagram and mark what is UNKNOWN.
Step 3: Write an equation with the box in the correct position.
Step 4: Use related addition/subtraction or counting to find the box.
Step 5: Substitute into the original equation and check with the story.
Step 6: Compare all four Han situations: unknown start □+9=15, unknown removed 15-□=9, unknown joining 9+□=15, and unknown result 15-9=□. State which quantity is missing BEFORE computing it; explain with counters, drawings, numbers, or words to a teacher.",
            @"Do not change a start-unknown or change-unknown story into result-unknown without justification. A missing number in 7+□=12 is 5, not 19. The equal sign must be true after substitution.",
            @"I can identify and solve addition/subtraction stories with start, change or result unknown."),
        new(@"PED:US-CCSS-MATH:G1:U03:L27",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-27/preparation.html",@"19d5c3db8365d26e5ada5e0b8e59344a6c8132a8707eac7bb0e9d787e47ad854",new[] { @"CCSS:1.OA.C.5", @"CCSS:1.OA.C.6" },
            @"Center Day 4",
            @"Practice adding and subtracting within 20 through teacher-facilitated center games. The digital numeric exercises can check calculations and equations but do not replace physical turn-taking, card comparison, strategy explanation or partner games.",
            @"Learning goal: add and subtract within 20 using known facts, make-ten and counting strategies. Carefully distinguish an addition expression from a subtraction expression, and do not allow negative numbers in the within-20 first-grade card set.",
            @"Example 1: 8+7=15 by making ten: 8+2+5=15.
Example 2: 16-9=7 by subtracting 6 to ten and 3 more.
Example 3: 12+6=18 using a ten and eight ones.
Example 4: 20-8=12 because 12+8=20.",
            @"Step 1: Identify the operation chosen by the center card/game task.
Step 2: Use counters, a ten-frame or a trusted fact.
Step 3: Calculate a sum or a nonnegative difference within 20.
Step 4: Verify the result with an opposite operation or second strategy.
Step 5: If in classroom play, explain and compare strategies with your partner.
Original 2021 How Close? Subtract from 20, Stage 2: a student starts with four numeral cards from 0 to 10, selects TWO or THREE cards, subtracts their combined amounts from 20, and records a correct equation; a result closer to ZERO wins a round. This requires an actual teacher-led card game, so Edulytics grades only the chosen-card subtraction and its equation, not partner competition or winning.
Original 2021 Compare, Add and Subtract Within 20: compare the values of addition or subtraction expression cards; explain strategies in a teacher-led center.",
            @"Do not subtract a larger card from a smaller card and claim a negative difference is in the reviewed first-grade center scope. Partner center participation cannot be automatically certified by a multiple-choice quiz.",
            @"I can add and subtract within 20 and explain a strategy while following the teacher's center rules. Original 2021 warm-up: 20-10=10, 20-10-1=9, 18-10-4=4, and 18-14=4. During the teacher-led Compare center, 9+6=15 is greater than 19-6=13."),
        new(@"PED:US-CCSS-MATH:G1:U04:L01",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-1/preparation.html",@"5f7a4f43ed4319bca795c0b82d6a05acb7fd3bf1e20175f4e57825aa89c9dd16",new[] { @"CCSS:1.NBT.A.1", @"CCSS:1.OA.C.5", @"CCSS:1.OA.C.6", @"CCSS:1.OA.D.8" },
            @"Count Large Collections",
            @"Count large collections by organizing loose counters or classroom objects into groups of TEN. Show the number as a multiple of ten up to sixty, and explain why grouping prevents double counting. Real objects and class discussion are teacher-led.",
            @"Learning goal: count 10, 20, 30, 40, 50 and 60 objects, and compare strategies for making and recording equal groups of ten. One ten is ten ones; six tens are sixty ones. Zero tens are zero. Original 2021 Activity 1 uses a physical collection of 40 objects; Activity 2 uses several different collections of multiples of ten with groups trading bags. Warm-up: choral count by tens and discuss patterns.",
            @"Example 1: Three equal bags with ten counters each contain 10+10+10=30 counters.
Example 2: Five bundles of ten pencils contain 5 tens=50 pencils.
Example 3: Six groups of ten cubes contain 60 cubes. Count 10,20,30,40,50,60.
Example 4: Two plates each hold ten counters, so 2 tens=20 counters.
Original 2021 first collection: 40 objects can be arranged as four groups of ten (4 tens=40).
Original 2021 group activity: pupils count a second bag after trading collections; the teacher checks that physical objects are counted only once.",
            @"Step 1: Arrange your collection into separate groups of exactly ten objects.
Step 2: Count the number of complete groups, without recounting an object.
Step 3: Skip-count by tens, beginning at ten and stopping at the last group.
Step 4: Write 'n tens = 10n ones' and show a drawing/bundles.
Step 5: Check each group has ten and the total is at most sixty for this lesson.",
            @"Do not count five groups of ten as five objects or count the same group twice. Six tens is 60, not 6, and the unit 'ten' matters.",
            @"I can count and represent collections of up to six complete groups of ten."),
        new(@"PED:US-CCSS-MATH:G1:U04:L02",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-2/preparation.html",@"f916e5b744814c256800ad18834e648885de72c02121535104d72d46e32ed067",new[] { @"CCSS:1.NBT.A.1", @"CCSS:1.NBT.B.2" },
            @"Match Representations of Tens",
            @"Match the numeral, the number of tens and a picture of towers/bundles of ten for MULTIPLES OF TEN. Explain why three towers of ten, '3 tens', and numeral 30 describe the same collection.",
            @"Learning goal: interpret base-ten representations of multiples of ten, compare a numeral with bundles and words, and justify each match. In 70, the digit 7 represents seven tens, not seven single cubes. The original 2021 estimation warm-up asks for a too-low, about-right, and too-high estimate before examining ten-cube towers; estimates must not be reported as observed pupil work by a numeric auto-grader.",
            @"Example 1: 3 tens = 3 towers of ten = 30 cubes.
Example 2: 7 tens = 7 towers of ten = 70 cubes.
Example 3: 5 tens = 50 ones: 10+10+10+10+10=50.
Example 4: 4 towers of ten represent 40, not 14.
Original 2021 matching tasks pair number words (such as seventy), a numeral (70), and seven ten-cube towers or tens drawings. The classroom movement/matching with two classmates remains teacher-facilitated.",
            @"Step 1: Count complete ten-cube towers or drawn bundles.
Step 2: Write the number of 'tens' in words.
Step 3: Skip-count by ten to find the total objects.
Step 4: Match this total to the correct numeral (20,30,...,90).
Step 5: Explain how the tens digit records the number of towers.",
            @"Do not confuse the count of towers with the count of loose ones. Four tens means 40, not 4. Avoid matching pictures by their approximate size without counting.",
            @"I can match different representations of a multiple of ten."),
        new(@"PED:US-CCSS-MATH:G1:U04:L04",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-4/preparation.html",@"aea5b61803df4c4b9df6fd46f89e9413a5bb48d0ef33c3e8178327ec9e22b5c7",new[] { @"CCSS:1.NBT.B.2", @"CCSS:1.NBT.C.4", @"CCSS:1.NBT.C.5", @"CCSS:1.NBT.C.6" },
            @"More Addition and Subtraction with Tens PLC Activity",
            @"Add and subtract different MULTIPLES of ten by combining or taking away groups of ten. Use drawings, simple story contexts and equations. Unlike the previous lesson, the change can be 20, 30 or more—not always one ten.",
            @"Learning goal: add and subtract multiples of ten from multiples of ten (within 0–100) using tens as a unit. Addition 30+20 joins three tens and two tens; subtraction 70-40 removes four of seven tens. The original 2021 warm-up asks 30+10=40, 40+10=50, 80-10=70 and 70-10=60; pupil strategies may use the number of tens.",
            @"Example 1: 30+20=(3+2) tens=50.
Example 2: 70-40=(7-4) tens=30.
Example 3: 40+50=90, nine groups of ten.
Example 4: 90-60=30, and 30+60=90.
Original 2021 story: Jada counts 30 cubes in Bag A and 2 towers of 10 in Bag B, so 30+20=50 cubes.
Original 2021 story: Tyler has 7 towers of ten in Bag C, removes 40 cubes, and has 70-40=30 cubes.
Original 2021 student tasks: 4 tens and 5 tens make 9 tens=90; 8 tens take away 3 tens leaves 5 tens=50; 20+60=80; 70-20=50; 90-70=20; 40+40=80.",
            @"Step 1: Draw a tower for each ten in the starting number.
Step 2: Model adding or removing the stated multiple of ten, not single ones.
Step 3: Add or subtract the number of ten-blocks.
Step 4: Multiply the block count by ten to get the result.
Step 5: Check the result stays within 0–100 and agrees with a story context.",
            @"Do not confuse 30+20 with 3+2=5: it is 5 tens=50. Do not reverse subtraction and produce negative bundles; model a nonnegative within-grade context.",
            @"I can add and subtract multiples of ten by reasoning with whole groups of ten."),
        new(@"PED:US-CCSS-MATH:G1:U04:L05",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-5/preparation.html",@"e7cfc5b7e32e9bb81e4fd304bfbf0fcb85259e5e85278ccbdb5f1314f63f34ce",new[] { @"CCSS:1.NBT.B.2", @"CCSS:1.NBT.C.4", @"CCSS:1.NBT.C.6", @"CCSS:1.OA.C.6", @"CCSS:1.OA.D.8" },
            @"Center Day 1",
            @"Teacher-led Center Day: practice adding and subtracting multiples of TEN and, in other reviewed centers, numbers within 20. The Check It Off Stage 2 card activity uses number cards marked 0,10,...,90 and students choose a valid addition/subtraction expression. The digital portion grades mathematically valid numbers but never simulates partner turn-taking as completed.",
            @"Learning goals: use tens as units when adding/subtracting whole tens, and fluently add/subtract within 20. If the tens cards are 40 and 30, students may compute 40+30=70 or 40-30=10. For number cards producing totals above 100, those are outside the reviewed early place-value scope. Original 2021 Check It Off Stage 2 uses two multiples-of-ten cards (0–90) with the chosen result checked on the board; a result must lie within the activity's board range. The warm-up Which One Doesn't Belong? uses four pictures of tens whose explanations require a teacher.",
            @"Example 1: Cards 40 and 30: 40+30=70.
Example 2: Cards 70 and 20: 70-20=50.
Example 3: A previous center includes 8+7=15 by making ten.
Example 4: 16-8=8 because 8+8=16.
Original 2021 Check It Off example: cards 40 and 50 can produce a 90 sum or 10 difference. If 90 is already marked, subtract 50-40=10 to mark the remaining value; the teacher-led center tracks card and board state.",
            @"Step 1: Read the number cards and choose a permitted operation.
Step 2: For tens, combine or remove whole groups of ten.
Step 3: For within-20 numbers, count on, make ten or use a known fact.
Step 4: Check that the expression and answer obey the activity's value limits.
Step 5: If participating in center play, discuss the strategy and turns with the teacher/partner.",
            @"Do not treat 40+30 as seven ones. Do not automatically mark center-play requirements met by solving one digital equation. Check that subtraction does not remove more than the starting number.",
            @"I can calculate with tens and with numbers within 20; classroom center games stay teacher-led."),
        new(@"PED:US-CCSS-MATH:G1:U04:L06",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-6/preparation.html",@"c4019c5dfc3093c6cc9a477048c2aaa08c0a29618b5be8034c615c6c1306953d",new[] { @"CCSS:1.NBT.A.1", @"CCSS:1.NBT.B.2", @"CCSS:1.NBT.C.4", @"CCSS:1.NBT.C.5", @"CCSS:1.NBT.C.6", @"CCSS:1.OA.C.5", @"CCSS:1.OA.C.6", @"CCSS:1.OA.D.8" },
            @"Count Larger Collections",
            @"Organize and count a mixed collection by grouping ten ones into each bundle of ten, with leftover single objects kept separate. Use cubes, drawings or physical objects to show how the same total is counted by tens and ones. Teacher-led formative counting and comparing classmates' methods are not simulated as completed.",
            @"Learning goal: organize, count and represent a collection up to 99 using TENS and ONES, including partial groups. Ten ones compose one ten; if there are 4 bundles and 6 loose cubes the total is 46. Make every bundle exactly ten before counting the leftover ones.",
            @"Example 1: Thirty-seven cubes form 3 complete bundles of ten and 7 separate ones: 30+7=37.
Example 2: Two tens and five ones make 20+5=25 counters.
Example 3: Fifty-four buttons can be counted as five tens and four ones: 50+4=54.
Example 4: Six bundles of ten and zero loose ones represent 60 objects.
Original 2021 collection task: group 52 objects into five complete tens and two ones, 50+2=52.
Original 2021 Noah discussion: he claims his tower collection shows 50; it actually has four complete tens and eight ones, 40+8=48. A tower with eight objects is NOT another ten.
Original choral warm-up: count by ones from 50 to 81 and notice the change across 59, 60 and 69, 70.",
            @"Step 1: Touch or move each object exactly once while making complete groups of ten.
Step 2: Count whole bundles first by tens.
Step 3: Count remaining loose objects by ones (0–9).
Step 4: Record both the groups-of-ten count and the leftover ones, then calculate the total.
Step 5: Check by recounting or another arrangement and explain how the representations match.",
            @"Do not leave ten or more loose objects after claiming grouping is complete; exchange ten ones for one ten. Four tens and seven ones is 47, not 11 or 74. Do not count a cube twice.",
            @"I can count a larger mixed collection using complete tens and leftover ones and explain my arrangement."),
        new(@"PED:US-CCSS-MATH:G1:U04:L07",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-7/preparation.html",@"ed75036a11c93eb282fbaeb8df00eb8591b49297eb7eb9528ed79e95d90ea5a5",new[] { @"CCSS:1.NBT.A.1", @"CCSS:1.NBT.B.2", @"CCSS:1.NBT.C.4", @"CCSS:1.NBT.C.6", @"CCSS:1.OA.A.1", @"CCSS:1.OA.C.5", @"CCSS:1.OA.C.6" },
            @"Numbers With Tens and Ones",
            @"Read a two-digit number by recognizing its tens and ones. Describe a number shown using ten-sticks and one-cubes or a ten-frame collection. At this stage the original teacher preparation emphasizes READING and COMPOSING two-digit numbers; formal writing is introduced in a later lesson.",
            @"Learning goal: understand ANY two-digit number as groups of tens and leftover ones. 54 has 5 tens and 4 ones. A zero in the ones position, as in 70, means seven tens and zero loose ones. This lesson should not demand unreviewed production of written numerals as the primary activity.",
            @"Example 1: Four bundles of ten and three cubes represent forty-three (43): 40+3=43.
Example 2: The number 58 is read 'fifty-eight'; it contains five tens and eight ones.
Example 3: Seven tens and zero ones represents seventy (70).
Example 4: A picture with two tens and nine ones shows twenty-nine (29).
Original 2021 Notice and Wonder uses single digits 0–9 and two-digit numbers 10, 23, 45, 76, 89; compare the number of digits.
Original 2021 Clare/Diego task: Clare claims a shown model is 68 and Diego claims 86. It has EIGHT tens and SIX ones, so Diego's 86 is correct even if the ones appear before the tens in the drawing. The written numeral still places tens first.",
            @"Step 1: Identify how many complete groups of ten are represented.
Step 2: Count loose individual objects, fewer than ten.
Step 3: Say the number by naming the total number of tens and ones (for example, 'fifty-eight').
Step 4: Confirm the quantity shown by counting by tens and then by ones.
Step 5: Explain how the parts make the complete two-digit number.",
            @"Do not read 46 as 'four six' or treat the 4 as four loose ones; it represents four tens. If there are ten loose ones, make one more ten first.",
            @"I can read and explain a two-digit number using tens and ones."),
        new(@"PED:US-CCSS-MATH:G1:U04:L08",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-8/preparation.html",@"f3fa805d6421b2677b3a20200f257432271cbfee1e3b9c1ec6d34b8ba855d4da",new[] { @"CCSS:1.NBT.A.1", @"CCSS:1.NBT.B.2" },
            @"Different Representations of Tens and Ones PLC Activity",
            @"Interpret multiple representations of the SAME two-digit number: a drawing of ten-rods and one-dots, words such as '4 tens 2 ones', and the addition expression 40+2. Match the representations and explain their equivalence. Teachers manage any physical card or group activity.",
            @"Learning goal: interpret base-ten diagrams, '__ tens __ ones' descriptions and expanded additions for two-digit numbers. In 62 the six denotes six tens and the two denotes two ones. Different pictures and expressions must preserve the same total.",
            @"Example 1: Five ten-rods and three dots = 5 tens 3 ones = 50+3 = 53.
Example 2: 70+8 corresponds to 7 tens and 8 ones, a total of 78.
Example 3: Two tens and nine ones = 20+9 = 29.
Example 4: 4 ten-rods and 0 dots corresponds to 40+0 = 40.
Original 2021 compare-representations activity: Kiran describes 3 ones and 7 tens, and Priya writes 70+3. Both show 73, regardless of drawing order; Clare's and Han's diagrams must be interpreted in their source context.
Original estimation warm-up separately asks for a too-low, about-right and too-high estimate before counting; no student's actual estimate is invented by Practice.",
            @"Step 1: Count the tens and ones in a diagram.
Step 2: Translate into words, for example '5 tens and 3 ones'.
Step 3: Write or recognize the matching expanded expression 50+3.
Step 4: Calculate or read the numeral and compare all representations.
Step 5: Explain why all representations show the same quantity.
Step 6: For the original 2021 card sort, match picture, tens-and-ones description and expanded expression, and explain the match to a teacher; the Grab and Count real cube estimate/count is a classroom task.",
            @"Do not equate 5 tens and 3 ones with 5+3=8; the five tens are fifty. Do not match 50+3 to 35 by reversing the places.",
            @"I can match diagrams, words and addition expressions for two-digit numbers."),
        new(@"PED:US-CCSS-MATH:G1:U04:L09",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-9/preparation.html",@"eecf5776e8b806188c62ba41baf5e045e8b9425ba1d411a1df483e635d1d1688",new[] { @"CCSS:1.NBT.A.1", @"CCSS:1.NBT.B.2", @"CCSS:1.NBT.C.4", @"CCSS:1.NBT.C.6", @"CCSS:1.OA.A.1", @"CCSS:1.OA.C.5", @"CCSS:1.OA.C.6" },
            @"Show Me Your Number",
            @"Show a chosen two-digit number using multiple representations: bundle tens with leftover ones, sketch ten-rods and dots, name the tens and ones, and give an expanded addition expression. ALSO trade a full ten into ten extra ones and describe a SECOND, different but mathematically equal model, e.g. 47=4 tens 7 ones=3 tens 17 ones. The original gallery walk and peer discussion require a classroom setting; a digital answer does not certify they occurred.",
            @"Learning goal: REPRESENT the place-value structure of two-digit numbers, not only recognize a preprinted match. Use diagrams, words and expressions, keeping the total unchanged in all forms.",
            @"Example 1: Represent 64 as six rods of ten plus four dots, '6 tens 4 ones', and 60+4.
Example 2: Represent 35 as three rods and five dots, '3 tens 5 ones', and 30+5.
Example 3: Represent 80 as eight rods and no leftover ones, and 80+0.
Example 4: Represent 47 as 4 tens and 7 ones, 40+7=47.
Original 2021 Show Collections In Many Ways asks students to visit AT LEAST FOUR real collections and show each in several equivalent forms. Teacher/partner gallery participation is not established by solving a generated number.",
            @"Step 1: Pick or read the target two-digit value.
Step 2: Organize 10-object bundles and loose ones.
Step 3: Draw one rod per ten and one dot per remaining one.
Step 4: State the number of tens and ones and write a matching expanded expression.
Step 5: Check that all three representations have the same total and explain to a partner if teacher-led.",
            @"Do not draw six dots when you mean six tens or write 60+4=10. Do not claim to have completed a gallery walk without participating in the real class activity.",
            @"I can create correct drawings, words and an expanded expression for a two-digit number."),
        new(@"PED:US-CCSS-MATH:G1:U04:L10",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-10/preparation.html",@"c124655e43c41ae5360f7e5ca5f49ae8e4a483a583cdd2be74d5266000debe47",new[] { @"CCSS:1.NBT.A.1", @"CCSS:1.NBT.B.2" },
            @"Write Two-digit Numbers",
            @"Write a two-digit numeral from a base-ten picture, number of tens and ones, or expanded tens-plus-ones expression. This lesson introduces writing the standard numeral for these representations and checks place-value position carefully.",
            @"Learning goal: write two-digit numerals that correspond to ten-rods plus one-cubes, written tens-and-ones descriptions and expanded expressions. The tens digit counts complete groups of ten, and the ones digit counts leftovers.",
            @"Example 1: 3 tens and 7 ones are written 37 (30+7).
Example 2: Four ten-rods and two ones are written 42 (40+2).
Example 3: Nine tens and zero ones are written 90 (90+0).
Example 4: 60+5 is written 65; the 6 is in the tens place.
Original 2021 learner entries: 1 ten and 4 ones=14, 9 tens=90, 20+5=25, 7+40=47, 2 ones and 8 tens=82, and 1+40=41. Blank numeral-writing prompts belong to the student; these are worked answers for instruction.",
            @"Step 1: Read the number of tens in the supplied diagram, words or expression.
Step 2: Read the number of single ones (0–9).
Step 3: Write the tens digit in the first position and the ones digit in the second.
Step 4: Read the finished numeral aloud and match it to the expanded expression.
Step 5: Check the total of the represented objects and that the digits are not reversed.
Step 6: In the original Write Numbers Stage 1 center, players take turns writing the next one, two or three numerals on the board, counting forward or backward within 99; the winner and physical board are teacher-observed.",
            @"Do not write 3 tens and 7 ones as 73; the first digit is tens. Do not omit the zero in 8 tens and 0 ones: write 80, not 8.",
            @"I can write two-digit numerals from tens-and-ones representations."),
        new(@"PED:US-CCSS-MATH:G1:U04:L11",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-11/preparation.html",@"87d02e4b8c376f99a6d01c847535fa868f7d915d3ffa539930c6905f981d9b96",new[] { @"CCSS:1.NBT.B.2", @"CCSS:1.NBT.C.4" },
            @"Add Tens to Two-digit Numbers",
            @"Add a WHOLE multiple of ten to a two-digit number while keeping the existing ones unchanged. Model the number with bundles of ten and single ones and explain why adding tens affects only the tens count when the result stays within 100.",
            @"Learning goal: add a two-digit number and a multiple of ten, using place value. For 34+20, combine 3 tens and 2 tens, keep 4 ones, and obtain 54. Unlike adding single ones, adding full tens does not change the ones digit.",
            @"Example 1: 34+20=54 because 3 tens+2 tens=5 tens and four ones remain.
Example 2: 47+30=77, as 40+30+7=70+7.
Example 3: 62+10=72, as 6 tens+1 ten=7 tens and two ones.
Example 4: 51+40=91, with one extra one unchanged.
Original 2021 student equations: 37+20=57; 60+23=83; 48+50=98; 54+20=74; 30+65=95.
Original 2021 True/False warm-up: 80+5=5+80 TRUE; 70+1=80+1 FALSE; 20+6=6+30 FALSE.
The original Missing Digit activity has a smudged diagram that requires visual source inspection; do not certify the missing digit from prose alone.",
            @"Step 1: Break the non-round number into tens and ones.
Step 2: Count the number of full tens being added.
Step 3: Add tens to tens; copy the original number of ones.
Step 4: Recombine the tens and ones in a two-digit answer within 100.
Step 5: Verify with base-ten rods or a drawing.",
            @"Do not add 20 to the ones place in 34+20. The answer is 54, not 36 or 314. Check for totals above 100 rather than silently using out-of-scope operands.",
            @"I can add a multiple of ten to a two-digit number without changing the ones."),
        new(@"PED:US-CCSS-MATH:G1:U04:L12",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-12/preparation.html",@"6d8b12cdd219f0b4ec414c51ecc71740b29e31b04dcbe0fb57e85494df44658d",new[] { @"CCSS:1.NBT.A.1", @"CCSS:1.NBT.C.5", @"CCSS:1.OA.C.6" },
            @"Mentally Add and Subtract Tens (optional)",
            @"Optional mental-math activity: use place-value reasoning to find TEN MORE or TEN LESS than a two-digit number. No written algorithm is necessary when students can describe the effect on the tens place. Explain the mental pattern rather than memorizing isolated answers.",
            @"Learning goal: mentally find 10 more and 10 less than a two-digit number. Tens change by one full group, ones remain the same: 46+10=56 and 46-10=36. Restrict subtracting 10 to numbers with at least one ten.",
            @"Example 1: Ten more than 58 is 68; the eight ones stay the same.
Example 2: Ten less than 64 is 54; the four ones stay the same.
Example 3: Ten more than 39 is 49; only the tens count increases.
Example 4: Ten less than 71 is 61.
Original 2021 source equations: 67+10=77, 67-10=57; 39+10=49, 39-10=29; 52+10=62, 52-10=42; 75+10=85, 75-10=65.
Further original sequences: 67+10+10+10=97; 99-10-10-10=69; 45+10+10=65, 45-10-10=25, 45+10-10=45.
The original Write Numbers Stage 2 center counts by TEN forward or backward, writing one, two or three numbers on each turn; teacher supervises real gameplay.",
            @"Step 1: Read the starting two-digit number.
Step 2: Identify the tens and ones digits.
Step 3: Add or remove ONE ten, leaving the ones unchanged.
Step 4: Name the new two-digit number.
Step 5: Explain mentally why the number is ten more or ten less.",
            @"Do not change the ones digit when only ten is added or removed. 58+10 is 68, not 59. Do not remove a full ten from a number with no tens.",
            @"I can mentally say ten more or ten less than a two-digit number using tens and ones."),
        new(@"PED:US-CCSS-MATH:G1:U04:L13",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-13/preparation.html",@"aa81321cc4958d6903637c2972c2530def7482777bd0ff87af07b86cc29522b3",new[] { @"CCSS:1.NBT.A.1", @"CCSS:1.NBT.C.4", @"CCSS:1.NBT.C.5", @"CCSS:1.NBT.C.6", @"CCSS:1.OA.A.1", @"CCSS:1.OA.C.5", @"CCSS:1.OA.C.6", @"CCSS:1.OA.D.8" },
            @"Center Day 2",
            @"Practice within-20 addition/subtraction AND work with two-digit numbers in a teacher-led center choice. In the two-digit centers, model adding tens; in counting centers, represent mixed collections; in within-20 centers, use known number relationships. The classroom choice and partner games are not automatically evaluated by digital Practice.",
            @"Learning goals: add and subtract within 20, add tens to two-digit numbers, and count and represent collections. Do not reduce this center day to only one arithmetic type.",
            @"Example 1: 8+7=15 using make-ten: 8+2+5=15.
Example 2: 17-9=8 using a ten-frame or a known fact.
Example 3: 43+20=63 because four tens plus two tens makes six tens, and three ones remain.
Example 4: Four tens and six ones represent 46 objects.
Original 2021 number talk: 10+4=14, 9+4=13, 9+6=15 and 15-6=9.
Original 2021 Check It Off center: 70 and 20 produce 90 by addition or 50 by subtraction.
Original 2021 How Close? source hand 8, 4, 1, 7, 5 can select THREE cards 8+7+5=20, precisely reaching the target. Real center selection and peer gameplay remain teacher-led.",
            @"Step 1: Read the selected center's actual task and quantity representation.
Step 2: Identify whether it calls for within-20 calculation, tens addition, or counting tens and ones.
Step 3: Solve using a matching strategy, with drawings or counters if helpful.
Step 4: Verify that all answers match the task's number range.
Step 5: For physical center play, follow the teacher's rules and explain the strategy to a partner.",
            @"Do not mark physical game turns or partner explanations complete from a numeric answer. Do not apply the same ten-addition algorithm to a within-20 subtraction problem.",
            @"I can use different place-value and within-20 methods in the right center activity."),
        new(@"PED:US-CCSS-MATH:G1:U04:L14",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-14/preparation.html",@"0b28935630a688ace4cb6cd871768f2f6cfe349d7740017b705cbad49f18418d",new[] { @"CCSS:1.NBT.A.1", @"CCSS:1.NBT.B.2", @"CCSS:1.NBT.B.3", @"CCSS:1.NBT.C.5", @"CCSS:1.OA.C.5", @"CCSS:1.OA.C.6" },
            @"Let’s Compare",
            @"Compare TWO-DIGIT numbers using the VALUE of their tens and ones digits. First compare the number of tens; only if those are equal, compare the ones. Explain which number is greater or less using words and drawings. The source introduces the terms 'greater than' and 'less than' before formal symbol writing.",
            @"Learning goal: compare two-digit numbers based on tens and ones, and use greater-than/less-than language. For 64 and 57, six tens exceed five tens even though 4 is less than 7. For 63 and 68, the tens match, so compare the ones.",
            @"Example 1: 64 is GREATER THAN 57 because six tens exceed five tens.
Example 2: 63 is LESS THAN 68 because both have six tens and three ones are less than eight.
Example 3: 42 and 42 are EQUAL: four tens and two ones each.
Example 4: 79 is GREATER THAN 71 because nine ones exceed one when tens are both seven.
Original 2021 Elena argues 75>65 because 7>6; refine the explanation to SEVEN TENS exceeds SIX TENS.
Original 2021 Noah incorrectly claims 39>41 because 9 is the largest single digit; 39<41 because THREE tens are fewer than FOUR tens. Explain why comparing ones first is wrong.
The original comparison-spinner and partner discussion are physical teacher-led tasks.",
            @"Step 1: Represent or name the tens and ones in each two-digit number.
Step 2: Compare the tens counts first.
Step 3: If the tens match, compare the ones.
Step 4: State the relationship in full words: greater than, less than or equal to.
Step 5: Support the comparison with rods/dots or the value of each digit.",
            @"Do not compare just the ones digit when tens are different: 64 is greater than 57 even though four ones are fewer than seven. Formal < and > notation is taught in the next lesson, not required here.",
            @"I can compare two-digit values by their tens and ones and explain greater or less."),
        new(@"PED:US-CCSS-MATH:G1:U04:L15",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-15/preparation.html",@"461322580dbb19eef16268d3c8eed6a02c5ca5a037c2a5ba2d106d53481c72b6",new[] { @"CCSS:1.NBT.B.3", @"CCSS:1.NBT.C.5", @"CCSS:1.OA.D.7" },
            @"Greater Than, Less Than",
            @"Understand and INTERPRET the comparison symbols > (greater than), < (less than) and = (equal). The wide-opening side faces the GREATER number. Translate a true symbol statement into a sentence and back, reasoning first with place value.",
            @"Learning goal: interpret comparisons of two-digit numbers using <, > and =. Compare values rather than simply drawing the symbol shape. For 65>48, sixty-five is greater than forty-eight; 36<63 means thirty-six is less than sixty-three.",
            @"Example 1: 52 > 39 because five tens exceed three tens.
Example 2: 36 < 63 because three tens are fewer than six tens.
Example 3: 47 = 47 since both represent four tens and seven ones.
Example 4: 71 < 79 because both have seven tens and one one is fewer than nine.
Original 2021 comparison choices include 17<27, 34<36, 25<52, 54>24, 29>21, 85>58, 45<54, 74<78 and 21>12. Learners select a TRUE comparison from each pair.
Original 2021 True/False activity: 17<47 TRUE, 58=53 FALSE, 45>63 FALSE, 39<93 TRUE, 4=46 FALSE. For a false statement, write a corrected comparison and explain the place values.
Original source warm-up: 35+10=45, 52+10=62, 52-10=42, and 83-10=73.",
            @"Step 1: Compare tens, then compare ones only if tens are tied.
Step 2: Determine whether the first value is greater, less or equal.
Step 3: Write or interpret the correct symbol between the two numbers.
Step 4: Read the full comparison sentence aloud.
Step 5: Recheck direction by explaining which digit determines the comparison.",
            @"Do not reverse < and >: the OPEN side points toward the larger value. Do not treat = as a prompt to calculate a new result; it means both sides have equal values.",
            @"I can interpret greater-than, less-than and equals symbols in two-digit comparisons.")
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
            throw new InvalidOperationException("G1 full-grade source/body/outcome drift: "+lesson.LessonCode);
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
                throw new InvalidOperationException("G1 full-grade unexpected pre-repair body: "+lesson.LessonCode+" | example="+en.WorkedExamples[..Math.Min(42,en.WorkedExamples.Length)]+" | steps="+en.StepByStepSolutions[..Math.Min(42,en.StepByStepSolutions.Length)]);
            en.Title=r.Title;en.Explanation=r.Explanation;en.KeyConceptsAndRules=r.Concepts;
            en.WorkedExamples=r.Examples;en.StepByStepSolutions=r.Steps;
            en.CommonMistakes=r.Mistakes;en.QuickSummary=r.Summary;
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previous)=>
        IsTarget(doc,lesson)
            ? (lesson.LessonCode switch
            {
                ZooLessonCode=>ZooSourceContentVersion,
                CenterLessonCode=>CenterSourceContentVersion,
                TeenSubtractLessonCode=>TeenSubtractSourceContentVersion,
                TenSubtractLessonCode=>TenSubtractSourceContentVersion,
            CountRelationLessonCode=>CountRelationSourceContentVersion,
            ChooseSubtractLessonCode=>ChooseSubtractSourceContentVersion,
            StoryLessonCode=>StorySourceContentVersion,
            CenterFourLessonCode=>CenterFourSourceContentVersion,
            UnitFourCollectionLessonCode=>UnitFourCollectionSourceContentVersion,
            UnitFourTensMatchLessonCode=>UnitFourTensMatchSourceContentVersion,
            UnitFourMultipleTensLessonCode=>UnitFourMultipleTensSourceContentVersion,
            UnitFourCenterLessonCode=>UnitFourCenterSourceContentVersion,
            var l when UnitFourLaterLessonCodes.Contains(l)=>UnitFourLaterSourceContentVersion,
                _=>Version
            })
            : previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previous)=>
        IsTarget(doc,lesson)&&
        (previous==CommonCoreRichContentCorrections.CorrectionContentVersion||
         (lesson.LessonCode==ZooLessonCode&&(previous=="us-ccss-g1-u03-l20-zoo-2021-source-v6"||previous=="us-ccss-g1-u03-l20-zoo-2021-source-v7"))||
         (lesson.LessonCode==CenterLessonCode&&previous=="us-ccss-g1-u03-l21-centers-2021-source-v6")||
         (lesson.LessonCode==TeenSubtractLessonCode&&previous=="us-ccss-g1-u03-l22-teen-2021-source-v6")||
         (lesson.LessonCode==TenSubtractLessonCode&&(previous=="us-ccss-g1-u03-l23-ten-2021-source-v6"||previous=="us-ccss-g1-u03-l23-ten-2021-source-v7"))||
         (lesson.LessonCode==CountRelationLessonCode&&previous=="us-ccss-g1-u03-l24-original-activities-v6")||
         (lesson.LessonCode==ChooseSubtractLessonCode&&previous=="us-ccss-g1-u03-l25-original-activities-v6")||
         (lesson.LessonCode==StoryLessonCode&&previous=="us-ccss-g1-u03-l26-original-activities-v6")||
         (previous==Version&&lesson.LessonCode is
             ZooLessonCode or CenterLessonCode or TeenSubtractLessonCode or TenSubtractLessonCode or
             CountRelationLessonCode or ChooseSubtractLessonCode or StoryLessonCode or CenterFourLessonCode or
             UnitFourCollectionLessonCode or UnitFourTensMatchLessonCode or
             UnitFourMultipleTensLessonCode or UnitFourCenterLessonCode)||
         (previous==Version && UnitFourLaterLessonCodes.Contains(lesson.LessonCode)));
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
