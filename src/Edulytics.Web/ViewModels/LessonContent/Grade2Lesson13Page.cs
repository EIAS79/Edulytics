namespace Edulytics.Web.ViewModels.LessonContent;

public sealed class Grade2Lesson13Answers
{
    public string? WarmFirst {get;set;}
    public string? WarmSecond {get;set;}
    public string? WarmThird {get;set;}
    public string? PetsDifference {get;set;}
    public string? PetsCountingOn {get;set;}
    public string? PetsCountingBack {get;set;}
    public string? DogsMore {get;set;}
    public string? DogsFewer {get;set;}
    public string? DogsAddition {get;set;}
    public string? DogsSubtraction {get;set;}
    public string? CoolDifference {get;set;}
    public string? CoolAddition {get;set;}
    public string? CoolSubtraction {get;set;}
    public string? Explain {get;set;}
}
public sealed record Grade2Lesson13Page(
    Guid AdoptionId,Guid LessonId,string LessonTitle,bool Submitted,
    Grade2Lesson13Answers Answers,IReadOnlyList<Grade2Lesson13Feedback> Feedback);
public sealed record Grade2Lesson13Feedback(
    string Task,bool? Correct,bool TeacherReviewNeeded,string Message);
