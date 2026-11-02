namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson29Answers
{
    public string? WarmupStoryKnowledge{get;set;}
    public string? DiegoJadaDifference{get;set;}
    public string? DiegoJadaDiagram{get;set;}
    public string? LinRemaining{get;set;}
    public string? NoahBlue{get;set;}
    public string? AndrePumpkin{get;set;}
    public string? MyOtherThreeDiagrams{get;set;}
    public string? PartnerStrategy{get;set;}
    public string? FirstPartyCompare{get;set;}
    public string? FirstPartyTake{get;set;}
    public string? FirstPartyPart{get;set;}
    public string? FirstPartySmaller{get;set;}
}
public sealed record Grade2Lesson29Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson29Answers Answers,IReadOnlyList<Grade2Lesson29Feedback> Feedback);
public sealed record Grade2Lesson29Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
