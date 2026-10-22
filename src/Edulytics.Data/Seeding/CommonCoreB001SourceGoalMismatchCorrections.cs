using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;

/// <summary>
/// Exact original IM 2021 learning-goal corrections for four B001 math lessons.
/// Author-written examples; guarded against old source, objectives and persisted text drift.
/// This alone is NOT academic certification or a batch release.
/// </summary>
public static class CommonCoreB001SourceGoalMismatchCorrections
{
    public const string CorrectedContentVersion = "us-ccss-b001-original-source-goals-repaired-v1";
    private sealed record Repair(string Code,string SourceUrl,string SourceContentSha,string[] Outcomes,
        string PreviousSteps,string PreviousExamplePrefix,string Explanation,string Concepts,
        string Examples,string Steps,string Mistakes,string Summary);
    private static readonly Repair[] Repairs =
    [
        new("PED:US-CCSS-MATH:G2:U02:L01", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-2/lesson-1/preparation.html", "8e445fd79a998d2d470388219246bf782699e3f09f9f109c3ca3d633de7e3139", ["CCSS:2.MD.D.10", "CCSS:2.NBT.B.5"],
            "Step 1: Read the category values from the bar graph.\nStep 2: Decide whether the question asks for a total or a comparison.\nStep 3: For a total, add the values, such as \\(27+15=42\\).\nStep 4: For how many more or how many fewer, subtract the smaller value from the larger, such as \\(32-18=14\\).\nStep 5: Check that the answer matches the graph and the wording of the question.", "Example 1: A bar graph shows 32 sunny days and 18 cloudy days. How man",
            "In this lesson you will add and subtract within 100 without making or breaking a ten. Start by reading a bar graph. Work with tens and ones separately and compare two ways to find the same total or difference. When adding, keep the ones under ten. When subtracting, make sure the ones being removed are no more than the ones available.", "Learning goal: Add and subtract within 100 without composing or decomposing a ten. Use bar-graph quantities to show a total or compare two groups.\nA tens tower is worth 10 and a single cube is worth 1. For 34 + 23, 3 tens + 2 tens = 5 tens, and 4 ones + 3 ones = 7 ones. The sum is 57 without exchanging any tens.\nTo compare 68 and 25, subtract 2 tens from 6 tens and 5 ones from 8 ones. The difference is 43.", "Example 1 — Compare two snack choices. A bar graph shows 46 students chose popcorn and 22 chose crackers. How many more chose popcorn? Compare 46 − 22: 4 tens − 2 tens = 2 tens; 6 ones − 2 ones = 4 ones. There are 24 more students choosing popcorn.\nExample 2 — Find the total from the same graph. 46 students chose popcorn and 22 chose crackers. Together: 46 + 22 = 68 (6 tens and 8 ones). The ones do not need to be regrouped.\nExample 3 — Compare methods. Find 34 + 23 by combining 30 + 20 = 50 and 4 + 3 = 7, or by using base-ten blocks. Both give 57.", "Step 1: Read the title, labels, and exact values of the categories in the bar graph.\nStep 2: Decide whether the question asks for a combined total or how many more or fewer.\nStep 3: Represent each number using tens and ones (for example, 46 is 4 tens and 6 ones).\nStep 4: Add or subtract tens separately from ones without exchanging groups of ten.\nStep 5: Check the answer by rereading the graph and comparing a second method.", "Do not use 27 + 15 as the first worked example for a no-regrouping lesson: 7 ones + 5 ones needs a new ten. Use 34 + 23 instead.\nDo not use 32 - 18 here: subtracting 8 ones from 2 requires decomposing a ten. Try 68 - 25 instead.\nWhen reading a bar graph, do not choose subtraction merely because two category values are shown; choose the operation that answers the question.", "I can add and subtract 2-digit graph values without exchanging a ten and explain my tens-and-ones strategy."),
        new("PED:US-CCSS-MATH:G2:U03:L12", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-3/lesson-12/preparation.html", "d72cead79fd5a191a191af5bf4801296111b55633bfe608ddb195b27fb4b7adb", ["CCSS:2.MD.B.5", "CCSS:2.NBT.B.5"],
            "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.", "Example 1: Decide if each statement is true or false. Be prepared to e",
            "In this lesson you will solve story problems about lengths in centimeters or inches. Some stories need two operations: after you solve the first part, use its answer to solve the next part. Draw a labelled tape diagram or strips to keep track of the remaining length and include the correct unit.", "Learning goal: Represent and solve one- and two-step length story problems within 100. Use a tape diagram, labelled lengths, or equations to explain the two linked steps.\nA part removed from a ribbon leaves a shorter remaining part. Draw a starting bar and divide it into pieces for each cut and what remains. Label all values in the same unit.\nIn a two-step story, the answer after the first subtraction becomes the starting quantity for the second subtraction.", "Example 1 — Two ribbon cuts. Elena has 86 cm of ribbon. She cuts 24 cm for a card, then 18 cm for a gift. First, 86 − 24 = 62 cm remain. Next, 62 − 18 = 44 cm remain. A tape diagram can show 86 cm split into 24 cm, 18 cm, and 44 cm.\nExample 2 — Two pieces joined. A blue ribbon is 26 cm long. A green ribbon is 34 cm long. Joined end to end they measure 26 + 34 = 60 cm. Show the two parts in a tape diagram and mark 60 cm for the whole.\nExample 3 — Check the cuts. The two removed parts measure 24 + 18 = 42 cm. Adding 42 + 44 = 86 cm confirms the original ribbon length.", "Step 1: Read the story, identify lengths and units, and say what each question asks.\nStep 2: Draw a tape diagram showing the whole and each known or unknown ribbon part.\nStep 3: Solve the first operation, such as 86 − 24 = 62 cm.\nStep 4: Use the first result in the second operation: 62 − 18 = 44 cm.\nStep 5: Label the answer in centimeters and check that the diagram and both equations represent the story.", "Do not subtract 18 cm from the original 86 cm again after already removing 24 cm. The second cut is from the 62 cm remaining.\nDo not omit the unit: 44 cm is a length, not just the number 44.\nDo not use an unlabelled tape diagram or choose a one-step equation when the story contains two separate cuts.", "I can draw and label a two-step ribbon story, find the first remaining length, use it for the next calculation, and check my final result in the correct unit."),
        new("PED:US-CCSS-MATH:G3:U01:L12", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-1/lesson-12/preparation.html", "6b08f29c7abd8b9f4e19a52355707c7da0417d05ad856db570554c7c0edbcc2a", ["CCSS:3.OA.A.1", "CCSS:3.OA.A.3"],
            "Step 1: Identify the equal groups, rate, or scaling relationship.\nStep 2: Represent the groups or factors.\nStep 3: Write the multiplication relationship.\nStep 4: Calculate the product using an efficient method.\nStep 5: Check the product against the representation or context.", "Example 1: How many do you see? How do you see them?\nApproach: Identif",
            "In this lesson you will ask questions about an equal-groups situation, make a drawing or a tape diagram, and write a multiplication equation to solve it. The two factors have different meanings: one tells how many groups, and the other how many objects are in each group.", "Learning goal: Represent and solve multiplication stories with equal groups, drawings, arrays, tape diagrams and equations.\nA situation with 4 equal groups of 3 means four groups, each containing three objects. The multiplication 4 × 3 = 12 gives the total of twelve objects.\nA diagram makes the meaning of each factor visible. You can use skip-counting or known products to calculate, but explain how the answer matches the groups.", "Example 1 — Create the question first. Four trays are on a desk. Ask: How many pencils are on each tray? How many pencils are there altogether? Now learn that each tray has 3 pencils. Draw four groups of three. Write 4 × 3 = 12; there are 12 pencils.\nExample 2 — Show the same story three ways. Draw 5 circles with 2 stars in each, draw a tape diagram with 5 parts each labelled 2, and write 5 × 2 = 10. All three models show five equal groups of two.\nExample 3 — Explain the factors. In 3 × 6 = 18, 3 might count three cartons and 6 might count six eggs in each carton. Write a complete sentence explaining the product of 18 eggs.", "Step 1: Read the story and ask what information you need about the groups.\nStep 2: Identify the number of equal groups and the number of objects in each group.\nStep 3: Draw groups, an array, or a labelled tape diagram showing the relationship.\nStep 4: Write an equation using the group count and group size, then calculate the product.\nStep 5: Check the product using the drawing, skip-counting, or a known multiplication fact.", "Do not multiply numbers from the story before deciding what each factor counts.\nDo not treat unequal groups as equal groups. Draw or count the objects in each group first.\nDo not mistake a picture's size for the number of objects; use its labels and the actual group counts.", "I can make sense of equal-group stories, draw or label a useful model, write a matching multiplication equation, and explain its factors."),
        new("PED:US-CCSS-MATH:G3:U01:L15", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-1/lesson-15/preparation.html", "aac738c55f6a91abc87d7a65ca0c7daef877875a719e3ca398e2725691b8fd7f", ["CCSS:3.OA.A.3", "CCSS:3.OA.A.4", "CCSS:3.OA.D.9"],
            "Step 1: Identify the quantities being combined.\nStep 2: Represent each quantity with objects, a drawing, number line, or expression.\nStep 3: Write an addition relationship that matches the representation.\nStep 4: Find the total using an efficient strategy.\nStep 5: Check that the total matches the original quantities.", "Example 1: Find the value of each expression mentally. \\(1\\times10\\) \\",
            "In this lesson you will solve multiplication stories, including situations in which one factor is unknown. Use equal-group models and a multiplication equation with a box for the missing number. A drawing, array, or skip-count can help find the number that makes the equation true.", "Learning goal: Solve multiplication problems and write an equation with a symbol for the unknown factor or product.\nIn 4 × [ ] = 20, four equal groups make twenty. Each group must contain five because 4 × 5 = 20.\nMultiplication describes equal-size groups and arrays. Repeated addition may help compute a product but does not by itself define every meaning of multiplication. Explain what each factor represents.", "Example 1 — Unknown group size. Four picnic tables each have the same number of chairs. There are 20 chairs in all. Write 4 × [ ] = 20. Put five chairs at each table or count 4, 8, 12, 16, 20 in equal groups; 4 × 5 = 20, so the missing factor is 5 chairs per table.\nExample 2 — Unknown group count. There are 18 crayons placed in equal packs of 6. How many packs are there? [ ] × 6 = 18. Three packs of six make 18; the missing factor is 3 packs.\nExample 3 — Products grow in groups of ten. 2 × 10 = 20 and 3 × 10 = 30. One more group of ten increases the product by ten; explain the pattern with a sketch of groups.", "Step 1: Identify the equal groups and decide whether group count, group size, or the total is unknown.\nStep 2: Draw an array, equal groups, or a tape diagram and label the known quantities.\nStep 3: Write a multiplication equation with a box for the unknown number.\nStep 4: Find the value that makes the equation true using a diagram, skip-counting or multiplication facts.\nStep 5: Replace the box with your value and check that the equation and its units match the story.", "Do not replace the multiplication relationship with only an addition relationship. Write an equal-groups equation with ×.\nDo not confuse the number of groups with the number in each group; identify the meaning of each factor.\nDo not assume the missing number is always the product after =. It may be the first or second factor.", "I can solve equal-group stories, show the unknown factor with a box, use a model to find it, and check the resulting multiplication equation.")
    ];
    public static IReadOnlyCollection<string> TargetLessonCodes => Repairs.Select(x=>x.Code).ToArray();
    private static Repair? Find(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson) =>
        doc.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        doc.VersionCode=="CCSSM-2010" &&
        doc.ContentVersion=="phase30-common-core-rich-content-v1" ?
            // Later, separately SHA-guarded original-source repairs for these two
            // lessons supersede the older B001 generic corrections. Keep the
            // original B001 candidates for all other genuinely targeted lessons.
            (CommonCoreG2Unit2AddSubtractCompareTeachingCorrection.IsTarget(doc,lesson) ||
             CommonCoreG2Unit3FriendshipBraceletsTeachingCorrection.IsTarget(doc,lesson)
               ? null
               : Repairs.SingleOrDefault(x=>x.Code==lesson.LessonCode)) : null;

