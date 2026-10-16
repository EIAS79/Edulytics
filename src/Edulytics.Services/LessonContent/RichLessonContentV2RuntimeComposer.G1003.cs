using Edulytics.Core.Lessons;

namespace Edulytics.Services.LessonContent;

// Grade 1 Unit 1 Lessons 11–15; independently authored first-party learner
// explanations based on documented 2021 IM teacher goals. Existing Rich V2
// UI stays unchanged; no publisher text/images or fake survey participants.
public static partial class RichLessonContentV2RuntimeComposer
{
    private sealed record G1003Teaching(
        string Goal, string Activity, string Method,
        string FirstQuestion, string[] FirstSteps, string FirstAnswer, string FirstCheck,
        string SecondQuestion, string[] SecondSteps, string SecondAnswer, string SecondCheck,
        string Mistake, string WhyWrong, string Correction);

    private static readonly IReadOnlyDictionary<int,G1003Teaching> G1003TeachingByLesson =
        new Dictionary<int,G1003Teaching>
        {
            [11]=new(
                "Decide whether statements about a labelled tally chart are true or false, then write a true statement of your own.",
                "Try: make a tally table showing four dog votes, three cat votes and two fish votes. Read each row before checking a claim.",
                "Count tally marks in each named category; a claim is true only when the marks support it.",
                "A sample class-pet chart shows Dogs ||||, Cats |||, Fish ||. Is 'Five children chose cats' true?",
                ["Read the Cats row.","Count the three tally marks.","Compare three with the claim of five."],
                "False","The cat count is 3, not 5.",
                "Write one true statement about the same chart.",
                ["Count the marks in Dogs and Fish.","Compare four dogs with two fish.","Write a statement supported by those counts."],
                "Four children chose dogs; more chose dogs than fish.",
                "Four is greater than two, so the comparison is true.",
                "Saying cats have the most votes because that row is in the middle.",
                "Row position does not tell how many votes there are.",
                "Read each label and tally count first. Write only statements proved by the data."),
            [12]=new(
                "Find how many are in each category and how many in all from labelled counts or tally marks.",
                "Try: build a table for 3 dogs, 2 cats and 4 birds. Count each category, then combine all three.",
                "Read category labels and count each row exactly once before adding for the total.",
                "A tally table has Dogs |||, Cats ||, Birds ||||. How many animals in each category and altogether?",
                ["Count 3 dog marks, 2 cat marks and 4 bird marks.","Add 3 + 2 + 4.","Recount all nine marks to check."],
                "Dogs 3; Cats 2; Birds 4; Total 9",
                "3 + 2 + 4 = 9, matching all nine marks.",
                "A second table has 2 red counters, 3 blue and 1 yellow. How many altogether?",
                ["Read the three category counts.","Add 2 + 3 + 1.","Check that none of the categories was missed."],
                "6","Two plus three plus one is six.",
                "Giving one category count as the total.",
                "The question 'in all' asks for every category, not only the largest.",
                "List each category once and add the counts to find the total."),
            [13]=new(
                "Ask questions that a data representation can answer, and use the data to answer them.",
                "Try: read a fruit-vote table showing 4 apples, 2 bananas and 3 pears. Ask a question whose answer is in the table.",
                "Use category labels and numbers to ask answerable 'how many', 'which has most' or 'how many more' questions.",
                "A chart has Apples 4, Bananas 2, Pears 3. Ask how many chose bananas and answer.",
                ["Locate the Bananas category.","Read the value 2.","Answer the question using the category name."],
                "Two chose bananas.","The Bananas row shows two votes.",
                "Can you tell which child chose bananas from this anonymous chart?",
                ["Read the information recorded: category and count only.","Look for individual children's names; there are none.","Decide whether the chart contains enough information."],
                "No; the chart does not tell us who voted.",
                "A data chart with category totals cannot reveal individual names.",
                "Asking for a person's name when a chart contains only anonymous totals.",
                "An anonymous chart does not list who chose each option.",
                "Ask questions the shown labels and numbers can actually answer."),
            [14]=new(
                "Practise adding and subtracting within ten and sorting objects into labelled groups.",
                "Try a Sort and Display center: sort ten counters (4 red, 3 blue, 3 yellow), then also solve 6 + 2 and 8 - 3.",
                "Sort each object into one group, record its category count, then add or subtract with counters within ten.",
                "Sort 4 red, 3 blue and 3 yellow buttons. What does the display show?",
                ["Put buttons under Red, Blue and Yellow headings.","Count each group: 4, 3 and 3.","Check 4 + 3 + 3 = 10 buttons in all."],
                "Red 4; Blue 3; Yellow 3; Total 10",
                "Every button is in exactly one group and the total is ten.",
                "You have eight counters. Take away three. How many remain?",
                ["Begin with eight counters.","Move three aside.","Count the five still showing; check 5 + 3 = 8."],
                "5","8 - 3 = 5, and 5 + 3 = 8.",
                "Putting an object in two groups or doing only arithmetic and skipping the sort.",
                "The center has two skills: sorting/displaying and adding/subtracting.",
                "Show the sorted categories with counts, then check the operation with counters."),
            [15]=new(
                "Optional project: ask a survey question, collect real responses, show category counts and interpret the result.",
                "Try a real survey with permission: ask classmates or family members which jungle animal they prefer. Use three choices, record each person's answer once and make a labelled chart. The sample below uses made-up numbers to teach the method.",
                "Ask one clear question, tally one response per person, display category counts and ask answerable questions about the data.",
                "A fictional animal survey has Lions 2, Zebras 3 and Monkeys 4. Which animal received most votes?",
                ["Draw one row for each animal.","Mark 2 lion votes, 3 zebra votes and 4 monkey votes.","Compare the three counts."],
                "Monkeys, with 4 votes.",
                "Four is greater than three and two. These are fictional teaching votes.",
                "Using the same sample, how many responses were recorded in total?",
                ["Read the three counts: 2, 3 and 4.","Add 2 + 3 + 4.","Make sure each response was counted once."],
                "9","There are nine fictional responses: 2 + 3 + 4 = 9.",
                "Subtracting a bigger count from a smaller one to invent a negative number of votes.",
                "A category count cannot be negative; the lesson is about counting survey responses.",
                "Count votes as non-negative whole numbers, use labels, and never invent real survey participants.")
        };

