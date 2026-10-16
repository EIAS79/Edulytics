using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Original IM 2021 G1 U02 L11-L15 source-goal instructional corrections.
/// Authored against publisher lesson purposes and student-facing activities.
/// Source-guarded content candidates; neither Practice acceptance nor B001 release.
/// </summary>
public static class CommonCoreG1Unit2Part4SourceTeachingCorrections
{
    public const string CorrectedContentVersion = "us-ccss-g1-u02-l11-l15-original-source-teaching-v1";
    private sealed record Repair(string Code,string SourceUrl,string Sha,string[] Outcomes,
        string OldStepPrefix,string OldExamplePrefix,string Explanation,
        string Concepts,string Examples,string Steps,string Mistakes,string Summary);
    private static readonly Repair[] Repairs =
    [
        new("PED:US-CCSS-MATH:G1:U02:L11","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-11/preparation.html","b0774a01ae8cf0db4cbde91a4fc46f50f42cd778964e6786a268701fe345b568",
            ["CCSS:1.OA.A.1"],
            "Step 1: Identify the two quantities, 3 and 7.","Example 1: The source task combines 3 and 7. Start at 3 an",
            "Two towers have different numbers of connecting cubes. Compare the heights rather than joining the towers. The difference is the number of cubes needed to make their amounts equal. You can add cubes to the shorter tower or remove cubes from the taller one. Both strategies show the same difference.","Learning goal: Represent and solve Compare, Difference Unknown problems with cube towers, drawings and equations. Match the difference to the question, not the combined number of cubes.\nIf one tower has 9 cubes and another has 5, four cubes are missing from the shorter tower: 5 + [4] = 9. Equivalently, 9 - 5 = [4]. Mark the difference with a box.","Example 1 — Make two towers the same. Diego has a 9-cube tower, while Jada has a 5-cube tower. Build or draw the towers aligned at the bottom. Count the four extra cubes on the longer tower. Add four cubes to 5: 5 + [4] = 9; or remove four from 9: 9 - 4 = 5.\nExample 2 — A second pair of towers. One tower has 8 cubes and the other has 3 cubes. Five cubes make the shorter tower equal: 3 + [5] = 8 and 8 - 3 = [5].\nExample 3 — Check. Towers of 7 and 4 cubes differ by three. Add three to the shorter: 4 + 3 = 7. The total of 11 cubes is not the requested difference.",
            "Step 1: Build or sketch the two cube towers with their bases level.\nStep 2: Count each tower and identify which is taller.\nStep 3: Compare aligned cubes; count the unmatched cubes at the top of the taller tower.\nStep 4: Write shorter + [difference] = taller or taller - shorter = [difference].\nStep 5: Check by adding that many cubes to the shorter tower or removing them from the taller tower; explain why the towers are now equal.","Do not add the towers to answer how many more or fewer; that asks for the difference, not the combined total.\nDo not subtract the smaller amount from itself. Identify the two towers before writing the equation.\nAlign the towers from the bottom so you can see the unmatched cubes clearly.","I can use aligned cube towers and a drawing to find how many more cubes one tower has, and write an addition or subtraction equation that shows the difference."),
        new("PED:US-CCSS-MATH:G1:U02:L12","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-12/preparation.html","4fce1045bbb704d1f2d1ce0a587ba65c6a68a757879d6ab0bedfb083216a67cb",
            ["CCSS:1.OA.A.1","CCSS:1.OA.C.5"],
            "Step 1: Represent the quantities or objects in the task.","Example 1: “How is their work the same? How is it differen",
            "School supplies can be compared one-to-one with the students or objects that need them. Read the story carefully, decide which quantity is greater and find how many items are missing so each object can be matched. A comparison difference can be represented with a drawing, cubes, an addition equation or subtraction.","Learning goal: Solve Compare, Difference Unknown stories and relate counting on to addition and subtraction. Draw one-to-one matches where possible.\nIf 10 students need a pencil and there are 6 pencils, four students have no pencil. The difference is 10 - 6 = [4] or 6 + [4] = 10. Box four: the question does not ask how many students there are.","Example 1 — Pencils for students. Ten students sit at a table, but only six pencils are available. Draw ten students and connect six pencil drawings to six students. Four students remain without pencils: 10 - 6 = 4. Check by counting on from 6 to 10: 7, 8, 9, 10 means four more.\nExample 2 — Marker caps. Nine markers are in a bin, but there are four caps. Pair each cap with a marker and count five uncovered markers. Write 4 + [5] = 9 and 9 - 4 = [5].\nExample 3 — Check a smaller set. Seven notebooks need labels, but there are five labels. Two labels are missing: 5 + 2 = 7.",
            "Step 1: Read the school-supply story three times: identify the objects, the known numbers and what is being asked.\nStep 2: Draw or place counters for both amounts and make as many one-to-one pairs as possible.\nStep 3: Count the unpaired objects; this is the difference.\nStep 4: Write larger - smaller = [difference] and smaller + [difference] = larger.\nStep 5: Count on or recombine the amounts to check the answer and explain which objects were missing.","Do not treat every comparison as taking objects away physically; pairing is enough to see the difference.\nDo not answer ten when the question asks how many students do not receive a pencil.\nFor 9 markers with only 4 caps, subtract caps from markers, not markers from caps.","I can compare school-supply amounts by pairing, drawing or counting on, and explain the same difference using addition and subtraction."),
        new("PED:US-CCSS-MATH:G1:U02:L13","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-13/preparation.html","c96a292f5540d845faffc5fdecc78d1702ea587b17545274b70091023c8582e4",
            ["CCSS:1.MD.C.4","CCSS:1.OA.A.1"],
            "Step 1: Identify the two quantities, 5 and 3.","Example 1: The source task combines 5 and 3. Start at 5 an",
            "A chart about favorite art supplies shows how many responses each category received. Read or build the data display, compare two categories, and decide which has more. The difference is the number of extra responses in the larger category. Keep the category labels connected to the numbers and write an equation for the question.","Learning goal: Compare categorical data with a difference-unknown story and a matching equation. For two categories, count their marks or objects and compare the totals; do not add them when asked how many more or fewer.\nIllustrative practice data: 7 children chose crayons and 4 chose paintbrushes. The difference is 7 - 4 = [3], and 4 + [3] = 7. This made-up small example is not a real classroom survey.","Example 1 — Favorite art supplies, illustrative practice data. A simple class chart has seven marks by 'crayons' and four by 'paintbrushes'. Pair four crayon marks with four paintbrush marks. Three crayon marks remain. Answer: three more children chose crayons; 7 - 4 = [3].\nExample 2 — Addition checks the comparison. Four children chose paintbrushes. Three more chose crayons than paintbrushes. Four plus three equals seven: 4 + 3 = 7.\nExample 3 — Read a new sample chart. Six votes for pencils and two for glue sticks mean 6 - 2 = 4 more votes for pencils. Explain the four unmatched votes, not the total of eight.",
            "Step 1: Read the chart title and identify the two art-supply categories in the question.\nStep 2: Count the symbols or tally marks for each category, keeping labels with their counts.\nStep 3: Represent the two totals with two rows of counters or a labelled drawing.\nStep 4: Count the unmatched marks and write larger - smaller = [difference], or smaller + [difference] = larger.\nStep 5: State which category has more or fewer and check the equation against the chart.","Do not invent a real classroom survey from an illustrative chart; use actual observed votes only when they were collected.\nDo not combine both categories if the question asks how many more children chose one art supply.\nDo not swap category labels when comparing quantities.","I can read and compare a data chart, represent how many more or fewer, and write a correct comparison equation."),
        new("PED:US-CCSS-MATH:G1:U02:L14","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-14/preparation.html","0e88df18d97bb0f6faead369bc64bc5e7758791a87a81cd9f6e3171f8bbcae2e",
            ["CCSS:1.OA.A.1","CCSS:1.OA.C.5","CCSS:1.OA.C.6","CCSS:1.OA.D.7"],
            "Step 1: Identify the starting quantity 8 and the amount 3 being removed or compared.","Example 1: The source task compares or removes 3 from 8. T",
            "A compare story about two groups can match both an addition equation and a subtraction equation. Read what the question asks, find the difference, and explain why two different equations represent the same relationship. Draw a labelled model, then choose among possible equations rather than assuming every equation containing the same numbers is correct.","Learning goal: Match addition and subtraction equations to Compare, Difference Unknown story problems. If there are 8 glue sticks and 3 scissors, there are five fewer scissors: 8 - 3 = [5]. The same relationship is 3 + [5] = 8.\nIn the Which Equation? activity, choose an equation based on the story. The equal sign represents the same amount on both sides, not simply 'write the answer next'.","Example 1 — Art station. There are eight glue sticks and three scissors. How many fewer scissors than glue sticks? Pair three scissors with three glue sticks, leaving five glue sticks unmatched. Both 8 - 3 = [5] and 3 + [5] = 8 are true statements of the difference.\nExample 2 — Reading rug. Five red pillows and three blue pillows are on the rug. Two more red pillows than blue pillows: 5 - 3 = [2], or 3 + [2] = 5. The equation 5 + 3 = 8 counts all pillows but does not answer 'how many more red'.\nExample 3 — Explain a choice. If a classmate says 8 + 3 = 11 for the glue-stick question, show why eleven is the total of both groups and not the difference.",
            "Step 1: Read the comparison story and identify the two labelled quantities.\nStep 2: Draw or model matched pairs to reveal how many objects are extra.\nStep 3: Write the subtraction equation larger - smaller = [difference].\nStep 4: Write the related addition equation smaller + [difference] = larger; check that both equations match the story.\nStep 5: Explain why a tempting equation that adds the two groups is not the correct answer to a 'how many more or fewer' question. Choose a familiar center only as a separate practice activity.","Do not select an equation simply because it contains the story's numbers; identify what each number represents.\nDo not confuse the total of 8+3 with the difference of 8 and 3.\nIn a comparison question, the answer is the unmatched number of objects, even when the subtraction and addition equations place the blank differently.","I can solve a comparison, match it to a subtraction equation and an addition equation, and justify which equations answer the question."),
        new("PED:US-CCSS-MATH:G1:U02:L15","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-15/preparation.html","fe8826ae5ada9f876f89fab1ec84535dfb501fa21ef9fb9f9bc19669d656e0b1",
            ["CCSS:1.OA.A.1","CCSS:1.OA.B.4","CCSS:1.OA.C.6"],
            "Step 1: Identify the two quantities, 3 and 4.","Example 1: The source task combines 3 and 4. Start at 3 an",
            "Story problems may ask for the total, a missing part, a change, or a comparison difference. Read the story first to locate the unknown. Represent the quantities with objects or a labelled drawing and write an equation that fits. Different story types can use the same numbers but place the unknown in different parts of the equation.","Learning goal: Solve different types of addition and subtraction stories and write matching equations. Choose whether the unknown is a whole, a part, a change, or a difference.\nPart-whole example: Eight people are at a table and six are students. Two are teachers: 6 + [2] = 8 or 8 - 6 = [2]. This is not an Add To story because nobody joins the group.","Example 1 — People at a table. Eight people sit at a table; six are students. How many are teachers? Draw eight counters, label six as students and circle the remaining two. Equation: 6 + [2] = 8; check with 8 - 6 = [2].\nExample 2 — Change unknown. Five children are playing. Some more join and now there are nine. The number who joined is 9 - 5 = [4], and 5 + [4] = 9.\nExample 3 — Difference unknown. Seven birds are on one fence and four are on another. How many more birds are on the first fence? Pair four birds; three are extra. 7 - 4 = [3]. Discuss why the same numbers can appear in different stories.",
            "Step 1: Read each story and identify the quantities and the exact question.\nStep 2: Decide whether the story asks for the whole, a missing part, an amount that joined or left, or a comparison difference.\nStep 3: Draw labelled groups or represent the story with counters and a box for the unknown.\nStep 4: Write an equation whose numbers correspond to the labelled groups, then solve it.\nStep 5: Check by acting out or redrawing the story. If another equation matches the same situation, explain why it also works.","Do not assume an action happens in a Put Together story; 'eight people, six students' does not say more people arrived.\nDo not automatically add every pair of numbers: a difference-unknown question requires comparing them.\nDo not box the final position of an equation if that number is already given; box the quantity asked about.","I can tell different types of story problems apart and use drawings, addition and subtraction equations to solve the unknown quantity."),
    ];
    public static IReadOnlyCollection<string> TargetLessonCodes => Repairs.Select(r=>r.Code).ToArray();
    private static Repair? Find(CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson)=>
        doc.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        doc.VersionCode=="CCSSM-2010" &&
        doc.ContentVersion=="phase29-source-intelligence-en-v2-g1"
            ? Repairs.SingleOrDefault(r=>r.Code==lesson.LessonCode) : null;

