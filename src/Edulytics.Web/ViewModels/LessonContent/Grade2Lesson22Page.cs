namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson22Answers
{
    public string? TalkOne{get;set;}
    public string? TalkTwo{get;set;}
    public string? TalkThree{get;set;}
    public string? TalkFour{get;set;}
    public string? MentalStrategy{get;set;}
    public string? CaptureWild{get;set;}="No";
    public string? CaptureSpinner{get;set;}="8";
    public string? CaptureCard{get;set;}="7";
    public string? CaptureSum{get;set;}
    public string? CaptureBoardExplanation{get;set;}
    public string? FiveTopFirst{get;set;}="25";
    public string? FiveBottomFirst{get;set;}="8";
    public string? FiveSumFirst{get;set;}
    public string? FiveTopSecond{get;set;}="25";
    public string? FiveBottomSecond{get;set;}="9";
    public string? FiveSumSecond{get;set;}
    public string? FiveBoardExplanation{get;set;}
    public string? CoolCapture{get;set;}
    public string? CoolCompose{get;set;}
    public string? CoolSubtract{get;set;}
    public string? PartnerReflection{get;set;}
}
public sealed record Grade2Lesson22Page(Guid AdoptionId,Guid LessonId,
    string LessonTitle,bool Submitted,Grade2Lesson22Answers Answers,
    IReadOnlyList<Grade2Lesson22Feedback> Feedback);
public sealed record Grade2Lesson22Feedback(string Activity,bool? Correct,bool TeacherReviewNeeded,string Feedback);
