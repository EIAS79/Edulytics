namespace Edulytics.Web.ViewModels.LessonContent;

public sealed class Grade2Lesson17Answers
{
    public string? WarmOne {get;set;}
    public string? WarmTwo {get;set;}
    public string? WarmThree {get;set;}
    public string? WarmFour {get;set;}
    public string? DigitalGrouping {get;set;}="Tens";
    public string? DigitalGroups {get;set;}
    public string? DigitalRemainder {get;set;}
    public string? DigitalTotal {get;set;}
    public string? Estimate {get;set;}
    public string? ActualGrouping {get;set;}="Tens";
    public string? ActualGroups {get;set;}
    public string? ActualRemainder {get;set;}
    public string? ActualTotal {get;set;}
    public string? ActualRepresentation {get;set;}
    public string? FirstCenter {get;set;}
    public string? FirstAnswer {get;set;}
    public string? SecondCenter {get;set;}
    public string? SecondAnswer {get;set;}
    public string? CoolCount {get;set;}
    public string? CoolAdd {get;set;}
    public string? CoolGraph {get;set;}
    public string? Reflection {get;set;}
}
public sealed record Grade2Lesson17Page(
    Guid AdoptionId,Guid LessonId,string LessonTitle,bool Submitted,
    Grade2Lesson17Answers Answers,
    IReadOnlyList<Grade2Lesson17Feedback> Feedback);
public sealed record Grade2Lesson17Feedback(
    string Activity,bool? Correct,bool TeacherReviewNeeded,string Explanation);
