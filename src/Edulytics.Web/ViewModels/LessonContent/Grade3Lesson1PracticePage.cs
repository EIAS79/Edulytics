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
    // Source Student Facing activity 2, part 2 is separate from comparison.
    public string? GraphLearnings { get; set; }
    public string? TwoGraphQuestions { get; set; }
    public string? BusCount { get; set; }
    public string? BusSkateboardDifference { get; set; }
    public string? TotalStudents { get; set; }
    public string? CoolDownCount { get; set; }
    public string? CoolDownDifference { get; set; }
    // Separate authoring evidence: the 2021 teacher-guide Cool-down asks for one
    // fact AND one question; our independent graph preserves both skill families.
    public string? CoolDownFact { get; set; }
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


/// <summary>
/// A written response is evidence for a teacher to examine, never a machine
/// pass for original 2021 reasoning, question writing or classroom discussion.
/// Missing writing is NOT evidence that an oral task was carried out.
/// </summary>
public static class Grade3Lesson1TeacherReviewPolicy
{
    public static Grade3Lesson1Feedback AssessForReview(
        string label, string? response, string criteria)
    {
        if (string.IsNullOrWhiteSpace(response))
            return new Grade3Lesson1Feedback(
                label, null, true,
                "No written response was submitted. An in-class oral or partner " +
                "activity cannot be certified without teacher observation. " + criteria);

        return new Grade3Lesson1Feedback(
            label, null, true,
            "Written response supplied; it is NOT an automatic pass. " +
            "A teacher must check the following before accepting it: " + criteria);
    }
}
