namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson38Answers
{
    public string? ValidSourceRulers{get;set;}="ABD";
    public string? LengthFromOneToFive{get;set;}
    public string? ExplanationOfImageC{get;set;}
    public string? RealObjectsAndEstimates{get;set;}
    public string? ThreeEstimatesLow{get;set;}
    public string? ThreeEstimatesAbout{get;set;}
    public string? ThreeEstimatesHigh{get;set;}
    public string? RealBeforeAfterEstimate{get;set;}
    public string? RealMeasuredLengths{get;set;}
    public string? ZeroTickExplanation{get;set;}
    public string? VirtualStart{get;set;}="2";
    public string? VirtualEnd{get;set;}="17";
    public string? VirtualLength{get;set;}
    public string? LongFullRulers{get;set;}="2";
    public string? LongRemaining{get;set;}="8";
    public string? LongTotal{get;set;}
    public string? IndependentTickStart{get;set;}="3";
    public string? IndependentTickEnd{get;set;}="14";
    public string? IndependentTickAnswer{get;set;}
    public string? PartnerReflection{get;set;}
}
public sealed record Grade2Lesson38Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson38Answers Answers,IReadOnlyList<Grade2Lesson38Feedback> Feedback);
public sealed record Grade2Lesson38Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
