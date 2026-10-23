namespace Edulytics.Web.ViewModels.LessonContent;

public sealed class Grade2Lesson14Answers
{
    public string? AppleGrapeGap {get;set;}
    public string? AppleGrapeAdd {get;set;}
    public string? AppleGrapeSub {get;set;}
    public string? PartyMore {get;set;}
    public string? PartyFewer {get;set;}
    public string? PartyGap {get;set;}
    public string? PartyAdd {get;set;}
    public string? PartySub {get;set;}
    public string? PizzaBurgerGap {get;set;}
    public string? PizzaBurgerAdd {get;set;}
    public string? PizzaBurgerSub {get;set;}
    public string? PizzaHotdogsGap {get;set;}
    public string? PizzaHotdogsAdd {get;set;}
    public string? PizzaHotdogsSub {get;set;}
    public string? CoolGap {get;set;}
    public string? CoolAdd {get;set;}
    public string? CoolSub {get;set;}
    public string? TeacherExplanation {get;set;}
}
public sealed record Grade2Lesson14Page(
    Guid AdoptionId,Guid LessonId,string Title,bool Submitted,
    Grade2Lesson14Answers Answers,IReadOnlyList<Grade2Lesson14Feedback> Feedback);
public sealed record Grade2Lesson14Feedback(
    string Task,bool? Correct,bool TeacherReviewNeeded,string Feedback);
