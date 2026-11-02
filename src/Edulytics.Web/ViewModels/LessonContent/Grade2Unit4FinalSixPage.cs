namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Unit4FinalSixAnswers
{
    public string? SourceA{get;set;}public string? SourceB{get;set;}
    public string? SourceC{get;set;}public string? SourceD{get;set;}
    public string? IndependentA{get;set;}public string? IndependentB{get;set;}
    public string? IndependentC{get;set;}public string? IndependentD{get;set;}
    public string? OriginalDiagramOrGame{get;set;}public string? OriginalPartnerEvidence{get;set;}
    public string? ActualOriginalTeacherObservation{get;set;}
}
public sealed record Grade2Unit4FinalSixPage(Guid AdoptionId,Guid LessonId,string Title,int LessonNumber,bool Submitted,
    IReadOnlyList<string> SourceQuestions,IReadOnlyList<string> FirstPartyQuestions,string AuthenticSourceTasks,
    string TeacherOnlyBoundary,Grade2Unit4FinalSixAnswers Answers,IReadOnlyList<Grade2Unit4FinalSixFeedback> Feedback);
public sealed record Grade2Unit4FinalSixFeedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Message);
