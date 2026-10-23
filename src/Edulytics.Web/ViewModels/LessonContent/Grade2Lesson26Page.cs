namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson26Answers
{
    public string? TalkA{get;set;}
    public string? TalkB{get;set;}
    public string? TalkC{get;set;}
    public string? TalkD{get;set;}
    public string? TalkReasoning{get;set;}
    public string? LinClareDifference{get;set;}
    public string? ClareResidual{get;set;}
    public string? ClareAfterExchangeOnes{get;set;}
    public string? CompareMethods{get;set;}
    public string? Andre{get;set;}
    public string? Source34{get;set;}
    public string? Source82{get;set;}
    public string? Source71{get;set;}
    public string? StudentDiagram{get;set;}
    public string? ExitA{get;set;}
    public string? ExitB{get;set;}
    public string? ExitC{get;set;}
    public string? PartnerReasoning{get;set;}
}
public sealed record Grade2Lesson26Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson26Answers Answers,IReadOnlyList<Grade2Lesson26Feedback> Feedback);
public sealed record Grade2Lesson26Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Message);
