namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson47Answers
{
 public string? Talk1{get;set;}public string? Talk2{get;set;}public string? Talk3{get;set;}public string? Talk4{get;set;}
 public string? Target{get;set;}="8";public string? Actual{get;set;}="10";public string? Unit{get;set;}="cm";public string? Score{get;set;}
 public string? PartnerLineReflection{get;set;}public string? RealEightRoundRecord{get;set;}
 public string? PuzzleAdd{get;set;}public string? PuzzleSub{get;set;}public string? RealDigitCardReflection{get;set;}
 public string? ExitMeasure{get;set;}public string? ExitAdd{get;set;}
}
public sealed record Grade2Lesson47Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson47Answers Answers,IReadOnlyList<Grade2Lesson47Feedback> Feedback);
public sealed record Grade2Lesson47Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Explanation);
