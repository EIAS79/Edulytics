namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson46Answers
{
    public string? WarmA{get;set;}="True";
    public string? WarmB{get;set;}="True";
    public string? WarmC{get;set;}="True";
    public string? WarmD{get;set;}="False";
    public string? LinAfterNoah{get;set;}
    public string? LinAfterJada{get;set;}
    public string? HanExtra{get;set;}
    public string? ClareFinal{get;set;}
    public string? AndreMissing{get;set;}
    public string? MaiOriginal{get;set;}
    public string? ActualThreeReadsAndDiagrams{get;set;}
    public string? RealPartnerDiscussion{get;set;}
    public string? IndependentA{get;set;}
    public string? IndependentB{get;set;}
    public string? IndependentC{get;set;}
}
public sealed record Grade2Lesson46Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson46Answers Answers,IReadOnlyList<Grade2Lesson46Feedback> Feedback);
public sealed record Grade2Lesson46Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
