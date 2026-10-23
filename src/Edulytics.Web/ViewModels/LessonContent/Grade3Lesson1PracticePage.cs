namespace Edulytics.Web.ViewModels.LessonContent;

/// <summary>
/// Unsaved Grade 3 U01 L01 learner answers: never treats open reasoning,
/// classroom participation, invented questions or diagrams as machine passes.
/// </summary>
public sealed class Grade3Lesson1Answers
{
    public string? WarmupObservations { get; set; }
    public string? PlausibleCategories { get; set; }
    public string? GraphComparison { get; set; }
    public string? TwoGraphQuestions { get; set; }
    public string? BusCount { get; set; }
    public string? BusSkateboardDifference { get; set; }
    public string? TotalStudents { get; set; }
    public string? CoolDownCount { get; set; }
    public string? CoolDownDifference { get; set; }
    public string? CoolDownOriginalQuestion { get; set; }
    public string? SynthesisReflection { get; set; }
    public string? MathCommunityReflection { get; set; }
}

public sealed record Grade3Lesson1Feedback(
    string Label, bool? MachineCorrect, bool TeacherReviewRequired, string Reason);

public sealed record Grade3Lesson1PracticePage(
    Guid CurriculumAdoptionId,
    Guid LessonId,
    string LessonTitle,
    bool Submitted,
    Grade3Lesson1Answers Answers,
    IReadOnlyList<Grade3Lesson1Feedback> Feedback);
