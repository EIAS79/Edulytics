namespace Edulytics.Core.Mathematics.Curriculum;

/// <summary>
/// G3 U01 L04 independent scaled picture-graph implementation.
/// The source asks pupils to survey preferred travel choices from real peers;
/// those counts are not fixed or authenticated by this model. A whole mark
/// stands for two answers, a half mark for one. Do not silently round odds.
/// </summary>
public static class UsCcssG3Unit1Lesson4ScaledTravelSurvey
{
    public const string LessonCode = "PED:US-CCSS-MATH:G3:U01:L04";
    public const int StudentsPerWholeSymbol = 2;
    public sealed record TravelCount(string Category, int Students);
    public static readonly IReadOnlyList<string> Categories =
        Array.AsReadOnly(new[] { "Car", "Train", "Boat", "Balloon", "Plane", "Helicopter" });

    public static bool TryValidate(IReadOnlyList<TravelCount>? data, out string reason)
    {
        reason = "Six teacher-supplied class category totals are required.";
        if (data is null || data.Count != Categories.Count)
            return false;
        if (data.GroupBy(x => x.Category, StringComparer.OrdinalIgnoreCase).Any(g => g.Count() > 1))
        {
            reason = "Each category is allowed once.";
            return false;
        }
        if (data.Any(x => x.Students < 0 || x.Students > 40))
        {
            reason = "Each class category count must be from 0 to 40.";
            return false;
        }
        if (Categories.Any(cat => !data.Any(x =>
            string.Equals(cat, x.Category, StringComparison.OrdinalIgnoreCase))))
        {
            reason = "All six listed travel categories are required.";
            return false;
        }
        var total = data.Sum(x => x.Students);
        if (total is < 1 or > 40)
        {
            reason = "The entered class survey must total between 1 and 40; teacher handles larger classes.";
            return false;
        }
        reason = "Count format valid; not proof that classmates were surveyed.";
        return true;
    }
    public static string RepresentWithMarks(int students)
    {
        if (students < 0 || students > 40)
            throw new ArgumentOutOfRangeException(nameof(students));
        return string.Concat(Enumerable.Repeat("● ", students / 2)) +
               (students % 2 == 1 ? "◐" : "");
    }
    public static (int Whole, int Half) GlyphCounts(int students)
    {
        if (students < 0 || students > 40)
            throw new ArgumentOutOfRangeException(nameof(students));
        return (students / 2, students % 2);
    }

    // Separate, original Edulytics Cool-down: independent fictional data.
    public static readonly IReadOnlyList<TravelCount> OriginalCoolDown =
        Array.AsReadOnly(new[] {
            new TravelCount("Car", 4), new TravelCount("Train", 5),
            new TravelCount("Boat", 2), new TravelCount("Balloon", 1),
            new TravelCount("Plane", 3), new TravelCount("Helicopter", 3)
        });
    public static bool TryGradeCoolDown(string? questionId, int answer, out bool correct)
    {
        correct = false;
        int? expected = questionId switch {
            "G3-U01-L04-CD-TRAIN-WHOLE" => 2,
            "G3-U01-L04-CD-TRAIN-HALF" => 1,
            "G3-U01-L04-CD-PLANE-PLUS-TWO-WHOLE" => 2,
            "G3-U01-L04-CD-TOTAL" => 18,
            _ => null
        };
        if (expected is null) return false;
        correct = answer == expected;
        return true;
    }
    public static bool RequiresHumanReview(string task) => task is
        "actual-survey-provenance" or "student-named-records" or
        "partner-collaboration" or "student-drawn-graph" or
        "open-justification" or "cool-down-own-graph";
}
