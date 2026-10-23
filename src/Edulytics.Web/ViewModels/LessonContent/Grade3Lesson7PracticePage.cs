namespace Edulytics.Web.ViewModels.LessonContent;
/// <summary>Original L07 classroom data tasks and separate fictional Cool-down.</summary>
public sealed class Grade3Lesson7Answers
{
    public string? Warmup{get;set;}
    public string? SeasonsTotal{get;set;}
    public string? SpringOrFall{get;set;}
    public string? SummerWinter{get;set;}
    public string? FallSpring{get;set;}
    public string? SeasonExplanation{get;set;}
    public string? BugTotalEstimate{get;set;}
    public string? AntSpiderEstimate{get;set;}
    public string? AntLadybugEstimate{get;set;}
    public string? BugGraphQuestions{get;set;}
    public string? BugEstimateReasoning{get;set;}
    public string? CoolSoccerTennis{get;set;}
    public string? CoolSwimBasket{get;set;}
    public string? CoolTotal{get;set;}
    public string? CoolExplanation{get;set;}
}
public sealed record Grade3Lesson7Feedback(string Label,bool? MachineCorrect,
    bool TeacherReview,string Message);
public sealed record Grade3Lesson7PracticePage(Guid AdoptionId,Guid LessonId,
    bool Submitted,Grade3Lesson7Answers Answers,
    IReadOnlyList<Grade3Lesson7Feedback> Feedback);
