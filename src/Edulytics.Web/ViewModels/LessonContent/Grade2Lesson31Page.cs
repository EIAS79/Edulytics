namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson31Answers
{
    public string? WarmupMissing{get;set;}
    public string? WarmupDiagramReason{get;set;}
    public string? ActualCardMatches{get;set;}
    public string? ActualCardMultipleEquations{get;set;}
    public string? LinStart{get;set;}
    public string? AndreWon{get;set;}
    public string? DiegoTotal{get;set;}
    public string? KiranPart{get;set;}
    public string? StoryEquationsAndTapeDiagrams{get;set;}
    public string? FirstPartyStart{get;set;}
    public string? FirstPartyChange{get;set;}
    public string? FirstPartyTotal{get;set;}
    public string? FirstPartyPart{get;set;}
    public string? PartnerSummary{get;set;}
}
public sealed record Grade2Lesson31Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson31Answers Answers,IReadOnlyList<Grade2Lesson31Feedback> Feedback);
public sealed record Grade2Lesson31Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
