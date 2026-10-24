using System.Collections.ObjectModel;

namespace Edulytics.Core.Mathematics.Curriculum;

/// <summary>
/// Mathematical data from the public first-edition 2021 source Teacher Guide
/// Grade 3 Unit 1 Lesson 3 "Birds in the Park". Independently drawn by
/// Edulytics without copying the publisher's image or original markings.
/// https://cms-assets.illustrativemathematics.org/49w6zcnda7ycoimt136p0k2kqc0b
/// </summary>
public static class UsCcssG3Unit1Lesson3SourceCoolDown
{
    public const string LessonCode = "PED:US-CCSS-MATH:G3:U01:L03";
    public const string GraphTitle = "Birds in the Park";
    public const int BirdsPerSymbol = 2;
    public sealed record BirdRow(string Category, int Symbols);
    private static readonly ReadOnlyCollection<BirdRow> Data =
        Array.AsReadOnly(new[] {
            new BirdRow("Cardinals", 2),
            new BirdRow("Blue Jays", 3),
            new BirdRow("Pigeons", 4),
            new BirdRow("Sparrows", 5)
        });
    public static IReadOnlyList<BirdRow> Values => Data;
    public static int Count(string category) =>
        Values.Single(x => x.Category == category).Symbols * BirdsPerSymbol;
    public static int Total => Values.Sum(x => x.Symbols * BirdsPerSymbol);
}
