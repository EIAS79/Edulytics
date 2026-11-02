namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Unit5FirstSixAnswers
{
    public string? SourceA{get;set;}public string? SourceB{get;set;}
    public string? SourceC{get;set;}public string? SourceD{get;set;}
    public string? IndependentA{get;set;}public string? IndependentB{get;set;}
    public string? IndependentC{get;set;}public string? IndependentD{get;set;}
    public string? ActualSourceDiagram{get;set;}public string? ActualTeacherObservedBlocks{get;set;}
    public string? RealPartnerReasoning{get;set;}public string? OriginalNumberNamesOrPoster{get;set;}
}
public sealed record Grade2Unit5FirstSixPage(Guid AdoptionId,Guid LessonId,string Title,
    int LessonNumber,bool Submitted,IReadOnlyList<string> SourcePrompts,IReadOnlyList<string> FirstPartyPrompts,
    IReadOnlyList<string> AuthenticRubric,Grade2Unit5FirstSixAnswers Answers,IReadOnlyList<Grade2Unit5Feedback> Feedback);
public sealed record Grade2Unit5Feedback(string Section,bool? Correct,bool HumanReviewRequired,string Explanation);
