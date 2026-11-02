namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson31Answers
{
    public string? WarmupMissing{get;set;}
    public string? WarmupRealDiagrams{get;set;}
    public string? RealSourceCardMatches{get;set;}
    public string? RealSourceSolvedTwo{get;set;}
    public string? LinEquation{get;set;}
    public string? LinStart{get;set;}
    public string? AndreEquation{get;set;}
    public string? AndreGained{get;set;}
    public string? DiegoEquation{get;set;}
    public string? DiegoTotal{get;set;}
    public string? KiranEquation{get;set;}
    public string? KiranPart{get;set;}
    public string? AuthenticDrawings{get;set;}
    public string? RealPartnerMethod{get;set;}
    public string? FirstPartyStart{get;set;}
    public string? FirstPartyChange{get;set;}
    public string? FirstPartyTotal{get;set;}
    public string? FirstPartyPart{get;set;}
}
public sealed record Grade2Lesson31Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson31Answers Answers,IReadOnlyList<Grade2Lesson31Feedback> Feedback);
public sealed record Grade2Lesson31Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
