namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson27Answers
{
    public string? WarmupRealImages{get;set;}
    public string? RealCardMatches{get;set;}
    public string? RealAdditionCard{get;set;}
    public string? RealSubtractionCard{get;set;}
    public string? RealGroupExplanation{get;set;}
    public string? SourceAddOne{get;set;}
    public string? SourceSubtractOne{get;set;}
    public string? SourceAddTwo{get;set;}
    public string? SourceSubtractTwo{get;set;}
    public string? SectionSummary{get;set;}
    public string? MyDiagramAndGallery{get;set;}
    public string? ExtraAdd{get;set;}
    public string? ExtraSubtract{get;set;}
    public string? ExtraSecondAdd{get;set;}
}
public sealed record Grade2Lesson27Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson27Answers Answers,IReadOnlyList<Grade2Lesson27Feedback> Feedback);
public sealed record Grade2Lesson27Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
