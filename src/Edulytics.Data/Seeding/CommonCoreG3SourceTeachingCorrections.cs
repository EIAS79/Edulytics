using System.Text.Json;
using Edulytics.Core.Curriculum;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Authoring-stage 2021 source-skill corrections for exactly 143 Grade 3
/// lessons. These corrections do not certify the original figure geometry,
/// teacher-led activities, subquestion-by-subquestion parity, tests, or
/// deployment. The signed-off source comparison remains a separate gate.
/// </summary>
public static class CommonCoreG3SourceTeachingCorrections
{
    public const string PackCode = "US-CCSS-MATH";
    public const string BaseContentVersion = "phase30-common-core-rich-content-v1";
    public const string CorrectionContentVersion =
        "g3-2021-source-skill-rich-v2-authoring-v1";
    private const string Prefix = "PED:US-CCSS-MATH:G3:";
    private const string Resource = "Edulytics.Data.Grade3.SourceTeachingCorrections2021";
    private static readonly Lazy<IReadOnlyDictionary<string, Correction>> Records =
        new(Load, LazyThreadSafetyMode.ExecutionAndPublication);

    public static int CorrectionCount => Records.Value.Count;

    public static bool IsTarget(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        string.Equals(document.PackCode, PackCode, StringComparison.Ordinal) &&
        string.Equals(document.ContentVersion, BaseContentVersion, StringComparison.Ordinal) &&
        lesson.LessonCode.StartsWith(Prefix, StringComparison.Ordinal) &&
        Records.Value.TryGetValue(lesson.LessonCode, out var rule) &&
        string.Equals(lesson.CanonicalBodySha256, rule.BaseBodyHash, StringComparison.Ordinal) &&
        string.Equals(lesson.SourceEdition, "1st Edition (2021)", StringComparison.Ordinal);

    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string previous) =>
        IsTarget(document, lesson) ? CorrectionContentVersion : previous;

    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingContentVersion) =>
        IsTarget(document, lesson) &&
        string.Equals(existingContentVersion, BaseContentVersion, StringComparison.Ordinal);

    public static void Apply(CanonicalLessonContentPackDocument document)
    {
        if (!string.Equals(document.PackCode, PackCode, StringComparison.Ordinal) ||
            !string.Equals(document.ContentVersion, BaseContentVersion, StringComparison.Ordinal))
            return;

        var grade3 = document.Lessons.Where(
            lesson => lesson.LessonCode.StartsWith(Prefix, StringComparison.Ordinal)).ToArray();
        // Common Core grade packs share one programme/version identifier.
        // Documents without Grade 3 must be left completely untouched.
        if (grade3.Length == 0) return;
        if (grade3.Length != 143)
            throw new InvalidOperationException(
                $"Expected the full Grade 3 2021 set of 143 lessons, got {grade3.Length}.");

        foreach (var lesson in grade3)
        {
            if (!IsTarget(document, lesson))
                throw new InvalidOperationException(
                    $"Grade 3 source hash, edition or lesson identity changed: {lesson.LessonCode}. " +
                    "Re-review the source before authoring any replacement body.");

            var rule = Records.Value[lesson.LessonCode];
            var en = lesson.Translations.SingleOrDefault(
                t => string.Equals(t.CultureCode, "en", StringComparison.Ordinal))
                ?? throw new InvalidOperationException(
                    $"Missing canonical English Grade 3 translation: {lesson.LessonCode}.");

            en.KeyConceptsAndRules =
                "Mathematical learning focus: " + rule.LearningGoal +
                "\nReview the original 2021 learning goals and class activity context before " +
                "drawing conclusions from a graph, diagram, equation or measurement." +
                "\nActivity families requiring teacher evidence: " + rule.ActivityNames;

            en.StepByStepSolutions = string.Join("\n",
                rule.TeachingSteps.Select((step, i) => $"Step {i + 1}: {step}"));

            en.CommonMistakes =
                "Check this lesson-specific risk: " + rule.MisconceptionCaution +
                "\nA correct numeric value never certifies a source diagram, physical " +
                "manipulative, peer task or mathematical explanation by itself.";

            en.QuickSummary =
                "Learning goal: " + rule.LearningGoal +
                "\nApproach: " + rule.TeachingSteps[0] +
                " Show the mathematical representation and explain the meaning.";

            if (string.Equals(lesson.LessonCode,
                "PED:US-CCSS-MATH:G3:U01:L01", StringComparison.Ordinal))
            {
                // U01 L01 is the first sequential A-B-C lesson. The baseline
                // generic examples do not teach the 2021 reveal sequence.
                // Authored from the public lesson/preparation and independent
                // Edulytics student companion; NOT an original-art reproduction.
                en.Explanation =
                    "We are learning to read picture and bar graphs and to ask " +
                    "questions that the data can answer. A picture-graph key tells " +
                    "how many people each symbol represents; a bar graph uses a " +
                    "numbered axis. First, study six upright picture columns " +
                    "without a title or category names. The numbers of symbols " +
                    "from left to right are 4, 3, 1, 2, 7 and 6, with one symbol " +
                    "representing one learner. We cannot know what the columns " +
                    "represent from their heights alone. Next, reveal the title " +
                    "'How We Get Home'; it narrows the possible context but still " +
                    "does not identify the six categories. Finally, read the " +
                    "fully labelled picture graph together with a bar graph " +
                    "showing the SAME data. Walk 4, Bike 3, Skateboard 1, Car 2, " +
                    "Bus 7, Train 6; the bar axis runs from 0 to 8 in steps of 1. " +
                    "Compare the displays and propose two different questions " +
                    "that can actually be answered. Discuss the reasons with a " +
                    "partner. The discussion and student-authored questions need " +
                    "teacher review and are not auto-graded.";

                en.WorkedExamples =
                    "Example 1 - Notice and wonder: a six-column picture graph " +
                    "has 4, 3, 1, 2, 7, 6 symbols, but no title or labels. " +
                    "Describe the visible counts without guessing a proven " +
                    "category name.\n" +
                    "Example 2 - Picture time: add only the title 'How We Get " +
                    "Home'. Suggest possible ways of getting home, explaining " +
                    "why they are hypotheses and not labelled facts.\n" +
                    "Example 3 - Compare representations: the categories are " +
                    "Walk 4, Bike 3, Skateboard 1, Car 2, Bus 7, Train 6. " +
                    "On a picture graph one symbol means one learner; on the " +
                    "bar graph use a vertical 0-to-8 axis in steps of 1. " +
                    "Explain what is the same and different, then write two " +
                    "answerable questions. A teacher checks the reasoning.";

                en.KeyConceptsAndRules =
                    "Lesson 3.1.1 learning target: read a categorical " +
                    "picture graph and a bar graph, and ask questions that " +
                    "the graphs can answer. The picture graph's key says " +
                    "one symbol means one student; the bar graph uses a " +
                    "vertical number axis from 0 through 8 in ones. " +
                    "Do not give anonymous columns invented category " +
                    "labels as though they are known. A title provides " +
                    "context, not all missing labels. Once categories " +
                    "are revealed, both graphs must preserve Walk 4, " +
                    "Bike 3, Skateboard 1, Car 2, Bus 7 and Train 6 " +
                    "(23 students in all). Discuss two learner-authored " +
                    "questions, and verify that the displayed data " +
                    "actually answer them. Teacher-led explanation, " +
                    "Math Community norms and graph comparison are " +
                    "not digitally auto-graded.";

                en.StepByStepSolutions =
                    "Step 1 (15-minute Notice and Wonder): look at " +
                    "the 4, 3, 1, 2, 7, 6 anonymous picture columns. " +
                    "Record an observation and a question without " +
                    "guessing a fixed category label. Think quietly, " +
                    "then discuss with a partner.\n" +
                    "Step 2 (10-minute Picture Time): show the " +
                    "title How We Get Home, retain the six values " +
                    "and one-symbol key, and suggest possible " +
                    "categories with reasons. Explain why the " +
                    "labels cannot be determined from the title.\n" +
                    "Step 3 (20-minute paired graph analysis): " +
                    "reveal Walk 4, Bike 3, Skateboard 1, Car 2, " +
                    "Bus 7, Train 6 in the picture graph and in " +
                    "a bar graph with y-axis 0..8 in ones. Check " +
                    "every bar/column against its category and key.\n" +
                    "Step 4: describe how the picture and bar " +
                    "graphs are alike and different, then say " +
                    "what the graphs tell us. Write two distinct " +
                    "questions whose answers can be read from " +
                    "these graphs. Teacher reviews explanations.\n" +
                    "Step 5 (10-minute synthesis): explain the " +
                    "role of labels, title, picture key, and bar " +
                    "scale. Invite learners to restate a peer's " +
                    "reasoning and discuss Doing Math / Norms.\n" +
                    "Step 6 (5-minute independent Edulytics " +
                    "Cool-down): read a NEW graph and respond. " +
                    "Numeric responses can be checked; invented " +
                    "questions and explanations require review. " +
                    "This is not the restricted publisher item.";

                en.CommonMistakes =
                    "Mistake: interpreting the unnamed first " +
                    "and second graphs as if their category " +
                    "names were supplied. Repair: distinguish " +
                    "a plausible guess from a shown label.\n" +
                    "Mistake: treating one picture as more " +
                    "than one learner or misreading the bar " +
                    "scale. Repair: verify one symbol equals " +
                    "one student and the axis advances by " +
                    "exactly one, starting at zero.\n" +
                    "Mistake: confusing columns or comparing " +
                    "different categories. Repair: match Walk " +
                    "4, Bike 3, Skateboard 1, Car 2, Bus 7 " +
                    "and Train 6 in BOTH representations.\n" +
                    "Mistake: offering a question that the " +
                    "graph cannot answer or treating partner " +
                    "reasoning as a numeric grade. Repair: " +
                    "require source-visible evidence and a " +
                    "teacher rubric; open questions are not " +
                    "automatic passes.";

                en.QuickSummary =
                    "The same six category counts can be shown as a picture " +
                    "graph or a vertical bar graph. First examine the six " +
                    "unlabelled columns, then the titled but unlabelled graph, " +
                    "and only then the fully labelled pair. A title provides " +
                    "context, a picture key defines one symbol, and the bar " +
                    "axis shows the numerical scale. Good graph questions " +
                    "must be answerable from the shown data; oral reasoning, " +
                    "partner work and invented questions require teacher review.";
            }
            else if (string.Equals(lesson.LessonCode,
                "PED:US-CCSS-MATH:G3:U01:L02", StringComparison.Ordinal))
            {
                // Preserve real classroom survey provenance: NO fixed L01
                // travel graph or invented class-wide answers. First-party
                // wording follows 2021 public teacher lesson/preparation.
                en.Explanation =
                    "Our goal is to represent real class-collected categorical " +
                    "data using a picture graph and a bar graph, then solve " +
                    "one- and two-step more/fewer questions within 20. Start " +
                    "with 3 brief dot images (6, 8 and 10 dots) and compare " +
                    "the second and third arrangements: 8 + 2 = 10. Next, " +
                    "every learner places one response in a teacher-prepared " +
                    "class survey about how they travel home. The teacher " +
                    "may change the category labels to fit the pupils. " +
                    "Give the picture graph a title and key, then draw a " +
                    "vertical bar graph of the SAME collected dataset, " +
                    "with a labelled number axis beginning at zero. " +
                    "Use the recorded category values, not fixed answers " +
                    "from another class. Evaluate four true/false statements " +
                    "against the actual graph; the teacher chooses labels " +
                    "for one-step and two-step comparison questions. " +
                    "Students must explain equations and conclusions to " +
                    "partners; teacher reviews the collection, drawing, " +
                    "justification and Math Community participation.";

                en.KeyConceptsAndRules =
                    "A picture graph and a bar graph must show the SAME " +
                    "survey counts. Each pupil contributes exactly one " +
                    "real response; one picture stands for one learner. " +
                    "The bar height equals the category count on a " +
                    "common number axis. First learn the numbers from " +
                    "the real class; only then ask 'how many more/fewer'. " +
                    "For an arithmetic task that combines two categories, " +
                    "add those two counts and compare with a third. " +
                    "Use addition/subtraction within 20 for automatically " +
                    "graded arithmetic; a larger class size does not " +
                    "prove any particular numerical solution. " +
                    "Categories and one/two-step question choices are " +
                    "teacher-set and need not have fictional source values.";

                en.WorkedExamples =
                    "Warm-up grouping: 8 dots become 10 with 2 more, " +
                    "so 8 + 2 = 10. Describe a grouping strategy rather " +
                    "than simply naming the result.\n" +
                    "Graph construction: place one marked note for " +
                    "each real pupil in one teacher-selected travel " +
                    "category; give the picture graph a title and " +
                    "one-response key. Count each category; transfer " +
                    "those SAME counts to a bar graph with a consistent " +
                    "scale. The values depend on the actual class.\n" +
                    "One-step comparison example: if the class counts " +
                    "for two teacher-selected categories are a and b, " +
                    "compute the difference between them and explain " +
                    "which category is greater. Only generate an " +
                    "auto-gradable item when the actual within-20 " +
                    "arithmetic scope has been checked.\n" +
                    "Two-step comparison example: if two distinct " +
                    "teacher-selected categories contain a and b " +
                    "learners and a third contains c, compare (a+b) " +
                    "with c. Use the classroom counts; do not supply " +
                    "made-up values as if they were collected.";

                en.StepByStepSolutions =
                    "Step 1 (10-minute dot warm-up): notice dot " +
                    "groups and explain 8+2=10 orally with a peer.\n" +
                    "Step 2 (15-minute class-survey activity): " +
                    "collect actual sticky-note responses, choose " +
                    "contextually accurate labels, then construct " +
                    "a titled picture graph with a one-pupil key.\n" +
                    "Step 3: transfer exactly the same observed " +
                    "class values to a bar graph with a numbered " +
                    "vertical axis and category labels. Have " +
                    "students justify equivalent representations.\n" +
                    "Step 4 (20-minute graph questions): decide " +
                    "the four source-style true/false comparison " +
                    "claims using the real categories, then let the " +
                    "teacher fill the category blanks for a one-step " +
                    "and a two-step how-many-more question.\n" +
                    "Step 5 (10-minute synthesis): share a true " +
                    "equation, explain bar-versus-picture scale " +
                    "and revise the Math Community norms.\n" +
                    "Step 6 (5-minute independent Edulytics " +
                    "Cool-down): solve an original separate " +
                    "graph-reading problem. Hand-drawn graphs " +
                    "and explanations require teacher review.";

                en.CommonMistakes =
                    "Mistake: using the fixed data from Lesson 1 " +
                    "or another fictional survey in place of " +
                    "actual class responses. Repair: collect " +
                    "and verify the present class counts.\n" +
                    "Mistake: using different numbers in the " +
                    "picture graph and bar graph, or omitting " +
                    "the title, key, or axis. Repair: verify " +
                    "each count before comparing.\n" +
                    "Mistake: grading a class-dependent true/false " +
                    "statement against hard-coded correct/incorrect " +
                    "answers. Repair: calculate each statement " +
                    "from a validated supplied dataset.\n" +
                    "Mistake: grading oral reasoning, graph " +
                    "drawing or invented questions as a binary " +
                    "numeric pass. Repair: use teacher review; " +
                    "reject out-of-scope numeric items rather " +
                    "than guessing an answer.";

                en.QuickSummary =
                    "Read the real data from your class, draw " +
                    "matching picture and bar graphs with a title, " +
                    "one-pupil key and common axis, and use those " +
                    "same data to solve one- and two-step " +
                    "more/fewer problems within 20. Discuss " +
                    "your equations and improve the community " +
                    "discussion norms. The independently authored " +
                    "Edulytics Cool-down is not the publisher item.";
            }
            // Never embed restricted publisher Cool-down prompts or artwork.
            // No database migration, publication or A-C approval occurs here.
        }
    }

    private static IReadOnlyDictionary<string, Correction> Load()
    {
        using var stream = typeof(CommonCoreG3SourceTeachingCorrections)
            .Assembly.GetManifestResourceStream(Resource)
            ?? throw new InvalidOperationException("Embedded Grade 3 source teaching resource not found.");
        using var json = JsonDocument.Parse(stream);
        var root = json.RootElement;
        if (root.GetProperty("targetLessonCount").GetInt32() != 143 ||
            root.GetProperty("baseContentVersion").GetString() != BaseContentVersion ||
            root.GetProperty("proposedEffectiveContentVersion").GetString() != CorrectionContentVersion)
            throw new InvalidOperationException("Unexpected Grade 3 authoring resource baseline or version.");

        var map = new Dictionary<string, Correction>(StringComparer.Ordinal);
        foreach (var item in root.GetProperty("lessons").EnumerateArray())
        {
            var code = item.GetProperty("lessonCode").GetString() ?? "";
            var hash = item.GetProperty("baselineCanonicalBodySha256").GetString() ?? "";
            var goal = item.GetProperty("sourceGoal").GetString()?.Trim() ?? "";
            var activities = item.GetProperty("originalActivityNames").GetString()?.Trim() ?? "";
            var caution = item.GetProperty("studentMisconceptionCaution").GetString()?.Trim() ?? "";
            var steps = item.GetProperty("teachingSteps").EnumerateArray()
                .Select(s => s.GetString()?.Trim() ?? "").ToArray();
            if (!code.StartsWith(Prefix, StringComparison.Ordinal) || hash.Length != 64 ||
                goal.Length < 15 || caution.Length < 15 || steps.Length < 2 ||
                goal.Contains("Required Preparation", StringComparison.OrdinalIgnoreCase) ||
                goal.Contains("Lesson Timeline", StringComparison.OrdinalIgnoreCase) ||
                goal.Contains("Teacher Reflection", StringComparison.OrdinalIgnoreCase) ||
                steps.Any(string.IsNullOrWhiteSpace) || !map.TryAdd(code,
                    new Correction(hash, goal, activities, caution, steps)))
                throw new InvalidOperationException("Invalid or duplicate Grade 3 teaching rule: " + code);
        }

        if (map.Count != 143)
            throw new InvalidOperationException($"Expected 143 distinct Grade 3 rules; got {map.Count}.");
        return map;
    }

    private sealed record Correction(
        string BaseBodyHash,
        string LearningGoal,
        string ActivityNames,
        string MisconceptionCaution,
        IReadOnlyList<string> TeachingSteps);
}
