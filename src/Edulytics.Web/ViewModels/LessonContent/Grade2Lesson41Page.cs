namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson41Answers
{
    public string? TalkA{get;set;}
    public string? TalkB{get;set;}
    public string? TalkC{get;set;}
    public string? TalkD{get;set;}
    public string? NumberTalkStrategy{get;set;}
    public string? SourceEstimate{get;set;}="20";
    public string? SourceActual{get;set;}="31";
    public string? SourceDifference{get;set;}
    public string? RealObject{get;set;}
    public string? RealEstimateBefore{get;set;}
    public string? RealMeasurementCm{get;set;}
    public string? RealToolAndZero{get;set;}
    public string? RealPartnerEstimate{get;set;}
    public string? RealCenterChoice{get;set;}
    public string? FirstPartyA{get;set;}
    public string? FirstPartyB{get;set;}
    public string? FirstPartyC{get;set;}
}
public sealed record Grade2Lesson41Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson41Answers Answers,IReadOnlyList<Grade2Lesson41Feedback> Feedback);
public sealed record Grade2Lesson41Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
