using System.Security.Cryptography;
using System.Text;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Source-bound, Edulytics-authored Grade 1 instructional repair candidates.
/// Addresses lessons where generated example arithmetic was unrelated to the
/// publisher's learning goal (or taught negative integer subtraction in G1).
/// These edits are NOT independent mathematics reviewer signoff.
/// Original third-party source packs are kept byte-for-byte unchanged.
/// </summary>
public static class CommonCoreG1SourceGroundedInstructionalCorrections
{
    public const string DataQuestionsLessonCode = "PED:US-CCSS-MATH:G1:U01:L13";
    public const string JungleSurveyLessonCode = "PED:US-CCSS-MATH:G1:U01:L15";
    public const string TensLessonCode = "PED:US-CCSS-MATH:G1:U04:L03";

    public const string DataQuestionsVersion = "us-ccss-g1-u01-l13-data-questions-correction-v1";
    public const string JungleSurveyVersion = "us-ccss-g1-u01-l15-survey-and-tallies-correction-v1";
    public const string TensVersion = "us-ccss-g1-u04-l03-tens-strategy-correction-v1";

    private sealed record Repair(
        string Code, string ExpectedUrl, string OriginalCanonicalSha,
        string OldBodySha, string CorrectedVersion, string[] Outcomes,
        string OldSteps, string Concepts, string Examples, string Steps,
        string CommonMistakes);

    private const string OldAddSteps =
        "Step 1: Identify the two quantities, 7 and 1.\n" +
        "Step 2: Start at 7 and count on 1 more, or combine the two groups.\n" +
        "Step 3: The total is 8, so 7 + 1 = 8.\n" +
        "Step 4: Check that the result matches the quantities and the lesson goal.";

    private const string OldJungleSteps =
        "Step 1: Identify the starting quantity 1 and the amount 4 being removed or compared.\n" +
        "Step 2: Use subtraction to find the difference.\n" +
        "Step 3: 1 − 4 = -3.\n" +
        "Step 4: Check that the answer fits the original situation.";

    private const string OldTensSteps =
        "Step 1: Identify the starting quantity 2 and the amount 3 being removed or compared.\n" +
        "Step 2: Use subtraction to find the difference.\n" +
        "Step 3: 2 − 3 = -1.\n" +
        "Step 4: Check that the answer fits the original situation.";

    public const string DataQuestionsConcepts =
        "Learning goals: Ask and answer questions about information shown in a picture graph, " +
        "tally chart, or category-count table.\n" +
        "The categories, labels, and counts tell which questions can be answered from the display. " +
        "You can find the number in a category, compare two category counts, or add counts to find a total. " +
        "Do not answer a question if the required information is not displayed.";

    public const string DataQuestionsExamples =
        "Example 1 — Can the graph answer the question? A chart shows 7 children chose art, " +
        "5 chose music, and 3 chose sports. A question you can answer is: " +
        "How many chose music? Answer: 5 children, because the music category has 5.\n" +
        "Example 2 — Ask a comparison question. How many more children chose art than sports? " +
        "Use the category counts: 7 - 3 = 4 children. Check: 3 + 4 = 7.\n" +
        "Example 3 — Ask about the total. How many children took part? " +
        "Add 7 + 5 + 3 = 15 children.\n" +
        "Example 4 — Recognize a question the data cannot answer. " +
        "What is each child's favorite color? The display shows activity choices, not colors. " +
        "We need different data before answering.";

    public const string DataQuestionsSteps =
        "Step 1: Read the graph or table title, all category labels, and the count for each category.\n" +
        "Step 2: Ask a question that uses information the display actually provides.\n" +
        "Step 3: Find the relevant category count, compare two counts, or combine the counts. " +
        "For 7 art and 3 sports votes, 7 - 3 = 4 more chose art.\n" +
        "Step 4: Write an answer including what was counted, such as 4 children.\n" +
        "Step 5: Point to the labels and counts that support your answer; " +
        "identify questions the display does not answer.";

    public const string DataQuestionsMistakes =
        "Mistake: guessing from the height or area of a picture without reading labels and counts. " +
        "Correction: read the actual values or the scale.\n" +
        "Mistake: asking about information not collected in the survey. " +
        "Correction: first decide whether the display contains the necessary category.\n" +
        "Mistake: adding categories without saying what is counted. " +
        "Correction: include the people or objects counted in the answer.";

    public const string JungleConcepts =
        "Learning goals: Create a survey question, collect responses, organize them into " +
        "categories, show the results, and ask and answer questions about the data.\n" +
        "Each vote belongs to one named category. Tally marks or drawings help keep accurate counts. " +
        "The total votes should match the number of recorded responses. " +
        "Explain what the category counts show, and use the results to make a new question.";

    public const string JungleExamples =
        "Example 1 — Plan the survey. Ask: Which jungle animal would you most like to see, " +
        "a monkey, lion, or parrot? Every respondent chooses just one of the three.\n" +
        "Example 2 — Record the votes. Suppose 4 children choose monkeys, " +
        "3 choose lions, and 2 choose parrots. Use four tally marks for monkeys, " +
        "three for lions, and two for parrots; label every category.\n" +
        "Example 3 — Answer a question. How many children answered? 4 + 3 + 2 = 9. " +
        "How many more chose monkeys than parrots? 4 - 2 = 2.\n" +
        "Example 4 — Interpret the display. Monkeys have the most votes (4) " +
        "and parrots have the fewest (2). A larger picture of a parrot does not " +
        "mean more votes; use the tallies.";

    public const string JungleSteps =
        "Step 1: Choose a survey question about jungle animals and list possible responses.\n" +
        "Step 2: Collect one answer from each person; record a tally for the chosen category.\n" +
        "Step 3: Count the tallies and create a labeled table or picture graph. " +
        "For 4 monkeys, 3 lions, and 2 parrots, show counts 4, 3, and 2.\n" +
        "Step 4: Ask questions the data can answer: Which has the most votes? " +
        "How many votes in all? Find 4 + 3 + 2 = 9.\n" +
        "Step 5: Check that the category totals equal the responses collected. " +
        "Explain your findings and compare with a classmate.";

    public const string JungleMistakes =
        "Mistake: putting a response in two categories. Correction: count one vote per person.\n" +
        "Mistake: using different-sized symbols as if they were equal votes. " +
        "Correction: use one tally per response, or state the graph key.\n" +
        "Mistake: making a subtraction story with an impossible negative number of animals. " +
        "Correction: compare the counts using a greater count minus a smaller count.";

    public const string TensConcepts =
        "Learning goals: Add 10 to and subtract 10 from whole-number multiples of 10, " +
        "using towers of ten, drawings, and skip-counting.\n" +
        "Each tower holds 10 cubes. Adding one tower adds 10 to the number; " +
        "removing one tower subtracts 10 when at least one tower is present.\n" +
        "Keep the number of tens and the total number of cubes distinct: " +
        "three tens are 30 cubes, not 3 cubes.";

    public const string TensExamples =
        "Example 1 — Add a ten. Start with two towers of 10 cubes (20 cubes). " +
        "Add one tower: 20 + 10 = 30 cubes, or three tens.\n" +
        "Example 2 — Subtract a ten. Start with six towers of 10 cubes (60). " +
        "Remove one tower: 60 - 10 = 50, or five tens.\n" +
        "Example 3 — Use place value. For 80 - 10, think eight tens minus one ten. " +
        "Eight tens become seven tens: 80 - 10 = 70.\n" +
        "Example 4 — Check by reversing the operation: 40 + 10 = 50, " +
        "so 50 - 10 = 40. Show the result with towers or a number line.";

    public const string TensSteps =
        "Step 1: Read the starting multiple of 10; 60 means six groups of ten cubes.\n" +
        "Step 2: Decide whether the task adds one group of ten or takes one away.\n" +
        "Step 3: Model the action with ten-cube towers or jump forward/backward " +
        "once by 10 on a number line.\n" +
        "Step 4: Write the matching equation with multiples of ten: 60 - 10 = 50.\n" +
        "Step 5: Check by the inverse fact 50 + 10 = 60 and describe how " +
        "the number of tens changed.";

    public const string TensMistakes =
        "Mistake: reading three towers as 3 cubes. Correction: each tower is worth 10, " +
        "so three towers represent 30.\n" +
        "Mistake: subtracting 1 instead of 10 when removing one tower. " +
        "Correction: one tower means one ten.\n" +
        "Mistake: using a negative one-digit subtraction example rather than " +
        "the intended tens relationship. Correction: model 60 - 10 = 50 with towers.";

    public const string CountBackOneOrTwoLessonCode = "PED:US-CCSS-MATH:G1:U01:L04";
    public const string CountBackOneOrTwoVersion = "us-ccss-g1-u01-l04-count-back-one-two-v1";
    public const string CountBackOneOrTwoConcepts = "Learning goal: Subtract 1 or 2 within 10 by counting backward one step for each counter removed. When you begin at 6 and remove 2, count 5, 4 to find 4 left. You can show each subtraction on a ten-frame or number path. Addition checks a subtraction: 6 - 2 = 4 because 4 + 2 = 6.";
    public const string CountBackOneOrTwoExamples = "Example 1 — Subtract one: Show 6 counters. Remove 1 counter; 5 remain. The matching equation is 6 - 1 = 5. Check 5 + 1 = 6.\nExample 2 — Subtract two: On a number path, start at 6 and take two steps back: 6 to 5, then 5 to 4. Therefore 6 - 2 = 4.\nExample 3 — Another starting number: Begin at 8, count back one to 7. Thus 8 - 1 = 7 and 7 + 1 = 8.\nExample 4 — Reach zero: Put out 2 counters, take both away, and see 0 left. Therefore 2 - 2 = 0.";
    public const string CountBackOneOrTwoSteps = "Step 1: Read the starting quantity and the number being taken away (for example, 6 and 2).\nStep 2: Put 6 counters on a ten-frame or mark 6 on a number path.\nStep 3: Remove 2 counters or move back two distinct steps: from 6 to 5, then to 4.\nStep 4: State the remaining quantity and equation: 6 - 2 = 4.\nStep 5: Check by adding the removed amount back: 4 + 2 = 6.";
    public const string CountBackOneOrTwoMistakes = "Mistake: counting the starting number 6 as the first backward step. Correction: the first step from 6 lands on 5, and the second lands on 4.\nMistake: adding 2 instead of subtracting it. Correction: removal makes the quantity smaller or the same, never larger.\nMistake: writing 6 - 2 = 5 after one step. Correction: make two moves or remove two physical counters.";

