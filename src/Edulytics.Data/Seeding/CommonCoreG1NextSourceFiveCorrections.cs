using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Original 2021 IM teacher preparation, full lessons and student-facing tasks
/// for G1 ordinals 36–40. No independent publisher illustration parity,
/// physical partner gameplay or free-form teacher judgment is claimed.
/// </summary>
public static class CommonCoreG1NextSourceFiveCorrections
{
    public const string Version="us-ccss-g1-008-u02l21-u03l03-original-source-v1";

    private sealed record Repair(string Code,string Url,string Sha,
        string[] Outcomes,string OldExamplePrefix,string OldStepPrefix,
        string Title,string Explanation,string Concepts,string Examples,
        string Steps,string Mistakes,string Summary);

    private static readonly Repair[] Repairs=
    [
        new("PED:US-CCSS-MATH:G1:U02:L21",
            "https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-21/preparation.html",
            "ecbb3ab93b1b78091587963b92944a098db3182f988a3cbf465cb826b93985c2",
            ["CCSS:1.OA.A.1","CCSS:1.OA.A.2","CCSS:1.OA.B.4","CCSS:1.OA.C.6","CCSS:1.OA.D.8"],
            "Example 1:","Step 1:",
            "Center Day 4",
            "Today we solve Number Puzzles within 10. Each empty box is a missing number in a true addition or subtraction equation. Choose digit cards to fill every blank: a digit card may be placed only once in a puzzle. Use related addition and subtraction facts to check both sides. After the new puzzle activity, choose an earlier addition/subtraction center. Completing a physical center with a partner is a separate activity; this digital lesson models the mathematical choices.",
            "Goal: add and subtract within 10, fill missing values in related equations and use each Number Puzzles digit card no more than once. Addition and subtraction can show the same part-whole fact. The Number Talk includes pairs such as 4+□=5 and 5−□=4. Optional partner centers: Capture Squares, Math Stories and What's Behind My Back.",
            "Example 1: 4+□=5 means one more than four. Fill the box with 1; check 4+1=5.\nExample 2: 5−□=4 also has missing value 1; 5−1=4. The two facts show the same three numbers.\nExample 3: Number Puzzles cards 2 and 3: 4+□=7 needs card 3; 6−□=4 needs card 2. Each card is used exactly once.",
            "Step 1: Read ALL addition and subtraction equations in the puzzle; point to the unknown boxes.\nStep 2: Solve one missing value with counting, objects, or a related fact.\nStep 3: Place the matching number card, then remove that card from the available set.\nStep 4: Complete the other equations using only unused digit cards.\nStep 5: Check each entire equation is true and no card has been used twice; explain a strategy with a partner when available.",
            "An empty box is not zero. A true arithmetic answer is not a legal Number Puzzles move if its digit card has already been used. Do not treat the warm-up and the partner center as identical tasks.",
            "I can solve related missing-value equations and use each puzzle card only once to make all equations true."),
        new("PED:US-CCSS-MATH:G1:U02:L22",
            "https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-22/preparation.html",
            "7afbdf796ab6cb0033428c5e1804cac7fd6b22c78dcd37eff5d260278e28dfde",
            ["CCSS:1.OA.B.4"],"Example 1: The source task combines","Step 1: Identify",
            "Story Problems and Equations (optional)",
            "Use survey category counts to invent equations and questions. The original lesson is optional because it revisits mathematical modeling rather than introducing a new content standard. Use actual class survey data when available, or clearly marked SAMPLE data instead. For example, a sample vote has 3 apples, 5 pears and 2 plums: write different equations about the data, then make and answer how-many-more and how-many-fewer questions. You must choose what the numbers mean, not merely add an unrelated pair of numbers.",
            "Goal: write at least three equations representing a single table of survey data; ask and answer meaningful comparison questions; write an equation that matches a student-authored story. Comparing two counts means subtracting the smaller from the larger. A real survey and partner feedback remain classroom tasks; our sample-data Practice never claims actual children voted.",
            "Example 1: SAMPLE votes: apples 3, pears 5, plums 2. Apples plus pears: 3+5=8. All three: 8+2=10.\nExample 2: How many more children chose pears than apples? 5−3=2; there are two more pear votes.\nExample 3: How many fewer children chose plums than pears? 5−2=3. Each equation is about the SAME sample chart, not an arbitrary true fact.",
            "Step 1: Read the chart labels and their category counts; confirm whether data are real or labeled sample values.\nStep 2: Choose meaningful groups and write at least three different complete true equations about the chart.\nStep 3: Create a how-many-more or how-many-fewer question that names two categories.\nStep 4: Answer using a related addition/subtraction equation, checking the difference against the counts.\nStep 5: Invent another relevant question or short story and ask a partner or teacher to check its meaning.",
            "Do not invent a real classroom survey when none took place. Do not accept an unrelated 4+5=9 merely because it is true; every number needs a chart referent. More and fewer comparisons need the correct order of the category counts.",
            "I can write three chart-based equations and ask, answer, and model meaningful survey comparison questions."),
        new("PED:US-CCSS-MATH:G1:U03:L01",
            "https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-1/preparation.html",
            "92fe6c1a47321348cc26707360ac41cfe26ca02ab9787ee42e77d8a8de52a9ca",
            ["CCSS:1.OA.C.6"],"Example 1: The source task combines","Step 1: Identify",
            "Sums I Know",
            "What addition facts within 10 do you know quickly? Choose a favorite sum and show why you like it. Use addition cards or a chart: solve one card mentally and place it in your honest 'Got it' or 'Not yet' group. 'Not yet' shows what you will practice, not a wrong grade. Your arithmetic answer is checked digitally, but only YOU can tell us whether a fact came quickly.",
            "Goal: add within ten, explain a favorite addition fact, and notice which sums are fluent and which still need practice. The category 'Got it' means you can say the answer quickly; 'Not yet' means you need to think more. These are self-assessment choices, NOT right/wrong mathematics answers.",
            "Example 1: My favorite sum could be 5+5=10 because doubles are easy for me to see as two groups of five.\nExample 2: 4+1=5 is one more than four. If I say five immediately, I may place it in 'Got it'.\nExample 3: 3+4=7. If I needed counters today, I can place it in 'Not yet' and practice again later.",
            "Step 1: Pick an addition expression whose total is at most 10.\nStep 2: Say or calculate its sum using mental facts or counters.\nStep 3: Choose one favorite sum and give your own reason using a picture, words or a mathematical pattern.\nStep 4: Sort each fact honestly into 'Got it' (quick) or 'Not yet' (needs practice).\nStep 5: Repeat 'Not yet' cards over time and notice which move to 'Got it'.",
            "Do not mark yourself 'Got it' just to earn points. Fluency confidence is not automatically graded as a mathematical error. Some sums may be accurate but not yet quick, which is normal for a self-check.",
            "I can add within 10, explain a favorite sum, and identify facts I know quickly and those I will continue to practice."),
        new("PED:US-CCSS-MATH:G1:U03:L02",
            "https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-2/preparation.html",
            "847b6901d88bc965981af4673fa134251b10962fe06515dab9c0358adf01321b",
            ["CCSS:1.OA.A.1","CCSS:1.OA.B.3","CCSS:1.OA.C.5","CCSS:1.OA.C.6","CCSS:1.OA.D.8"],
            "Example 1: The source task combines","Step 1: Identify",
            "Relate Counting to Addition",
            "Use 'count on' when you add within ten: start from one known addend and count forward once for each counter in the other group. In Shake and Spill two colors show the same total. Count from either color's amount; the order of addends does not change the sum, although one method may take fewer steps. Draw or model both methods and write a complete matching equation.",
            "Goal: apply counting on, compare counting methods and use the commutative property of addition. If 2 red and 7 yellow counters are spilled, 2+7 and 7+2 represent the same nine counters. Counting on from 7 takes two steps; counting on from 2 takes seven steps. The better starting number is a strategy choice, not a change to the answer.",
            "Example 1: 7+2: start at 7 and count 8, 9. The sum is 9.\nExample 2: 2+7: start at 2 and count to 9, or switch the order and count two steps from 7. Both are true: 7+2=9 and 2+7=9.\nExample 3: Shake and Spill shows 3 red and 6 yellow: 3+6=9 or 6+3=9. The same two parts make the same whole.",
            "Step 1: Identify the two groups and represent them with counters, a ten-frame or a drawing.\nStep 2: Start from one addend, say the next counting numbers for the other addend, and record the total.\nStep 3: Swap the starting addend and compare how many counting steps each strategy takes.\nStep 4: Write both complete equations showing the addends in opposite orders.\nStep 5: Check the total is unchanged and explain why both strategies represent the same counters.",
            "Do not count the starting number as the first new step. Counting on from either addend should reach the same total; a difference in counting steps does not imply a different sum. Do not teach swapping addends as moving numbers around a subtraction equation.",
            "I can count on from either addend, show two strategies, and explain why changing addition order does not change the sum."),
        new("PED:US-CCSS-MATH:G1:U03:L03",
            "https://im.kendallhunt.com/k5/teachers/grade-1/unit-3/lesson-3/preparation.html",
            "c84526f98dd95a7ea6ed6e71d8fcfce09d3703aebd440ec145b6a912df43f4e8",
            ["CCSS:1.OA.B.3","CCSS:1.OA.C.6","CCSS:1.OA.D.7"],
            "Example 1: Check","Step 1: Evaluate",
            "Are the Expressions Equal?",
            "An equal sign says two sides have the SAME VALUE. Sometimes both sides are addition expressions, not just a number. Add each expression within ten. Group expressions with the same result, then decide which complete equations are TRUE and which are FALSE. The addends can be reversed without changing a sum; changing a number may change the value.",
            "Goal: sort addition expressions by equal sums, interpret equations with additions on BOTH sides, and identify true/false equations using the commutative property and the meaning of '='. Equal is not a command meaning 'write the answer next'.",
            "Example 1: 4+2=2+4 is TRUE because both sides are six.\nExample 2: 3+4=5+2 is TRUE because seven equals seven even though the numbers differ.\nExample 3: 3+4=5+3 is FALSE because seven is not eight.\nExample 4: Sort 2+5 and 4+3 together because both are seven, but put 4+4 in another group.",
            "Step 1: Read the expression on the LEFT of '=' and find its value.\nStep 2: Find the value of the expression on the RIGHT.\nStep 3: Decide whether the two values match and say TRUE or FALSE.\nStep 4: Sort expressions with the same total into the same group, even if they have different addends.\nStep 5: Explain a true/false decision using counters or a ten-frame if needed.",
            "An equal sign does not mean the answer must be written immediately afterward. A correct expression can be on either side. Two expressions are equal only when their VALUES match, not when they look alike or contain the same symbols.",
            "I can sort equal sums and explain whether an equation with expressions on both sides is true or false.")
    ];
    private static Repair? Find(CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson)=>
        doc.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        doc.VersionCode=="CCSSM-2010"
            ?Repairs.SingleOrDefault(x=>x.Code==lesson.LessonCode)
            :null;
    public static bool IsTarget(CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson)=>
        Find(doc,lesson)!=null;
    private static Repair Checked(CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson)
    {
        var r=Find(doc,lesson)??throw new InvalidOperationException("Not G1-008");
        if(lesson.SourceUrl!=r.Url||
           !string.Equals(lesson.CanonicalBodySha256,r.Sha,StringComparison.OrdinalIgnoreCase)||
           !lesson.OutcomeCodes.SequenceEqual(r.Outcomes))
            throw new InvalidOperationException("V1 G1-008 original source/identity/outcome drift: "+lesson.LessonCode);
        return r;
    }
    private static bool Same(CanonicalLessonContentPackTranslation t,Repair r)=>
        t.Title==r.Title&&t.Explanation==r.Explanation&&
        t.KeyConceptsAndRules==r.Concepts&&t.WorkedExamples==r.Examples&&
        t.StepByStepSolutions==r.Steps&&t.CommonMistakes==r.Mistakes&&
        t.QuickSummary==r.Summary;
    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        foreach(var lesson in doc.Lessons)
        {
            if(!IsTarget(doc,lesson))continue;
            var r=Checked(doc,lesson);
            var en=lesson.Translations.Single(x=>x.CultureCode=="en");
            if(Same(en,r))continue;
            if(!en.WorkedExamples.StartsWith(r.OldExamplePrefix,StringComparison.Ordinal)||
               !en.StepByStepSolutions.StartsWith(r.OldStepPrefix,StringComparison.Ordinal))
                throw new InvalidOperationException("V1 G1-008 unexpected pre-repair student body: "+lesson.LessonCode);
            en.Title=r.Title;en.Explanation=r.Explanation;
            en.KeyConceptsAndRules=r.Concepts;en.WorkedExamples=r.Examples;
            en.StepByStepSolutions=r.Steps;en.CommonMistakes=r.Mistakes;
            en.QuickSummary=r.Summary;
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previous)=>
        IsTarget(doc,lesson)?Version:previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previousVersion)=>
        IsTarget(doc,lesson)&&previousVersion==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string existingVersion,
        IReadOnlyCollection<CurriculumLessonContentTranslation> existing)
    {
        var r=Checked(doc,lesson);
        if(!CanUpgradeExisting(doc,lesson,existingVersion))
            throw new InvalidOperationException("V1 G1-008 production predecessor/version drift");
        if(existing.Count==0)return; // JSON-only live content mode
        if(existing.Count!=1)throw new InvalidOperationException("V1 G1-008 culture drift");
        var en=existing.Single();
        if(en.CultureCode!="en"||
           !en.StepByStepSolutions.StartsWith(r.OldStepPrefix,StringComparison.Ordinal)||
           !en.WorkedExamples.StartsWith(r.OldExamplePrefix,StringComparison.Ordinal))
            throw new InvalidOperationException("V1 G1-008 persisted lesson body drift");
    }
}
