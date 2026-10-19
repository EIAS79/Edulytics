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
    public const string Version = "us-ccss-g1-full-grade-u04-l22-through-u05-l14-teaching-v1";
    private sealed record Repair(string Code,string SourceUrl,string OriginalSha,string[] Outcomes,
        string Title,string Explanation,string Examples,string Steps,string Mistakes,string Summary);
    private static readonly Repair[] Repairs =
    [
        new(@"PED:US-CCSS-MATH:G1:U04:L22",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-22/preparation.html",@"b85aea088222d4e41c7277cd0b59f55d8e177689037f8ba35a6c28b7e1fd7519",[@"CCSS:1.NBT.B.2",@"CCSS:1.NBT.B.3"],
            @"Center Day 4",
            @"Compare within 99 using place value and describe what a numeral means in tens and ones.",
            @"Example 1: 74 is greater than 68 because seven tens are more than six tens. Example 2: 53 and 59 share five tens; 53 is less because three ones are fewer than nine.",
            @"Step 1: Represent each number with tens and ones. Step 2: Compare tens, then ones when tens match. Step 3: Explain which quantity is greater and why.",
            @"Comparing only ones is wrong: 68 is not greater than 74 simply because 8 exceeds 4.",
            @"I can identify and compare two-digit numbers, including number-card center representations."),
        new(@"PED:US-CCSS-MATH:G1:U04:L23",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-23/preparation.html",@"b64853c6fba2b3fb89c0192538cb6629db49583e2844ce48f4b367e4721b9384",[@"CCSS:1.NBT.A.1",@"CCSS:1.NBT.B.2",@"CCSS:1.NBT.B.3"],
            @"Two-Digit Numbers in Our World (optional)",
            @"Optional investigation: estimate a collection before organizing and counting it. A teacher supervises actual collection and measurement, which a digital response cannot certify.",
            @"Example 1: I estimate about 50 counters. Group into four tens and seven ones to count 47; my estimate is 3 more. Example 2: Six tens and two ones represent 62.",
            @"Step 1: View the real or pictured collection; estimate a plausible number up to 99. Step 2: Organize into full groups of ten and leftover ones. Step 3: Count accurately and compare estimate with actual.",
            @"Do not claim the estimate is an exact count. Five tens and three ones is 53, not 8.",
            @"I can estimate, count, and represent a collection up to 99. A real-life investigation needs real evidence."),
        new(@"PED:US-CCSS-MATH:G1:U05:L01",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-1/preparation.html",@"39aba045753ec23ceacd9c40cbd05dfc4e50f98ce3f39f1fe0f9799e4435f276",[@"CCSS:1.NBT.A.1",@"CCSS:1.NBT.B.2",@"CCSS:1.NBT.C.4"],
            @"Add Tens or Ones",
            @"Add TENS or ONES to a two-digit number without composing a new ten. Draw rods for tens and single dots for ones.",
            @"Example 1: 43 + 5 = 48; three ones plus five ones makes eight ones. Example 2: 43 + 20 = 63; four tens plus two tens makes six tens, and three ones remain.",
            @"Step 1: Identify whether the addend contains tens or ones. Step 2: Add like units without combining ten ones. Step 3: Rewrite and check the resulting two-digit number.",
            @"Do not treat +20 as adding two ones. Do not generate a sum with ten or more ones in this no-composing lesson.",
            @"I can add tens or ones to two-digit numbers when no new ten is made."),
        new(@"PED:US-CCSS-MATH:G1:U05:L02",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-2/preparation.html",@"d5f9bd01b30984d63c60ef831e3ba9db91dfba03a1ff02e398bac487c9f4ef6a",[@"CCSS:1.NBT.A.1",@"CCSS:1.NBT.C.4"],
            @"How Did You Add? PLC Activity",
            @"Use place-value methods to add two two-digit numbers WITHOUT composing a new ten, and interpret equations showing those methods.",
            @"Example 1: 23 + 45 = (20 + 40) + (3 + 5) = 60 + 8 = 68. Example 2: 32 + 54 = 86 by combining eight tens and six ones.",
            @"Step 1: Break both addends into tens and ones. Step 2: Add tens together. Step 3: Add ones together (fewer than ten). Step 4: Interpret every equality and check the sum.",
            @"Do not regroup when the ones sum is below ten, and do not claim 23+45=20+40 without adding the ones.",
            @"I can explain valid equations for two-digit addition with no composing."),
        new(@"PED:US-CCSS-MATH:G1:U05:L03",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-3/preparation.html",@"2d5c8b8a5fd81e87736b2e54acfd6fe1f0ef46c6e6a4316edac14ae0a5cd6d35",[@"CCSS:1.NBT.C.4"],
            @"Add It, Explain It",
            @"Write, not merely interpret, equations representing two-digit addition without composing a new ten.",
            @"Example 1: 13 + 21 = 10 + 20 + 3 + 1 = 30 + 4 = 34. Example 2: 24 + 32 = (20+30)+(4+2)=56.",
            @"Step 1: Sketch tens and ones for each addend. Step 2: Write an equation decomposing the quantities. Step 3: Combine tens and ones and write a true final equality. Step 4: Explain the equation.",
            @"An equation such as 13+21=30 is false because four ones have been dropped.",
            @"I can write my own equations showing a correct place-value addition method."),
        new(@"PED:US-CCSS-MATH:G1:U05:L04",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-4/preparation.html",@"d0dfcf1a9ddb0e856a1e95d63556dd08dda10a6cdfa1ff7d3d8c92a91ab40170",[@"CCSS:1.NBT.A.1",@"CCSS:1.NBT.C.4",@"CCSS:1.OA.A.1",@"CCSS:1.OA.C.6",@"CCSS:1.OA.D.8"],
            @"Center Day 1",
            @"Teacher-led center for addition within 100 WITHOUT composing a new ten. Digital Practice checks numerical totals but not classroom partner turns.",
            @"Example 1: 31 + 26 = 57: three tens plus two tens is fifty; one plus six makes seven. Example 2: 42 + 15 = 57 without regrouping.",
            @"Step 1: Read the selected center's two numbers. Step 2: Add tens and ones separately. Step 3: Make sure the ones sum is below ten. Step 4: Check by showing bundles and ones.",
            @"Do not mark a center game completed from a single numeric answer; do not slip in composing-ten questions as though they are the same objective.",
            @"I can add within 100 without composing a new ten in a teacher-facilitated center."),
        new(@"PED:US-CCSS-MATH:G1:U05:L05",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-5/preparation.html",@"3d56c54577b9ed9dbaa2eca03ba5f8c41e0aed29b705dde350dd9ccbb7cd7c6c",[@"CCSS:1.NBT.C.4"],
            @"Make a Ten",
            @"Add a one-digit number to a two-digit number by composing a new TEN; decompose the one-digit addend to complete the next multiple of ten.",
            @"Example 1: 47 + 8 = 47 + 3 + 5 = 50 + 5 = 55. Example 2: 36 + 7 = 36 + 4 + 3 = 43.",
            @"Step 1: Find how many ones the two-digit number needs to reach the next ten. Step 2: Split the one-digit addend into that part and leftovers. Step 3: Compose a full ten and add leftovers. Step 4: Check by counting on.",
            @"Do not stop at 50 after using only three of the eight added ones in 47+8; five ones still remain.",
            @"I can make a new ten while adding a one-digit number to a two-digit number."),
        new(@"PED:US-CCSS-MATH:G1:U05:L06",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-6/preparation.html",@"973308633a370cd4487792a7ea75b8a8123eccf12337992b7cb76d277b123854",[@"CCSS:1.NBT.C.4",@"CCSS:1.OA.C.6"],
            @"Make a Ten and Make Sense of Equations PLC Activity",
            @"Analyze make-ten equations for adding a one-digit and a two-digit number; explain why regrouping maintains equality. This is a teacher-led PLC discussion.",
            @"Example 1: 47+8 = 47+3+5 = 50+5 = 55. Example 2: 28+5=28+2+3=33. Example 3: 39+4=39+1+3=43.",
            @"Step 1: Identify the needed amount to make the next ten. Step 2: Decompose the other addend. Step 3: Write a chain of equations whose sides truly have the same value. Step 4: Explain each equality.",
            @"Do not write 47+8=50+8: moving three ones to complete a ten leaves five, not eight.",
            @"I can explain correct make-ten equations; partner discussion is verified separately by the teacher."),
        new(@"PED:US-CCSS-MATH:G1:U05:L07",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-7/preparation.html",@"241b1c73722edb009d4121faa6cbcbe86161c462bdf79e9671cdd56b909761ee",[@"CCSS:1.NBT.C.4"],
            @"Does it Make a New Ten?",
            @"Determine WHETHER adding a one-digit number to a two-digit number composes a new ten and write an equation showing the strategy.",
            @"Example 1: 45+5=50; five ones plus five ones makes one new ten. Example 2: 43+5=48, so no new ten. Example 3: 58+7=65; 58+2+5=65.",
            @"Step 1: Add the ones of both addends. Step 2: If the ones total reaches ten or more, compose one new ten. Step 3: Write an equation recording what changes. Step 4: Verify the total.",
            @"Do not claim every two-digit plus one-digit addition makes a new ten. 43+5=48 does not.",
            @"I can tell when adding composes a new ten and justify with an equation."),
        new(@"PED:US-CCSS-MATH:G1:U05:L08",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-8/preparation.html",@"aad87cbe8f02f23279a42bd4de1e5febd1a53b58cf29e1c41e45294fd6d2c3eb",[@"CCSS:1.NBT.C.4",@"CCSS:1.OA.C.6",@"CCSS:1.OA.D.8"],
            @"Center Day 2",
            @"Teacher-led center day: add within 100, including examples with or without composing a ten. Show the math and check the value; physical game participation is not auto-scored.",
            @"Example 1: 38+6=38+2+4=44 (make a ten). Example 2: 32+24=56 (no new ten). Example 3: 57+25=82 (new ten).",
            @"Step 1: Identify the addends. Step 2: Choose add-tens/ones or make-ten. Step 3: Write a correct equation. Step 4: Verify the total is within 100 and explain your choice.",
            @"Do not reuse a no-regrouping algorithm when ones make ten; 57+25 is 82, not 72.",
            @"I can add within 100 using strategies that fit the numbers."),
        new(@"PED:US-CCSS-MATH:G1:U05:L09",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-9/preparation.html",@"77345a8d0297619605cf994f800beeb7c99b1e74bf8bd70a4598fdbf50575d42",[@"CCSS:1.NBT.C.4"],
            @"Add 2 Two-digit Numbers",
            @"Add two two-digit numbers that compose a ten. Explain where the new ten comes from rather than mechanically carrying a digit.",
            @"Example 1: 38+25=38+20+2+3=63. Example 2: 46+27=73; six ones plus seven ones make one ten and three ones.",
            @"Step 1: Add tens. Step 2: Combine ones. Step 3: Exchange ten of the ones for one additional ten. Step 4: Record the new number of tens and leftover ones. Step 5: Verify.",
            @"Do not ignore the newly made ten: 38+25 is 63, not 53.",
            @"I can add two two-digit numbers within 100 by composing a new ten."),
        new(@"PED:US-CCSS-MATH:G1:U05:L10",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-10/preparation.html",@"d959d22c069d35452c3261e2d47ece8b5602cd6c284962754be8a8432241201e",[@"CCSS:1.NBT.C.4",@"CCSS:1.OA.D.8"],
            @"Tens and Tens, Ones and Ones",
            @"Add two-digit numbers by explicitly combining TENS with TENS and ONES with ONES, exchanging when there are ten or more ones.",
            @"Example 1: 37+26 = (30+20)+(7+6) = 50+13=63. Exchange ten of the thirteen ones for an additional ten.",
            @"Step 1: Represent each addend as tens and ones. Step 2: Add tens and ones separately. Step 3: Regroup ten ones into one ten. Step 4: Combine to a final total and check.",
            @"Do not mistake 50+13 for 513 or for 53; ten ones represent an additional ten.",
            @"I can explain how tens and ones combine in two-digit addition."),
        new(@"PED:US-CCSS-MATH:G1:U05:L11",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-11/preparation.html",@"0560868a9b518234ee6d9396ab88055086709f55a9d9830b085ed7d4044b29d9",[@"CCSS:1.NBT.C.4",@"CCSS:1.OA.D.7"],
            @"How Did You Do That?",
            @"Analyze two-digit addition methods and explain how different true equations represent the same sum.",
            @"Example 1: 37+25=37+20+5=57+3+2=62. Example 2: (30+20)+(7+5)=50+12=62. Both methods agree.",
            @"Step 1: Read both addends and one proposed strategy. Step 2: Check each transformation preserves the total. Step 3: Identify where a ten forms. Step 4: Compare with another valid method.",
            @"Do not write 37+25=37+20+2 because three ones disappear.",
            @"I can interpret and compare accurate place-value addition strategies."),
        new(@"PED:US-CCSS-MATH:G1:U05:L12",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-12/preparation.html",@"c62773916f506f2731dbb37579492188cf54f92f50fda9fa2146209e03e6de31",[@"CCSS:1.NBT.C.4"],
            @"Add it Up PLC Activity",
            @"Write equations to show HOW two two-digit numbers can be added, including making a new ten.",
            @"Example 1: 37+25=(30+20)+(7+5)=50+12=62. Example 2: 48+14=48+10+2+2=62.",
            @"Step 1: Choose a strategy (tens/ones or add-on). Step 2: Write the starting addition. Step 3: Show the tens composition as a true equation. Step 4: Write the final sum and verify.",
            @"Do not omit ones or create an equality that changes the value between steps.",
            @"I can write equivalent equations describing my two-digit addition method."),
        new(@"PED:US-CCSS-MATH:G1:U05:L13",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-13/preparation.html",@"bd186fde246e3a3b3ff5dd24dae4d2b54f0a8192642f291372a14de5b2a9228a",[@"CCSS:1.NBT.A.1",@"CCSS:1.NBT.B.3",@"CCSS:1.NBT.C.4",@"CCSS:1.OA.C.5",@"CCSS:1.OA.C.6",@"CCSS:1.OA.D.8"],
            @"Center Day 3",
            @"Center day: practice adding within 100 with place-value representations and explain the observed number pattern. Teacher evaluates partner discussion; digital tasks evaluate calculations only.",
            @"Example 1: 26+38=64 because 20+30=50 and 6+8=14. Example 2: 54+25=79 because ones make nine and tens make seventy.",
            @"Step 1: Read the actual center prompt. Step 2: Decide whether ten must be composed. Step 3: Use a drawing/equation to add accurately. Step 4: Explain the pattern if a teacher supervises the activity.",
            @"Do not treat a teacher-led game as completed when only an addition answer was entered.",
            @"I can use place-value methods to add within 100 in a classroom center."),
        new(@"PED:US-CCSS-MATH:G1:U05:L14",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-5/lesson-14/preparation.html",@"aba40eb78d9fadc64bea53be9bfdb4c7c5604a2d00880f4339b38d2d95d6ed69",[@"CCSS:1.NBT.C.4",@"CCSS:1.NBT.C.5",@"CCSS:1.NBT.C.6"],
            @"Food Drive (optional)",
            @"ORIGINAL STUDENT FACING TASK: Use the FOUR-STUDENT Food Drive table (Lin 18, Priya 24, Han 13, Tyler 30) to calculate several different pair totals and all four together: 85 cans. Then use the source Day 1 grade table (Kindergarten 18, first 51, second 23, third 13, fourth 39, fifth 40, sixth 8, seventh 29, eighth 30) to find which TWO grades could fill a box with between 35 and 65 cans; optional challenge: can THREE grades share a box? Write each matching equation. Real donations, packing, pupils, and classroom participation are never presumed or auto-scored.",
            @"Example 1: Lin 18 cans + Priya 24 cans = 42; Han 13 + Tyler 30 = 43; all four students together collected 85 cans. Example 2: From the Day 1 grade table, Kindergarten 18 and second grade 23 make 41, so this pair qualifies for a 35–65-can box. Example 3: First grade 51 + fifth grade 40 = 91, so they cannot share one 35–65-can box. Example 4: Kindergarten 18 + second grade 23 + third grade 13 = 54, so those THREE grades could fit the optional challenge.",
            @"Step 1: Read ALL four original student counts and calculate each named pair; check that the two disjoint pairs still add to 85. Step 2: Read the original Day 1 grade table and choose two distinct grades. Step 3: Add tens and ones, composing a new ten as needed, and decide whether the box has 35–65 cans inclusive. Step 4: Record the two grades, equation, total and qualifying decision. Step 5: For the optional challenge, try three distinct grades and verify the same total interval. Teacher reviews class discussion and real packing if later carried out.",
            @"Do not claim a real food drive took place or that the two groups were personally counted by a student based on a digital question.",
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
       IsTarget(doc,lesson)?Version:previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson,string previous)=>
       IsTarget(doc,lesson)&&previous==CommonCoreRichContentCorrections.CorrectionContentVersion;
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
