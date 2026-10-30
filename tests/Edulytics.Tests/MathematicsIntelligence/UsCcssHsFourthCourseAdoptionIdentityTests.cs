using Edulytics.Core.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Exact course identity guard for #453; these assertions do not replace
/// authenticated two-school persistence and score tests.
/// </summary>
public sealed class UsCcssHsFourthCourseAdoptionIdentityTests
{
    [Theory]
    [InlineData("ADV-ALG-FUNC")]
    [InlineData("ADV-TRIG-GEO")]
    public void FourthCoursesAreExplicitSchoolAdoptionChoicesAndNotTraditional(string course)
    {
        var levels = CurriculumLevelIdentityRegistry.ForPack("US-CCSS-MATH")
            .Where(x => string.Equals(x.CourseCode, course, StringComparison.Ordinal))
            .ToArray();
        Assert.NotEmpty(levels);
        Assert.All(levels, level =>
        {
            Assert.Equal("Fourth Courses", level.Pathway);
            Assert.Equal("US-CCSS-MATH", level.PackCode);
            Assert.Equal(course, level.CourseCode);
            Assert.True(level.LogicalLevel is >= 10 and <= 13);
            var roundtrip = CurriculumLevelIdentityRegistry.Find(level.Key);
            Assert.NotNull(roundtrip);
            Assert.Equal(level, roundtrip);
            Assert.False(CurriculumLevelIdentityRegistry.IsUnresolvedAmericanHighSchoolPathway(level));
            Assert.NotEqual(
                CurriculumLevelIdentityRegistry.BuildKey("US-CCSS-MATH",
                    level.LogicalLevel, "Traditional", "ALG2"), level.Key);
        });
    }

    [Fact]
    public void TraditionalAlgebraTwoCannotMatchFourthCourseKeys()
    {
        var catalog = CurriculumLevelIdentityRegistry.ForPack("US-CCSS-MATH");
        var algebraTwo = catalog.Where(x => x.CourseCode == "ALG2").ToArray();
        var fourth = catalog.Where(x => x.CourseCode is "ADV-ALG-FUNC" or "ADV-TRIG-GEO").ToArray();
        Assert.NotEmpty(algebraTwo);
        Assert.NotEmpty(fourth);
        Assert.All(algebraTwo, x => Assert.Equal("Traditional", x.Pathway));
        Assert.All(fourth, x => Assert.Equal("Fourth Courses", x.Pathway));
        Assert.Empty(algebraTwo.Select(x => x.Key).Intersect(fourth.Select(x => x.Key)));
    }

    [Fact]
    public void GenericAmericanHighSchoolPathwayIsNotSelectableAsAnExplicitCourse()
    {
        var levels = CurriculumLevelIdentityRegistry.ForPack("US-CCSS-MATH");
        Assert.DoesNotContain(levels, x => x.LogicalLevel is >= 10 and <= 13 &&
            string.IsNullOrWhiteSpace(x.CourseCode));
        var genericKey = CurriculumLevelIdentityRegistry.BuildKey(
            "US-CCSS-MATH", 13, "Fourth Courses");
        var legacy = CurriculumLevelIdentityRegistry.Find(genericKey);
        if (legacy is not null)
            Assert.True(CurriculumLevelIdentityRegistry.IsUnresolvedAmericanHighSchoolPathway(legacy));
    }
}