    public static bool IsTarget(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson) => Find(doc,lesson)!=null;

    private static void CheckIdentity(CanonicalLessonContentPackLesson lesson,Repair r)
    {
        if(lesson.SourceUrl!=r.SourceUrl ||
           !string.Equals(lesson.CanonicalBodySha256,r.SourceContentSha,StringComparison.OrdinalIgnoreCase) ||
           !lesson.OutcomeCodes.SequenceEqual(r.Outcomes))
           throw new InvalidOperationException($"B001 source/standard drift: {lesson.LessonCode}");
    }
    private static bool Matches(CanonicalLessonContentPackTranslation t,Repair r) =>
        t.Explanation==r.Explanation && t.KeyConceptsAndRules==r.Concepts &&
        t.WorkedExamples==r.Examples && t.StepByStepSolutions==r.Steps &&
        t.CommonMistakes==r.Mistakes && t.QuickSummary==r.Summary;
    public static void Apply(CanonicalLessonContentPackDocument document)
    {
        if(document.PackCode!=MathematicsCurriculumPackRegistry.CommonCoreCode ||
           document.VersionCode!="CCSSM-2010") return;
        foreach(var lesson in document.Lessons)
        {
            var repair=Find(document,lesson);
            if(repair is null)continue;
            CheckIdentity(lesson,repair);
            var en=lesson.Translations.Single(t=>t.CultureCode=="en");
            if(Matches(en,repair))continue;
            if(en.StepByStepSolutions!=repair.PreviousSteps ||
               !en.WorkedExamples.StartsWith(repair.PreviousExamplePrefix,StringComparison.Ordinal))
               throw new InvalidOperationException(
                   $"Refusing unexpected original learner-body mismatch: {lesson.LessonCode}");
            en.Explanation=repair.Explanation;
            en.KeyConceptsAndRules=repair.Concepts;
            en.WorkedExamples=repair.Examples;
            en.StepByStepSolutions=repair.Steps;
            en.CommonMistakes=repair.Mistakes;
            en.QuickSummary=repair.Summary;
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previous) =>
        IsTarget(doc,lesson)?CorrectedContentVersion:previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string existingVersion) =>
        IsTarget(doc,lesson) &&
        existingVersion==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string version,
        IReadOnlyCollection<CurriculumLessonContentTranslation> existing)
    {
        var r=Find(doc,lesson) ?? throw new InvalidOperationException("Not source-locked B001 lesson");
        CheckIdentity(lesson,r);
        if(!CanUpgradeExisting(doc,lesson,version))
            throw new InvalidOperationException($"Unexpected persisted B001 version: {lesson.LessonCode}");
        if(existing.Count==0)return; // metadata only, learner content loaded from JSON
        if(existing.Count!=1)
            throw new InvalidOperationException("Unexpected stored translation count");
        var en=existing.Single();
        if(en.CultureCode!="en" || en.StepByStepSolutions!=r.PreviousSteps ||
            !en.WorkedExamples.StartsWith(r.PreviousExamplePrefix,StringComparison.Ordinal))
            throw new InvalidOperationException(
                $"Unrecognized persisted B001 source-goal content: {lesson.LessonCode}");
    }
}
