using Edulytics.Core.Mathematics.Curriculum;

namespace Edulytics.Web.ViewModels.LessonContent;
/// <summary>G3 L04 volatile student form; not official assessed proof.</summary>
public sealed class Grade3Lesson4Answers
{
    public string? Car { get; set; }
    public string? Train { get; set; }
    public string? Boat { get; set; }
    public string? Balloon { get; set; }
    public string? Plane { get; set; }
    public string? Helicopter { get; set; }
    public string? Warmup { get; set; }
    public string? GraphExplanation { get; set; }
    public string? TeacherConfirmedSurvey { get; set; }
    public string? CarWholeSymbols { get; set; }
    public string? TrainHalfSymbol { get; set; }
    public string? TeacherReflection { get; set; }
    // Two distinct source synthesis constructions; the student must reason
    // about the current class pictograph rather than a fixed numeric answer.
    public string? BalloonPlusTwoGraphChange { get; set; }
    public string? CarPlusOneGraphChange { get; set; }
    public string? CoolDownTrainWhole { get; set; }
    public string? CoolDownTrainHalf { get; set; }
    public string? CoolDownPlanePlusTwoWhole { get; set; }
    public string? CoolDownTotal { get; set; }
    public string? CoolDownExplanation { get; set; }
}
public sealed record Grade3Lesson4Feedback(
    string Question, bool? Correct, bool TeacherReview, string Explanation);
public sealed record Grade3Lesson4PracticePage(
    Guid CurriculumAdoptionId, Guid LessonId, bool Submitted,
    Grade3Lesson4Answers Answers,
    IReadOnlyList<UsCcssG3Unit1Lesson4ScaledTravelSurvey.TravelCount>? EnteredSurvey,
    IReadOnlyList<Grade3Lesson4Feedback> Feedback);
