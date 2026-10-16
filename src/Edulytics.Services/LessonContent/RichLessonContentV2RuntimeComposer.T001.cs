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
,
            [11]=new(
                "Read class-pet tally data and decide whether statements are true or false.",
                "Try: make a pretend pet chart with 6 cat votes, 4 dog votes and 2 fish votes. Write one true and one false statement about the chart.",
                "The chart shows cats 6, dogs 4 and fish 2. Is 'Five children chose dogs' true?",
                "Read the exact label and its count before judging the claim.",
                ["Find the Dogs row: it shows 4, not 5.",
                 "Compare the claim of 5 with the chart count of 4.",
                 "Mark the statement false and write a correct one: Four chose dogs."],
                "False","The dogs category has exactly four marks.",
                "Counting a bundle of five tally marks as four.",
                "Each tally stroke counts; a crossed bundle still represents five.",
                "Recount the marks once and point to the matching label."),
            [12]=new(
                "Count how many items are in each data category and how many there are in all.",
                "Try: draw a chart showing 3 books, 5 blocks and 2 pencils. Ask how many in each category and how many altogether.",
                "There are 3 books, 5 blocks and 2 pencils. How many objects are there in all?",
                "Read each count and combine every category exactly once.",
                ["Count the three labeled groups: 3, 5 and 2.",
                 "Combine 3 + 5 = 8 and 8 + 2 = 10.",
                 "Check by recounting all ten objects once."],
                "10","Three category counts add to ten items.",
                "Reporting only the largest category count as the total.",
                "The question says 'in all', so every category must contribute.",
                "Add the three groups and recount to check."),
            [13]=new(
                "Ask questions that a graph can answer, then use its counts to answer them.",
                "Try: look at a chart of 4 cat votes, 3 dog votes and 2 fish votes. Ask how many chose fish. Then explain why you cannot tell any voter's name.",
                "Can a chart of animal votes tell us the name of every child who voted?",
                "Check whether the question asks for information actually shown in the chart.",
                ["Read the graph labels: Cats, Dogs, Fish and their counts.",
                 "Notice there are no children's names in the display.",
                 "Answer no, and propose an answerable question: How many chose cats? Four."],
                "No","The chart shows categories and counts, not names.",
                "Guessing a person's name from a category-count chart.",
                "No individual names were collected or shown.",
                "Ask for a label, count, total or comparison supported by the graph."),
            [14]=new(
                "Practise adding and subtracting within 10 while sorting and displaying objects.",
                "Try: sort ten or more safe classroom items by shape or type and draw a labelled mark for each. In another center, use counters to add and take away within 10.",
                "You sorted 4 tiles, 3 blocks and 3 counters. How many did you sort?",
                "Choose one sorting rule and count each pile once.",
                ["Label the groups Tiles, Blocks and Counters.",
                 "Show four, three and three equal marks in the matching rows.",
                 "Check 4 + 3 + 3 = 10. Practise a separate subtraction: 7 - 2 = 5."],
                "10","Every one of the ten items is counted once; addition and subtraction remain separate centers.",
                "Placing one object in two sorting categories.",
                "One sorting rule gives each item exactly one group.",
                "Move each object once and match every label to its count."),
            [15]=new(
                "Create a survey, record anonymous votes, represent the counts and ask questions about the data (optional lesson).",
                "Try: ask family or classmates which jungle animal they would most like to see: monkey, lion or parrot. Record each response once without names. The automatic exercise can check the votes and graph, but cannot confirm who answered.",
                "A sample jungle survey has 4 monkey votes, 3 lion votes and 2 parrot votes. How many votes in all?",
                "Organize the three responses in a labelled tally chart.",
                ["Label three categories: Monkey, Lion and Parrot.",
                 "Add four marks, three marks and two marks respectively.",
                 "Count the marks: 4 + 3 + 2 = 9; compare monkeys with parrots: 4 - 2 = 2."],
                "9","The chart contains nine votes and monkeys received two more than parrots.",
                "Drawing unequal numbers of graph marks compared with recorded votes.",
                "A graph must represent the responses recorded, not new guesses.",
                "Transfer each anonymous response once and check category totals.")
        };

    private static RichLessonWorkedExample OneMoreModel(int number,string method)
    {
        // An additional concrete case preserves Rich V2 worked-example
        // completeness without exposing raw multiple-choice distractor walls.
        (string Question,string Step,string Answer,string Check) sample=number switch
        {
            2=>("A dot card has 2 dots and another card has 5. Write the addition expression and total.",
                "Combine the groups: 2 + 5 = 7.","7","2 + 5 has two addends and exactly seven dots."),
            3=>("Start with 7 counters. Add one more. What is the new total?",
                "Count one step after seven: eight.","8","7 + 1 = 8."),
            4=>("Start with 9 counters. Take away one. How many remain?",
                "Move one step back from nine: eight.","8","9 - 1 = 8; 8 + 1 = 9."),
            5=>("Join three counters and four more, then remove three from the total. Write both equations.",
                "Join to make 3 + 4 = 7, then remove to make 7 - 3 = 4.",
                "3 + 4 = 7; 7 - 3 = 4","Both adding and subtracting are shown by the counters."),
            6=>("A card shows 8. What card completes ten? How can subtraction check?",
                "Eight and two fill all ten places: 8 + 2 = 10.",
                "2","10 - 8 = 2, so the missing partner is two."),
            7=>("Sort 3 counters, 2 pattern blocks and 4 inch tiles by tool type.",
                "Put each tool into one labelled group, then count the three groups.",
                "Counters 3; Blocks 2; Tiles 4",
                "3 + 2 + 4 = 9 objects, each counted once."),
            8=>("Draw category marks for 2 triangle cards, 3 squares and 4 circles.",
                "Draw two marks under Triangles, three under Squares and four under Circles.",
                "2, 3, 4","There are nine equal picture marks in three labelled rows."),
            9=>("A sample survey records 2 apple votes, 4 banana votes and 3 orange votes. Make a table.",
                "Give each fruit one row and one mark per response.",
                "Apples 2; Bananas 4; Oranges 3",
                "The table shows nine votes with no response counted twice."),
            10=>("You can see five cubes and five are hidden. How many cubes were there altogether?",
                "Five visible and five hidden form ten: 5 + 5 = 10.",
                "10","10 - 5 = 5 confirms the hidden part."),
            11=>("Cats have 5 votes, dogs 3 and fish 2. Is 'Dogs have 3 votes' true?",
                "Read the Dogs row: three votes are shown.",
                "True","The statement agrees with the recorded data."),
            12=>("A chart has 2 squares, 4 circles and 3 triangles. How many shapes in all?",
                "Add all category counts: 2 + 4 + 3 = 9.",
                "9","All nine shapes belong to exactly one of the three categories."),
            13=>("A chart shows apples 3, bananas 4 and oranges 2. Can it answer who likes books?",
                "The chart records fruits, not book preferences.",
                "No","Ask instead how many chose bananas; the chart answers four."),
            14=>("You sort 5 tiles, 3 blocks and 2 counters. How many items are sorted?",
                "Count each category once: 5 + 3 + 2 = 10.",
                "10","All ten objects are represented in the display."),
            15=>("A jungle survey has 3 monkey, 4 lion and 2 parrot votes. Which received the most?",
                "Compare the category totals 3, 4 and 2.",
                "Lions","Four is the largest count; nine responses in all."),
            _=>throw new InvalidOperationException("No G1 unit-one worked model for lesson "+number)
        };
        return new RichLessonWorkedExample
        {
            Title="One more worked example",
            Question=sample.Question,
            Method=method,
            Steps=["Show the objects or draw the groups.",sample.Step,
                "Write the answer and explain what each number means."],
            Answer=sample.Answer,
            Check=sample.Check
        };
    }

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
        // The canonical pack also contains teacher-facing meta prose
        // (MP7/MP8 references, "students", "you may lead them").
        // Preserve that data in the pack, but do not expose it as a first-grade
        // learner explanation. The explicit goal, activity, worked model,
        // misconception, and picture below teach the exact source purpose.
        var earlierExamples=lesson.WorkedExamples.ToArray();
        var originalMistake=lesson.CommonMistakes.FirstOrDefault();

        lesson.ExplanationParagraphs.Clear();
        lesson.ExplanationParagraphs.Add("Learning goal: "+teaching.Goal);
        lesson.ExplanationParagraphs.Add(teaching.Activity);

        lesson.ExplanationParagraphs.Add("Now look at the worked examples. " +
            "Show the quantities with counters, dots, cards or a table before answering.");

        lesson.KeyConcepts.Clear();

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
        lesson.KeyConcepts.Add(new RichLessonKeyConcept
        {
            Title="Try it with objects",
            Definition=teaching.Activity,
            Rule=teaching.Method,
            Example=teaching.Question
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

        lesson.WorkedExamples.Add(OneMoreModel(number,teaching.Method));

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
