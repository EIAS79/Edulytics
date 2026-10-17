using Edulytics.Core.Curriculum;

namespace Edulytics.Services.LessonContent;

/// <summary>
/// Independently authored Grade 1 B001 lessons 11-20 (IM 2021 goal alignment).
/// Each visual is actual counters/tallies/equations for the lesson objective;
/// never substitute a pre-answered Practice multiple-choice wall for teaching.
/// B001 still requires external source task/activity comparison and P5 UAT.
/// </summary>
public static partial class RichLessonContentV2RuntimeComposer
{
    private sealed record B001TailTeaching(
        string Goal,string Activity,string Method,string Question,string Answer,
        string Check,string Mistake,string Correction,string VisualTitle,
        string VisualDescription,(string Label,string Cells)[] Diagram);

    private static (string,string) R(string label,string markers)=>(label,markers);

    private static readonly IReadOnlyDictionary<string,B001TailTeaching> B001TailTeachingByCode =
        new Dictionary<string,B001TailTeaching>(StringComparer.Ordinal)
    {
        ["PED:US-CCSS-MATH:G1:U01:L11"]=new(
            "Read a class-pet chart, write true statements about its numbers, and correct false ones.",
            "Point to each bar or row before comparing categories. Read the labels aloud, then tell a partner how you know a statement is true.",
            "Count the marks in each row, then compare the counts or add them to find the total.",
            "Our sample pet chart shows 6 cats, 4 dogs and 2 fish. Is 'More chose cats than dogs' true?",
            "Yes. Six is greater than four. Twelve children are represented in this fictional chart.",
            "Count six cat marks and four dog marks; the difference is two. 6+4+2=12.",
            "Saying there are six dogs because the cat bar has six marks.",
            "Follow the row label first; its marks belong only to that category.",
            "Read a three-category pet survey",
            "Fictional example, not votes from a real classroom. Each dot represents one response.",
            [R("Cats — 6","●●●●●●"),R("Dogs — 4","●●●●"),R("Fish — 2","●●")]),
        ["PED:US-CCSS-MATH:G1:U01:L12"]=new(
            "Read tally marks and number tables to answer how many in each category and how many in all.",
            "Group five tally marks together, including the crossed fifth mark, and count any remaining marks one by one.",
            "A complete tally bundle represents five, not four; add each category count for the total.",
            "Seven apples and three bananas were counted. How many fruits are in all?",
            "10 fruits.",
            "Seven equals a group of five plus two; 7+3=10.",
            "Counting a bundle of five tally strokes as only four.",
            "The diagonal fifth mark also represents a count. Count all five strokes.",
            "Read a category tally chart",
            "A five-mark bundle plus two more represents 7 apples. Three single marks represent 3 bananas.",
            [R("Apples: first five","││││╱"),R("Apples: two more","││"),R("Bananas: three","│││")]),
        ["PED:US-CCSS-MATH:G1:U01:L13"]=new(
            "Ask a question that a given data chart can answer and use a marked category to prove the answer.",
            "Look at what the chart records. Ask how many, which has more, or how many more. Do not guess an answer that the chart cannot show.",
            "An answerable question refers to categories or values actually displayed.",
            "This picture chart shows art 7, music 5 and sports 3. How many chose music?",
            "5 children.",
            "Count the five marks in the music row. The chart cannot tell you which music they like.",
            "Asking what each child's favourite instrument is when instruments were never recorded.",
            "Ask only about categories and counts visible in the chart.",
            "Ask and answer questions about a chart",
            "Fictional sample data; one dot is one choice.",
            [R("Art — 7","●●●●●●●"),R("Music — 5","●●●●●"),R("Sports — 3","●●●")]),
        ["PED:US-CCSS-MATH:G1:U01:L14"]=new(
            "Sort objects into categories, show the groups in a display, and keep adding and subtracting within 10.",
            "Put each object in exactly one group and record the count. Then choose another centre to practise pairs and subtraction.",
            "A picture display needs category labels, one mark per object and no duplicate entries.",
            "Sort 12 classroom objects: 5 counters, 4 tiles and 3 blocks. How many objects are displayed?",
            "12 objects.",
            "5+4+3=12. Every one of the twelve objects belongs to one labeled row.",
            "Putting the same object in two different categories and counting it twice.",
            "Choose one sorting rule; each object has only one place in the display.",
            "Sort and display a collection",
            "A first-party illustrated sort of twelve sample objects, not publisher artwork.",
            [R("Counters — 5","●●●●●"),R("Tiles — 4","■■■■"),R("Blocks — 3","▣▣▣")]),
        ["PED:US-CCSS-MATH:G1:U01:L15"]=new(
            "Make a survey question, collect and organize real responses, and explain what a data display says.",
            "Write a question with clear choices. Ask each classmate once, record each response, and build your own chart. The sample chart below is only for practising how to read results.",
            "Keep the survey votes, tally counts and display totals consistent. Genuine participants are not proved by sample data.",
            "A practice survey records 4 votes for monkeys, 3 for lions and 2 for parrots. How many choices are recorded?",
            "9 choices.",
            "4+3+2=9; one mark means one response. This does not claim nine real classmates were asked.",
            "Calling sample votes real classmates' answers.",
            "Only call data a class survey after participants actually answer and the tally is recorded.",
            "Practice chart for a planned survey",
            "These are fictional demonstration counts; collect authentic survey votes in the classroom.",
            [R("Monkey — 4","●●●●"),R("Lion — 3","●●●"),R("Parrot — 2","●●")]),
        ["PED:US-CCSS-MATH:G1:U02:L01"]=new(
            "Show joining and taking-away stories with counters, then write the matching addition or subtraction expression.",
            "Listen to what happens in the story. Draw the starting group; add or cross out the change. Tell which operation matches the action.",
            "Joining adds a group and taking away subtracts a group. An expression names the action but need not show an equals sign.",
            "Four books are on a shelf. Three arrive. Which expression describes the change?",
            "4 + 3",
            "Four filled and three open dots show seven books together; the written expression is 4+3.",
            "Writing 4-3 for books that arrived.",
            "More joining means addition; giving away means subtraction.",
            "Model a joining story about books",
            "Four books at first, with three newly added. The two dot groups match 4+3.",
            [R("Starting books — 4","●●●●"),R("New books — 3","○○○"),R("Together — 7","●●●●○○○")]),
        ["PED:US-CCSS-MATH:G1:U02:L02"]=new(
            "Write complete equations for joining and taking-away stories and explain what each number means.",
            "Draw the objects before and after the story change, then write an equation with an equals sign and the correct result.",
            "An expression shows an operation; a complete equation states the equality and result.",
            "Six children are reading. Two more join. Write a complete equation.",
            "6 + 2 = 8",
            "Count the six first dots and two new dots to get eight; the answer agrees with the story.",
            "Stopping at '6+2' when a complete equation was requested.",
            "Include the equals sign and the correct final amount: 6+2=8.",
            "Make a story into a complete equation",
            "Six children to start, two join, eight in all.",
            [R("Started reading — 6","●●●●●●"),R("Joined — 2","○○"),R("Now reading — 8","●●●●●●○○")]),
        ["PED:US-CCSS-MATH:G1:U02:L03"]=new(
            "Find the unknown number added when you know the starting and ending amounts.",
            "Draw the starting group first. Add one different-coloured dot at a time until the ending group is complete.",
            "The missing amount is the change, not the number at the end of the story.",
            "Six books become eight books after some arrive. How many were added?",
            "2 books.",
            "Count on from six to eight: seven, eight. Two new dots make 6+2=8.",
            "Answering eight when the question asks how many were added.",
            "Circle the change or missing group, not the final total.",
            "Find the missing change",
            "Six books are present at first. Two more create a final group of eight.",
            [R("Starting books — 6","●●●●●●"),R("Books added — 2","○○"),R("Final books — 8","●●●●●●○○")]),
        ["PED:US-CCSS-MATH:G1:U02:L04"]=new(
            "Tell result-unknown from change-unknown stories and also solve take-from stories.",
            "Compare two stories about four books and seven books. Mark which number is unknown, draw the action, and explain why the matching equation fits.",
            "For a missing result, solve 4+3=?. For a missing change, solve 4+?=7. For leaving, use subtraction.",
            "Mia has four books and ends with seven. How many new books arrived?",
            "3 books.",
            "Count on four, five, six, seven: three added dots. The equation is 4+3=7.",
            "Writing seven as the number that arrived; seven is the final number.",
            "Read what the question asks: the result, the added group, or the group taken away.",
            "Compare starting amount and unknown change",
            "Four books at the start, three new books, seven at the end. Draw and explain the missing change.",
            [R("Starting books — 4","●●●●"),R("New books — 3","○○○"),R("Final books — 7","●●●●○○○")]),
        ["PED:US-CCSS-MATH:G1:U02:L05"]=new(
            "Invent and explain a math story from an illustrated group, then write the matching equation.",
            "Use the picture to tell a complete story to a partner. Point to the start, what changed, and how many remained or arrived. Then write an equation. Choice-time centres can also practise sorting and pairs.",
            "Every story needs a beginning, a change and a math question. The equation must describe that change.",
            "Nine blocks are on the floor. Four are packed away. How many blocks remain?",
            "5 blocks; 9 - 4 = 5.",
            "Count nine at first, cross out four, and count five dots still there.",
            "Choosing addition for objects that were removed.",
            "Say what happened before choosing the operation; then check the diagram.",
            "Tell a subtraction story using counters",
            "Nine blocks before the action; four taken away; five remain.",
            [R("Before — 9 blocks","●●●●●●●●●"),R("Packed away — 4","××××"),R("Remain — 5","●●●●●")])
    };


