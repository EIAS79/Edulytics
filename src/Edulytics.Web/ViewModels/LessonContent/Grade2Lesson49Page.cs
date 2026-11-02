namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson49Answers
{
    public string? TalkA{get;set;}public string? TalkB{get;set;}
    public string? TalkC{get;set;}public string? TalkD{get;set;}
    public string? RealPencilMeasurements{get;set;}
    public string? RealTwoMeasurersExplanation{get;set;}
    public string? RealPlotDescription{get;set;}
    public string? SourceTotal{get;set;}
    public string? SourceMode{get;set;}
    public string? SourceShortest{get;set;}
    public string? SourceLongest{get;set;}
    public string? SourceLongerThan12{get;set;}
    public string? EmptyAt11{get;set;}
    public string? EmptyAt15{get;set;}
    public string? CountAt10{get;set;}
    public string? CountAt16{get;set;}
    public string? ExplainNonzeroStart{get;set;}
    public string? IndependentMode{get;set;}
    public string? IndependentTotal{get;set;}
    public string? IndependentRange{get;set;}
}
public sealed record Grade2Lesson49Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,
    Grade2Lesson49Answers Answers,IReadOnlyList<Grade2Lesson49Feedback> Feedback);
public sealed record Grade2Lesson49Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Message);
