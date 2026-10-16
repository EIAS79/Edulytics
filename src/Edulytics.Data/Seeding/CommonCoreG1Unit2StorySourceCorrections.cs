using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// B001 pedagogical corrections grounded in publicly available IM 2021 teacher
/// preparation AND full lesson/student-facing activities for G1 U02 L01-L03.
/// Original publisher source packs are never modified. Academic approval is separate.
/// </summary>
public static class CommonCoreG1Unit2StorySourceCorrections
{
    public const string CorrectedContentVersion = "us-ccss-g1-u02-story-source-fidelity-v1";

    private sealed record Repair(
        string Code, string Url, string CanonicalSourceSha, string[] Outcomes,
        string PreviousSteps, string PreviousExampleMarker,
        string Explanation, string Concepts, string Examples, string Steps,
        string Mistakes, string Summary);

    private static readonly Repair[] Repairs =
    [
        new(
            "PED:US-CCSS-MATH:G1:U02:L01",
            "https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-1/preparation.html",
            "dbe90880614183b41bddfbf85fcea4cde71b4903f59ef3501b2f5029a8bc2fad",
            ["CCSS:1.OA.A.1"],
            "Step 1: Identify the starting quantity 9 and the amount 2 being removed or compared.\nStep 2: Use subtraction to find the difference.\nStep 3: 9 − 2 = 7.\nStep 4: Check that the answer fits the original situation.",
            "The source task compares or removes 2 from 9",
            "Today we will listen to a story, show what happens with counters or a picture, and write an expression that matches the action. When more join, add; when some leave, subtract. A number expression such as 4 + 3 shows the action without yet naming the answer. Tell what each number means in the story.",
            "Learning goal: Represent and solve Add To and Take From stories when the result is unknown. Write a matching expression.\nAn expression such as 4 + 3 or 8 - 2 shows an operation; an equation such as 4 + 3 = 7 also states its value. Today the important step is matching the expression to the story.\nUse objects, a drawing, fingers, or a 10-frame; explain what was added or taken away.",
            "Example 1 — More books arrive. Four picture books are on a shelf. Three more are added. Show four counters and put three beside them. The action is adding, so write the expression 4 + 3. Count seven books. The expression has value 7; in a later lesson we can write 4 + 3 = 7.\nExample 2 — Some children leave. Eight children are playing. Two go home. Show eight dots and cross out two. The matching expression is 8 - 2, and six children remain.\nExample 3 — Compare the actions. Five pencils are on a table; two are added. Expression: 5 + 2. In a new story, five pencils are on a table and two are removed. Expression: 5 - 2. The same numbers describe different actions.",
            "Step 1: Listen to the story: who or what is being counted?\nStep 2: Notice the action: are objects joining or leaving?\nStep 3: Show the starting group and the change with counters, dots, or a 10-frame.\nStep 4: Write an expression with + for joining or - for leaving. Name what each number counts.\nStep 5: Count or count on/back to answer the story. Check the expression describes the same action.",
            "Mistake: use 8 + 2 when two of eight objects leave. Correction: taking away makes 8 - 2, because the group becomes smaller.\nMistake: write an equation without understanding which part each number represents. Correction: point to the starting group, action, and result before writing.\nMistake: count eight remaining after crossing out two of eight dots. Correction: count only the six dots still visible.",
            "I can act out an adding or taking-away story and write a matching addition or subtraction expression. I can explain each number and check my answer with a picture."),
        new(
            "PED:US-CCSS-MATH:G1:U02:L02",
            "https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-2/preparation.html",
            "edf04a9002733f83cff534a3607047e888bb10d011d96bbae3b90cfd61149281",
            ["CCSS:1.OA.A.1", "CCSS:1.OA.C.6"],
            "Step 1: Identify the two quantities, 5 and 1.\nStep 2: Start at 5 and count on 1 more, or combine the two groups.\nStep 3: The total is 6, so 5 + 1 = 6.\nStep 4: Check that the result matches the quantities and the lesson goal.",
            "The source task combines 5 and 1",
            "Today we will use story problems to write complete equations. An expression, such as 6 + 3, tells us the operation. An equation, such as 6 + 3 = 9, shows the result too. Act out the story, choose + if more join or - if some leave, and explain what each number means. Put a box around the number that answers the question.",
            "Learning goal: Solve Add To and Take From, Result Unknown stories. Write an addition or subtraction equation and explain it.\nAn equation includes an equals sign: 6 + 3 = 9. The numbers on each side represent the same amount. The answer can be boxed, for example 6 + 3 = [9].\nUse counters, drawings, fingers, or a 10-frame to connect the equation to what happens in the story. If the story states all numbers without asking a question, write the equation without boxing an answer.",
            "Example 1 — More children join. Six children are reading. Three more arrive. Draw six dots and add three. There are nine now. The story equation is 6 + 3 = 9. We box 9 because it tells how many children there are now.\nExample 2 — Books are taken away. Nine books are on a cart. Four are removed. Show nine counters and move four away. Five remain, so write 9 - 4 = 5 and box 5.\nExample 3 — Tell what each number means. In 7 + 2 = 9, the 7 is the original group, the 2 is the new group, and the 9 is the total. If two of seven objects leave instead, the story needs 7 - 2 = 5.",
            "Step 1: Read or listen to the story and say what happens first and next.\nStep 2: Show the quantities with counters, drawings, or a 10-frame.\nStep 3: Choose + for more joining or - for some leaving.\nStep 4: Write a complete equation with =, then point to each number and explain its meaning.\nStep 5: Box the answer to the story question, count to check, and reread the story to check the operation.",
            "Mistake: writing 9 + 4 = 13 for a story where four objects are removed from nine. Correction: the group becomes smaller; write 9 - 4 = 5.\nMistake: confusing 6 + 3 (an expression) with 6 + 3 = 9 (an equation). Correction: an equation includes an equals sign and a value.\nMistake: boxing the first number instead of the result asked for. Correction: first identify the question and box the amount that answers it.",
            "I can show a joining or leaving story, write an equation using +, - and =, explain what each number represents, and box the result when the story asks for it."),
        new(
            "PED:US-CCSS-MATH:G1:U02:L03",
            "https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-3/preparation.html",
            "1c7332e9cd2f449d50c6c8f3348bd2ea682380ec8adf6518011534113a8f1820",
            ["CCSS:1.MD.C.4", "CCSS:1.OA.A.1", "CCSS:1.OA.B.4", "CCSS:1.OA.C.5", "CCSS:1.OA.C.6"],
            "Step 1: Identify the two quantities, 3 and 6.\nStep 2: Start at 3 and count on 6 more, or combine the two groups.\nStep 3: The total is 9, so 3 + 6 = 9.\nStep 4: Check that the result matches the quantities and the lesson goal.",
            "The source task combines 3 and 6",
            "Some stories tell how many we have at the start and at the end, but do not say how many were added. Today we find that missing change. Show the first group and count on until you reach the final group. The number you counted on is the answer, even though it may appear in the middle of an equation.",
            "Learning goal: Solve Add To, Change Unknown stories and locate the answer inside an equation.\nFor example, there are 6 books at first and 8 books after some more arrive. Represent 6 + ? = 8. Count on 7, 8: two books were added, so 6 + [2] = 8.\nThe answer depends on the story question. Do not automatically box the number after =. You can check a missing addend by counting on or using the related subtraction.",
            "Example 1 — Unknown books added. A student starts with 6 books and ends with 8. Draw six dots; add two dots until there are eight. 6 + [2] = 8. The answer is 2 new books, not 8 total books.\nExample 2 — Find the missing group. A child has 3 stickers and later has 9. Count on 4, 5, 6, 7, 8, 9: six more stickers. 3 + [6] = 9. Check that 3 + 6 = 9.\nExample 3 — Which number is the answer? In a story asking how many more blocks were added to 4 to make 7, 4 + [3] = 7. Seven is the final amount; three is the amount added. Explain the difference.",
            "Step 1: Identify the amount at the start and the total amount at the end.\nStep 2: Ask what is unknown: the number of new objects added.\nStep 3: Show the starting amount with counters or a drawing; add one at a time until you reach the final total.\nStep 4: Count only the objects added. Write start + [change] = total, boxing the missing change.\nStep 5: Check by adding the start and the change. Explain why the boxed number, not necessarily the final total, answers the story.",
            "Mistake: add the start and total, as in 6 + 8 = 14, when finding a missing increase. Correction: six is already included in eight; count on from six to eight.\nMistake: box 8 in 6 + 2 = 8 for a story asking how many books were given. Correction: box 2, because that is the change.\nMistake: count the starting six objects as six new objects. Correction: count only the two additional objects.",
            "I can find how many objects were added when I know the starting and final amounts. I can write 6 + [2] = 8, explain why 2 answers the question, and check with counters.")
    ];

