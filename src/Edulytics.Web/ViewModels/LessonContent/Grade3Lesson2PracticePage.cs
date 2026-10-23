using Edulytics.Core.Mathematics.Curriculum;

namespace Edulytics.Web.ViewModels.LessonContent;

/// <summary>Grade 3 U01 L02 unsaved class-survey response form.</summary>
public sealed class Grade3Lesson2Answers
{
    public string? LabelBike { get; set; }
    public string? LabelWalk { get; set; }
    public string? LabelVan { get; set; }
    public string? LabelBus { get; set; }
    public string? LabelCar { get; set; }
    public string? LabelTrain { get; set; }
    public string? Bike { get; set; }
    public string? Walk { get; set; }
    public string? Van { get; set; }
    public string? Bus { get; set; }
    public string? Car { get; set; }
    public string? Train { get; set; }
    public string? WalkMost { get; set; }
    public string? BusMoreThanCar { get; set; }
    public string? WalkFewerThanBike { get; set; }
    public string? WalkBikeMoreThanVan { get; set; }
    public string? BusCarDifference { get; set; }
    public string? WalkBikeVanDifference { get; set; }
    public string? WarmupReasoning { get; set; }
    public string? GraphExplanation { get; set; }
    public string? TrueFalseExplanation { get; set; }
    public string? CommunityRevisions { get; set; }
    public string? CoolDownOne { get; set; }
    public string? CoolDownTwo { get; set; }
    public string? CoolDownGraphExplanation { get; set; }
}
public sealed record Grade3Lesson2Feedback(
    string Label, bool? MachineCorrect, bool TeacherReviewRequired, string Explanation);
public sealed record Grade3Lesson2PracticePage(
    Guid CurriculumAdoptionId, Guid LessonId, string LessonTitle,
    bool Submitted, Grade3Lesson2Answers Answers,
    IReadOnlyList<UsCcssG3Unit1Lesson2ClassSurveyCandidate.ClassroomCount>? ValidatedShapeOnlySurvey,
    IReadOnlyList<Grade3Lesson2Feedback> Feedback);