    public const string CenterAddSubtractLessonCode = "PED:US-CCSS-MATH:G1:U01:L10";
    public const string CenterAddSubtractVersion = "us-ccss-g1-u01-l10-center-add-subtract-v1";
    public const string CenterAddSubtractConcepts = "Learning goals: Add and subtract within 10 and explain how one addition fact helps solve a related subtraction fact. Use connecting cubes, a ten-frame, or counting on and back. A missing addend can be found by subtraction: 6 + ? = 10 means 10 - 6 = 4. Each number in a fact must match the represented cubes.";
    public const string CenterAddSubtractExamples = "Example 1 — Make 10 with cubes. Place 6 red and 4 blue cubes into a ten-frame; all ten spaces are full. Write 6 + 4 = 10.\nExample 2 — Take away from 10. Keep the same frame but remove 4 blue cubes. Six red cubes remain: 10 - 4 = 6. Check that 6 + 4 = 10.\nExample 3 — Find the missing part. A frame holds 7 counters. How many more reach 10? Three empty spaces show 7 + 3 = 10; 10 - 7 = 3.\nExample 4 — Subtract within 10. Start with 9 cubes, hide 2, and count 7 visible cubes: 9 - 2 = 7. Check 7 + 2 = 9.";
    public const string CenterAddSubtractSteps = "Step 1: Identify what is known: a whole, its parts, and whether the question asks for a sum or a part.\nStep 2: Model the numbers with a ten-frame or two colors of counters. For 6 and 4, fill 6 red spaces and 4 blue spaces.\nStep 3: Write and solve the addition equation: 6 + 4 = 10.\nStep 4: Remove the four blue counters and write the related subtraction: 10 - 4 = 6.\nStep 5: Check each expression against the ten-frame and explain how addition and subtraction undo one another.";
    public const string CenterAddSubtractMistakes = "Mistake: treating a quoted classroom reflection as a solved math example. Correction: give a numerical problem and its worked solution.\nMistake: writing 10 - 4 = 4 after four counters are removed. Correction: count the six remaining counters and verify 6 + 4 = 10.\nMistake: reading a missing-addend equation as a command to add 10. Correction: show the known part and count the empty spaces to find the unknown.";

    public const string PuppiesTulipsCompareLessonCode = "PED:US-CCSS-MATH:G1:U06:L17";
    public const string PuppiesTulipsCompareVersion = "us-ccss-g1-u06-l17-puppies-tulips-compare-v1";
    public const string PuppiesTulipsCompareConcepts = "Learning goals: Tell, model, and solve Compare story problems about puppies and origami tulips using 'more' and 'fewer'. Compare two non-negative quantities by matching objects one-to-one, then count the unmatched objects. Use addition to find a larger group given the smaller group and the difference, or subtraction to find the difference when both groups are known. ORIGINAL AUTHORING TASK: Create your own puppy-and-tulip comparison problem, state enough quantities and a question, read it to a partner group, and have your classmates represent it with drawings, numbers, words or equations. Partner role-play requires actual teacher facilitation.";
    public const string PuppiesTulipsCompareExamples = "Example 1 — Find the difference. Lina makes 7 puppy models and 4 tulips. Match four puppies to four tulips; three puppies remain unmatched. She has 7 - 4 = 3 more puppies than tulips.\nExample 2 — Fewer means the same difference. In the same display, there are 3 fewer tulips than puppies because 4 + 3 = 7.\nExample 3 — Find the larger group. Amir makes 5 tulips and 3 more puppies than tulips. He makes 5 + 3 = 8 puppies. Check 8 - 5 = 3.\nExample 4 — Find the smaller group. There are 9 puppies, which is 2 more than the number of tulips. Let t be the tulip count: t + 2 = 9, so t = 7. Check 9 - 7 = 2.";
    public const string PuppiesTulipsCompareSteps = "Step 1: Read the two types of objects and decide whether 'more' or 'fewer' compares their counts.\nStep 2: Draw or place one marker for each puppy and each tulip; pair as many markers as possible.\nStep 3: If both counts are known, subtract the smaller from the larger: 7 - 4 = 3 unpaired puppy models.\nStep 4: If one count and the difference are known, add or subtract to find the missing group: 5 + 3 = 8 puppies.\nStep 5: Check the story, units, and equation by matching groups or using the inverse fact.\nStep 6: Write an original puppies-and-tulips Compare story with a question and enough data, exchange it with a partner group, and ask them to act it out, show their thinking, and write a matching equation. The teacher reviews authored stories when this optional activity is taught.";
    public const string PuppiesTulipsCompareMistakes = "Mistake: subtracting the greater count from the smaller and reporting a negative number of models. Correction: difference in this story is 7 - 4 = 3 unmatched objects.\nMistake: treating 'more' as an instruction to add in every story. Correction: if both counts are given, subtract to find how many more.\nMistake: omitting what the number represents. Correction: say '3 more puppy models than tulips' instead of only '3'.";

    public const string CompareRepresentedNumbersLessonCode = "PED:US-CCSS-MATH:G1:U04:L21";
    public const string CompareRepresentedNumbersVersion = "us-ccss-g1-g1-u04-l21-topic-correction-v1";
    public const string CompareRepresentedNumbersConcepts = "Compare two-digit numbers even when their ten-rods and ones are grouped differently. Exchange ten ones for one ten before using <, >, or =.";
    public const string CompareRepresentedNumbersExamples = "A has 4 tens and 2 ones: 40 + 2 = 42. B has 3 tens and 12 ones: 30 + 12 = 42. Therefore A = B.\nA model shows 5 tens and 1 one: 51. Another shows 4 tens and 9 ones: 49. Therefore 51 > 49.\nA model shows 2 tens and 16 ones: 36. Another shows 3 tens and 7 ones: 37. Therefore 36 < 37.\nThe original activity compares Elena's 5 tens and 32 ones (82) with Kiran's 7 tens and 2 ones (72): 82 > 72.";
    public const string CompareRepresentedNumbersSteps = "Step 1: Count tens and individual ones in each model.\nStep 2: Regroup ten ones into a ten when needed.\nStep 3: Calculate each two-digit value, for example 30 + 12 = 42.\nStep 4: Compare the values and write <, >, or =.\nStep 5: Check the inequality against both models.";
    public const string CompareRepresentedNumbersMistakes = "Mistake: deciding the drawing with more individual cubes has the greater value. Correction: each ten-rod stands for ten units. Mistake: reading 3 tens and 12 ones as 312. Correction: 30 + 12 = 42.";

    public const string CompareLengthsIndirectlyLessonCode = "PED:US-CCSS-MATH:G1:U06:L02";
    public const string CompareLengthsIndirectlyVersion = "us-ccss-g1-g1-u06-l02-topic-correction-v1";
    public const string CompareLengthsIndirectlyConcepts = "Compare the lengths of two objects indirectly using the SAME unchanged third reference (for example, one marked string) with aligned endpoints. ORIGINAL 2021 Student Facing: find TWO classroom objects longer than an unsharpened pencil and TWO shorter than that same pencil, and write a comparison sentence for EACH object. Then compare the length of your desk side with one desk leg using one unchanged string, and explain with a drawing or words. The real objects, measurements and drawing require teacher observation; a digital number alone does not certify the activity.";
    public const string CompareLengthsIndirectlyExamples = "Original 2021 number talk: 35+20=55, 35+25=60, 30+45=75, 37+45=82. These arithmetic questions are a warm-up, NOT the indirect-length learning objective.\nSource pencil comparison: if a classroom ruler is longer than the unsharpened pencil and a crayon is shorter than that pencil, write one true sentence about EACH chosen object; find two objects on each side of the comparison.\nFor the original desk-side versus desk-leg task, compare both lengths with the SAME unchanged string. Hypothetical illustration: if the desk side is longer than an 8-unit marked string but the leg is shorter than that same string, then the desk side is longer than the leg; do not present 8 as an actual student's measured desk length.";
    public const string CompareLengthsIndirectlySteps = "Step 1: In the ORIGINAL Student Facing activity, identify the unsharpened pencil as the common reference.\nStep 2: Find and name TWO classroom objects longer than it and TWO other objects shorter than it. Align endpoints and write FOUR complete comparison sentences.\nStep 3: For the second ORIGINAL task, choose one piece of string as the SAME comparison tool for your desk's side and one desk leg; keep its length fixed.\nStep 4: Compare each desk length against the unchanged string with endpoints aligned; do not compare different strings or stretch one.\nStep 5: Draw or write an explanation of which desk length is longer and WHY; a teacher checks the physical comparison, evidence and language.";
    public const string CompareLengthsIndirectlyMistakes = "Do not answer only the original number talk as if that demonstrates indirect measurement. Do not give only one longer and one shorter pencil example: the source requires TWO of each and complete sentences. Do not change the reference string, stretch it or misalign endpoints when comparing the desk side with the desk leg. Real measured lengths cannot be invented from generic examples.";

    public const string MeasureConnectingCubeLengthLessonCode = "PED:US-CCSS-MATH:G1:U06:L05";
    public const string MeasureConnectingCubeLengthVersion = "us-ccss-g1-g1-u06-l05-topic-correction-v1";
    public const string MeasureConnectingCubeLengthConcepts = "Measure an object's length by laying equal-sized connecting cubes end to end from one endpoint to the other, without gaps or overlaps. The answer is a number of cube-length units, not a count of faces.";
    public const string MeasureConnectingCubeLengthExamples = "A caterpillar lines up exactly with a straight tower of 4 cubes. Its length is 4 cube lengths.\nA pencil lines up with 9 equal cubes and an eraser with 3 of the same cubes. The pencil is 9 - 3 = 6 cube lengths longer.\nA tower has 8 cubes, but 1 starts before the object's left edge and 1 ends past its right edge. Only the 6 cubes between the endpoints measure the object: 8 - 2 = 6.";
    public const string MeasureConnectingCubeLengthSteps = "Step 1: Find both endpoints of the object to measure.\nStep 2: Build a straight tower of equal connecting cubes without gaps.\nStep 3: Align the beginning of the first cube with the first endpoint.\nStep 4: Count only cubes that reach from the first to the last endpoint.\nStep 5: Report the length in cubes and check for extra cubes outside the object.";
    public const string MeasureConnectingCubeLengthMistakes = "Mistake: counting all 8 cubes of an oversized tower. Correction: count only the 6 that align with the object. Mistake: counting cube faces instead of their side-length units. Correction: one equal cube gives one unit of length.";

