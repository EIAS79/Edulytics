namespace Edulytics.Web.ViewModels.LessonContent;
public sealed class Grade2Lesson29Answers
{
    public string? WarmupStoryStructures{get;set;}
    public string? DiegoDifference{get;set;}
    public string? LinRemaining{get;set;}
    public string? NoahBlue{get;set;}
    public string? AndreSmaller{get;set;}
    public string? DiagramComparison{get;set;}
    public string? TeacherCubeWork{get;set;}
    public string? ExtraCompare{get;set;}
    public string? ExtraTakeFrom{get;set;}
    public string? ExtraPart{get;set;}
    public string? ExitA{get;set;}
    public string? ExitB{get;set;}
}
public sealed record Grade2Lesson29Page(Guid AdoptionId,Guid LessonId,string Title,bool Submitted,Grade2Lesson29Answers Answers,IReadOnlyList<Grade2Lesson29Feedback> Feedback);
public sealed record Grade2Lesson29Feedback(string Section,bool? Correct,bool TeacherReviewNeeded,string Feedback);
