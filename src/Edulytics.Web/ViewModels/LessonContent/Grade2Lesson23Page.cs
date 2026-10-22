namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson23Answers
{
    public string? TalkA{get;set;}
    public string? TalkB{get;set;}
    public string? TalkC{get;set;}
    public string? TalkD{get;set;}
    public string? Reasoning{get;set;}
    public string? EightyTwoAnswer{get;set;}
    public string? EightyTwoTens{get;set;}
    public string? EightyTwoOnes{get;set;}
    public string? EightyTwoDrawing{get;set;}
    public string? DiegoStart{get;set;}
    public string? DiegoAfterTens{get;set;}
    public string? DiegoTradeTens{get;set;}
    public string? DiegoTradeOnes{get;set;}
    public string? DiegoEnd{get;set;}
    public string? DiegoDrawing{get;set;}
    public string? CoolA{get;set;}
    public string? CoolB{get;set;}
    public string? CoolC{get;set;}
    public string? Reflection{get;set;}
}
public sealed record Grade2Lesson23Page(Guid AdoptionId,Guid LessonId,string Title,
    bool Submitted,Grade2Lesson23Answers Answers,IReadOnlyList<Grade2Lesson23Feedback> Feedback);
public sealed record Grade2Lesson23Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Message);
