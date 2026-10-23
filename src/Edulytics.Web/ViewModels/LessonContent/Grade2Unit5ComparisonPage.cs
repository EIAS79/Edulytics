namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Unit5ComparisonAnswers
{
 public string? SourceA{get;set;}public string? SourceB{get;set;}
 public string? SourceC{get;set;}public string? SourceD{get;set;}
 public string? IndependentA{get;set;}public string? IndependentB{get;set;}
 public string? IndependentC{get;set;}public string? IndependentD{get;set;}
 public string? OriginalImageObservations{get;set;}public string? OriginalDiagramOrNumberLine{get;set;}
 public string? SourceCenterOrBeanCount{get;set;}public string? PeerReasoning{get;set;}
}
public sealed record Grade2Unit5ComparisonPage(Guid AdoptionId,Guid LessonId,string Title,int LessonNumber,bool Submitted,
 IReadOnlyList<string> SourcePrompts,IReadOnlyList<string> IndependentPrompts,IReadOnlyList<string> SourceTeacherRubric,
 Grade2Unit5ComparisonAnswers Answers,IReadOnlyList<Grade2Unit5ComparisonFeedback> Feedback);
public sealed record Grade2Unit5ComparisonFeedback(string Section,bool? Correct,bool HumanReviewRequired,string Feedback);
