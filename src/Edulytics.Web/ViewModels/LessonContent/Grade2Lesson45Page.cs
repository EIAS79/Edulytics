namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson45Answers
{
    public string? WarmupRemain{get;set;}
    public string? SourcePriyaAfter{get;set;}
    public string? SourceElenaAfter{get;set;}
    public string? SourceHanAfter{get;set;}
    public string? SourcePriyaOriginal{get;set;}
    public string? ThreeReadsAndDiagram{get;set;}
    public string? ActualBlockAndPartner{get;set;}
    public string? FirstPartyRemaining{get;set;}
    public string? FirstPartyStarting{get;set;}
    public string? FirstPartyExtra{get;set;}
}
public sealed record Grade2Lesson45Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson45Answers Answers,IReadOnlyList<Grade2Lesson45Feedback> Feedback);
public sealed record Grade2Lesson45Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