    public static bool IsTarget(CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson)=>
        Find(doc,lesson)!=null;

    private static void Check(CanonicalLessonContentPackLesson lesson,Repair r)
    {
        if(lesson.SourceUrl!=r.SourceUrl ||
           !string.Equals(lesson.CanonicalBodySha256,r.Sha,StringComparison.OrdinalIgnoreCase) ||
           !lesson.OutcomeCodes.SequenceEqual(r.Outcomes))
            throw new InvalidOperationException("Original G1 U02 L11-L15 source drift: "+lesson.LessonCode);
    }
    private static bool Matches(CanonicalLessonContentPackTranslation en,Repair r)=>
        en.Explanation==r.Explanation && en.KeyConceptsAndRules==r.Concepts &&
        en.WorkedExamples==r.Examples && en.StepByStepSolutions==r.Steps &&
        en.CommonMistakes==r.Mistakes && en.QuickSummary==r.Summary;

    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        if(doc.PackCode!=MathematicsCurriculumPackRegistry.CommonCoreCode ||
           doc.VersionCode!="CCSSM-2010")return;
        foreach(var lesson in doc.Lessons)
        {
            var repair=Find(doc,lesson);
            if(repair is null)continue;
            Check(lesson,repair);
            var en=lesson.Translations.Single(t=>t.CultureCode=="en");
            if(Matches(en,repair))continue;
            if(!en.StepByStepSolutions.StartsWith(repair.OldStepPrefix,StringComparison.Ordinal) ||
               !en.WorkedExamples.StartsWith(repair.OldExamplePrefix,StringComparison.Ordinal))
                throw new InvalidOperationException("Unknown G1 U02 L11-L15 learner body: "+lesson.LessonCode);
            en.Explanation=repair.Explanation;
            en.KeyConceptsAndRules=repair.Concepts;
            en.WorkedExamples=repair.Examples;
            en.StepByStepSolutions=repair.Steps;
            en.CommonMistakes=repair.Mistakes;
            en.QuickSummary=repair.Summary;
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string prior)=>
        IsTarget(doc,lesson)?CorrectedContentVersion:prior;

    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string existingVersion)=>
        IsTarget(doc,lesson) &&
        existingVersion==CommonCoreRichContentCorrections.CorrectionContentVersion;

    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string existingVersion,
        IReadOnlyCollection<CurriculumLessonContentTranslation> existing)
    {
        var repair=Find(doc,lesson) ?? throw new InvalidOperationException("Not a G1 U02 L11-L15 target");
        Check(lesson,repair);
        if(!CanUpgradeExisting(doc,lesson,existingVersion))
            throw new InvalidOperationException("Unsafe G1 U02 L11-L15 persisted version: "+lesson.LessonCode);
        if(existing.Count==0)return;
        if(existing.Count!=1)throw new InvalidOperationException("Unexpected G1 U02 L11-L15 culture count");
        var en=existing.Single();
        if(en.CultureCode!="en" ||
           !en.StepByStepSolutions.StartsWith(repair.OldStepPrefix,StringComparison.Ordinal) ||
           !en.WorkedExamples.StartsWith(repair.OldExamplePrefix,StringComparison.Ordinal))
            throw new InvalidOperationException("Persisted G1 U02 L11-L15 text differs: "+lesson.LessonCode);
    }
}