    public const string ComposeThreeDimensionalShapesLessonCode = "PED:US-CCSS-MATH:G1:U07:L02";
    public const string ComposeThreeDimensionalShapesVersion = "us-ccss-g1-g1-u07-l02-topic-correction-v1";
    public const string ComposeThreeDimensionalShapesConcepts = "Construct a larger three-dimensional solid by joining smaller solids and explain which blocks and faces were used. Naming faces without actually composing shapes does not satisfy this lesson.";
    public const string ComposeThreeDimensionalShapesExamples = "Join 2 identical cubes face to face in a straight row. The new rectangular-prism shape is 2 cubes long, 1 cube wide and 1 cube high.\nArrange 4 equal cubes in a 2-by-2 flat layer, aligning whole faces. The resulting flat rectangular-prism shape is one cube high and uses 4 cubes.\nStack 2 matching layers of 4 cubes each to make a box-shaped solid. The composite three-dimensional shape uses 4 + 4 = 8 cubes; sketch the two layers to show their composition.";
    public const string ComposeThreeDimensionalShapesSteps = "Step 1: Choose a new three-dimensional shape to build.\nStep 2: Select smaller blocks that can join on matching faces.\nStep 3: Join the blocks, describing how their faces connect.\nStep 4: Count the cubes in rows or layers, such as 4 + 4 = 8.\nStep 5: Identify the resulting composed solid and show a different arrangement when possible.";
    public const string ComposeThreeDimensionalShapesMistakes = "Mistake: simply listing that a cube has 6 faces rather than constructing a solid. Correction: join cubes to make a new object. Mistake: counting two layers as 2 cubes. Correction: count every cube in both layers.";

    public const string ReadHalfPastClockLessonCode = "PED:US-CCSS-MATH:G1:U07:L14";
    public const string ReadHalfPastClockVersion = "us-ccss-g1-g1-u07-l14-topic-correction-v1";
    public const string ReadHalfPastClockConcepts = "ORIGINAL STUDENT FACING TASK: SORT the clock cards into O'CLOCK (color BLUE) and HALF PAST (color RED) and WRITE each time in words. When half past, the long minute hand points to 6 and the short hour hand is halfway between the current hour and the next. When o'clock, the long hand points to 12. Half a circular clock represents 30 minutes. Digital Practice checks one color, time and correct words per item; the complete physical card sorting and coloring remain teacher-led.";
    public const string ReadHalfPastClockExamples = "For 3:30, the long hand points to 6 and the short hand halfway between 3 and 4. Read half past three.\nAt 3:00, the minute hand points to 12 and hour hand to 3. At 3:30, the minute hand points to 6 and hour hand advances toward 4.\nFor 12:30, point the long hand to 6 and the short hand between 12 and 1. Read half past twelve.";
    public const string ReadHalfPastClockSteps = "Step 1: Identify the longer minute hand and shorter hour hand.\nStep 2: Place the minute hand at 6 to show half a turn around the clock.\nStep 3: Locate the current hour and the next hour; for 3:30 these are 3 and 4.\nStep 4: Put the short hand halfway between those two numbers.\nStep 5: Read half past three and verify both hands.";
    public const string ReadHalfPastClockMistakes = "Mistake: placing the short hand exactly on 4 at 3:30. Correction: it is halfway from 3 to 4. Mistake: calling the clock 6:03 because the minute hand touches 6. Correction: the 6 position means 30 minutes.";

    public const string WriteClockTimesLessonCode = "PED:US-CCSS-MATH:G1:U07:L15";
    public const string WriteClockTimesVersion = "us-ccss-g1-g1-u07-l15-topic-correction-v1";
    public const string WriteClockTimesConcepts = "ORIGINAL STUDENT FACING TASK: START AT 12 and COUNT the minutes clockwise in five-minute hops until HALFWAY around the clock, then CIRCLE where you stop (6). Six five-minute hops equal 30 minutes. Afterwards WRITE the time shown on each analog clock, including whole hours and half past. Minute hand at 12 means :00; at 6 it means :30. Student counting, circling and multi-clock writing are teacher-reviewed; digital checks count and recognize a clock separately.";
    public const string WriteClockTimesExamples = "Minute hand on 12 and hour hand on 2 shows 2:00, two o'clock.\nMinute hand on 6 and hour hand halfway from 4 to 5 shows 4:30, half past four.\nMinute hand on 6 and hour hand halfway from 12 to 1 shows 12:30, not 1:30.";
    public const string WriteClockTimesSteps = "Step 1: Inspect the long minute hand; 12 indicates :00 and 6 indicates :30.\nStep 2: Inspect the short hour hand, including whether it is between numbers.\nStep 3: For half past, use the earlier hour of those two numbers.\nStep 4: Write hour, colon, and two digits, e.g. 4:30.\nStep 5: Compare the written time with both hands again.";
    public const string WriteClockTimesMistakes = "Mistake: writing 4:06 because the long hand points at 6. Correction: it points to the half-hour mark, so write :30. Mistake: choosing hour 5 when the shorter hand lies between 4 and 5. Correction: write 4:30.";

    public const string ReadClockDailyScheduleLessonCode = "PED:US-CCSS-MATH:G1:U07:L16";
    public const string ReadClockDailyScheduleVersion = "us-ccss-g1-g1-u07-l16-topic-correction-v1";
    public const string ReadClockDailyScheduleConcepts = "Read, draw, and write o'clock and half-past times in a daily schedule, with particular care when minute and hour hands point near 6 or 12. ORIGINAL STUDENT FACING TASK: create your OWN ideal Sunday schedule with time, activity and drawn analog clock entries; the teacher reviews each authored entry. A single automatically graded clock-reading question cannot prove the learner constructed a schedule.";
    public const string ReadClockDailyScheduleExamples = "Breakfast at 6:00: minute hand on 12 and hour hand on 6.\nPlay time at 6:30: long hand on 6 and short hand halfway from 6 to 7; the time is not 12:30.\nLunch at 12:30: long hand on 6 and short hand halfway from 12 to 1.\nAn afternoon schedule says 2:00 then 2:30; draw a clock for each and explain which hands moved.";
    public const string ReadClockDailyScheduleSteps = "Step 1: Read a schedule time and identify whether the minutes are :00 or :30.\nStep 2: Put the long minute hand on 12 for :00 or on 6 for :30.\nStep 3: Put the short hour hand on the hour number for :00, or halfway to the next for :30.\nStep 4: Write YOUR ideal Sunday schedule with a time and activity for each entry, and DRAW and label its matching analog clock; this learner-created work is teacher-reviewed.\nStep 5: Double-check 6:30 versus 12:30 by looking at the shorter hand.";
    public const string ReadClockDailyScheduleMistakes = "Mistake: confusing 6:30 and 12:30 because the minute hand points to 6. Correction: inspect the hour hand. Mistake: keeping the hour hand on 6 at 6:30. Correction: move it halfway toward 7.";

    public const string SortMathToolsLessonCode = "PED:US-CCSS-MATH:G1:U01:L07";
    public const string SortMathToolsVersion = "us-ccss-g1-g1-u01-l07-instruction-v1";
    public const string SortMathToolsConcepts = "Sort familiar math tools into labeled categories and count how many objects are in each. Each object must belong to exactly one chosen category; category labels and accurate counts make a display meaningful.";
    public const string SortMathToolsExamples = "Example 1 — Make categories. Sort 4 pattern blocks, 3 two-color counters and 2 inch tiles into three labeled piles. Count 4 blocks, 3 counters, and 2 tiles.\nExample 2 — Make a display. Draw a picture graph with rows labeled 'pattern blocks', 'counters' and 'inch tiles' and show 4, 3 and 2 identical marks. Each mark stands for one tool.\nExample 3 — Read and compare. Four blocks versus two tiles gives 4 - 2 = 2 more pattern blocks. Altogether there are 4 + 3 + 2 = 9 tools.\nExample 4 — Explain another rule. Re-sort the same tools into 'can show a shape' and 'can show a count'; choose one rule per display and explain where every tool goes.";
    public const string SortMathToolsSteps = "Step 1: Choose and say the rule used to sort the math tools.\nStep 2: Create distinct category labels for pattern blocks, two-color counters and inch tiles.\nStep 3: Place each tool in one category and count the objects.\nStep 4: Show those counts using one picture mark for one tool, or a labeled table.\nStep 5: Ask a question about the counts; for 4 blocks and 2 tiles, 4 - 2 = 2 more blocks.";
    public const string SortMathToolsMistakes = "Mistake: mixing two sorting rules in one graph. Correction: choose and state one rule before sorting.\nMistake: placing the same tool in two categories. Correction: count each item exactly once.\nMistake: using bigger pictures to imply more tools. Correction: one equal mark must mean one tool.";

    public const string ClassPetSurveyStatementsLessonCode = "PED:US-CCSS-MATH:G1:U01:L11";
    public const string ClassPetSurveyStatementsVersion = "us-ccss-g1-g1-u01-l11-instruction-v1";
    public const string ClassPetSurveyStatementsConcepts = "Read a class-pet tally chart, write statements supported by data, and test whether a proposed statement is true or false. Five tallies are often shown as a group of four crossed by a fifth mark, but each tally still counts as one response.";
    public const string ClassPetSurveyStatementsExamples = "Example 1 — Read tally data. A class survey reports cats: 6, dogs: 4, and fish: 2. These are three labeled categories with 12 responses in all: 6 + 4 + 2 = 12.\nExample 2 — Check a true statement. 'More children chose cats than dogs' is true because the cat count is 6 and the dog count is 4; 6 > 4.\nExample 3 — Check a false statement. 'Dogs were chosen by 5 children' is false because the dog tally count is 4, not 5.\nExample 4 — Write a comparison. There are 6 - 2 = 4 more votes for cats than for fish. This statement can be verified from the chart.";
    public const string ClassPetSurveyStatementsSteps = "Step 1: Read the survey question and category labels, for example cats, dogs and fish.\nStep 2: Count each tally carefully, remembering a crossed bundle of five marks represents five.\nStep 3: Choose a statement to test; identify the two categories or total it refers to.\nStep 4: Compare the numbers in the data to decide whether the statement is true or false.\nStep 5: Write your own supported statement, such as 6 - 2 = 4 more cat votes than fish votes.";
    public const string ClassPetSurveyStatementsMistakes = "Mistake: reading a crossed group of five tallies as four. Correction: the diagonal stroke is a fifth tally.\nMistake: deciding a statement is true without reading its category labels. Correction: point to the supporting counts.\nMistake: treating personal pet preferences as survey results. Correction: use only the responses collected.";


