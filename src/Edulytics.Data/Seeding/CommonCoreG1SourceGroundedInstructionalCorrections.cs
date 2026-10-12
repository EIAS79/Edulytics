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
    public const string PuppiesTulipsCompareConcepts = "Learning goals: Tell, model, and solve Compare story problems about puppies and origami tulips using 'more' and 'fewer'. Compare two non-negative quantities by matching objects one-to-one, then count the unmatched objects. Use addition to find a larger group given the smaller group and the difference, or subtraction to find the difference when both groups are known.";
    public const string PuppiesTulipsCompareExamples = "Example 1 — Find the difference. Lina makes 7 puppy models and 4 tulips. Match four puppies to four tulips; three puppies remain unmatched. She has 7 - 4 = 3 more puppies than tulips.\nExample 2 — Fewer means the same difference. In the same display, there are 3 fewer tulips than puppies because 4 + 3 = 7.\nExample 3 — Find the larger group. Amir makes 5 tulips and 3 more puppies than tulips. He makes 5 + 3 = 8 puppies. Check 8 - 5 = 3.\nExample 4 — Find the smaller group. There are 9 puppies, which is 2 more than the number of tulips. Let t be the tulip count: t + 2 = 9, so t = 7. Check 9 - 7 = 2.";
    public const string PuppiesTulipsCompareSteps = "Step 1: Read the two types of objects and decide whether 'more' or 'fewer' compares their counts.\nStep 2: Draw or place one marker for each puppy and each tulip; pair as many markers as possible.\nStep 3: If both counts are known, subtract the smaller from the larger: 7 - 4 = 3 unpaired puppy models.\nStep 4: If one count and the difference are known, add or subtract to find the missing group: 5 + 3 = 8 puppies.\nStep 5: Check the story, units, and equation by matching groups or using the inverse fact.";
    public const string PuppiesTulipsCompareMistakes = "Mistake: subtracting the greater count from the smaller and reporting a negative number of models. Correction: difference in this story is 7 - 4 = 3 unmatched objects.\nMistake: treating 'more' as an instruction to add in every story. Correction: if both counts are given, subtract to find how many more.\nMistake: omitting what the number represents. Correction: say '3 more puppy models than tulips' instead of only '3'.";

    public const string CompareRepresentedNumbersLessonCode = "PED:US-CCSS-MATH:G1:U04:L21";
    public const string CompareRepresentedNumbersVersion = "us-ccss-g1-g1-u04-l21-topic-correction-v1";
    public const string CompareRepresentedNumbersConcepts = "Compare two-digit numbers even when their ten-rods and ones are grouped differently. Exchange ten ones for one ten before using <, >, or =.";
    public const string CompareRepresentedNumbersExamples = "A has 4 tens and 2 ones: 40 + 2 = 42. B has 3 tens and 12 ones: 30 + 12 = 42. Therefore A = B.\nA model shows 5 tens and 1 one: 51. Another shows 4 tens and 9 ones: 49. Therefore 51 > 49.\nA model shows 2 tens and 16 ones: 36. Another shows 3 tens and 7 ones: 37. Therefore 36 < 37.";
    public const string CompareRepresentedNumbersSteps = "Step 1: Count tens and individual ones in each model.\nStep 2: Regroup ten ones into a ten when needed.\nStep 3: Calculate each two-digit value, for example 30 + 12 = 42.\nStep 4: Compare the values and write <, >, or =.\nStep 5: Check the inequality against both models.";
    public const string CompareRepresentedNumbersMistakes = "Mistake: deciding the drawing with more individual cubes has the greater value. Correction: each ten-rod stands for ten units. Mistake: reading 3 tens and 12 ones as 312. Correction: 30 + 12 = 42.";

    public const string CompareLengthsIndirectlyLessonCode = "PED:US-CCSS-MATH:G1:U06:L02";
    public const string CompareLengthsIndirectlyVersion = "us-ccss-g1-g1-u06-l02-topic-correction-v1";
    public const string CompareLengthsIndirectlyConcepts = "Compare lengths of two objects that cannot be placed side by side, by checking both against the same third object, such as an unchanged pencil or marked string.";
    public const string CompareLengthsIndirectlyExamples = "A ribbon is longer than a pencil, while a crayon is shorter than that same pencil. Therefore the ribbon is longer than the crayon.\nA piece of string exactly matches the width of Desk A. The marked string extends beyond Desk B, aligned at one end; Desk A is wider than Desk B.\nTwo books each match exactly the same marked string. They are equally long, even though the books were not brought together.";
    public const string CompareLengthsIndirectlySteps = "Step 1: Choose two objects that need an indirect length comparison.\nStep 2: Select the same reference object, such as a pencil or marked string.\nStep 3: Compare the first object to the reference with endpoints aligned.\nStep 4: Compare the second object to that unchanged reference.\nStep 5: Explain what the two comparisons establish; if both are longer than the reference, find better evidence.";
    public const string CompareLengthsIndirectlyMistakes = "Mistake: stretching the string or replacing the pencil between comparisons. Correction: use one unchanged reference. Mistake: judging lengths with misaligned endpoints. Correction: line up starting ends first.";

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
    public const string ReadHalfPastClockConcepts = "At half past an hour, the long minute hand points to 6 and the short hour hand is halfway between the current hour number and the next. Show how half a circular clock represents 30 minutes.";
    public const string ReadHalfPastClockExamples = "For 3:30, the long hand points to 6 and the short hand halfway between 3 and 4. Read half past three.\nAt 3:00, the minute hand points to 12 and hour hand to 3. At 3:30, the minute hand points to 6 and hour hand advances toward 4.\nFor 12:30, point the long hand to 6 and the short hand between 12 and 1. Read half past twelve.";
    public const string ReadHalfPastClockSteps = "Step 1: Identify the longer minute hand and shorter hour hand.\nStep 2: Place the minute hand at 6 to show half a turn around the clock.\nStep 3: Locate the current hour and the next hour; for 3:30 these are 3 and 4.\nStep 4: Put the short hand halfway between those two numbers.\nStep 5: Read half past three and verify both hands.";
    public const string ReadHalfPastClockMistakes = "Mistake: placing the short hand exactly on 4 at 3:30. Correction: it is halfway from 3 to 4. Mistake: calling the clock 6:03 because the minute hand touches 6. Correction: the 6 position means 30 minutes.";

    public const string WriteClockTimesLessonCode = "PED:US-CCSS-MATH:G1:U07:L15";
    public const string WriteClockTimesVersion = "us-ccss-g1-g1-u07-l15-topic-correction-v1";
    public const string WriteClockTimesConcepts = "Write analog times to the hour and half hour in digital notation. A minute hand at 12 means :00; at 6 it means :30. The short hand identifies the current hour.";
    public const string WriteClockTimesExamples = "Minute hand on 12 and hour hand on 2 shows 2:00, two o'clock.\nMinute hand on 6 and hour hand halfway from 4 to 5 shows 4:30, half past four.\nMinute hand on 6 and hour hand halfway from 12 to 1 shows 12:30, not 1:30.";
    public const string WriteClockTimesSteps = "Step 1: Inspect the long minute hand; 12 indicates :00 and 6 indicates :30.\nStep 2: Inspect the short hour hand, including whether it is between numbers.\nStep 3: For half past, use the earlier hour of those two numbers.\nStep 4: Write hour, colon, and two digits, e.g. 4:30.\nStep 5: Compare the written time with both hands again.";
    public const string WriteClockTimesMistakes = "Mistake: writing 4:06 because the long hand points at 6. Correction: it points to the half-hour mark, so write :30. Mistake: choosing hour 5 when the shorter hand lies between 4 and 5. Correction: write 4:30.";

    public const string ReadClockDailyScheduleLessonCode = "PED:US-CCSS-MATH:G1:U07:L16";
    public const string ReadClockDailyScheduleVersion = "us-ccss-g1-g1-u07-l16-topic-correction-v1";
    public const string ReadClockDailyScheduleConcepts = "Read, draw, and write o'clock and half-past times in a daily schedule, with particular care when minute and hour hands point near 6 or 12.";
    public const string ReadClockDailyScheduleExamples = "Breakfast at 6:00: minute hand on 12 and hour hand on 6.\nPlay time at 6:30: long hand on 6 and short hand halfway from 6 to 7; the time is not 12:30.\nLunch at 12:30: long hand on 6 and short hand halfway from 12 to 1.\nAn afternoon schedule says 2:00 then 2:30; draw a clock for each and explain which hands moved.";
    public const string ReadClockDailyScheduleSteps = "Step 1: Read a schedule time and identify whether the minutes are :00 or :30.\nStep 2: Put the long minute hand on 12 for :00 or on 6 for :30.\nStep 3: Put the short hour hand on the hour number for :00, or halfway to the next for :30.\nStep 4: Label the analog clock with its digital time and event.\nStep 5: Double-check 6:30 versus 12:30 by looking at the shorter hand.";
    public const string ReadClockDailyScheduleMistakes = "Mistake: confusing 6:30 and 12:30 because the minute hand points to 6. Correction: inspect the hour hand. Mistake: keeping the hour hand on 6 at 6:30. Correction: move it halfway toward 7.";

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
                ReadClockDailyScheduleMistakes)
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
