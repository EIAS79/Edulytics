using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Web.ViewModels.LessonContent;

/// <summary>
/// First-party, student-facing authored companion task and exit ticket for
/// US-CCSS-MATH Grade 2 Unit 1 Lesson 1, separate from publisher documents.
/// No claim that the response is persisted as official student assessment.
/// </summary>
public sealed record Grade2Lesson1PracticePage(
    Guid CurriculumAdoptionId,
    Guid LessonId,
    string LessonTitle,
    IReadOnlyList<UsCcssG2Unit1FirstLessonEdulyticsCoolDown.Task> CoolDownTasks,
    IReadOnlyList<string> TeacherRubric,
    bool Submitted,
    string? AnswerOne,
    string? AnswerTwo,
    string? ExpressionOne,
    string? ExpressionTwo,
    string? Explanation,
    string? CheckOffTarget,
    string? CheckOffExpressionOne,
    string? CheckOffExpressionTwo,
    IReadOnlyList<Grade2Lesson1Result> Results);

public sealed record Grade2Lesson1Result(
    string Label, bool? MachineCorrect, bool TeacherReviewNeeded, string Feedback);
