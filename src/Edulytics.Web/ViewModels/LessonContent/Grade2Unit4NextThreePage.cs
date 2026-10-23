namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Unit4NextThreeAnswers
{
    public string? SourceA{get;set;}public string? SourceB{get;set;}
    public string? SourceC{get;set;}public string? SourceD{get;set;}
    public string? IndependentA{get;set;}public string? IndependentB{get;set;}
    public string? IndependentC{get;set;}public string? IndependentD{get;set;}
    public string? SourceRealWork{get;set;}public string? SourceDiscussion{get;set;}
    public string? SourceTeacherObservation{get;set;}
}
public sealed record Grade2Unit4NextThreePage(
    Guid AdoptionId,Guid LessonId,string Title,int LessonNumber,bool Submitted,
    IReadOnlyList<string> SourceQuestions,IReadOnlyList<string> IndependentQuestions,
    string SourceActivity1,string SourceActivity2,string TeacherBoundary,
    Grade2Unit4NextThreeAnswers Answers,IReadOnlyList<Grade2Unit4NextThreeFeedback> Feedback);
public sealed record Grade2Unit4NextThreeFeedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
