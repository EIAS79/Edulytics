namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson36Answers
{
    public string? WarmupCmMeaning{get;set;}
    public string? SourceDragonCubes{get;set;}
    public string? SourceDragonRods{get;set;}
    public string? SourceDragonExtraCm{get;set;}
    public string? SourceDragonLength{get;set;}
    public string? RealReptileLengthTable{get;set;}
    public string? RealDayGeckoDifference{get;set;}
    public string? PartnerExplanation{get;set;}
    public string? FirstPartyRods{get;set;}
    public string? FirstPartyExtra{get;set;}
    public string? FirstPartyDifference{get;set;}
    public string? ExitRods{get;set;}
    public string? ExitExtra{get;set;}
}
public sealed record Grade2Lesson36Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson36Answers Answers,IReadOnlyList<Grade2Lesson36Feedback> Feedback);
public sealed record Grade2Lesson36Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
