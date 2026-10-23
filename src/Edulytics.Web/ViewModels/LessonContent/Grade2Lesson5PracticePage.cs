namespace Edulytics.Web.ViewModels.LessonContent;

public sealed record Grade2Lesson5PracticePage(
 Guid CurriculumAdoptionId,Guid LessonId,string Title,bool Submitted,
 string? TwoDigitEquation,string? Mental,string? Regroup,
 string? ToFifty,string? Explanation,
 IReadOnlyList<Grade2Lesson5Feedback> Feedback);

public sealed record Grade2Lesson5Feedback(
 string Task,bool? Correct,bool TeacherReviewNeeded,string Feedback);
