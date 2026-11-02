namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson44Answers
{
    public string? TrueA{get;set;}="True";
    public string? TrueB{get;set;}="True";
    public string? TrueC{get;set;}="False";
    public string? TrueD{get;set;}="True";
    public string? CorrectFalseReason{get;set;}
    public string? JadaStart{get;set;}="2";
    public string? JadaEnd{get;set;}="10";
    public string? JadaNotebookLength{get;set;}
    public string? LinDeskStart{get;set;}="16";
    public string? LinDeskShortEnd{get;set;}="34";
    public string? LinDeskShortLength{get;set;}
    public string? LinLongSideActualImageNotes{get;set;}
    public string? RealObjectAndPartner{get;set;}
    public string? VirtualA{get;set;}
    public string? VirtualB{get;set;}
    public string? VirtualC{get;set;}
    public string? ExitA{get;set;}
    public string? ExitB{get;set;}
}
public sealed record Grade2Lesson44Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson44Answers Answers,IReadOnlyList<Grade2Lesson44Feedback> Feedback);
public sealed record Grade2Lesson44Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
