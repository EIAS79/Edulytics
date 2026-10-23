namespace Edulytics.Core.Mathematics.Curriculum;

/// <summary>
/// L05 data identity is critical: teacher's first comparison graph uses
/// Bike 12, Walk 4, Bus or Van 6, Car 3 with axes step 1 or 2.
/// New learner-created charts must reuse the REAL class survey from L04
/// (counts unknown to the app) and can choose step 2 or 5.
/// </summary>
public static class UsCcssG3Unit1Lesson5ScaledBarGraph
{
    public const string LessonCode = "PED:US-CCSS-MATH:G3:U01:L05";
    public sealed record Bar(string Category, int Students);
    public static IReadOnlyList<Bar> SourceComparison { get; } =
        Array.AsReadOnly(new[] {
            new Bar("Bike",12), new Bar("Walk",4),
            new Bar("Bus or Van",6), new Bar("Car",3)
        });
    public static IReadOnlyList<Bar> IndependentCoolDown { get; } =
        Array.AsReadOnly(new[] {
            new Bar("Science Club",7), new Bar("Art Club",9),
            new Bar("Music Club",4)
        });

    public static bool TryGradeSource(string? id,int answer,out bool correct)
    {
        correct = false;
        int? expected = id switch {
            "L05-SOURCE-BIKE" => 12,
            "L05-SOURCE-BUS-VAN" => 6,
            "L05-SOURCE-TOTAL" => 25,
            "L05-SOURCE-BIKE-MINUS-CAR" => 9,
            _ => null
        };
        if (expected is null) return false;
        correct = answer == expected;
        return true;
    }

    public static bool TryGradeCoolDown(string? id,int answer,out bool correct)
    {
        correct=false;
        int? expected=id switch {
            "L05-EDU-CD-ART" => 9,
            "L05-EDU-CD-SCIENCE-MINUS-MUSIC" => 3,
            "L05-EDU-CD-TOTAL" => 20,
            _ => null
        };
        if (expected is null) return false;
        correct=answer==expected;
        return true;
    }
    public static int AxisCeiling(int highest,int tickStep)
    {
        if (highest<0 || highest>40 || tickStep is not (2 or 5))
            throw new ArgumentOutOfRangeException(nameof(highest));
        var ceiling=((highest+tickStep-1)/tickStep)*tickStep;
        return Math.Max(tickStep==2?16:40,ceiling);
    }

    public static bool TryValidatePriorClassCounts(
        IReadOnlyList<UsCcssG3Unit1Lesson4ScaledTravelSurvey.TravelCount>? survey,
        out string note) =>
        UsCcssG3Unit1Lesson4ScaledTravelSurvey.TryValidate(survey,out note);

    public static bool RequiresTeacherReview(string type) => type is
        "actual-class-survey" or "prior-lesson-continuity" or "student-bar-graph"
            or "scale-choice-justification" or "partner-comparison"
            or "cool-down-drawing" or "math-community";
}
