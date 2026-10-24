using System.Collections.ObjectModel;

namespace Edulytics.Core.Mathematics.Curriculum;

/// <summary>
/// Independently-authored, source-anchored Grade 3 U01 L01 graph data candidate.
/// Based on the mathematical values of IM K-5 First Edition (2021), Grade 3
/// Unit 1 Lesson 1; original graphs/artwork are NOT embedded or reproduced.
/// This is NOT yet routed into production Practice. Original oral, picture
/// interpretation, partner, question-writing and teacher community tasks
/// require their own teacher-led evidence, not a surrogate numeric pass.
/// Source: https://im.kendallhunt.com/k5/teachers/grade-3/unit-1/lesson-1/lesson.html
/// </summary>
public static class UsCcssG3Unit1Lesson1GraphCandidate
{
    public const string LessonCode = "PED:US-CCSS-MATH:G3:U01:L01";
    public const int PictographStudentsPerSymbol = 1;
    public const int BarYAxisMinimum = 0;
    public const int BarYAxisMaximum = 8;
    public const int BarYAxisStep = 1;
    // The original 2021 Lesson page describes this horizontal-axis caption.
    public const string BarXAxisTitle = "Ways to get home";

    public sealed record TravelCategory(string Label, int NumberOfStudents);

    private static readonly ReadOnlyCollection<TravelCategory> SourceValues =
        Array.AsReadOnly(new[]
        {
            new TravelCategory("Walk", 4),
            new TravelCategory("Bike", 3),
            new TravelCategory("Skateboard", 1),
            new TravelCategory("Car", 2),
            new TravelCategory("Bus", 7),
            new TravelCategory("Train", 6)
        });

    public static IReadOnlyList<TravelCategory> Values => SourceValues;

    public static int TotalStudents => Values.Sum(x => x.NumberOfStudents);

    public static bool TryGetCategoryCount(string? category, out int count)
    {
        count = 0;
        if (string.IsNullOrWhiteSpace(category))
            return false;

        foreach (var row in Values)
        {
            if (!string.Equals(row.Label, category.Trim(),
                    StringComparison.OrdinalIgnoreCase))
                continue;
            count = row.NumberOfStudents;
            return true;
        }

        return false;
    }

    // Deliberately strict whitelist; no free-form learner answer may be
    // certified by these numeric checks.
    public static bool IsAutomaticallyGradeable(string? taskKind) =>
        taskKind is "named-category-count" or "named-category-difference"
            or "all-category-total";

    /// <summary>Returns false when mode/category is ineligible for numeric grading.</summary>
    public static bool TryGradeNumeric(
        string? taskKind,
        string? firstCategory,
        string? secondCategory,
        int learnerAnswer,
        out bool correct)
    {
        correct = false;
        if (!IsAutomaticallyGradeable(taskKind))
            return false;

        if (taskKind == "all-category-total")
        {
            if (firstCategory is not null || secondCategory is not null)
                return false;
            correct = learnerAnswer == TotalStudents;
            return true;
        }

        if (!TryGetCategoryCount(firstCategory, out var first))
            return false;

        if (taskKind == "named-category-count")
        {
            if (secondCategory is not null)
                return false;
            correct = learnerAnswer == first;
            return true;
        }

        if (!TryGetCategoryCount(secondCategory, out var second))
            return false;

        correct = learnerAnswer == Math.Abs(first - second);
        return true;
    }

    public static bool RequiresTeacherJudgment(string? sourceTaskKind) =>
        sourceTaskKind is "notice-and-wonder"
            or "infer-unlabeled-graph-categories"
            or "explain-graph-similarities"
            or "explain-graph-learnings"
            or "author-two-graph-questions"
            or "partner-discussion"
            or "math-community-norms"
            or "oral-synthesis"
            or "original-cool-down";
}
