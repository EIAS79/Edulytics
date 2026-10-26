using System.Collections.ObjectModel;

namespace Edulytics.Core.Mathematics.Curriculum;

/// <summary>
/// Mathematical data in the first-edition (2021) G3 U01 L01 teacher-guide
/// Cool-down "Describe and Ask". Edulytics renders a NEW independent bar chart;
/// no original publisher images or logos are copied. Both source prompts are
/// open teacher-reviewed text, NOT numeric auto-grading tasks.
/// Source: https://cms-assets.illustrativemathematics.org/k6qyojyjw2s7jgle51o53gvczrp3
/// </summary>
public static class UsCcssG3Unit1Lesson1SourceCoolDown
{
    public const string LessonCode = "PED:US-CCSS-MATH:G3:U01:L01";
    public const string GraphTitle = "How We Get Home";
    public const int YAxisMin = 0;
    public const int YAxisMax = 10;
    public const int YAxisStep = 1;

    public sealed record TravelResponse(string Label, int Count);

    private static readonly ReadOnlyCollection<TravelResponse> Rows =
        Array.AsReadOnly(new[]
        {
            new TravelResponse("Bike", 6),
            new TravelResponse("Walk", 4),
            new TravelResponse("Bus", 8),
            new TravelResponse("Car", 2)
        });

    public static IReadOnlyList<TravelResponse> Values => Rows;
    public static int TotalResponses => Values.Sum(x => x.Count);

    public static bool IsTeacherReviewedPrompt(string? id) =>
        id is "2021-cooldown-one-fact" or "2021-cooldown-one-question";

    public static bool MayAwardAutomaticSourceScore(string? id) => false;
}