    public const string HowManyLessonCode = "PED:US-CCSS-MATH:G1:U01:L12";
    public const string HowManyVersion = "us-ccss-g1-u01-l12-data-tallies-and-totals-v1";
    public const string HowManyConcepts =
        "Learning goals: Read labeled tables, tally marks and numerals; tell how many objects are in each category and how many in all. " +
        "Each tally mark stands for exactly one object. A group of four marks crossed by a fifth represents five, not four. " +
        "Add category counts without counting any category twice.";
    public const string HowManyExamples =
        "Example 1 — Read a number table. The chart shows 4 books, 3 blocks and 2 pencils. How many books? 4 books.\n" +
        "Example 2 — Count tallies. Five crossed tally marks represent 5 votes, not 4. Two more marks make 7.\n" +
        "Example 3 — Add the categories. For books 4, blocks 3 and pencils 2, find 4 + 3 + 2 = 9 objects in all. Count each group once.\n" +
        "Example 4 — Check with a different display. Draw four identical marks for books, three for blocks and two for pencils; there are nine marks in all.";
    public const string HowManySteps =
        "Step 1: Read the title and three category labels on the table or tally chart.\n" +
        "Step 2: Read the numeral or count each tally carefully, including the diagonal fifth mark.\n" +
        "Step 3: To answer 'how many in a category', name that category and its count.\n" +
        "Step 4: To answer 'how many in all', add the counts of every category once. For 4, 3 and 2, 4 + 3 + 2 = 9.\n" +
        "Step 5: Point to every count to check that the total matches the data.";
    public const string HowManyMistakes =
        "Mistake: answering 'in all' by choosing the largest group. Correction: add every category count once.\n" +
        "Mistake: counting a tally group with a diagonal stroke as four. Correction: the crossed tally is the fifth mark.\n" +
        "Mistake: dropping the category label when reporting a number. Correction: answer with both the number and what it counts.";

    public const string CenterThreeLessonCode = "PED:US-CCSS-MATH:G1:U01:L14";
    public const string CenterThreeVersion = "us-ccss-g1-u01-l14-sort-display-centers-v1";
    public const string CenterThreeConcepts =
        "Learning goals: Sort 10 to 20 classroom objects into two or three named groups, record a display, and practise addition and subtraction within 10. " +
        "Each object appears once in the display; a category name explains the sorting rule. " +
        "Addition combines and subtraction takes away or finds a missing part.";
    public const string CenterThreeExamples =
        "Example 1 — Sort and display. Sort 12 items by type: 5 counters, 4 tiles and 3 blocks. Draw five, four and three equal marks in the labeled rows; 5 + 4 + 3 = 12.\n" +
        "Example 2 — Check your sorting rule. If a counter is drawn under two categories, the picture graph incorrectly shows 13 objects. Move it to the correct one category only.\n" +
        "Example 3 — Addition center. Start with 4 counters and add 3. Count on 5, 6, 7; 4 + 3 = 7.\n" +
        "Example 4 — Subtraction center. Begin with 9 tiles and remove 2. Seven remain; 9 - 2 = 7. Check with 7 + 2 = 9.";
    public const string CenterThreeSteps =
        "Step 1: Choose a sorting rule and prepare two or three category labels.\n" +
        "Step 2: Sort 10–20 classroom objects; place each object in exactly one group.\n" +
        "Step 3: Draw one same-sized mark per object and add the category counts to check the total. For 5, 4 and 3, the total is 12.\n" +
        "Step 4: At an arithmetic center, solve an addition within 10, then a subtraction within 10 using counters.\n" +
        "Step 5: Explain your sorting rule and check the arithmetic by recounting or using a related fact.";
    public const string CenterThreeMistakes =
        "Mistake: placing one item in two sorting groups. Correction: one sorting rule gives each item exactly one group.\n" +
        "Mistake: drawing pictures of different sizes as if each stood for different counts. Correction: use one equal mark for each object.\n" +
        "Mistake: changing subtraction into addition when removing counters. Correction: model the action with counters and check the inverse addition fact.";

    public const string ChooseIndirectLengthComparatorLessonCode = "PED:US-CCSS-MATH:G1:U06:L03";
    public const string ChooseIndirectLengthComparatorVersion = "us-ccss-g1-g1-u06-l03-instruction-v1";
    public const string ChooseIndirectLengthComparatorConcepts = "Choose an appropriate third object to compare two lengths indirectly. The same comparison tool must retain its length, and the object's endpoints must be aligned each time; a string with marked endpoints may be used when the objects cannot be moved.";
    public const string ChooseIndirectLengthComparatorExamples = "Example 1 — Pick a tool. Two classroom shelves cannot be moved side by side, so use a piece of string with the endpoints of Shelf A marked. Hold it beside Shelf B without stretching it. If B ends before the marked string endpoint, Shelf A is longer.\nExample 2 — Use a common pencil. A ribbon is longer than the pencil, while a crayon is shorter than that same pencil. The ribbon is longer than the crayon.\nExample 3 — Check whether a comparison is supported. If both ribbons are longer than the pencil, we still do not know which ribbon is longer. Choose a longer shared reference or mark their endpoints on the same string.\nExample 4 — Choose a flexible comparator. To compare two curved paper strips, trace each strip with the same non-stretching string, then straighten the marked lengths against a common baseline.";
    public const string ChooseIndirectLengthComparatorSteps = "Step 1: Name the two objects whose lengths cannot be compared directly.\nStep 2: Select a portable third object such as a marked piece of string or a pencil.\nStep 3: Align one end and compare the first object's length against the unchanged third object.\nStep 4: Make the same comparison with the second object, keeping the reference length unchanged.\nStep 5: Explain whether the evidence proves longer, shorter, or equal lengths; choose a new reference if it does not.";
    public const string ChooseIndirectLengthComparatorMistakes = "Mistake: stretching the string when measuring the second item. Correction: use non-stretching string and keep endpoint marks unchanged.\nMistake: forgetting to align the starting ends. Correction: line up both starting points.\nMistake: claiming A is longer than B just because both exceed the reference. Correction: that evidence alone cannot rank them.";

    public const string ClockHourIntroductionLessonCode = "PED:US-CCSS-MATH:G1:U07:L13";
    public const string ClockHourIntroductionVersion = "us-ccss-g1-g1-u07-l13-instruction-v1";
    public const string ClockHourIntroductionConcepts = "ORIGINAL STUDENT FACING TASK: compare two displayed clocks and explain how you know whether they SHOW THE SAME TIME, connect analog-clock hands with digital clock-card notation, and sort clocks by clock type. Tell and write whole-hour times. At an exact hour, the long minute hand points to 12 and the short hand points to the hour number. The clock face is not a measurement with connecting cubes. A single pairwise digital MATCH test does not certify classroom card sorting or student discussion.";
    public const string ClockHourIntroductionExamples = "Example 1 — Read 5 o'clock. The minute hand points to 12 and the hour hand points to 5. Say 'five o'clock' and write 5:00.\nExample 2 — Read 4 o'clock. The long minute hand points straight up to 12, and the short hour hand to 4. This clock shows 4:00.\nExample 3 — Draw an hour. For 9:00, put the long minute hand at 12 and the short hour hand at 9. Write 9:00 below the drawing.\nExample 4 — Read noon. Both the long minute hand and the short hour hand point to 12, so the clock shows 12:00.";
    public const string ClockHourIntroductionSteps = "Step 1: Identify which hand is longer (minute hand) and which is shorter (hour hand).\nStep 2: Check that the long minute hand points to 12 to show a whole hour, :00.\nStep 3: Read the numeral indicated by the short hour hand.\nStep 4: Write the hour, colon and 00, for example 5:00.\nStep 5: Draw or check the two hand positions to verify the written time.";
    public const string ClockHourIntroductionMistakes = "Mistake: using the long minute hand's position as the hour number. Correction: read the shorter hour hand for the hour.\nMistake: reading the numeral 12 as twelve elapsed minutes. Correction: at 12, the long hand indicates :00.\nMistake: forgetting the two minute digits when writing time. Correction: write 5:00, not 5.";

    public const string DrawFlatShapesOnDotsLessonCode = "PED:US-CCSS-MATH:G1:U07:L04";
    public const string DrawFlatShapesOnDotsVersion = "us-ccss-g1-g1-u07-l04-instruction-v1";
    public const string DrawFlatShapesOnDotsConcepts = "ORIGINAL STUDENT FACING TASK: Pick a shape card. EACH partner must DRAW the given flat shape on dot paper, then take turns DESCRIBING the drawn shape to the partner. Draw two-dimensional shapes using defining attributes: triangles have three straight sides and three vertices; rectangles have four straight sides and right-angle corners. Rotation does not change the attributes. A numerical SIDES/CORNERS answer only checks prerequisite shape vocabulary and does not certify any learner drawing.";
    public const string DrawFlatShapesOnDotsExamples = "Example 1 — Draw a triangle. Connect three non-collinear dots with three straight segments to form a closed triangular shape. Count 3 sides and 3 vertices.\nExample 2 — Draw a rectangle. Use dot-paper points (1,1), (5,1), (5,3) and (1,3); connect the points in order and close the shape. The rectangle has 4 sides and 4 right-angle corners.\nExample 3 — Draw a square. On equally spaced dot paper connect (1,1), (3,1), (3,3) and (1,3). All 4 sides have equal length.\nExample 4 — Compare orientations. Rotate the triangle or square on your page: its side count and corner count remain unchanged.";
    public const string DrawFlatShapesOnDotsSteps = "Step 1: Choose the target two-dimensional shape and name its defining sides and corners.\nStep 2: Select appropriate dots to form its vertices; for a triangle use three non-collinear dots.\nStep 3: Join neighboring dots with straight lines and close the shape.\nStep 4: Check the side count, corners, and equal lengths or right angles when relevant.\nStep 5: Rotate the drawing and explain why the shape's defining properties remain the same.";
    public const string DrawFlatShapesOnDotsMistakes = "Mistake: joining three dots on a straight line and calling it a triangle. Correction: choose three non-collinear points to form a closed shape.\nMistake: counting an open path as a polygon. Correction: connect the final vertex back to the first.\nMistake: calling every four-sided shape a rectangle. Correction: a rectangle has four right angles.";

