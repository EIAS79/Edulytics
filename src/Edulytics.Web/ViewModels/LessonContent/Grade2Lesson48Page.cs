namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson48Answers
{
    public string? WarmupCompare{get;set;}
    public string? RealHandSpanInches{get;set;}
    public string? RealPartnerCheck{get;set;}
    public string? RealClassLongest{get;set;}
    public string? RealClassShortest{get;set;}
    public string? RealClassMode{get;set;}
    public string? RealClassTotalXs{get;set;}
    public string? RealClassStatement{get;set;}
    public string? FictionalShortest{get;set;}
    public string? FictionalLongest{get;set;}
    public string? FictionalMode{get;set;}
    public string? FictionalTotal{get;set;}
    public string? FictionalSevenCount{get;set;}
    public string? FictionalRange{get;set;}
}
public sealed record Grade2Lesson48Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,
    Grade2Lesson48Answers Answers,IReadOnlyList<Grade2Lesson48Feedback> Feedback);
public sealed record Grade2Lesson48Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Message);
