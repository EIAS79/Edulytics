namespace Edulytics.Web.ViewModels.LessonContent;

public sealed record Grade2Lesson4PracticePage(
    Guid CurriculumAdoptionId,Guid LessonId,string Title,bool Submitted,
    string? CardEquation,string? SeventeenMinusNine,string? SevenPlusSix,
    string? Explanation,IReadOnlyList<Grade2Lesson4Feedback> Feedback);

public sealed record Grade2Lesson4Feedback(string Task,bool? Correct,
    bool TeacherReviewNeeded,string Reason);