    public static IReadOnlyCollection<string> TargetLessonCodes =>
        Repairs.Select(r => r.Code).ToArray();

    private static Repair? Find(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson) =>
        doc.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode &&
        doc.VersionCode == "CCSSM-2010" &&
        doc.ContentVersion == "phase29-source-intelligence-en-v2-g1"
            ? Repairs.SingleOrDefault(r => r.Code == lesson.LessonCode)
            : null;

    public static bool IsTarget(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson) => Find(doc, lesson) is not null;

    private static void CheckSourceIdentity(CanonicalLessonContentPackLesson lesson, Repair r)
    {
        if (lesson.SourceUrl != r.Url ||
            !string.Equals(lesson.CanonicalBodySha256, r.CanonicalSourceSha,
                StringComparison.OrdinalIgnoreCase) ||
            !lesson.OutcomeCodes.SequenceEqual(r.Outcomes))
            throw new InvalidOperationException(
                $"B001 G1 U02 original source/standards drift: {lesson.LessonCode}");
    }

    private static bool Matches(CanonicalLessonContentPackTranslation t, Repair r) =>
        t.Explanation == r.Explanation && t.KeyConceptsAndRules == r.Concepts &&
        t.WorkedExamples == r.Examples && t.StepByStepSolutions == r.Steps &&
        t.CommonMistakes == r.Mistakes && t.QuickSummary == r.Summary;

