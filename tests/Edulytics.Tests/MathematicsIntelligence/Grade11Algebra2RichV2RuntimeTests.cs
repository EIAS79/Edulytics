using Edulytics.Core.Curriculum;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Stage D: 120 Algebra 2 lessons must load via the actual RichV2 runtime registry,
/// not merely pass source-only JSON authoring tests.
/// </summary>
public sealed class Grade11Algebra2RichV2RuntimeTests
{
    private const string Prefix = "PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:";
    private static readonly int[] UnitCounts = [11, 26, 19, 18, 11, 19, 16];

    [Fact]
    public void All120RealLessonResourcesAreValidRuntimeAndDoNotBreakSharedRegistry()
    {
        var resources = RichLessonContentV2Registry.AllDocuments
            .Where(d => d.Lessons.Any(x => x.LessonCode.StartsWith(Prefix, StringComparison.Ordinal)))
            .ToArray();
        var rows = resources.SelectMany(x => x.Lessons)
            .Where(x => x.LessonCode.StartsWith(Prefix, StringComparison.Ordinal))
            .ToArray();

        Assert.Equal(120, rows.Length);
        Assert.All(resources, d =>
        {
            Assert.Equal(1, d.SchemaVersion);
            RichLessonContentV2Registry.Validate(d, "Grade 11 Stage D resource");
        });
        Assert.Equal(120, rows.Select(x => x.LessonCode).Distinct().Count());
        var expected = new List<string>();
        for (var u = 0; u < UnitCounts.Length; u++)
        for (var l = 1; l <= UnitCounts[u]; l++)
            expected.Add(Prefix + $"U{u + 1:D2}:L{l:D2}");
        Assert.Equal(expected.Order(StringComparer.Ordinal),
            rows.Select(x => x.LessonCode).Order(StringComparer.Ordinal));
    }

    [Theory]
    [InlineData("U01:L01")]
    [InlineData("U02:L02")]
    [InlineData("U03:L01")]
    [InlineData("U05:L10")]
    [InlineData("U07:L16")]
    public void SelectedLessonsCanActuallyRenderWithMathAndTeacherBoundary(string suffix)
    {
        var lesson = RichLessonContentV2Registry.Find(Prefix + suffix, "en");
        Assert.NotNull(lesson);
        Assert.True(lesson.KeyConcepts.Count >= 3);
        Assert.True(lesson.WorkedExamples.Count >= 3);
        Assert.All(lesson.WorkedExamples, e =>
        {
            Assert.True(e.Steps.Count >= 3);
            Assert.False(string.IsNullOrWhiteSpace(e.Answer));
            Assert.False(string.IsNullOrWhiteSpace(e.Check));
        });
        Assert.Contains("teacher", string.Join(" ", lesson.ExplanationParagraphs),
            StringComparison.OrdinalIgnoreCase);
    }
}