    public const string ComposeFlatPatternShapesLessonCode = "PED:US-CCSS-MATH:G1:U07:L07";
    public const string ComposeFlatPatternShapesVersion = "us-ccss-g1-g1-u07-l07-instruction-v1";
    public const string ComposeFlatPatternShapesConcepts = "ORIGINAL STUDENT FACING TASK: BUILD something using pattern blocks, TRACE the created shape and RECORD exactly how many pattern blocks were used. Explain how smaller two-dimensional shapes meet at their full edges to make a larger shape, without gaps or overlaps. Make the SAME resulting object again using DIFFERENT pattern blocks and RECORD the new count; optionally find the MOST pattern blocks that can create the same object, then compare counts. A digital PARTS count checks only one subset; genuine tracing/creation, recomposition and counting the student's own blocks require teacher review.";
    public const string ComposeFlatPatternShapesExamples = "Example 1 — Make a rectangle. Place two identical square tiles next to each other, sharing a full edge. The resulting larger shape is a rectangle twice as wide as one square.\nExample 2 — Make a square with triangles. Join two congruent right-isosceles triangular paper pieces along their equal hypotenuse edges; together they make a larger square shape.\nExample 3 — Make a new square. Place four congruent square tiles in two rows of two, with no overlaps or gaps. The outer boundary is a square, formed from 4 smaller squares.\nExample 4 — Explain the pieces. Cover a large rectangle with two smaller rectangles, then move the pieces to make a different flat shape and describe which edges were joined.";
    public const string ComposeFlatPatternShapesSteps = "Step 1: Select pattern blocks or flat paper shapes such as two small squares or triangles.\nStep 2: Place the pieces together along matching full edges without gaps or overlaps.\nStep 3: Trace the outer boundary of the new composite flat shape.\nStep 4: Name and describe the larger resulting shape, for example a rectangle from two squares.\nStep 5: Try a different arrangement and explain how the same pieces compose another shape.";
    public const string ComposeFlatPatternShapesMistakes = "Mistake: merely counting three sides on a triangle without joining pieces. Correction: compose and trace the new outer boundary.\nMistake: leaving gaps and claiming the result is one solid flat shape. Correction: make the edges touch.\nMistake: including an interior joining line as part of the final outer boundary. Correction: trace the boundary around all the joined pieces.";

    public const string ShapeCardCountsLessonCode = "PED:US-CCSS-MATH:G1:U01:L08";
    public const string ShapeCardCountsVersion = "us-ccss-g1-g1-u01-l08-source-goal-v1";
    public const string ShapeCardCountsConcepts = "Sort shape cards into nonoverlapping categories using a single rule, then count the number in each category and show the counts with equal marks in a labeled picture graph. Rotating a shape does not change its category.";
    public const string ShapeCardCountsExamples = "Example 1: Sort 5 triangle cards, 3 rectangle cards and 4 circle cards. Put one card in one category; record triangle 5, rectangle 3, circle 4.\nExample 2: Draw 5 equal marks beside Triangle, 3 beside Rectangle and 4 beside Circle. Each mark represents one card.\nExample 3: Count all the cards: 5 + 3 + 4 = 12. There are 5 - 3 = 2 more triangles than rectangles.\nExample 4: Rotate a rectangle card; it still belongs to the Rectangle category because the shape did not change.";
    public const string ShapeCardCountsSteps = "Step 1: State the sorting rule and name three shape categories.\nStep 2: Classify each card once by its defining shape properties.\nStep 3: Count and record each category.\nStep 4: Make a picture graph with equal marks and clear labels.\nStep 5: Compare category counts and check that they total all cards.";
    public const string ShapeCardCountsMistakes = "Mistake: sorting the same card twice. Correction: use one rule and one category for each item.\nMistake: drawing large icons to mean more cards. Correction: one identical mark represents one card.\nMistake: omitting one category when finding the total. Correction: add all counts: 5 + 3 + 4 = 12.";

    public const string ClassFavoriteSurveyLessonCode = "PED:US-CCSS-MATH:G1:U01:L09";
    public const string ClassFavoriteSurveyVersion = "us-ccss-g1-g1-u01-l09-source-goal-v1";
    public const string ClassFavoriteSurveyConcepts = "Collect and organize categorical survey data. A table, tally chart, and picture graph may show the same data; category totals must remain the same. Count each person's answer once.";
    public const string ClassFavoriteSurveyExamples = "Example 1: Survey 13 classmates about their favorite fruit: 6 choose apples, 4 choose bananas and 3 choose oranges. Record one tally per answer.\nExample 2: Draw a picture graph with 6 equal marks beside Apples, 4 beside Bananas and 3 beside Oranges. Match these counts to the tally chart.\nExample 3: The survey includes 6 + 4 + 3 = 13 responses. Apples received 6 - 3 = 3 more votes than oranges.\nExample 4: Redraw the picture graph as a three-row frequency table with counts 6, 4 and 3. The category frequencies must not change.";
    public const string ClassFavoriteSurveySteps = "Step 1: Ask a survey question with clearly named answer categories.\nStep 2: Collect one response from each student and place a tally in its category.\nStep 3: Count category tallies and verify the total number surveyed.\nStep 4: Represent the same data using equal marks in a labeled graph.\nStep 5: Use the recorded counts to answer comparison and total questions.";
    public const string ClassFavoriteSurveyMistakes = "Mistake: one student's answer counted twice. Correction: place one tally for each respondent.\nMistake: changing the counts while switching charts. Correction: keep each category's frequency unchanged.\nMistake: drawing unlabeled symbols. Correction: label each row and define what one mark means.";

    public const string AroundRoomStoryProblemsLessonCode = "PED:US-CCSS-MATH:G1:U03:L28";
    public const string AroundRoomStoryProblemsVersion = "us-ccss-g1-g1-u03-l28-source-goal-v1";
    public const string AroundRoomStoryProblemsConcepts = "Create and solve addition and subtraction stories using classroom objects. Explain how an equation represents the situation, including three-addend sums within 20 and unknown numbers in different positions.";
    public const string AroundRoomStoryProblemsExamples = "Example 1: A classroom has 4 red folders, 3 blue folders and 5 green folders. Combine 4 + 3 = 7, then 7 + 5 = 12, so 4 + 3 + 5 = 12 folders.\nExample 2: There are 6 crayons on one desk, 2 on another and 4 on a third. Combine 6 + 4 = 10, then add 2 to make 6 + 2 + 4 = 12 crayons.\nExample 3: A shelf held 15 books and 6 were borrowed. Model the 15 and remove 6: 15 - 6 = 9 books remain.\nExample 4: There are 7 pencils; then some are added and 11 are on the desk. The missing number solves 7 + ? = 11, so 7 + 4 = 11.";
    public const string AroundRoomStoryProblemsSteps = "Step 1: Notice classroom objects and choose quantities for a solvable story.\nStep 2: Write a question with an unknown part or total and its matching equation.\nStep 3: Draw counters or a number path to represent every quantity.\nStep 4: Solve, for example 4 + 3 + 5 = 12 by combining the first two groups.\nStep 5: Report the answer using units and check it against the story.";
    public const string AroundRoomStoryProblemsMistakes = "Mistake: listing objects without giving a problem to solve. Correction: include an unknown and a complete solution.\nMistake: ignoring the third group in a three-addend question. Correction: account for all three addends.\nMistake: answering with an unlabeled number. Correction: identify folders, pencils or books.";

    public const string MeasuredShoesStoriesLessonCode = "PED:US-CCSS-MATH:G1:U06:L11";
    public const string MeasuredShoesStoriesVersion = "us-ccss-g1-g1-u06-l11-source-goal-v1";
    public const string MeasuredShoesStoriesConcepts = "Compare shoe lengths using identical length units, then write and solve addition or subtraction story equations. State results in cube-length units and use drawings of aligned units to justify differences.";
    public const string MeasuredShoesStoriesExamples = "Example 1: Shoe A measures 17 identical connecting-cube lengths and Shoe B measures 8. Subtract to compare: 17 - 8 = 9 cube lengths longer.\nExample 2: Mia's shoe measures 12 cubes. Her friend's shoe is 3 cube lengths longer. Find 12 + 3 = 15 cube lengths.\nExample 3: A 14-cube shoe is 5 cube lengths longer than another. Solve ? + 5 = 14 by 14 - 5 = 9 cube lengths.\nExample 4: Draw aligned rows of 11 and 7 same-size cubes; four cubes remain unmatched, so 11 - 7 = 4 cube lengths.";
    public const string MeasuredShoesStoriesSteps = "Step 1: Measure each shoe with equal-size connecting cubes without gaps or overlaps.\nStep 2: Record each length with its unit and align starting endpoints.\nStep 3: Read the story and decide whether the unknown is a length or a difference.\nStep 4: Write and solve the equation, such as 17 - 8 = 9.\nStep 5: Match or count cubes to verify the answer and report cube-length units.";
    public const string MeasuredShoesStoriesMistakes = "Mistake: subtracting measurements made with different cube sizes. Correction: use the same length unit.\nMistake: giving the answer 9 without units. Correction: say 9 cube lengths.\nMistake: overlooking gaps between units. Correction: measure with touching, aligned cubes.";

    public const string RectanglesSquaresAttributesLessonCode = "PED:US-CCSS-MATH:G1:U07:L06";
    public const string RectanglesSquaresAttributesVersion = "us-ccss-g1-g1-u07-l06-source-goal-v1";
    public const string RectanglesSquaresAttributesConcepts = "A rectangle is a closed flat shape with four straight sides and four right-angle (square) corners. A square is a rectangle whose FOUR sides are ALL the SAME LENGTH. Rotation, size and color are not defining attributes. The ORIGINAL 2021 STUDENT FACING asks pupils to DRAW FIVE rectangles and THREE shapes that are NOT rectangles, explain how each classification is justified; then separately DRAW FIVE squares and THREE shapes that are NOT squares, again explaining the defining features. These are TWO distinct sets of eight source learner drawings. Non-square rectangles qualify as rectangles but not as squares. The teacher must review authentic drawings; a yes/no digital shape check does NOT prove all sixteen source drawings were produced.";
    public const string RectanglesSquaresAttributesExamples = "Example 1: The four connected dot-paper corners (1,1), (5,1), (5,3), (1,3) define a rectangle with unequal adjacent side lengths, so it is NOT a square.\nExample 2: (1,1), (3,1), (3,3), (1,3) define a square: four equal sides and four right angles; it also satisfies every requirement for a rectangle.\nExample 3: A slanted parallelogram with no right angles is NOT a rectangle and therefore not a square, even when it has four straight sides.\nExample 4: A non-square rectangle is a valid RECTANGLE nonexample of a SQUARE. Rotating any valid rectangle or square does not destroy its defining sides and angles.\nORIGINAL source products: FIVE rectangles, THREE non-rectangles, FIVE squares and THREE non-squares, with individual explanations. Do not substitute one eight-picture set for the other.";
    public const string RectanglesSquaresAttributesSteps = "Step 1: Identify four straight sides and four right-angle corners as the rectangle test.\nStep 2: Draw FIVE different valid rectangles on dot paper, including some of different sizes or orientation; explain why each is a rectangle.\nStep 3: Draw THREE figures that are NOT rectangles and state which defining rectangle property is missing.\nStep 4: Identify FOUR EQUAL sides in addition to four right angles as the square test, remembering that every square is a rectangle.\nStep 5: Separately draw FIVE squares and THREE figures that are NOT squares. A rectangle with unequal adjacent sides is an appropriate non-square. Explain every classification.\nStep 6: Compare all FOUR requested drawing sets and have a teacher check your actual drawn figures. Do not claim this classroom work is done from a numeric answer.";
    public const string RectanglesSquaresAttributesMistakes = "Do not say a rotated rectangle is not a rectangle; rotation does not change right angles. Do not exclude squares from rectangles; every square also meets rectangle properties. Do not classify every rectangle as a square; a non-square rectangle has two pairs of equal sides, not four equal sides. Do not omit the SECOND source task: FIVE squares and THREE non-squares are required IN ADDITION TO FIVE rectangles and THREE non-rectangles. Only genuinely produced figures can receive the teacher-authorship check.";

