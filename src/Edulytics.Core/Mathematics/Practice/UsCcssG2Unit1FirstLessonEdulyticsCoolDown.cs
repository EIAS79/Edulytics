using System.Globalization;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Edulytics independently authored exit ticket for IM 2021 Grade 2 U01 L01.
/// These prompts and examples are NOT copies of, or claims to reproduce,
/// an educator-login-restricted publisher Cool-down.
/// </summary>
public static class UsCcssG2Unit1FirstLessonEdulyticsCoolDown
{
    public const string LessonCode = "PED:US-CCSS-MATH:G2:U01:L01";
    public const string Edition = "edulytics-original-cooldown-v1";
    public const int EstimatedMinutes = 5;

    public sealed record Task(
        string Id, string Prompt, string AnswerMode, string Representation,
        string Evaluation, string Feedback);

    public sealed record ResponseResult(
        bool ScorableByMachine, bool Correct, bool TeacherReviewRequired, string Reason);

    public static readonly IReadOnlyList<Task> Tasks =
    [
        new("CD-1",
            "Four yellow counters and five green counters are together on a desk. How many counters are there altogether?",
            "exact_integer", "original-colored-counters-4-and-5",
            "9", "Add 4 + 5 to count the full collection."),
        new("CD-2",
            "A ten-frame holds ten counters. A child removes six. How many counters are still on the frame?",
            "exact_integer", "original-ten-frame-10-then-remove-6",
            "4", "Use 10 − 6 = 4 and check 4 + 6 = 10."),
        new("CD-3",
            "Write TWO DIFFERENT expressions with + or − that both have a value of six. Use numbers from 0 to 10.",
            "two_student_authored_expressions", "symbolic-expression-record",
            "target=6;two-distinct-valid-expressions",
            "Examples of different correct responses include 4 + 2 and 10 − 4; other valid expressions are accepted."),
        new("CD-4",
            "Make ten in a new way. Draw or arrange counters for 8 + 2 and explain in words or to your teacher how you know the sum is ten.",
            "teacher_scored_explanation_and_visual",
            "original-empty-ten-frame",
            "teacher_rubric",
            "A complete explanation accounts for eight counters and two counters with exactly ten in the whole frame.")
    ];

    public static ResponseResult AssessInteger(string id, string? response)
    {
        var task = Tasks.SingleOrDefault(t => t.Id == id);
        if (task is null || task.AnswerMode != "exact_integer")
            return new(false, false, true, "Not a numeric-only Edulytics task");
        if (!int.TryParse(response, NumberStyles.None,
            CultureInfo.InvariantCulture, out var supplied))
            return new(true, false, false, "Enter one non-negative integer");
        var expected = int.Parse(task.Evaluation, CultureInfo.InvariantCulture);
        return new(true, supplied == expected, false,
            supplied == expected ? task.Feedback : "Recount or use the inverse fact");
    }

    public static ResponseResult AssessTwoExpressions(
        string taskId, IReadOnlyList<string>? expressions)
    {
        if (taskId != "CD-3")
            return new(false, false, true, "Not the authored-expression Cool-down task");
        if (expressions is null || expressions.Count != 2)
            return new(true, false, false, "Record exactly two expressions");

        var reviewed = expressions
            .Select(x => UsCcssG2Unit1FirstLessonExpressionRecord.Check(6, x))
            .ToArray();
        var valid = reviewed.All(x => x.ExpressionValid && x.TargetMatched);
        var distinct = reviewed.Select(x => x.NormalizedExpression)
            .Distinct(StringComparer.Ordinal).Count() == 2;
        return new(true, valid && distinct, true,
            valid && distinct
                ? "Mathematical expressions verified; teacher may review the student's reasoning"
                : "Both expressions must be mathematically valid, different and equal to six");
    }

    /// <summary>
    /// Original teacher-rubric support. A response isn't automatically graded
    /// on its language or drawing; teacher records conceptual evidence.
    /// </summary>
    public static IReadOnlyList<string> ExplanationRubric =
    [
        "The learner represents 8 counters plus 2 counters in one total of 10.",
        "The learner describes how the two parts form the entire ten, not simply repeating '10'.",
        "The drawing/ten-frame or physical counter representation agrees with the student's words.",
        "Different mathematically valid strategies and accessible descriptions are accepted."
    ];

    // First-party instructional description, free of publisher image assets.
    public const string OriginalTenFrameAccessibleDescription =
        "Ten equal squares arranged as two rows of five. Place eight counters, then two counters, to fill all ten squares.";
}
