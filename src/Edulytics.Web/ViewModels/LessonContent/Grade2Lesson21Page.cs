namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson21Answers
{
    public string? WarmupReason{get;set;}
    public string? ThreeReadsContext{get;set;}
    public string? ThreeReadsQuantities{get;set;}
    public string? BusStart{get;set;}
    public string? BusEquation{get;set;}
    public string? BusDrawing{get;set;}
    public string? Monkey{get;set;}
    public string? BearsGap{get;set;}
    public string? BirdsOutside{get;set;}
    public string? OtherRepresentations{get;set;}
    public string? CoolTake{get;set;}
    public string? CoolCompare{get;set;}
    public string? CoolPart{get;set;}
    public string? PartnerReasoning{get;set;}
}
public sealed record Grade2Lesson21Page(Guid AdoptionId,Guid LessonId,
    string LessonTitle,bool Submitted,Grade2Lesson21Answers Answers,
    IReadOnlyList<Grade2Lesson21Feedback> Feedback);
public sealed record Grade2Lesson21Feedback(string Activity,bool? Correct,bool TeacherReviewNeeded,string Feedback);
