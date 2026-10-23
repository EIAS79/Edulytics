namespace Edulytics.Core.Mathematics.Curriculum;

/// <summary>
/// Independently authored G3 U01 L02 numeric graph-comparison candidate.
/// Original first-edition lesson uses ACTUAL student-collected class survey
/// counts. Fixed U01 L01 travel data must never be substituted for this task.
/// NOT wired into production Practice and NOT a teacher-authenticated record.
/// https://im.kendallhunt.com/k5/teachers/grade-3/unit-1/lesson-2/lesson.html
/// </summary>
public static class UsCcssG3Unit1Lesson2ClassSurveyCandidate
{
    public const string LessonCode = "PED:US-CCSS-MATH:G3:U01:L02";

    public sealed record ClassroomCount(string Category, int Students);

    /// <summary>
    /// Verifies that supplied numeric data are internally valid for an
    /// independently authored Practice item. This does NOT verify the original
    /// classroom collection; teachers must separately attest real data.
    /// </summary>
    public static bool TryGetValidatedCounts(
        IReadOnlyList<ClassroomCount>? classroom,
        out IReadOnlyDictionary<string, int>? counts)
    {
        counts = null;
        if (classroom is null || classroom.Count < 2)
            return false;

        var map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var entry in classroom)
        {
            if (entry is null ||
                string.IsNullOrWhiteSpace(entry.Category) ||
                entry.Category != entry.Category.Trim() ||
                entry.Students < 0 ||
                entry.Students > 20 ||
                !map.TryAdd(entry.Category, entry.Students))
                return false;
        }

        if (map.Values.Sum() == 0)
            return false;

        counts = map;
        return true;
    }

    /// <summary>
    /// Numeric how-many-more difference is meaningful only when survey counts
    /// are actually supplied; equal counts correctly yield a difference of 0.
    /// </summary>
    public static bool TryGradeDifference(
        IReadOnlyList<ClassroomCount>? classroom,
        string? firstCategory,
        string? secondCategory,
        int answer,
        out bool correct)
    {
        correct = false;
        if (!TryGetValidatedCounts(classroom, out var counts) ||
            counts is null ||
            string.IsNullOrWhiteSpace(firstCategory) ||
            string.IsNullOrWhiteSpace(secondCategory) ||
            !counts.TryGetValue(firstCategory, out var first) ||
            !counts.TryGetValue(secondCategory, out var second) ||
            string.Equals(firstCategory, secondCategory, StringComparison.OrdinalIgnoreCase))
            return false;

        correct = Math.Abs(first - second) == answer;
        return true;
    }

    /// <summary>Original how-many-more A-or-B-than-C task, within 20.</summary>
    public static bool TryGradeTwoStep(
        IReadOnlyList<ClassroomCount>? classroom,
        string? firstCategory,
        string? secondCategory,
        string? comparisonCategory,
        int answer,
        out bool correct)
    {
        correct = false;
        if (!TryGetValidatedCounts(classroom, out var counts) ||
            counts is null ||
            string.IsNullOrWhiteSpace(firstCategory) ||
            string.IsNullOrWhiteSpace(secondCategory) ||
            string.IsNullOrWhiteSpace(comparisonCategory) ||
            !counts.TryGetValue(firstCategory, out var first) ||
            !counts.TryGetValue(secondCategory, out var second) ||
            !counts.TryGetValue(comparisonCategory, out var compare) ||
            string.Equals(firstCategory, secondCategory, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(firstCategory, comparisonCategory, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(secondCategory, comparisonCategory, StringComparison.OrdinalIgnoreCase))
            return false;

        var combined = first + second;
        if (combined > 20)
            return false; // This candidate covers only CCSS within-20 prompts.
        correct = Math.Abs(combined - compare) == answer;
        return true;
    }

    public static bool RequiresTeacherJudgment(string? taskKind) =>
        taskKind is "collect-real-sticky-note-survey"
            or "construct-class-picture-graph"
            or "construct-class-bar-graph"
            or "explain-true-false-statement"
            or "justify-with-partner"
            or "revise-math-community-norms"
            or "cool-down-open-response";
}
