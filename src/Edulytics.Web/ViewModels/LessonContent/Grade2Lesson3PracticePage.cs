namespace Edulytics.Web.ViewModels.LessonContent;

public sealed record Grade2Lesson3PracticePage(
    Guid CurriculumAdoptionId,Guid LessonId,string Title,bool Submitted,
    string? Hidden,string? Removed,string? Extension,
    string? StatementOne,string? StatementTwo,string? Explanation,
    IReadOnlyList<Grade2Lesson3Feedback> Feedback);

public sealed record Grade2Lesson3Feedback(
    string Task,bool? Correct,bool TeacherReviewNeeded,string Message);
