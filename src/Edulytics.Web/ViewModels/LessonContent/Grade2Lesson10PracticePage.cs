namespace Edulytics.Web.ViewModels.LessonContent;

/// <summary>
/// Original learner-constructed picture and bar graphs. Inputs contain
/// student authored titles, key and category heights; teacher explanation
/// is not awarded an automatic grade.
/// </summary>
public sealed class Grade2Lesson10Answers
{
    public string? PictureTitle {get;set;}
    public string? PictureSymbol {get;set;}
    public string? PictureKey {get;set;}
    public string?[]? PictureCounts {get;set;}
    public string? BarTitle {get;set;}
    public string? BarStart {get;set;}
    public string? BarStep {get;set;}
    public string?[]? BarCounts {get;set;}
    public string? ExitTitle {get;set;}
    public string? ExitSymbol {get;set;}
    public string? ExitKey {get;set;}
    public string?[]? ExitPictureCounts {get;set;}
    public string?[]? ExitBarCounts {get;set;}
    public string? Reflection {get;set;}
    public string? WhichDoesNotBelong {get;set;}
}
public sealed record Grade2Lesson10PracticePage(
    Guid AdoptionId,Guid LessonId,string LessonTitle,bool Submitted,
    Grade2Lesson10Answers Answers,
    IReadOnlyList<Grade2Lesson10Feedback> Feedback,
    bool GraphOneCorrect,bool GraphTwoCorrect,bool CoolDownCorrect);
public sealed record Grade2Lesson10Feedback(
    string Activity,bool? Correct,bool TeacherReviewNeeded,string Reason);
