namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson55Answers
{
    public string? Original24{get;set;}public string? Original37{get;set;}
    public string? Original48{get;set;}public string? Original83{get;set;}
    public string? OffsetBefore25{get;set;}public string? OffsetBefore40{get;set;}
    public string? OffsetBefore50{get;set;}public string? OffsetAfter80{get;set;}
    public string? FirstParty83{get;set;}public string? FirstPartyGap40to60{get;set;}
    public string? FirstPartyGap17to43{get;set;}
    public string? OriginalSource24Point{get;set;}public string? OriginalSource37Point{get;set;}
    public string? OriginalSource48Point{get;set;}public string? OriginalSource83Point{get;set;}
    public string? OriginalSource17Diagram{get;set;}public string? OriginalSource59Diagram{get;set;}
    public string? OriginalSource43Diagram{get;set;}public string? OriginalSource35Diagram{get;set;}
    public string? OriginalPartnerExplanation{get;set;}
}
public sealed record Grade2Lesson55Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,
    Grade2Lesson55Answers Answers,IReadOnlyList<Grade2Lesson55Feedback> Feedback);
public sealed record Grade2Lesson55Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