    public static void Apply(CanonicalLessonContentPackDocument document)
    {
        if (document.PackCode != MathematicsCurriculumPackRegistry.CommonCoreCode ||
            document.VersionCode != "CCSSM-2010") return;
        foreach (var lesson in document.Lessons)
        {
            var r = Find(document, lesson);
            if (r is null) continue;
            CheckSourceIdentity(lesson, r);
            var en = lesson.Translations.Single(t => t.CultureCode == "en");
            if (Matches(en, r)) continue;
            if (en.StepByStepSolutions != r.PreviousSteps ||
                !en.WorkedExamples.Contains(r.PreviousExampleMarker, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    $"Refusing B001 G1 U02 correction on unrecognized content: {lesson.LessonCode}");
            en.Explanation = r.Explanation;
            en.KeyConceptsAndRules = r.Concepts;
            en.WorkedExamples = r.Examples;
            en.StepByStepSolutions = r.Steps;
            en.CommonMistakes = r.Mistakes;
            en.QuickSummary = r.Summary;
        }
    }

    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson, string previous) =>
        IsTarget(document, lesson) ? CorrectedContentVersion : previous;

    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson, string existingVersion) =>
        IsTarget(document, lesson) &&
        existingVersion == CommonCoreRichContentCorrections.CorrectionContentVersion;

    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson, string existingVersion,
        IReadOnlyCollection<CurriculumLessonContentTranslation> existingTranslations)
    {
        var repair = Find(document, lesson)
            ?? throw new InvalidOperationException("Not a B001 G1 U02 correction target");
        CheckSourceIdentity(lesson, repair);
        if (!CanUpgradeExisting(document, lesson, existingVersion))
            throw new InvalidOperationException(
                $"Refusing unexpected B001 G1 U02 persisted version: {lesson.LessonCode}");
        if (existingTranslations.Count == 0) return; // JSON-only deployment mode
        if (existingTranslations.Count != 1)
            throw new InvalidOperationException("Unexpected persisted G1 U02 cultures");
        var t = existingTranslations.Single();
        if (t.CultureCode != "en" || t.StepByStepSolutions != repair.PreviousSteps ||
            !t.WorkedExamples.Contains(repair.PreviousExampleMarker, StringComparison.Ordinal))
            throw new InvalidOperationException(
                $"Unrecognized persisted G1 U02 teaching: {lesson.LessonCode}");
    }
}
