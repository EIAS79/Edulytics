namespace Edulytics.Web.ViewModels.LessonContent;

/// <summary>Unsaved learner practice; no official assessment outcome.</summary>
public sealed class Grade3Lesson3Answers
{
    public string? Warmup { get; set; }
    public string? DifferenceInKeys { get; set; }
    public string? OriginalQuestion { get; set; }
    public string? MathCommunity { get; set; }
    public string? CoolExplanation { get; set; }
    public string? SportsTotal { get; set; }
    public string? SportsBasketball { get; set; }
    public string? SportsCompare { get; set; }
    public string? FlowersRoses { get; set; }
    public string? FlowersTotal { get; set; }
    public string? FlowersCompare { get; set; }
    public string? CoolNature { get; set; }
    public string? CoolDifference { get; set; }
}
public sealed record Grade3Lesson3Feedback(string Name, bool? MachineCorrect, bool TeacherReview,
    string Message);
public sealed record Grade3Lesson3PracticePage(Guid AdoptionId, Guid LessonId,
    bool Submitted, Grade3Lesson3Answers Answers,
    IReadOnlyList<Grade3Lesson3Feedback> Feedback);