    public const string CompositeShapesAndFactsCenterLessonCode = "PED:US-CCSS-MATH:G1:U07:L08";
    public const string CompositeShapesAndFactsCenterVersion = "us-ccss-g1-g1-u07-l08-source-goal-v1";
    public const string CompositeShapesAndFactsCenterConcepts = "Compose and describe flat and solid shapes using smaller pieces, while separately practicing addition and subtraction within 20. A full solution shows a shape composition and an arithmetic representation.";
    public const string CompositeShapesAndFactsCenterExamples = "Example 1: Join 2 identical square tiles along one edge to form a larger flat rectangle. Name the two component squares and outer rectangle.\nExample 2: Join 3 equal cubes face to face in a row. The combined solid is 3 cubes long; count and describe the small blocks it uses.\nExample 3: Show 6 red counters and 4 blue counters on a ten-frame. Add to get 6 + 4 = 10.\nExample 4: Begin with 14 counters and remove 5. Nine remain, so 14 - 5 = 9; check with 9 + 5 = 14.";
    public const string CompositeShapesAndFactsCenterSteps = "Step 1: Choose either the shape-composition task or the addition/subtraction center.\nStep 2: For shapes, join flat pieces along edges or cubes along faces.\nStep 3: Trace or describe the larger new shape and identify its components.\nStep 4: For numbers, model and solve an equation such as 6 + 4 = 10 or 14 - 5 = 9.\nStep 5: Explain the constructed shape or check the arithmetic with an inverse fact.";
    public const string CompositeShapesAndFactsCenterMistakes = "Mistake: completing only addition without the shape task. Correction: the center also expects geometric composition.\nMistake: joining cubes at corners only. Correction: align whole faces to form the solid.\nMistake: saying 14 - 5 = 8. Correction: remove five counters and count nine remaining.";

