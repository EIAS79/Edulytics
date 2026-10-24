using System.Collections.ObjectModel;

namespace Edulytics.Core.Mathematics.Curriculum;

/// <summary>
/// First-edition 2021 G3 U01 Lesson 2 original Cool-down source mathematical
/// data. Independently rendered by Edulytics; no publisher pictures reused.
/// Source: https://cms-assets.illustrativemathematics.org/g4jh0k3mui8ljz0zeuxa3cvdaxwt
/// </summary>
public static class UsCcssG3Unit1Lesson2SourceCoolDown
{
    public const string LessonCode = "PED:US-CCSS-MATH:G3:U01:L02";
    public const string GraphTitle = "Favorite Place to Read";
    public const int AxisMin = 0;
    public const int AxisMax = 10;
    public const int AxisStep = 1;
    public sealed record CategoryCount(string Category, int Students);
    private static readonly ReadOnlyCollection<CategoryCount> Data =
        Array.AsReadOnly(new[]
        {
            new CategoryCount("Park", 8),
            new CategoryCount("Home", 5),
            new CategoryCount("School", 4),
            new CategoryCount("Library", 2)
        });
    public static IReadOnlyList<CategoryCount> Values => Data;
    public static int ParkMinusHome => 3;
    public static bool SchoolOrLibraryExceedsPark => false;
    public static int Total => 19;
}
