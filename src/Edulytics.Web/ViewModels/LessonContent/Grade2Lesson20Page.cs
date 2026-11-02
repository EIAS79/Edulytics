namespace Edulytics.Web.ViewModels.LessonContent;

public sealed class Grade2Lesson20Answers
{
    public string? Warm77 {get;set;}
    public string? Warm37 {get;set;}
    public string? Warm7 {get;set;}
    public string? ToolObservation {get;set;}
    public string? Source41 {get;set;}
    public string? Source17 {get;set;}
    public string? Source21 {get;set;}
    public string? SourceInverse {get;set;}
    public string? HanAddition {get;set;}
    public string? MaiSubtraction {get;set;}
    public string? DrawingExplanation {get;set;}
    public string? CoolFirst {get;set;}
    public string? CoolSecond {get;set;}
    public string? CoolThird {get;set;}
    public string? PartnerReflection {get;set;}
}
public sealed record Grade2Lesson20Page(Guid AdoptionId,Guid LessonId,
    string LessonTitle,bool Submitted,Grade2Lesson20Answers Answers,
    IReadOnlyList<Grade2Lesson20Feedback> Feedback);
public sealed record Grade2Lesson20Feedback(string Activity,bool? Correct,
    bool TeacherReviewNeeded,string Explanation);