    private static readonly IReadOnlyDictionary<string, Repair> Repairs =
        new Dictionary<string, Repair>(StringComparer.Ordinal)
        {
            [DataQuestionsLessonCode] = new Repair(
                DataQuestionsLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-13/preparation.html",
                "619ca1727ce80aad07b6487cedd836c7017369cf251c425d2090c2d32662affa",
                "9d47d76f36eaa815741f5ce83bf807b30c490f30a48cb550812981ddef1864b9",
                DataQuestionsVersion, ["CCSS:1.MD.C.4", "CCSS:1.OA.C.5", "CCSS:1.OA.C.6"],
                OldAddSteps, DataQuestionsConcepts, DataQuestionsExamples, DataQuestionsSteps, DataQuestionsMistakes),
            [JungleSurveyLessonCode] = new Repair(
                JungleSurveyLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-15/preparation.html",
                "68afdf12b866c914150e889635bda34a23420fc2ead634c36082b2fc4712f4ed",
                "524c8057fd7450477edf66b60aa88c2057435e8abbb44ca6db0924f85b932882",
                JungleSurveyVersion, ["CCSS:1.MD.C.4"],
                OldJungleSteps, JungleConcepts, JungleExamples, JungleSteps, JungleMistakes),
            [TensLessonCode] = new Repair(
                TensLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-3/preparation.html",
                "e353f2b71bd5eb8973dcf261433992a18a0bd8f454d84f27c0db0c33270f4a16",
                "bcb915fa6c9669eebf4dfb723002e53aacccb21001948f1f11822dd480e5cf94",
                TensVersion,
                ["CCSS:1.NBT.B.2", "CCSS:1.NBT.C.4", "CCSS:1.NBT.C.5",
                 "CCSS:1.NBT.C.6", "CCSS:1.OA.C.5", "CCSS:1.OA.C.6", "CCSS:1.OA.D.8"],
                OldTensSteps, TensConcepts, TensExamples, TensSteps, TensMistakes),
            [CountBackOneOrTwoLessonCode] = new Repair(
                CountBackOneOrTwoLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-4/preparation.html",
                "8806fa7a4977ea12d0c5bb3fe2dd20d8f04e440f4c64ca2ac4b4849234c7cbd4",
                "5041bfe615b64551b0fc59735b02065bcc4a6036ebea865faaf769e608f17eec",
                CountBackOneOrTwoVersion,
                ["CCSS:1.OA.C.5", "CCSS:1.OA.C.6"],
                "Step 1: Identify the two quantities, 6 and 1.\nStep 2: Start at 6 and count on 1 more, or combine the two groups.\nStep 3: The total is 7, so 6 + 1 = 7.\nStep 4: Check that the result matches the quantities and the lesson goal.",
                CountBackOneOrTwoConcepts,
                CountBackOneOrTwoExamples,
                CountBackOneOrTwoSteps,
                CountBackOneOrTwoMistakes),
            [CenterAddSubtractLessonCode] = new Repair(
                CenterAddSubtractLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-10/preparation.html",
                "522589f8997d542348d02624d4a654a6643134cc7b052355d2e1a0caf4e3b5cd",
                "287e0dcb25fc726798717075fc858e5f4cf529eead1b35837835ccfc2f7a20b8",
                CenterAddSubtractVersion,
                ["CCSS:1.OA.B.4", "CCSS:1.OA.C.5", "CCSS:1.OA.C.6"],
                "Step 1: Represent the quantities or objects in the task.\nStep 2: Describe the relationship or pattern you notice.\nStep 3: Use the lesson key idea to reach a conclusion.\nStep 4: Check the conclusion against the original task.",
                CenterAddSubtractConcepts,
                CenterAddSubtractExamples,
                CenterAddSubtractSteps,
                CenterAddSubtractMistakes),
            [PuppiesTulipsCompareLessonCode] = new Repair(
                PuppiesTulipsCompareLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-6/lesson-17/preparation.html",
                "b54d271acd6cde5e44bd8ac7b7b7dc694be00b109c6316c2e09d329cac57f3bd",
                "39bce6dd35872fc984af0b9017b8c7c61be8abc23c678db5b0b10f93c462e9dd",
                PuppiesTulipsCompareVersion,
                ["CCSS:1.OA.A.1", "CCSS:1.OA.C.6"],
                "Step 1: Identify the two quantities, 2 and 5.\nStep 2: Start at 2 and count on 5 more, or combine the two groups.\nStep 3: The total is 7, so 2 + 5 = 7.\nStep 4: Check that the result matches the quantities and the lesson goal.",
                PuppiesTulipsCompareConcepts,
                PuppiesTulipsCompareExamples,
                PuppiesTulipsCompareSteps,
                PuppiesTulipsCompareMistakes),
            [CompareRepresentedNumbersLessonCode] = new Repair(
                CompareRepresentedNumbersLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-21/preparation.html",
                "75bfcaa790901e7ccb22e69119fbe42878ba436db742c28ff1553b6b05a0b086",
                "bd508628be876dbdf9e4532bb65dd5f68ba6fc08ab025f07a0d0683792051e4d",
                CompareRepresentedNumbersVersion,
                ["CCSS:1.NBT.B.2", "CCSS:1.NBT.B.3", "CCSS:1.NBT.C.4"],
                "Step 1: Identify the larger addend 10 and the 6 extra ones.\nStep 2: Start at 10.\nStep 3: Add 6 ones to reach 16.\nStep 4: Therefore, 10 + 6 = 16. Check the ones and tens in the result.",
                CompareRepresentedNumbersConcepts,
                CompareRepresentedNumbersExamples,
                CompareRepresentedNumbersSteps,
                CompareRepresentedNumbersMistakes),
            [CompareLengthsIndirectlyLessonCode] = new Repair(
                CompareLengthsIndirectlyLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-6/lesson-2/preparation.html",
                "0e95958c0e4086c048279bad4b5619eb340d9102028d9fff3b722a6fb06e409a",
                "e4ed4ad714849ae34ac1061833731800876878f426c9bb907a757db366d4aec4",
                CompareLengthsIndirectlyVersion,
                ["CCSS:1.MD.A.1", "CCSS:1.NBT.C.4"],
                "Step 1: Identify the two quantities, 35 and 20.\nStep 2: Start at 35 and count on 20 more, or combine the two groups.\nStep 3: The total is 55, so 35 + 20 = 55.\nStep 4: Check that the result matches the quantities and the lesson goal.",
                CompareLengthsIndirectlyConcepts,
                CompareLengthsIndirectlyExamples,
                CompareLengthsIndirectlySteps,
                CompareLengthsIndirectlyMistakes),
            [MeasureConnectingCubeLengthLessonCode] = new Repair(
                MeasureConnectingCubeLengthLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-6/lesson-5/preparation.html",
                "ec52aeabb5defec5ac3768a96ae9fda8dafb4a6c338284f8998c184aed0475ec",
                "0f8974285c7acdaacf50665e04dea7666bf559bb135372351883542c35fac0aa",
                MeasureConnectingCubeLengthVersion,
                ["CCSS:1.MD.A.1", "CCSS:1.MD.A.2", "CCSS:1.NBT.C.4", "CCSS:1.NBT.C.5", "CCSS:1.OA.C.5", "CCSS:1.OA.C.6"],
                "Step 1: Look at the defining properties of the shape.\nStep 2: Count or describe relevant sides, corners, faces, or curved surfaces.\nStep 3: Use those properties to sort or identify the shape.\nStep 4: Check that turning or moving the shape would not change its classification.",
                MeasureConnectingCubeLengthConcepts,
                MeasureConnectingCubeLengthExamples,
                MeasureConnectingCubeLengthSteps,
                MeasureConnectingCubeLengthMistakes),
            [ComposeThreeDimensionalShapesLessonCode] = new Repair(
                ComposeThreeDimensionalShapesLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-7/lesson-2/preparation.html",
                "dfaca8ee104e5d6aec9a586242cfd6707afdd1c30be747b33b198bb1d55b6cd7",
                "7db31fd02450d748ead1765fb86dd634355fbf9dec422e5a87294ba0278855db",
                ComposeThreeDimensionalShapesVersion,
                ["CCSS:1.G.A.2"],
                "Step 1: Look at the defining properties of the shape.\nStep 2: Count or describe relevant sides, corners, faces, or curved surfaces.\nStep 3: Use those properties to sort or identify the shape.\nStep 4: Check that turning or moving the shape would not change its classification.",
                ComposeThreeDimensionalShapesConcepts,
                ComposeThreeDimensionalShapesExamples,
                ComposeThreeDimensionalShapesSteps,
                ComposeThreeDimensionalShapesMistakes),
            [ReadHalfPastClockLessonCode] = new Repair(
                ReadHalfPastClockLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-7/lesson-14/preparation.html",
                "4c08b7cb8ac996df3eac791599e20883b2ce307029c3484fc76f9878a363d806",
                "ef2e017e8a6128ff9ae42cf2281eff69c579ac141b17e8c38b06c7373857fabb",
                ReadHalfPastClockVersion,
                ["CCSS:1.MD.B.3", "CCSS:1.NBT.C.4"],
                "Step 1: Identify the two quantities, 30 and 10.\nStep 2: Start at 30 and count on 10 more, or combine the two groups.\nStep 3: The total is 40, so 30 + 10 = 40.\nStep 4: Check that the result matches the quantities and the lesson goal.",
                ReadHalfPastClockConcepts,
                ReadHalfPastClockExamples,
                ReadHalfPastClockSteps,
                ReadHalfPastClockMistakes),
            [WriteClockTimesLessonCode] = new Repair(
                WriteClockTimesLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-7/lesson-15/preparation.html",
                "4246e84799133548b71db146b4792b52d2b2e07443620d006031e2c1b7a4588f",
                "db33d8083e5d87c87d0cff839a39cccc8b86535140b77593cf5c3ea3f626cfab",
                WriteClockTimesVersion,
                ["CCSS:1.MD.B.3", "CCSS:1.NBT.A.1", "CCSS:1.OA.D.7"],
                "Step 1: Identify the two quantities, 10 and 15.\nStep 2: Start at 10 and count on 15 more, or combine the two groups.\nStep 3: The total is 25, so 10 + 15 = 25.\nStep 4: Check that the result matches the quantities and the lesson goal.",
                WriteClockTimesConcepts,
                WriteClockTimesExamples,
                WriteClockTimesSteps,
                WriteClockTimesMistakes),
            [ReadClockDailyScheduleLessonCode] = new Repair(
                ReadClockDailyScheduleLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-7/lesson-16/preparation.html",
                "70f75abad717f15a9985126f8ea44466d1adc54973411d26a342ae6b0832de91",
                "1aa84fffe2e8e12a9737bb3650af0a4b93d928db4fef058fd5707ccb30066329",
                ReadClockDailyScheduleVersion,
                ["CCSS:1.MD.B.3", "CCSS:1.NBT.C.4"],
                "Step 1: Identify the two quantities, 8 and 32.\nStep 2: Start at 8 and count on 32 more, or combine the two groups.\nStep 3: The total is 40, so 8 + 32 = 40.\nStep 4: Check that the result matches the quantities and the lesson goal.",
                ReadClockDailyScheduleConcepts,
                ReadClockDailyScheduleExamples,
                ReadClockDailyScheduleSteps,
                ReadClockDailyScheduleMistakes),
            [SortMathToolsLessonCode] = new Repair(
                SortMathToolsLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-7/preparation.html",
                "b3b1211bc301554a40a002320038b0061372672481ff3aee3241ac32bea9bde4",
                "a75376803e94155affb6c612601bf45203e532ddce5a1f9499882512ac5b3583",
                SortMathToolsVersion,
                ["CCSS:1.MD.C.4", "CCSS:1.OA.C.5", "CCSS:1.OA.C.6"],
                "Step 1: Read the category labels and the scale or counts.\nStep 2: Identify the values needed for the question.\nStep 3: Compare or combine the values as required.\nStep 4: State the conclusion using the data.",
                SortMathToolsConcepts,
                SortMathToolsExamples,
                SortMathToolsSteps,
                SortMathToolsMistakes),
            [ClassPetSurveyStatementsLessonCode] = new Repair(
                ClassPetSurveyStatementsLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-11/preparation.html",
                "1248db8c7dc7302a6677f06a190fb5a75599ead2f582eec30be7785ed32cdc75",
                "bb626ccc7886894e3ad63c8fa6170b5145dfc005c11e952fc2df3811fb7ea485",
                ClassPetSurveyStatementsVersion,
                ["CCSS:1.MD.C.4", "CCSS:1.OA.B.4", "CCSS:1.OA.C.5", "CCSS:1.OA.C.6"],
                "Step 1: Read the category labels and the scale or counts.\nStep 2: Identify the values needed for the question.\nStep 3: Compare or combine the values as required.\nStep 4: State the conclusion using the data.",
                ClassPetSurveyStatementsConcepts,
                ClassPetSurveyStatementsExamples,
                ClassPetSurveyStatementsSteps,
                ClassPetSurveyStatementsMistakes),

            [HowManyLessonCode] = new Repair(
                HowManyLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-12/preparation.html",
                "15fb22a92ba7bfca597ace5885c483c00d38cd3057625798dcaba420242c099f",
                "dcf400f0018af0fa6fd4ea5e7f035f158efa913b65128cb3554b62a4132e5bde",
                HowManyVersion, ["CCSS:1.MD.C.4"],
                "Step 1: Read the category labels and the scale or counts.\nStep 2: Identify the values needed for the question.\nStep 3: Compare or combine the values as required.\nStep 4: State the conclusion using the data.",
                HowManyConcepts, HowManyExamples, HowManySteps, HowManyMistakes),
            [CenterThreeLessonCode] = new Repair(
                CenterThreeLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-14/preparation.html",
                "9205c20801a8e4fdc646cc86a03aea1140cf42367cc34b68d184a69b92d84603",
                "4044330e49e2eb82b1d3cfcbecc58da85d3c0412d40a661311fbdceb1eaa05b1",
                CenterThreeVersion,
                ["CCSS:1.MD.C.4", "CCSS:1.OA.B.4", "CCSS:1.OA.C.5", "CCSS:1.OA.C.6"],
                "Step 1: Identify the two quantities, 4 and 2.\nStep 2: Start at 4 and count on 2 more, or combine the two groups.\nStep 3: The total is 6, so 4 + 2 = 6.\nStep 4: Check that the result matches the quantities and the lesson goal.",
                CenterThreeConcepts, CenterThreeExamples, CenterThreeSteps, CenterThreeMistakes),
            [ChooseIndirectLengthComparatorLessonCode] = new Repair(
                ChooseIndirectLengthComparatorLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-6/lesson-3/preparation.html",
                "46494ce0ab057e2044125ea8d24054b633a73d7d3e43bd3cd6613a611de04678",
                "7b46f4014213115f70ec094af62a1af1533847fd1a2688757deb45d60b3934d0",
                ChooseIndirectLengthComparatorVersion,
                ["CCSS:1.MD.A.1"],
                "Step 1: Identify the attribute being measured.\nStep 2: Choose equal-size units and place them without gaps or overlaps.\nStep 3: Count the units used.\nStep 4: State the measurement with its unit and check that it is reasonable.",
                ChooseIndirectLengthComparatorConcepts,
                ChooseIndirectLengthComparatorExamples,
                ChooseIndirectLengthComparatorSteps,
                ChooseIndirectLengthComparatorMistakes),
            [ClockHourIntroductionLessonCode] = new Repair(
                ClockHourIntroductionLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-7/lesson-13/preparation.html",
                "4f58f40ffa994e8ab85d00273e14315f1fa2e04e82df51313a75492cbc0f0704",
                "9960a84efcd887cb955e58b1a3440fc824bb5197211494c84a1d569e62ad4b2c",
                ClockHourIntroductionVersion,
                ["CCSS:1.MD.B.3"],
                "Step 1: Identify the attribute being measured.\nStep 2: Choose equal-size units and place them without gaps or overlaps.\nStep 3: Count the units used.\nStep 4: State the measurement with its unit and check that it is reasonable.",
                ClockHourIntroductionConcepts,
                ClockHourIntroductionExamples,
                ClockHourIntroductionSteps,
                ClockHourIntroductionMistakes),
            [DrawFlatShapesOnDotsLessonCode] = new Repair(
                DrawFlatShapesOnDotsLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-7/lesson-4/preparation.html",
                "50db64bffe87408d19b1dc5ecd7470ed9bc7be23ffdb477d5e2978ccdc4efb69",
                "8e466fef74d455a26a1a1a35bb97d15ac57e53412d21581d46dfc3d927824dee",
                DrawFlatShapesOnDotsVersion,
                ["CCSS:1.G.A.1", "CCSS:1.NBT.C.4", "CCSS:1.OA.C.6"],
                "Step 1: Look at the defining properties of the shape.\nStep 2: Count or describe relevant sides, corners, faces, or curved surfaces.\nStep 3: Use those properties to sort or identify the shape.\nStep 4: Check that turning or moving the shape would not change its classification.",
                DrawFlatShapesOnDotsConcepts,
                DrawFlatShapesOnDotsExamples,
                DrawFlatShapesOnDotsSteps,
                DrawFlatShapesOnDotsMistakes),
            [ComposeFlatPatternShapesLessonCode] = new Repair(
                ComposeFlatPatternShapesLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-7/lesson-7/preparation.html",
                "f391d575b24bb0223c206921799ca8e95ade5b94f7dd866ce486c02d690a729c",
                "eeecb355035aaa81a7b818cad9418fbe201e1f3117a6f26b1415383b0e574d81",
                ComposeFlatPatternShapesVersion,
                ["CCSS:1.G.A.2"],
                "Step 1: Look at the defining properties of the shape.\nStep 2: Count or describe relevant sides, corners, faces, or curved surfaces.\nStep 3: Use those properties to sort or identify the shape.\nStep 4: Check that turning or moving the shape would not change its classification.",
                ComposeFlatPatternShapesConcepts,
                ComposeFlatPatternShapesExamples,
                ComposeFlatPatternShapesSteps,
                ComposeFlatPatternShapesMistakes),
            [ShapeCardCountsLessonCode] = new Repair(
                ShapeCardCountsLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-8/preparation.html",
                "be08787b6ee918431235266d3571a0fa2e1bbb4e9a355412e5c19041e01bf144",
                "b6cafcc471d8245b43782f53fc885eed25198877b36c1b5999e6c80bca15a466",
                ShapeCardCountsVersion,
                ["CCSS:1.MD.C.4"],
                "Step 1: Look at the defining properties of the shape.\nStep 2: Count or describe relevant sides, corners, faces, or curved surfaces.\nStep 3: Use those properties to sort or identify the shape.\nStep 4: Check that turning or moving the shape would not change its classification.",
                ShapeCardCountsConcepts,
                ShapeCardCountsExamples,
                ShapeCardCountsSteps,
                ShapeCardCountsMistakes),
            [ClassFavoriteSurveyLessonCode] = new Repair(
                ClassFavoriteSurveyLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-9/preparation.html",
                "10e8219d47606e63f59409e4ed0624ca17a20aa2afe58ec914677013584671d0",
                "361b081710767655d3a35f613ed083479ffd8ccfcf57a778f5301a6a7ca558d3",
                ClassFavoriteSurveyVersion,
                ["CCSS:1.MD.C.4", "CCSS:1.OA.C.5"],
                "Step 1: Read the category labels and the scale or counts.\nStep 2: Identify the values needed for the question.\nStep 3: Compare or combine the values as required.\nStep 4: State the conclusion using the data.",
                ClassFavoriteSurveyConcepts,
                ClassFavoriteSurveyExamples,
                ClassFavoriteSurveySteps,
                ClassFavoriteSurveyMistakes),
            [AroundRoomStoryProblemsLessonCode] = new Repair(
                AroundRoomStoryProblemsLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-28/preparation.html",
                "d368a03846bbb97e423a2bec04c7ec4a68e18036f8b5a48073efc3c0e45cde01",
                "4fc5dca4bba3b6f64f3f381e3cd0c412965dbe62486aa7a6433786bf36c13340",
                AroundRoomStoryProblemsVersion,
                ["CCSS:1.OA.A.1", "CCSS:1.OA.A.2", "CCSS:1.OA.B.3", "CCSS:1.OA.C.6"],
                "Step 1: Represent the quantities or objects in the task.\nStep 2: Describe the relationship or pattern you notice.\nStep 3: Use the lesson key idea to reach a conclusion.\nStep 4: Check the conclusion against the original task.",
                AroundRoomStoryProblemsConcepts,
                AroundRoomStoryProblemsExamples,
                AroundRoomStoryProblemsSteps,
                AroundRoomStoryProblemsMistakes),
            [MeasuredShoesStoriesLessonCode] = new Repair(
                MeasuredShoesStoriesLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-6/lesson-11/preparation.html",
                "40e9d244f81eb227330854e9508ccd90be83ae7d776e4be1e53ed4e6eeed416d",
                "8f839a1365ce4ae2b2775ab2bd7f4c2fb88fa5d264d42a8b1e4432506cc35b89",
                MeasuredShoesStoriesVersion,
                ["CCSS:1.MD.A.2", "CCSS:1.OA.A.1", "CCSS:1.OA.A.2", "CCSS:1.OA.B.4", "CCSS:1.OA.C.6"],
                "Step 1: Identify the starting quantity 17 and the amount 8 being removed or compared.\nStep 2: Use subtraction to find the difference.\nStep 3: 17 − 8 = 9.\nStep 4: Check that the answer fits the original situation.",
                MeasuredShoesStoriesConcepts,
                MeasuredShoesStoriesExamples,
                MeasuredShoesStoriesSteps,
                MeasuredShoesStoriesMistakes),
            [RectanglesSquaresAttributesLessonCode] = new Repair(
                RectanglesSquaresAttributesLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-7/lesson-6/preparation.html",
                "a724a3695d3c5ae06cfe04378cdeff91413d2b9cc076a7a45e7310f7b03403d1",
                "dca80fd3613e6e14bd78adc2f54da524aa914f31f3bd519d4fc6e873388d7a3f",
                RectanglesSquaresAttributesVersion,
                ["CCSS:1.G.A.1", "CCSS:1.NBT.C.4"],
                "Step 1: Identify the two quantities, 57 and 10.\nStep 2: Start at 57 and count on 10 more, or combine the two groups.\nStep 3: The total is 67, so 57 + 10 = 67.\nStep 4: Check that the result matches the quantities and the lesson goal.",
                RectanglesSquaresAttributesConcepts,
                RectanglesSquaresAttributesExamples,
                RectanglesSquaresAttributesSteps,
                RectanglesSquaresAttributesMistakes),
            [CompositeShapesAndFactsCenterLessonCode] = new Repair(
                CompositeShapesAndFactsCenterLessonCode,
                "https://im.kendallhunt.com/k5/teachers/grade-1/unit-7/lesson-8/preparation.html",
                "3fbcb9f24fb43d0353bc1385058088aadaed7d09cacd4552d7a4cddab375163b",
                "0e1b2dd353d98a969aa41f0af2264754ca9e134d281b39bd55250fb66352ed36",
                CompositeShapesAndFactsCenterVersion,
                ["CCSS:1.G.A.1", "CCSS:1.G.A.2", "CCSS:1.OA.C.6"],
                "Step 1: Identify the two quantities, 6 and 4.\nStep 2: Start at 6 and count on 4 more, or combine the two groups.\nStep 3: The total is 10, so 6 + 4 = 10.\nStep 4: Check that the result matches the quantities and the lesson goal.",
                CompositeShapesAndFactsCenterConcepts,
                CompositeShapesAndFactsCenterExamples,
                CompositeShapesAndFactsCenterSteps,
                CompositeShapesAndFactsCenterMistakes)
        };

