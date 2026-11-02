namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson32Answers
{
    public string? WarmupOne{get;set;}public string? WarmupTwo{get;set;}
    public string? WarmupThree{get;set;}public string? WarmupFour{get;set;}
    public string? WarmupStrategy{get;set;}
    public string? AndreFirst{get;set;}public string? AndreTotal{get;set;}
    public string? AndreDiagram{get;set;}
    public string? PriyaSmaller{get;set;}public string? MaiMissing{get;set;}
    public string? KiranResult{get;set;}public string? GroupTotal{get;set;}
    public string? RealGroupDiagram{get;set;}
    public string? JadaStart{get;set;}
    public string? FirstPartyTriple{get;set;}public string? FirstPartySmaller{get;set;}
    public string? FirstPartyPart{get;set;}public string? FirstPartyResult{get;set;}
}
public sealed record Grade2Lesson32Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson32Answers Answers,IReadOnlyList<Grade2Lesson32Feedback> Feedback);
public sealed record Grade2Lesson32Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