    private sealed record B001SecondWorked(string Question,string Answer,string Check);

    // The second model must be a mathematically independent activity, not a
    // duplicate of the first example or a changed heading on the same answer.
    // Independently authored; source skill/grade scope reviewed in B001 P2.
    private static readonly IReadOnlyDictionary<string,B001SecondWorked> B001SecondWorkedByCode=
        new Dictionary<string,B001SecondWorked>(StringComparer.Ordinal)
    {
        ["PED:US-CCSS-MATH:G1:U01:L11"]=new(
            "Write two true observations and correct one false observation about the pet chart.",
            "Six children chose cats. Two children chose fish. 'Four chose cats' is false: four chose dogs.",
            "Check the labeled cat, fish and dog rows separately: 6, 2 and 4 marks."),
        ["PED:US-CCSS-MATH:G1:U01:L12"]=new(
            "The tally chart has seven apples and three bananas. How many more apples than bananas?",
            "4 more apples.",
            "Five tally strokes plus two make seven apples; 7 - 3 = 4."),
        ["PED:US-CCSS-MATH:G1:U01:L13"]=new(
            "Write a different question you can answer from the chart about art, music and sports, then answer it.",
            "How many children chose sports? Three children.",
            "The sports row has three marks. Ask about what the chart actually shows."),
        ["PED:US-CCSS-MATH:G1:U01:L14"]=new(
            "In the example sort of five counters, four tiles and three blocks, which category has two more items than blocks?",
            "Counters. Five counters is two more than three blocks.",
            "Each object is counted once; 5 - 3 = 2."),
        ["PED:US-CCSS-MATH:G1:U01:L15"]=new(
            "Write a survey question with the three animal choices. How would you collect actual replies?",
            "Which jungle animal would you most like to see: monkey, lion or parrot? Ask each person once and tally their one choice.",
            "This is a plan for collecting real responses. The demonstration 4, 3 and 2 are not real survey votes."),
        ["PED:US-CCSS-MATH:G1:U02:L01"]=new(
            "New story: Nine counters are on a table, and four are taken away. Write the subtraction expression.",
            "9 - 4.",
            "The story says four leave nine; 9 - 4 describes it without an equals sign."),
        ["PED:US-CCSS-MATH:G1:U02:L02"]=new(
            "New story: Eight pencils are on the table. Three are taken away. Write a complete equation.",
            "8 - 3 = 5.",
            "Start with eight, cross out three and count five remaining."),
        ["PED:US-CCSS-MATH:G1:U02:L03"]=new(
            "A child has three books and ends with eight books. How many new books arrived? Show an equation.",
            "Five new books; 3 + 5 = 8.",
            "Count on from three to eight: four, five, six, seven, eight is five additions."),
        ["PED:US-CCSS-MATH:G1:U02:L04"]=new(
            "Seven birds sit in a tree. Two fly away. Write the equation and name the unknown result.",
            "7 - 2 = 5. The unknown result is five birds remaining.",
            "Draw seven birds and cross out two; five remain."),
        ["PED:US-CCSS-MATH:G1:U02:L05"]=new(
            "Tell a new joining story with two counters at first and three joining. Write its equation.",
            "Two apples are in a basket. Three more are added. There are five apples: 2 + 3 = 5.",
            "Draw two filled dots and three open dots; count five dots in all.")
    };

