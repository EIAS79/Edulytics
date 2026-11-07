using Edulytics.Core.Curriculum;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Stage D runtime integration checks. Finds each of the 73 Grade 12 US CCSS
/// Precalculus Rich lesson documents through the actual embedded registry.
/// Does not certify mathematical equivalence, UI authentication or grading.
/// </summary>
public sealed class Grade12PrecalculusAllLessonRegistryRuntimeTests
{
    private const string Prefix =
        "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:";

    private static readonly int[] UnitCounts =
        [7, 4, 9, 8, 4, 3, 6, 8, 8, 5, 7, 4];

    public static IEnumerable<object[]> All73Codes()
    {
        for (var unit = 0; unit < UnitCounts.Length; unit++)
        for (var lesson = 1; lesson <= UnitCounts[unit]; lesson++)
            yield return [Prefix + $"U{unit + 1:D2}:L{lesson:D2}"];
    }

    [Fact]
    public void Exactly73ExpectedLessonIdentitiesAreDeclared()
    {
        var codes = All73Codes().Select(x => (string)x[0]).ToArray();
        Assert.Equal(73, codes.Length);
        Assert.Equal(73, codes.Distinct(StringComparer.Ordinal).Count());
    }

    [Theory]
    [MemberData(nameof(All73Codes))]
    public void ActualEmbeddedRegistryCanLoadEachGrade12StudentLesson(string lessonCode)
    {
        var lesson = RichLessonContentV2Registry.Find(lessonCode, "en");

        Assert.NotNull(lesson);
        Assert.Equal(lessonCode, lesson.LessonCode);
        Assert.Equal("en", lesson.CultureCode);
        Assert.False(string.IsNullOrWhiteSpace(lesson.Title));
        Assert.True(lesson.ExplanationParagraphs.Count >= 3);
        Assert.NotEmpty(lesson.KeyConcepts);
        Assert.NotEmpty(lesson.WorkedExamples);
        Assert.NotEmpty(lesson.SummaryPoints);
        Assert.NotEmpty(lesson.CommonMistakes);
        Assert.NotNull(lesson.Visuals);

        foreach (var example in lesson.WorkedExamples)
        {
            Assert.False(string.IsNullOrWhiteSpace(example.Question));
            Assert.False(string.IsNullOrWhiteSpace(example.Answer));
            Assert.NotEmpty(example.Steps);
        }
    }
}
