using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;

namespace Edulytics.Services.LessonContent;

/// <summary>
/// Nine learner-facing, first-party Grade 1 unit-one mini-lessons aligned to
/// the verified IM instructional purposes. Preserve original canonical and
/// generic worked examples; prepend a concrete model and activity. No author
/// certification implied by these teaching supplements.
/// </summary>
public static partial class RichLessonContentV2RuntimeComposer
{
    private sealed record T001Teaching(
        string Goal, string Activity, string Question, string Method,
        string[] Steps, string Answer, string Check, string Mistake,
        string WhyWrong, string Correction);

    private static readonly IReadOnlyDictionary<int,T001Teaching> T001TeachingByLesson =
        new Dictionary<int,T001Teaching>
        {
            [2]=new(
                "Represent two dot groups as an addition expression within 10 and find the total.",
                "Try: draw four dots in one box and three in another. Write an addition sentence matching your drawing, then count all the dots.",
                "There are four dots on one card and three on a second card. How many in all?",
                "Represent the two visible groups as an addition expression.",
                ["Draw ●●●● and ●●● in separate groups.",
                 "Write the expression 4 + 3.",
                 "Count the first four, then count on 5, 6, 7."],
                "7","4 + 3 = 7; seven dots have been counted exactly once.",
                "Writing 4 + 3 = 4 by counting only the first card.",
                "The second group also contributes dots to the total.",
                "Point to each dot once; 4 + 3 = 7."),
            [3]=new(
                "Add exactly one or two within 10 by counting forward from the starting number.",
                "Try: put five counters in a ten-frame and add two more, one at a time. Say the two next numbers aloud.",
                "Start at 5 and add 2. What number do you reach?",
                "Count on one step for every added counter.",
                ["Keep 5 as the number you already have.",
                 "Add one counter: the next number is 6.",
                 "Add the second counter: the next number is 7."],
                "7","5 + 2 = 7; exactly two steps forward: 6, then 7.",
                "Counting 5 as the first added step and answering 6.",
                "The starting number is not a new counter.",
                "Start after 5; say 6, 7 for two added counters."),
            [4]=new(
                "Subtract exactly one or two within 10 by removing counters or counting backward.",
                "Try: fill eight spots on a ten-frame, remove two counters, and record how many remain. Move back two steps on a number path.",
                "There are 8 counters. Take away 2. How many remain?",
                "Remove one object for each backward counting step.",
                ["Begin with 8 and remove one counter: 7 remain.",
                 "Remove another counter: 6 remain.",
                 "Check by replacing both counters: 6 + 2 = 8."],
                "6","8 - 2 = 6 and 6 + 2 = 8.",
                "Answering 7 after removing two counters.",
                "Seven is the result after only one removal.",
                "Remove two counters or make two backward steps: 8 → 7 → 6."),
            [5]=new(
                "Recognize when to add and when to subtract within 10 and use a model for each.",
                "Try: play two turns with six counters. Combine four and two; then start with six and remove two. Record both equations.",
                "Combine 4 and 2 counters, then remove 2 from the group of 6. What two equations describe the turns?",
                "Identify the operation from what happens to the collection.",
                ["Joining four and two groups increases the total: 4 + 2 = 6.",
                 "Taking two away from six decreases the quantity: 6 - 2 = 4.",
                 "Use counters to check both equations."],
                "4 + 2 = 6; 6 - 2 = 4","Both equations use the same six counters in different situations.",
                "Subtracting in both turns because two numbers are shown.",
                "The first turn joins groups, so it requires addition.",
                "Choose the operation from joining or taking away before calculating."),
            [6]=new(
                "Practise adding and subtracting within 10, including matching number cards that make ten.",
                "Try: play Find the Pair with number cards 0–10. Take a card, fill a ten-frame, find its partner, and write an equation. Then practise one adding and one taking-away turn.",
                "One number card shows 7. What partner makes 10, and how do you check?",
                "Find the missing part using the empty places in a ten-frame.",
                ["Show seven counters in a ten-frame.",
                 "Count the three empty places: 7 + 3 = 10.",
                 "Check in the opposite direction: 10 - 7 = 3."],
                "3","7 + 3 = 10 and 10 - 7 = 3; practise also 6 - 2 = 4 and 4 + 2 = 6.",
                "Choosing 2 as the partner for 7.",
                "Seven and two together only make nine.",
                "Count the three empty ten-frame places and check 7 + 3 = 10."),
            [7]=new(
                "Choose a sorting rule for math tools; name the groups and count each object exactly once.",
                "Try: collect safe familiar tools (for example counters, square tiles and blocks). Say your sorting rule, move each tool to one labelled pile, count each pile, and explain your categories.",
                "A collection contains 4 counters, 3 blocks and 2 tiles. Sort by tool type. How many of each?",
                "Sort first, then count each labelled category once.",
                ["Write labels: Counters, Blocks, Tiles.",
                 "Place all four counters under Counters, three blocks under Blocks, and two tiles under Tiles.",
                 "Record 4, 3, 2; check 4 + 3 + 2 = 9 total tools."],
                "Counters 4; Blocks 3; Tiles 2","Every one of the nine items belongs to one category.",
                "Counting the same tool in two different categories.",
                "A chosen sorting rule must give each tool just one category.",
                "Move each tool once, mark the correct group, and recount the totals."),
            [8]=new(
                "Sort shape cards into three categories and represent the counts so another person can read them.",
                "Try: cut or draw three types of shape cards, mix them, sort by shape, then draw one same-sized tally mark or dot for every card under each labelled category.",
                "You have 3 triangle cards, 2 square cards and 4 circle cards. Make a picture display. How many marks belong in each row?",
                "Make labels before drawing one equal mark per card.",
                ["Label three rows: Triangle, Square, Circle.",
                 "Draw ●●● next to Triangle, ●● next to Square, and ●●●● next to Circle.",
                 "Count every card once: 3 + 2 + 4 = 9."],
                "3, 2, 4","The graph has nine equal marks; rotating a shape does not change its category.",
                "Drawing two marks for a single card because it has several sides.",
                "One mark represents one whole card, not a side or corner.",
                "Match each card to just one labelled row and make one mark."),
            [9]=new(
                "Collect real category responses, organize a tally table and display how many people chose each answer.",
                "Try the real survey: ask a few classmates or family members one question with three clear choices, such as favourite fruit. Count each person's choice once, draw a labelled table and show one mark for each answer. The app's separate automatic quiz uses fictional votes only.",
                "Five people choose apples, three choose bananas and two choose oranges. What does a labelled picture graph show?",
                "For this fictional practice sample, transfer each recorded response to its labelled category without changing the counts.",
                ["Label rows Apples, Bananas and Oranges.",
                 "Use five equal marks for apples, three for bananas, two for oranges.",
                 "Check that 5 + 3 + 2 = 10 responses; one response per person."],
                "Apples 5; Bananas 3; Oranges 2","Both the tally table and picture graph describe the same ten responses.",
                "Changing the number of votes when copying tallies into a graph.",
                "The display represents recorded responses, not a new survey.",
                "Copy every person's response once and recheck the totals."),
            [10]=new(
                "Use centre games to add and subtract within 10, including the hidden part that makes ten.",
                "Try: hide some of ten cubes behind your back, ask a partner to find the missing part, and write two related equations. In another round combine groups and then remove some.",
                "Ten cubes are split into 8 visible and some hidden. How many are hidden?",
                "Find the part that completes the whole group of ten.",
                ["Show eight visible cubes and count the empty spaces up to ten: 2.",
                 "Write 8 + 2 = 10.",
                 "Check with the related subtraction 10 - 8 = 2."],
                "2","8 + 2 = 10 and 10 - 8 = 2; check a separate turn: 5 + 2 = 7, 7 - 2 = 5.",
                "Mistaking the total of ten for the missing part.",
                "The task asks only for what is hidden, not all the cubes.",
                "Count two missing cubes; verify visible plus hidden equals ten.")
        };

