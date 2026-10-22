namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson34Answers
{
    public string? MeasuringIdeas{get;set;}
    public string? RealLongStraws{get;set;}
    public string? RealShortStraws{get;set;}
    public string? RealCentimeterCubes{get;set;}
    public string? RealPhysicalAlignment{get;set;}
    public string? CentimeterDefinition{get;set;}
    public string? UnitChoiceExplanation{get;set;}
    public string? FirstPartyLong{get;set;}
    public string? FirstPartyShort{get;set;}
    public string? FirstPartyCubes{get;set;}
    public string? PartnerReflection{get;set;}
}
public sealed record Grade2Lesson34Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson34Answers Answers,IReadOnlyList<Grade2Lesson34Feedback> Feedback);
public sealed record Grade2Lesson34Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