    public static bool IsTarget(CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        string.Equals(document.PackCode,
            MathematicsCurriculumPackRegistry.CommonCoreCode, StringComparison.Ordinal) &&
        Repairs.ContainsKey(lesson.LessonCode);

    public static void Apply(CanonicalLessonContentPackDocument document)
    {
        if (!string.Equals(document.PackCode,
                MathematicsCurriculumPackRegistry.CommonCoreCode, StringComparison.Ordinal))
            return;

        foreach (var lesson in document.Lessons.Where(l => Repairs.ContainsKey(l.LessonCode)))
        {
            var repair = Repairs[lesson.LessonCode];
            if (!string.Equals(lesson.SourceUrl, repair.ExpectedUrl, StringComparison.Ordinal) ||
                !string.Equals(lesson.CanonicalBodySha256, repair.OriginalCanonicalSha,
                    StringComparison.OrdinalIgnoreCase) ||
                !lesson.OutcomeCodes.SequenceEqual(repair.Outcomes))
                throw new InvalidOperationException(
                    $"G1 instruction source/official-standard drift: {lesson.LessonCode}");

            var en = lesson.Translations.SingleOrDefault(t =>
                string.Equals(t.CultureCode, "en", StringComparison.Ordinal))
                ?? throw new InvalidOperationException("Missing English G1 lesson source");

            if (string.Equals(en.StepByStepSolutions, repair.Steps, StringComparison.Ordinal))
            {
                if (!string.Equals(en.KeyConceptsAndRules, repair.Concepts, StringComparison.Ordinal) ||
                    !string.Equals(en.WorkedExamples, repair.Examples, StringComparison.Ordinal) ||
                    !string.Equals(en.CommonMistakes, repair.CommonMistakes, StringComparison.Ordinal))
                    throw new InvalidOperationException(
                        $"Partial G1 correction drift: {lesson.LessonCode}");
                continue;
            }

            if (!string.Equals(en.StepByStepSolutions, repair.OldSteps, StringComparison.Ordinal) ||
                !string.Equals(ComputeBodySha256(en), repair.OldBodySha,
                    StringComparison.Ordinal))
                throw new InvalidOperationException(
                    $"Refusing G1 instruction correction of unexpected prose: {lesson.LessonCode}");

            en.KeyConceptsAndRules = repair.Concepts;
            en.WorkedExamples = repair.Examples;
            en.StepByStepSolutions = repair.Steps;
            en.CommonMistakes = repair.CommonMistakes;
        }
    }

    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string priorVersion) =>
        IsTarget(document, lesson) ? Repairs[lesson.LessonCode].CorrectedVersion : priorVersion;

    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingVersion) =>
        IsTarget(document, lesson) &&
        string.Equals(existingVersion, CommonCoreRichContentCorrections.CorrectionContentVersion,
            StringComparison.Ordinal);

    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingVersion,
        IReadOnlyCollection<CurriculumLessonContentTranslation> translations)
    {
        if (!CanUpgradeExisting(document, lesson, existingVersion))
            throw new InvalidOperationException(
                $"Expected preceding G1 Common Core learner content version: {lesson.LessonCode}");

        // Content metadata may exist without persisted prose in ReadFromJson mode.
        if (translations.Count == 0)
            return;

        if (translations.Count != 1 || translations.Single().CultureCode != "en")
            throw new InvalidOperationException(
                $"Unexpected G1 stored translation cultures: {lesson.LessonCode}");

        var repair = Repairs[lesson.LessonCode];
        var actualSha = ComputeBodySha256(translations.Single());
        var effective = lesson.Translations.Single(x => x.CultureCode == "en");
        var correctedSha = ComputeBodySha256(effective);
        if (!string.Equals(actualSha, repair.OldBodySha, StringComparison.Ordinal) &&
            !string.Equals(actualSha, correctedSha, StringComparison.Ordinal))
            throw new InvalidOperationException(
                $"Refusing to overwrite unrecognized G1 persisted text: {lesson.LessonCode}");
    }

    private static string ComputeBodySha256(CanonicalLessonContentPackTranslation x) =>
        ComputeBodySha256(x.CultureCode, x.Title, x.Explanation, x.KeyConceptsAndRules,
            x.WorkedExamples, x.StepByStepSolutions, x.CommonMistakes, x.QuickSummary);

    private static string ComputeBodySha256(CurriculumLessonContentTranslation x) =>
        ComputeBodySha256(x.CultureCode, x.Title, x.Explanation, x.KeyConceptsAndRules,
            x.WorkedExamples, x.StepByStepSolutions, x.CommonMistakes, x.QuickSummary);

    private static string ComputeBodySha256(params string[] fields) =>
        Convert.ToHexString(SHA256.HashData(
            Encoding.UTF8.GetBytes(string.Join("\n", fields) + "\n"))).ToLowerInvariant();
}