    private static void AddT001LearnerTeaching(
        string lessonCode, RichLessonContentV2Lesson lesson)
    {
        const string prefix = "PED:US-CCSS-MATH:G1:U01:L";
        if (!lessonCode.StartsWith(prefix,StringComparison.Ordinal) ||
            !int.TryParse(lessonCode.AsSpan(prefix.Length),out var number) ||
            !T001TeachingByLesson.TryGetValue(number,out var teaching) ||
            !string.Equals(lesson.CultureCode,"en",StringComparison.OrdinalIgnoreCase))
            return;

        // Unlike the catalogue-wide compiler, the actual Grade 1 learner
        // should see mathematical instruction, not engineering claims about
        // verified contracts, namespace IDs, solver pipelines or provenance.
        var originalIntro=lesson.ExplanationParagraphs.FirstOrDefault()??teaching.Goal;
        var originalCore=lesson.KeyConcepts.FirstOrDefault();
        var earlierExamples=lesson.WorkedExamples.ToArray();
        var originalMistake=lesson.CommonMistakes.FirstOrDefault();

        lesson.ExplanationParagraphs.Clear();
        lesson.ExplanationParagraphs.Add("Learning goal: "+teaching.Goal);
        lesson.ExplanationParagraphs.Add(teaching.Activity);
        lesson.ExplanationParagraphs.Add(originalIntro);
        lesson.ExplanationParagraphs.Add("Now look at the worked examples. " +
            "Show the quantities with counters, dots, cards or a table before answering.");

        lesson.KeyConcepts.Clear();
        if(originalCore is not null)lesson.KeyConcepts.Add(originalCore);
        lesson.KeyConcepts.Add(new RichLessonKeyConcept
        {
            Title="Our learning goal",
            Definition=teaching.Goal,
            Rule=teaching.Method,
            Example=teaching.Question
        });
        lesson.KeyConcepts.Add(new RichLessonKeyConcept
        {
            Title="Check what you did",
            Definition=teaching.Check,
            Rule=teaching.Correction,
            Example=teaching.Answer
        });

        lesson.WorkedExamples.Clear();
        lesson.WorkedExamples.Add(new RichLessonWorkedExample
        {
            Title = "Worked model — lesson skill",
            Question = teaching.Question,
            Method = teaching.Method,
            Steps = teaching.Steps.ToList(),
            Answer = teaching.Answer,
            Check = teaching.Check
        });
        // An authored Grade 1 lesson should not print a wall of MCQ
        // options and pre-filled answers as repeated examples. Save all
        // interactive choices for Practice, while retaining at most two
        // simple numerical examples alongside the explicit worked model.
        var additionalExamples=earlierExamples
            .Where(example=>!example.Question.Contains(
                "Options:",StringComparison.OrdinalIgnoreCase))
            .Take(2).ToArray();
        for(var i=0;i<additionalExamples.Length;i++)
        {
            var example=additionalExamples[i];
            lesson.WorkedExamples.Add(new RichLessonWorkedExample
            {
                Title=$"Try another example {i+1}",
                Question=example.Question,
                Method=teaching.Method,
                Steps=
                [
                    "Show or picture what the question describes.",
                    example.Steps.Count>2?example.Steps[2]:
                        "Count or sort the quantities using the lesson model.",
                    "Write your answer: "+example.Answer+".",
                    "Count again to make sure every object or step was used once."
                ],
                Answer=example.Answer,
                Check="Check the question and each counted step against your answer."
            });
        }

        lesson.CommonMistakes.Clear();
        lesson.CommonMistakes.Add(new RichLessonCommonMistake
        {
            Mistake = teaching.Mistake,
            WhyWrong = teaching.WhyWrong,
            Correction = teaching.Correction
        });
        if(originalMistake is not null)
            lesson.CommonMistakes.Add(originalMistake);

        lesson.SummaryPoints.Clear();
        lesson.SummaryPoints.Add(teaching.Goal);
        lesson.SummaryPoints.Add(teaching.Method);
        lesson.SummaryPoints.Add("Check: "+teaching.Check);
        lesson.SummaryPoints.Add("Remember: "+teaching.Correction);
    }
}
