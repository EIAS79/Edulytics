using Edulytics.Services.Mathematics;

namespace Edulytics.Services.Practice;

/// <summary>
/// Lesson-scoped Arabic presentation for verified two-set inclusion/exclusion.
/// This changes neither the mathematical family nor the correct answer, grader,
/// question fingerprint or generation parameters for other curricula.
/// Three-set Venn reasoning remains teacher-reviewed.
/// </summary>
internal static class UaeGrade8SetPracticeLocalization
{
    private const string TargetLesson =
        "PED:UAE-MOE-MATH:L8:GENERAL:12:04:USING-SETS-TO-SOLVE-PROBLEMS";
    private const string TargetFamily = "supporting.sets.union_count";

    public static string Prompt(string lessonCode, ExactSkillGeneratedQuestion question)
    {
        if (!string.Equals(lessonCode, TargetLesson, StringComparison.Ordinal))
            return question.Prompt;

        var (a, b, intersection) = CheckedParameters(question);
        return $"في مسألة عن مجموعتين، تحتوي المجموعة أ على {a} عنصرًا، " +
               $"وتحتوي المجموعة ب على {b} عنصرًا، ويوجد {intersection} عنصرًا " +
               "ينتمي إلى المجموعتين معًا. كم عدد عناصر اتحاد المجموعتين أ∪ب؟";
    }

    public static string Solution(string lessonCode, ExactSkillGeneratedQuestion question)
    {
        if (!string.Equals(lessonCode, TargetLesson, StringComparison.Ordinal))
            return question.Solution;

        var (a, b, intersection) = CheckedParameters(question);
        return $"نطبق مبدأ الشمول والاستبعاد: عدد عناصر الاتحاد = " +
               $"{a} + {b} − {intersection} = {a + b - intersection}. " +
               "نطرح عناصر التقاطع حتى لا نعدّها مرتين.";
    }

    private static (int A, int B, int Intersection) CheckedParameters(
        ExactSkillGeneratedQuestion question)
    {
        if (!string.Equals(question.Family, TargetFamily, StringComparison.Ordinal) ||
            !question.Parameters.TryGetValue("a", out var a) ||
            !question.Parameters.TryGetValue("b", out var b) ||
            !question.Parameters.TryGetValue("intersection", out var intersection) ||
            a < 1 || b < 1 || intersection < 1 || intersection > Math.Min(a, b))
        {
            throw new InvalidOperationException(
                "UAE G8 U12L04 Arabic Practice requires valid two-set union parameters.");
        }

        return (a, b, intersection);
    }
}