    private static void AddG1003LearnerTeaching(
        string lessonCode, RichLessonContentV2Lesson lesson)
    {
        const string prefix="PED:US-CCSS-MATH:G1:U01:L";
        if (!lessonCode.StartsWith(prefix,StringComparison.Ordinal) ||
            !int.TryParse(lessonCode.AsSpan(prefix.Length),out var number) ||
            !G1003TeachingByLesson.TryGetValue(number,out var teaching) ||
            !string.Equals(lesson.CultureCode,"en",StringComparison.OrdinalIgnoreCase))
            return;

        lesson.ExplanationParagraphs.Clear();
        lesson.ExplanationParagraphs.Add("Learning goal: "+teaching.Goal);
        lesson.ExplanationParagraphs.Add(teaching.Activity);

        lesson.KeyConcepts.Clear();
        lesson.KeyConcepts.Add(new RichLessonKeyConcept
        {
            Title="Our learning goal",Definition=teaching.Goal,
            Rule=teaching.Method,Example=teaching.FirstQuestion
        });
        lesson.KeyConcepts.Add(new RichLessonKeyConcept
        {
            Title="Check your work",Definition=teaching.FirstCheck,
            Rule=teaching.Correction,Example=teaching.FirstAnswer
        });
        lesson.WorkedExamples.Clear();
        lesson.WorkedExamples.Add(new RichLessonWorkedExample
        {
            Title="Worked model — source lesson goal",Question=teaching.FirstQuestion,
            Method=teaching.Method,Steps=teaching.FirstSteps.ToList(),
            Answer=teaching.FirstAnswer,Check=teaching.FirstCheck
        });
        lesson.WorkedExamples.Add(new RichLessonWorkedExample
        {
            Title="One more worked example",Question=teaching.SecondQuestion,
            Method=teaching.Method,Steps=teaching.SecondSteps.ToList(),
            Answer=teaching.SecondAnswer,Check=teaching.SecondCheck
        });
        lesson.CommonMistakes.Clear();
        lesson.CommonMistakes.Add(new RichLessonCommonMistake
        {
            Mistake=teaching.Mistake,WhyWrong=teaching.WhyWrong,
            Correction=teaching.Correction
        });
        lesson.SummaryPoints.Clear();
        lesson.SummaryPoints.Add(teaching.Goal);
        lesson.SummaryPoints.Add(teaching.Method);
        lesson.SummaryPoints.Add("Check: "+teaching.FirstCheck);
    }
}
