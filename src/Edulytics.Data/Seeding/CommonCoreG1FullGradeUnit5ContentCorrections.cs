using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// New Grade-1 complete-grade workstream 87–102 (excluding existing source-corrected
/// ordinal 86). Source-goal-specific first-party learner teaching drafts; not
/// independently verified original student tasks or final academic signoff.
/// </summary>
public static class CommonCoreG1FullGradeUnit5ContentCorrections
{
    public const string Version = "us-ccss-g1-full-grade-u04-l22-through-u05-l14-teaching-v2";
    private const string PreviousVersion = "us-ccss-g1-full-grade-u04-l22-through-u05-l14-teaching-v1";
    public const string Unit4EndSourceVersion="us-ccss-g1-u04-l22-l23-original-activities-v2";
    public const string Unit5EarlySourceVersion="us-ccss-g1-u05-l01-l07-source-activities-v2";
    public const string Unit5LaterSourceVersion="us-ccss-g1-u05-l08-l13-source-activities-v2";
    private static bool IsUnit5Later(string code)=>code is
        "PED:US-CCSS-MATH:G1:U05:L08" or "PED:US-CCSS-MATH:G1:U05:L09" or
        "PED:US-CCSS-MATH:G1:U05:L10" or "PED:US-CCSS-MATH:G1:U05:L11" or
        "PED:US-CCSS-MATH:G1:U05:L12" or "PED:US-CCSS-MATH:G1:U05:L13";
    private static bool IsUnit5Early(string code)=>code is
        "PED:US-CCSS-MATH:G1:U05:L01" or "PED:US-CCSS-MATH:G1:U05:L02" or
        "PED:US-CCSS-MATH:G1:U05:L03" or "PED:US-CCSS-MATH:G1:U05:L04" or
        "PED:US-CCSS-MATH:G1:U05:L05" or "PED:US-CCSS-MATH:G1:U05:L06" or
        "PED:US-CCSS-MATH:G1:U05:L07";
    private static bool IsUnit4End(string code)=>code is
        "PED:US-CCSS-MATH:G1:U04:L22" or "PED:US-CCSS-MATH:G1:U04:L23";
    private sealed record Repair(string Code,string SourceUrl,string OriginalSha,string[] Outcomes,
        string Title,string Explanation,string Examples,string Steps,string Mistakes,string Summary);
    private static readonly Repair[] Repairs =
    [
        new(@"PED:US-CCSS-MATH:G1:U04:L22",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-22/preparation.html",@"b85aea088222d4e41c7277cd0b59f55d8e177689037f8ba35a6c28b7e1fd7519",[@"CCSS:1.NBT.B.2",@"CCSS:1.NBT.B.3"],
            @"Center Day 4",
            @"Compare within 99 using place value and describe what a numeral means in tens and ones. Original 2021 warm-up evaluates four comparison cards, including explaining why 35<20 is false. Original Mystery Number center requires each student to give truthful clues about a two-digit mystery number; a partner guesses after clues and records a score. Physical center games and partner responses are teacher facilitated.",
            @"Example 1: 74 is greater than 68 because seven tens are more than six tens. Example 2: 53 and 59 share five tens; 53 is less because three ones are fewer than nine. Original 2021 warm-up: 92=90+2 TRUE; 90+2>80+12 FALSE (both equal 92); 20+13<30+13 TRUE (33<43).",
            @"Step 1: Represent each number with tens and ones. Step 2: Compare tens, then ones when tens match. Step 3: Explain which quantity is greater and why. Step 4: In original Mystery Number play, make a two-digit mystery number and give clues about tens and ones; partners make guesses. Digital mathematics cannot assert that pupils played a partner game.",
            @"Comparing only ones is wrong: 68 is not greater than 74 simply because 8 exceeds 4.",
            @"I can identify and compare two-digit numbers, including number-card center representations."),
        new(@"PED:US-CCSS-MATH:G1:U04:L23",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-23/preparation.html",@"b64853c6fba2b3fb89c0192538cb6629db49583e2844ce48f4b367e4721b9384",[@"CCSS:1.NBT.A.1",@"CCSS:1.NBT.B.2",@"CCSS:1.NBT.B.3"],
            @"Two-Digit Numbers in Our World (optional)",
            @"Optional investigation: estimate a collection before organizing and counting it. Original 2021 bean warm-up estimates too low, about right, too high from a picture marked with a group of TEN beans. Original Activity 1 has THREE two-handful estimation/counting experiments. Original Activity 2 sorts pictures by quantity into LESS THAN 20, 20 TO 50, and MORE THAN 50; pupils then represent one from each group using tens and ones. Real collection measurements and source-artwork comparisons require teacher supervision and verified visuals.",
            @"Example 1: I estimate about 50 counters. Group into four tens and seven ones to count 47; my estimate is 3 more. Example 2: Six tens and two ones represent 62. Original image sorting cannot be claimed to be mathematically graded without the source pictures or a fully reviewed first-party equivalent.",
            @"Step 1: View real objects or an authorized first-party picture; predict a too-low, about-right and too-high estimate. Step 2: For the original THREE two-handful experiments, estimate and then physically count and represent the actual amount. Step 3: Sort the original quantity pictures into LESS THAN 20, 20 TO 50 or MORE THAN 50, and choose one from each to model in tens and ones. Step 4: Explain estimates versus observed counts; teacher reviews physical work rather than fabricated digital participation.",
            @"Do not claim the estimate is an exact count. Five tens and three ones is 53, not 8.",
            @"I can estimate, count, and represent a collection up to 99. A real-life investigation needs real evidence."),
        new(@"PED:US-CCSS-MATH:G1:U05:L01",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-1/preparation.html",@"39aba045753ec23ceacd9c40cbd05dfc4e50f98ce3f39f1fe0f9799e4435f276",[@"CCSS:1.NBT.A.1",@"CCSS:1.NBT.B.2",@"CCSS:1.NBT.C.4"],
            @"Add Tens or Ones",
            @"Add TENS or ONES to a two-digit number without composing a new ten. Draw rods for tens and single dots for ones. Original 2021 What Did I Add? partner activity: spin a starting number; secretly choose a card, decide to add that number of ONES or TENS within 100, then let a partner infer the addition. The original warm-up also uses images of ten-frames; teacher verifies any partner reasoning.",
            @"Example 1: 43 + 5 = 48; three ones plus five ones makes eight ones. Example 2: 43 + 20 = 63; four tens plus two tens makes six tens, and three ones remain. Original student equations: 43+50=93; 51+3=54; 51+30=81; 2+75=77; 20+75=95; 93+6=99; 60+28=88; 5+74=79. These are no-composing examples with the same original quantities.",
            @"Step 1: Identify whether the addend contains tens or ones. Step 2: Add like units without combining ten ones. Step 3: Rewrite and check the resulting two-digit number.",
            @"Do not treat +20 as adding two ones. Do not generate a sum with ten or more ones in this no-composing lesson.",
            @"I can add tens or ones to two-digit numbers when no new ten is made."),
        new(@"PED:US-CCSS-MATH:G1:U05:L02",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-2/preparation.html",@"d5f9bd01b30984d63c60ef831e3ba9db91dfba03a1ff02e398bac487c9f4ef6a",[@"CCSS:1.NBT.A.1",@"CCSS:1.NBT.C.4"],
            @"How Did You Add? PLC Activity",
            @"Use place-value methods to add two two-digit numbers WITHOUT composing a new ten, and interpret equations showing those methods. Original 2021 Choral Count warm-up counts BACKWARD by ones from 70 to 20. Original student-facing task asks for 23+45. Next students match Elena and Andre drawings for 63+25 to two different correct methods. Original Five in a Row Stage 5 adds within 100 WITHOUT composing in a teacher-led partner game.",
            @"Example 1: 23 + 45 = (20 + 40) + (3 + 5) = 60 + 8 = 68. Example 2: Original Elena/Andre task 63+25=88: Elena may combine 60+20=80, 3+5=8, then 80+8=88. Andre can add 20 to 63 to get 83, then add 5 to get 88. Both are equivalent. Example 3: 32+54=86 by combining eight tens and six ones.",
            @"Step 1: Break both addends into tens and ones. Step 2: Add tens together. Step 3: Add ones together (fewer than ten). Step 4: Interpret every equality and check the sum.",
            @"Do not regroup when the ones sum is below ten, and do not claim 23+45=20+40 without adding the ones.",
            @"I can explain valid equations for two-digit addition with no composing."),
        new(@"PED:US-CCSS-MATH:G1:U05:L03",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-3/preparation.html",@"2d5c8b8a5fd81e87736b2e54acfd6fe1f0ef46c6e6a4316edac14ae0a5cd6d35",[@"CCSS:1.NBT.C.4"],
            @"Add It, Explain It",
            @"Write, not merely interpret, equations representing two-digit addition without composing a new ten. Original 2021 warm-up sums 13+21, 13+31, 23+31, 33+41. Original Lin and Han activity starts 32+54 differently: Lin has 30+50=80, Han has 54+30=84; pupils COMPLETE the method. Original next task uses 23+74 and the I Heard You Say partner reflection.",
            @"Example 1: 13 + 21 = 10 + 20 + 3 + 1 = 30 + 4 = 34. Example 2: Lin and Han original 32+54=86: Lin 30+50=80, 2+4=6, total 86; Han 54+30=84, then 84+2=86. Example 3: original 23+74=97, where 20+70=90 and 3+4=7.",
            @"Step 1: Sketch tens and ones for each addend. Step 2: Write an equation decomposing the quantities. Step 3: Combine tens and ones and write a true final equality. Step 4: Explain the equation.",
            @"An equation such as 13+21=30 is false because four ones have been dropped.",
            @"I can write my own equations showing a correct place-value addition method."),
        new(@"PED:US-CCSS-MATH:G1:U05:L04",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-4/preparation.html",@"d0dfcf1a9ddb0e856a1e95d63556dd08dda10a6cdfa1ff7d3d8c92a91ab40170",[@"CCSS:1.NBT.A.1",@"CCSS:1.NBT.C.4",@"CCSS:1.OA.A.1",@"CCSS:1.OA.C.6",@"CCSS:1.OA.D.8"],
            @"Center Day 1",
            @"Teacher-led center for addition within 100 WITHOUT composing a new ten. Original 2021 warm-up: Kiran has 14 marbles, Priya has 23; pupils NOTICE and WONDER and invent their own addition/comparison questions. Original Number Puzzles Stage 3 uses digit cards at most once per page to create within-100 no-composing equations; partner choices and original gameboard require teacher review. Original center choices: Number Puzzles, Five in a Row, Greatest of Them All. Digital Practice checks numerical totals but not classroom turns.",
            @"Example 1: 31 + 26 = 57: three tens plus two tens is fifty; one plus six makes seven. Example 2: 42 + 15 = 57 without regrouping.",
            @"Step 1: Read the selected center's two numbers. Step 2: Add tens and ones separately. Step 3: Make sure the ones sum is below ten. Step 4: Check by showing bundles and ones.",
            @"Do not mark a center game completed from a single numeric answer; do not slip in composing-ten questions as though they are the same objective.",
            @"I can add within 100 without composing a new ten in a teacher-facilitated center."),
        new(@"PED:US-CCSS-MATH:G1:U05:L05",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-5/preparation.html",@"3d56c54577b9ed9dbaa2eca03ba5f8c41e0aed29b705dde350dd9ccbb7cd7c6c",[@"CCSS:1.NBT.C.4"],
            @"Make a Ten",
            @"Add a one-digit number to a two-digit number by composing a new TEN; decompose the one-digit addend to complete the next multiple of ten. Original 2021 Choose a Way to Add uses 8+47 and accepts several strategies, including making ten. Add Em Up Partner runs a practice round and five regular rounds with actual partner interaction. The optional Tyler teacher story includes 37 dollars plus 7 more dollars and 8 students plus 26 on the rug; classroom actions are teacher-reviewed.",
            @"Example 1: Original 8+47=47+3+5=55; composing ten first gives 50+5=55. Example 2: 36+7=36+4+3=40+3=43. Example 3: Optional Tyler teacher field trip 37+7=44 dollars. Example 4: Optional classroom group 8+26=34 students.",
            @"Step 1: Find how many ones the two-digit number needs to reach the next ten. Step 2: Split the one-digit addend into that part and leftovers. Step 3: Compose a full ten and add leftovers. Step 4: Check by counting on.",
            @"Do not stop at 50 after using only three of the eight added ones in 47+8; five ones still remain.",
            @"I can make a new ten while adding a one-digit number to a two-digit number."),
        new(@"PED:US-CCSS-MATH:G1:U05:L06",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-6/preparation.html",@"973308633a370cd4487792a7ea75b8a8123eccf12337992b7cb76d277b123854",[@"CCSS:1.NBT.C.4",@"CCSS:1.OA.C.6"],
            @"Make a Ten and Make Sense of Equations PLC Activity",
            @"Analyze the ORIGINAL 2021 two complementary strategies for 34+9: Elena begins with 34+6=40 and adds the remaining 3, while Andre begins with 9+1=10 and adds 33. Both must equal 43. Source also asks for 6+68=74. Original 2021 Next Ten tasks: 45+□=50, 38+□=40, 63+□=70, 24+□=30, 42+□=50, 57+□=60, 71+□=80 and 89+□=90, with pupil explanation. Original Target Numbers game starts at 55 and seeks 95 or as close without exceeding it in six turns; genuine partner gameplay is teacher-led.",
            @"Original Elena: 34+9=34+6+3=40+3=43. Original Andre: 34+9=9+1+33=10+33=43. Original 6+68=68+2+4=70+4=74. Additional 47+8=47+3+5=55.",
            @"Step 1: Read Elena and Andre's different first steps for 34+9. Step 2: Complete Elena's 34+6=40 then add 3 to reach 43. Step 3: Complete Andre's 9+1=10 then add 33 to reach 43. Step 4: Explain with counters or a drawing why both methods preserve 34+9. Step 5: For the separate source task, solve 6+68 by making 70 then adding 4. Teacher reviews original individual reasoning.",
            @"Do not write 47+8=50+8: moving three ones to complete a ten leaves five, not eight.",
            @"I can explain correct make-ten equations; partner discussion is verified separately by the teacher."),
        new(@"PED:US-CCSS-MATH:G1:U05:L07",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-7/preparation.html",@"241b1c73722edb009d4121faa6cbcbe86161c462bdf79e9671cdd56b909761ee",[@"CCSS:1.NBT.C.4"],
            @"Does it Make a New Ten?",
            @"ORIGINAL 2021 Lin water-smudged source activity has FOUR separate open-choice situations: 32+□ with a ONE-DIGIT missing number that DOES make a ten; 16+□ with a ONE-DIGIT number that DOES NOT make a ten; 8+□ with a TWO-DIGIT missing number that DOES make a ten; and 8+□ with a TWO-DIGIT number that DOES NOT make a ten. Different correct values are possible; students write equations and a teacher reviews personal explanations.",
            @"Original one-digit examples: 32+8=40 and 32+9=41 BOTH make a new ten; 16+0=16, 16+1=17, 16+2=18 and 16+3=19 do not. Original two-digit examples: 8+12=20, 8+35=43 and 8+49=57 make a ten; 8+11=19, 8+30=38 and 8+41=49 do not. Original Jada decision prompts: 45+5 DOES make ten, 9+63 DOES, 26+3 DOES NOT, 8+47 DOES; explanations and true equations required. Source artwork behind the smudges still requires verified visual evidence. Source requires thinking, not a single preset answer.",
            @"Step 1: Add the ones of both addends. Step 2: If the ones total reaches ten or more, compose one new ten. Step 3: Write an equation recording what changes. Step 4: Verify the total.",
            @"Do not claim every two-digit plus one-digit addition makes a new ten. 43+5=48 does not.",
            @"I can tell when adding composes a new ten and justify with an equation."),
        new(@"PED:US-CCSS-MATH:G1:U05:L08",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-8/preparation.html",@"aad87cbe8f02f23279a42bd4de1e5febd1a53b58cf29e1c41e45294fd6d2c3eb",[@"CCSS:1.NBT.C.4",@"CCSS:1.OA.C.6",@"CCSS:1.OA.D.8"],
            @"Center Day 2",
            @"Teacher-led center day: add within 100, including examples with or without composing a ten. Original 2021 number talk: 68+2=70, 3+68=71, 5+45=50 and 45+7=52. Original Target Numbers Stage 2 STARTS AT 25, chooses a number card and adds that many TENS or ONES, attempting to reach 95 without going over in SIX rounds. The teacher-led center choices are Target Numbers, Number Puzzles and Five in a Row; optional original football stories are arithmetic contexts, not proof of pupil participation.",
            @"Example 1: 38+6=38+2+4=44 (make a ten). Example 2: 32+24=56 (no new ten). Example 3: 57+25=82 (new ten). Original optional football story: home team scores 35 and then 6 more, 35+6=41 points; 9 visiting and 45 home supporters make 9+45=54 fans.",
            @"Step 1: Identify the addends. Step 2: Choose add-tens/ones or make-ten. Step 3: Write a correct equation. Step 4: Verify the total is within 100 and explain your choice.",
            @"Do not reuse a no-regrouping algorithm when ones make ten; 57+25 is 82, not 72.",
            @"I can add within 100 using strategies that fit the numbers."),
        new(@"PED:US-CCSS-MATH:G1:U05:L09",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-9/preparation.html",@"77345a8d0297619605cf994f800beeb7c99b1e74bf8bd70a4598fdbf50575d42",[@"CCSS:1.NBT.C.4"],
            @"Add 2 Two-digit Numbers",
            @"Add two two-digit numbers that compose a ten. Original 2021 student Activity 1 finds 17+36 using a chosen place-value method. Activity 2 Grab and Add has partners combine genuine handfuls of tens towers and singles in multiple rounds, making tens when necessary; digital arithmetic cannot prove physical handful counts. Original number talk 38+2,40+3,38+5,38+15 prepares composing tens.",
            @"Example 1: Original 17+36=53 by 17+30=47, then 47+6=53; 7+6=13 ones creates a new ten. Example 2: 38+25=38+20+2+3=63. Example 3: 46+27=73; six ones plus seven ones make one ten and three ones.",
            @"Step 1: Add tens. Step 2: Combine ones. Step 3: Exchange ten of the ones for one additional ten. Step 4: Record the new number of tens and leftover ones. Step 5: Verify.",
            @"Do not ignore the newly made ten: 38+25 is 63, not 53.",
            @"I can add two two-digit numbers within 100 by composing a new ten."),
        new(@"PED:US-CCSS-MATH:G1:U05:L10",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-10/preparation.html",@"d959d22c069d35452c3261e2d47ece8b5602cd6c284962754be8a8432241201e",[@"CCSS:1.NBT.C.4",@"CCSS:1.OA.D.8"],
            @"Tens and Tens, Ones and Ones",
            @"Add two-digit numbers by explicitly combining TENS with TENS and ONES with ONES, exchanging when there are ten or more ones. Original 2021 pupils interpret Priya drawing for 37+26, finish 28+56 given first step 20+50, finish 27+44 given first step 7+4, then find 34+57 and 18+55. Original Number Puzzles Stage 4 uses one-use digit cards to create within-100 addition WITH composing; physical game is teacher-led.",
            @"Example 1: Original Priya 37+26=(30+20)+(7+6)=50+13=63. Example 2: 28+56=(20+50)+(8+6)=70+14=84. Example 3: 27+44=(20+40)+(7+4)=60+11=71. Example 4: 34+57=91; 18+55=73. In each case exchange ten ones when the ones sum exceeds nine.",
            @"Step 1: Represent each addend as tens and ones. Step 2: Add tens and ones separately. Step 3: Regroup ten ones into one ten. Step 4: Combine to a final total and check.",
            @"Do not mistake 50+13 for 513 or for 53; ten ones represent an additional ten.",
            @"I can explain how tens and ones combine in two-digit addition."),
        new(@"PED:US-CCSS-MATH:G1:U05:L11",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-11/preparation.html",@"0560868a9b518234ee6d9396ab88055086709f55a9d9830b085ed7d4044b29d9",[@"CCSS:1.NBT.C.4",@"CCSS:1.OA.D.7"],
            @"How Did You Do That?",
            @"Analyze two-digit addition methods and explain how different true equations represent the same sum. Original 2021 source cleanup story has 37 plastic bottles and 25 paper wrappers, with Jada, Kiran and Tyler demonstrating THREE distinct methods. Original student next problem is 23+68=91, including optional student-created story and partner gallery discussion. Source warm-up asks whether 24+3=54, 42+5=47 and 42+30=45 are true.",
            @"Example 1: Original Jada method 37+25: 30+20=50;7+5=12;50+12=62. Example 2: Original Kiran method 37+3+2=42;42+20=62. Example 3: Original Tyler method 37+20=57;57+3+2=62. Example 4: Student next task 23+68=91, for example 23+60=83, 83+8=91. Each equality remains true.",
            @"Step 1: Read both addends and one proposed strategy. Step 2: Check each transformation preserves the total. Step 3: Identify where a ten forms. Step 4: Compare with another valid method.",
            @"Do not write 37+25=37+20+2 because three ones disappear.",
            @"I can interpret and compare accurate place-value addition strategies."),
        new(@"PED:US-CCSS-MATH:G1:U05:L12",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-12/preparation.html",@"c62773916f506f2731dbb37579492188cf54f92f50fda9fa2146209e03e6de31",[@"CCSS:1.NBT.C.4"],
            @"Add it Up PLC Activity",
            @"Write equations to show HOW two two-digit numbers can be added, including making a new ten. Original 2021 pupils calculate 48+15 and 57+36 using any valid strategy and explain true equations. Original Reason About Addition provides number cards 37,22,18,56,41 and asks for smallest/largest sums, sums requiring NO new ten, sums composing a ten with some ones left, and sums composing an exact ten. Optional original volunteer stories require classroom review when used.",
            @"Example 1: Original 48+15=63 (48+2+13=50+13=63). Example 2: Original 57+36=93 (57+30+3+3=93). Example 3: Cards 37+22=59 makes no new ten; 22+18=40 composes exactly one new ten; 37+18=55 composes with five ones remaining. From the source card set, smallest pair is 18+22=40 and largest is 56+41=97. Multiple answers satisfy some other card rules.",
            @"Step 1: Choose a strategy (tens/ones or add-on). Step 2: Write the starting addition. Step 3: Show the tens composition as a true equation. Step 4: Write the final sum and verify.",
            @"Do not omit ones or create an equality that changes the value between steps.",
            @"I can write equivalent equations describing my two-digit addition method."),
        new(@"PED:US-CCSS-MATH:G1:U05:L13",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-13/preparation.html",@"bd186fde246e3a3b3ff5dd24dae4d2b54f0a8192642f291372a14de5b2a9228a",[@"CCSS:1.NBT.A.1",@"CCSS:1.NBT.B.3",@"CCSS:1.NBT.C.4",@"CCSS:1.OA.C.5",@"CCSS:1.OA.C.6",@"CCSS:1.OA.D.8"],
            @"Center Day 3",
            @"Original 2021 Center Day 3: count backward by ONES from 100 to 70 in the choral warm-up. Introduce Target Numbers Stage 3 (add TWO-DIGIT numbers) and Five in a Row Stage 6 (add within 100 WITH composing), then choose among other already learned centers. Real board turns, number-card manipulation and partner participation remain teacher-facilitated; digital tasks evaluate only applicable mathematics.",
            @"Example 1: 26+38=64 because 20+30=50 and 6+8=14. Example 2: 54+25=79 because ones make nine and tens make seventy.",
            @"Step 1: Read the actual center prompt. Step 2: Decide whether ten must be composed. Step 3: Use a drawing/equation to add accurately. Step 4: Explain the pattern if a teacher supervises the activity.",
            @"Do not treat a teacher-led game as completed when only an addition answer was entered.",
            @"I can use place-value methods to add within 100 in a classroom center."),
        new(@"PED:US-CCSS-MATH:G1:U05:L14",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-14/preparation.html",@"aba40eb78d9fadc64bea53be9bfdb4c7c5604a2d00880f4339b38d2d95d6ed69",[@"CCSS:1.NBT.C.4",@"CCSS:1.NBT.C.5",@"CCSS:1.NBT.C.6"],
            @"Food Drive (optional)",
            @"ORIGINAL STUDENT FACING TASK: Use the FOUR-STUDENT Food Drive table (Lin 18, Priya 24, Han 13, Tyler 30) to calculate several different pair totals and all four together: 85 cans. Then use the source Day 1 grade table (Kindergarten 18, first 51, second 23, third 13, fourth 39, fifth 40, sixth 8, seventh 29, eighth 30) to find which TWO grades could fill a box with between 35 and 65 cans; optional challenge: can THREE grades share a box? Write each matching equation. The original final student question also asks for the LEAST NUMBER OF BOXES for the whole school: the nine grade counts total 251, so a capacity of at most 65 requires at least FOUR boxes; explain why a four-box arrangement needs a three-grade box when all nine grade groups remain intact. Real donations, packing, pupils, and classroom participation are never presumed or auto-scored.",
            @"Example 1: Lin 18 cans + Priya 24 cans = 42; Han 13 + Tyler 30 = 43; all four students together collected 85 cans. Example 2: From the Day 1 grade table, Kindergarten 18 and second grade 23 make 41, so this pair qualifies for a 35–65-can box. Example 3: First grade 51 + fifth grade 40 = 91, so they cannot share one 35–65-can box. Example 4: Kindergarten 18 + second grade 23 + third grade 13 = 54, so those THREE grades could fit the optional challenge. Example 5: The other original partner pairings are Tyler 30 + Priya 24 = 54 and Lin 18 + Han 13 = 31; both disjoint pairs together make 85. Example 6: The nine grade levels together collect 251 cans, requiring at least 4 boxes since 3 boxes hold at most 195. Four actual boxes are possible: Kindergarten 18 + fourth 39 + sixth 8 = 65, first 51 + third 13 = 64, second 23 + fifth 40 = 63, seventh 29 + eighth 30 = 59. Every grade level is used once and every box is between 35 and 65. The first box has three grade levels, consistent with the optional extension.",
            @"Step 1: Read ALL four original student counts and calculate each named pair; check that the two disjoint pairs still add to 85. Step 2: Read the original Day 1 grade table and choose two distinct grades. Step 3: Add tens and ones, composing a new ten as needed, and decide whether the box has 35–65 cans inclusive. Step 4: Record the two grades, equation, total and qualifying decision. Step 5: For the optional challenge, try three distinct grades and verify the same total interval. Step 6: Answer the original final question about the least number of boxes for ALL nine grades. Add 18+51+23+13+39+40+8+29+30=251, derive at least four boxes from the 65-can maximum, then verify a four-box arrangement that uses each grade exactly once, with one optional three-grade box. Distinguish the at-most-two-grade pairing task from the optional three-grade and minimum-box challenge; do not falsely claim the minimum is four if three-grade boxes are prohibited. Teacher reviews class discussion and real packing if later carried out.",
            @"Do not claim a real food drive took place or that the two groups were personally counted by a student based on a digital question. Do not omit Partner B or the original minimum-box challenge, and do not claim four boxes are possible under a strict two-grades-per-box rule with all nine intact grade groups.",
            @"I can solve a two-digit addition story connected to a food drive when teacher-led.")
    ];
    private static Repair? Find(CanonicalLessonContentPackDocument doc, CanonicalLessonContentPackLesson lesson) =>
        doc.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        doc.VersionCode=="CCSSM-2010"
        ? Repairs.SingleOrDefault(x=>x.Code==lesson.LessonCode):null;
    public static bool IsTarget(CanonicalLessonContentPackDocument doc, CanonicalLessonContentPackLesson lesson)=>Find(doc,lesson)!=null;
    private static Repair Checked(CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson)
    {
        var r=Find(doc,lesson)??throw new InvalidOperationException("Not a new full-grade G1 Unit 5 target");
        if(lesson.SourceUrl!=r.SourceUrl || !string.Equals(lesson.CanonicalBodySha256,r.OriginalSha,StringComparison.OrdinalIgnoreCase)||
           !lesson.OutcomeCodes.SequenceEqual(r.Outcomes))
            throw new InvalidOperationException("G1 U04-U05 source, original body or standards drift: "+lesson.LessonCode);
        return r;
    }
    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        foreach(var lesson in doc.Lessons)
        {
            if(!IsTarget(doc,lesson))continue;
            var r=Checked(doc,lesson);
            var en=lesson.Translations.Single(x=>x.CultureCode=="en");
            if(en.Explanation==r.Explanation&&en.WorkedExamples==r.Examples&&
               en.StepByStepSolutions==r.Steps&&en.CommonMistakes==r.Mistakes&&en.QuickSummary==r.Summary)continue;
            if(!en.WorkedExamples.StartsWith("Example 1:",StringComparison.Ordinal)||
               !en.StepByStepSolutions.StartsWith("Step 1:",StringComparison.Ordinal))
                throw new InvalidOperationException("Original G1 lesson content shape unexpected: "+lesson.LessonCode);
            en.Title=r.Title;en.Explanation=r.Explanation;en.KeyConceptsAndRules=r.Explanation;
            en.WorkedExamples=r.Examples;en.StepByStepSolutions=r.Steps;
            en.CommonMistakes=r.Mistakes;en.QuickSummary=r.Summary;
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson,string previous)=>
       IsTarget(doc,lesson)
           ?(IsUnit4End(lesson.LessonCode)?Unit4EndSourceVersion:
             IsUnit5Early(lesson.LessonCode)?Unit5EarlySourceVersion:
             IsUnit5Later(lesson.LessonCode)?Unit5LaterSourceVersion:Version)
           :previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson,string previous)=>
       IsTarget(doc,lesson)&&
       (previous==CommonCoreRichContentCorrections.CorrectionContentVersion||
        previous==PreviousVersion||
        (previous==Version&&(IsUnit4End(lesson.LessonCode)||IsUnit5Early(lesson.LessonCode)||
           IsUnit5Later(lesson.LessonCode))));
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previous,
        IReadOnlyCollection<CurriculumLessonContentTranslation> translations)
    {
        Checked(doc,lesson);
        if(!CanUpgradeExisting(doc,lesson,previous))
            throw new InvalidOperationException("G1 unit 5 existing version cannot be upgraded safely: "+lesson.LessonCode);
        if(translations.Count==0)return;
        if(translations.Count!=1||translations.Single().CultureCode!="en"||
           !translations.Single().WorkedExamples.StartsWith("Example 1:",StringComparison.Ordinal)||
           !translations.Single().StepByStepSolutions.StartsWith("Step 1:",StringComparison.Ordinal))
            throw new InvalidOperationException("G1 unit 5 existing body drift: "+lesson.LessonCode);
    }
}