    private static bool TryB001TailTeaching(string code,out B001TailTeaching? value)=>
        B001TailTeachingByCode.TryGetValue(code,out value);

    private static RichLessonVisual? BuildB001TailVisual(string code)
    {
        if(!TryB001TailTeaching(code,out var spec) || spec is null)
            return null;
        return new RichLessonVisual
        {
            Kind=RichLessonVisualKind.PlaceValueChart,
            Title=spec.VisualTitle,
            Description=spec.VisualDescription,
            Rows=spec.Diagram.Select(row=>new RichLessonVisualRow
            {
                Label=row.Label,
                Cells=row.Cells.Select(c=>c.ToString()).ToList()
            }).ToList()
        };
    }

    private static void AddB001TailLearnerTeaching(
        string lessonCode,RichLessonContentV2Lesson lesson)
    {
        if(!string.Equals(lesson.CultureCode,"en",StringComparison.OrdinalIgnoreCase) ||
           !TryB001TailTeaching(lessonCode,out var spec) || spec is null)
            return;
        if(!B001SecondWorkedByCode.TryGetValue(lessonCode,out var second))
            throw new InvalidOperationException("Missing independent B001 follow-up worked model: "+lessonCode);
        var chartLabels=string.Join("; ",spec.Diagram.Select(row=>row.Label));
        lesson.ExplanationParagraphs.Clear();
        lesson.ExplanationParagraphs.Add("Learning goal: "+spec.Goal);
        lesson.ExplanationParagraphs.Add(spec.Activity);
        lesson.ExplanationParagraphs.Add("Look at the labeled dots or marks. "+
            "Tell someone why the counts agree with your answer.");
        lesson.KeyConcepts.Clear();
        lesson.KeyConcepts.Add(new RichLessonKeyConcept {
            Title="What you are learning",Definition=spec.Goal,
            Rule=spec.Method,Example=spec.Question
        });
        lesson.KeyConcepts.Add(new RichLessonKeyConcept {
            Title="Use a diagram or counters",Definition=spec.VisualDescription,
            Rule=spec.Activity,Example=spec.Answer
        });
        lesson.KeyConcepts.Add(new RichLessonKeyConcept {
            Title="Check your thinking",Definition=spec.Check,
            Rule=spec.Correction,Example=spec.Answer
        });
        lesson.WorkedExamples.Clear();
        lesson.WorkedExamples.Add(new RichLessonWorkedExample {
            Title="Worked model — lesson skill",
            Question=spec.Question,Method=spec.Method,
            Steps=[
                "Represent every amount using the chart or counters.",
                "Read what the question asks and use the matching category or operation.",
                "Show or say how the diagram matches your answer: "+spec.Answer,
                "Check the result: "+spec.Check
            ],
            Answer=spec.Answer,Check=spec.Check
        });
        lesson.WorkedExamples.Add(new RichLessonWorkedExample {
            Title="Try one more — a different lesson-skill problem",
            Question=second.Question,Method=spec.Method,
            Steps=[
                "Read this new question carefully and identify the requested group or action.",
                "Draw fresh counters, tallies or a new story diagram; do not assume the first example's numbers.",
                "Solve the new question yourself before reading the solution.",
                "Check using the new quantities: "+second.Check
            ],
            Answer=second.Answer,Check=second.Check
        });
        lesson.WorkedExamples.Add(new RichLessonWorkedExample {
            Title="Check with another representation",
            Question="How could you show the same answer using counters or a written calculation?",
            Method=spec.Method,
            Steps=[
                "Read the labels and put out the matching number of counters.",
                "Move, compare or combine the counters in the way the story or chart shows.",
                "Check the answer against the original worked model.",
                "Explain how each counter matches one item in the picture."
            ],
            Answer="Use these labeled groups: "+chartLabels+". "+spec.Check,
            Check=spec.Check
        });
        lesson.WorkedExamples.Add(new RichLessonWorkedExample {
            Title="Explain the diagram to a partner",
            Question="How can the diagram help you explain this lesson's worked model?",
            Method="Point to each relevant row, then say what the counts represent.",
            Steps=[
                "Find the labeled starting group or category in the diagram.",
                "Point out the changed group, another category or the total.",
                "Say which number is your answer and why.",
                "Check: "+spec.Check
            ],
            Answer="Name which group represents each number and explain the comparison or change. "+
                spec.Check,
            Check=spec.Check
        });
        if(lesson.WorkedExamples.Count<3) throw new InvalidOperationException(
            "B001 hand-authored lesson must provide at least three examples: "+lessonCode);
        lesson.CommonMistakes.Clear();
        lesson.CommonMistakes.Add(new RichLessonCommonMistake {
            Mistake=spec.Mistake,
            WhyWrong="This does not match the categories, quantities or action in the diagram.",
            Correction=spec.Correction
        });
        lesson.CommonMistakes.Add(new RichLessonCommonMistake {
            Mistake="Writing a number without explaining what it represents.",
            WhyWrong="The answer may count a different group than the one asked for.",
            Correction=spec.Check
        });
        lesson.SummaryPoints.Clear();
        lesson.SummaryPoints.Add(spec.Goal);
        lesson.SummaryPoints.Add(spec.Method);
        lesson.SummaryPoints.Add(spec.Check);
    }
}
