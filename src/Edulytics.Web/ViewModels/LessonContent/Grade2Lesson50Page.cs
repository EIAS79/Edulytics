namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson50Answers
{
    public string? TalkA{get;set;}public string? TalkB{get;set;}
    public string? TalkC{get;set;}public string? TalkD{get;set;}
    public string? GroupBShortest{get;set;}
    public string? GroupBTallest{get;set;}
    public string? GroupBDifference{get;set;}
    public string? GroupBMode{get;set;}
    public string? GroupBMore30{get;set;}
    public string? GroupBTotal{get;set;}
    public string? GroupBDiagramExplanation{get;set;}
    public string? HanPlotImageObservation{get;set;}
    public string? HanPlotTeacherFeedback{get;set;}
    public string? FirstPartyTotal{get;set;}
    public string? FirstPartyMode{get;set;}
    public string? FirstPartyRange{get;set;}
    public string? FirstPartyOver24{get;set;}
}
public sealed record Grade2Lesson50Page(Guid AdoptionId,Guid LessonId,string Title,
    bool Submitted,Grade2Lesson50Answers Answers,IReadOnlyList<Grade2Lesson50Feedback> Feedback);
public sealed record Grade2Lesson50Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Message);
