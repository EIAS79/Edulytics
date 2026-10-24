using Edulytics.Core.Mathematics.Curriculum;

namespace Edulytics.Web.ViewModels.LessonContent;
/// <summary>Volatile L05 questionnaire, never an official class graph record.</summary>
public sealed class Grade3Lesson5Answers
{
    public string? Car { get; set; }
    public string? Train { get; set; }
    public string? Boat { get; set; }
    public string? Balloon { get; set; }
    public string? Plane { get; set; }
    public string? Helicopter { get; set; }
    public string? SelectedScale { get; set; }
    public string? Warmup { get; set; }
    public string? OriginalGraphComparison { get; set; }
    public string? OriginalBikeCount { get; set; }
    public string? OriginalBusVanCount { get; set; }
    public string? OriginalTotal { get; set; }
    public string? OriginalBikeCarDifference { get; set; }
    public string? ReusedClassDataEvidence { get; set; }
    public string? ScaleExplanation { get; set; }
    public string? MathCommunity { get; set; }
    public string? SourceCoolDownTiger { get; set; }
    public string? SourceCoolDownOtter { get; set; }
    public string? SourceCoolDownDrawing { get; set; }
    public string? CoolDownArt { get; set; }
    public string? CoolDownDifference { get; set; }
    public string? CoolDownTotal { get; set; }
    public string? CoolDownDrawingExplanation { get; set; }
}
public sealed record Grade3Lesson5Feedback(string Label, bool? Correct, bool TeacherReview,string Note);
public sealed record Grade3Lesson5PracticePage(Guid CurriculumAdoptionId,Guid LessonId,bool Submitted,
    Grade3Lesson5Answers Answers,
    IReadOnlyList<UsCcssG3Unit1Lesson4ScaledTravelSurvey.TravelCount>? EnteredClassSurvey,
    int SelectedScale,
    IReadOnlyList<Grade3Lesson5Feedback> Feedback);
