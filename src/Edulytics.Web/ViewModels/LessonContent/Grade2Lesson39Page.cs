namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson39Answers
{
    public string? ChoralCount{get;set;}
    public string? TapeA{get;set;}
    public string? TapeB{get;set;}
    public string? TapeC{get;set;}
    public string? TapeD{get;set;}
    public string? TapeE{get;set;}
    public string? TapeF{get;set;}
    public string? TapeG{get;set;}
    public string? KomodoMeters{get;set;}
    public string? KomodoExtraCm{get;set;}
    public string? PhysicalTapeAndTools{get;set;}
    public string? PartnerCompare{get;set;}
    public string? ExitTotal247{get;set;}
    public string? ExitRemainder365{get;set;}
    public string? ExitFull412{get;set;}
}
public sealed record Grade2Lesson39Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson39Answers Answers,IReadOnlyList<Grade2Lesson39Feedback> Feedback);
public sealed record Grade2Lesson39Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
