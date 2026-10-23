namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson50Answers
{
    public string? TalkA{get;set;}
    public string? TalkB{get;set;}
    public string? TalkC{get;set;}
    public string? TalkD{get;set;}
    public string? SourceMin{get;set;}
    public string? SourceMax{get;set;}
    public string? SourceDifference{get;set;}
    public string? SourceAbove30{get;set;}
    public string? SourceTotal{get;set;}
    public string? SourceMode{get;set;}
    public string? At25{get;set;}
    public string? At33{get;set;}
    public string? Empty28{get;set;}
    public string? Empty34{get;set;}
    public string? RealOwnPlot{get;set;}
    public string? RealPartnerCompare{get;set;}
    public string? RealHanImageObservation{get;set;}
    public string? FictionMin{get;set;}
    public string? FictionMax{get;set;}
    public string? FictionMode{get;set;}
    public string? FictionAbove26{get;set;}
}
public sealed record Grade2Lesson50Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,
    Grade2Lesson50Answers Answers,IReadOnlyList<Grade2Lesson50Feedback> Feedback);
public sealed record Grade2Lesson50Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
