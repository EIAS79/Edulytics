namespace Edulytics.Web.ViewModels.LessonContent;
public sealed record Grade2Lesson6PracticePage(
    Guid AdoptionId,Guid LessonId,string LessonTitle,bool Submitted,
    string? First,string? Second,string? Third,string? Fourth,
    string? WarmUp,string? CenterChoice,string? Explanation,
    IReadOnlyList<Grade2Lesson6Feedback> Feedback);
public sealed record Grade2Lesson6Feedback(
    string Task,bool? Correct,bool TeacherReviewNeeded,string Text);
