namespace Edulytics.Web.ViewModels.LessonContent;
/// <summary>G3 U01 L08 unsaved student worksheet and assessment boundaries.</summary>
public sealed class Grade3Lesson8Answers
{
    public string? Warmup{get;set;}
    public string? FirstRead{get;set;}
    public string? SecondRead{get;set;}
    public string? ThirdRead{get;set;}
    public string? FeelingsTwoStep{get;set;}
    public string? FeelingsOtherMethod{get;set;}
    public string? PineFir{get;set;}
    public string? PineOakMaple{get;set;}
    public string? PineOak{get;set;}
    public string? FirOakMaple{get;set;}
    public string? TreeWork{get;set;}
    public string? CoolAdventureScience{get;set;}
    public string? CoolAdventureVsTwo{get;set;}
    public string? CoolTotal{get;set;}
    public string? CoolExplanation{get;set;}
}
public sealed record Grade3Lesson8Feedback(string Label,bool? NumericCorrect,
    bool TeacherReview,string Explanation);
public sealed record Grade3Lesson8PracticePage(Guid AdoptionId,Guid LessonId,
    bool Submitted,Grade3Lesson8Answers Answers,
    IReadOnlyList<Grade3Lesson8Feedback> Feedback);
