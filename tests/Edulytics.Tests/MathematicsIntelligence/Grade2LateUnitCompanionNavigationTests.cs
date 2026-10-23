using Edulytics.Web.Presentation;
namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Grade2LateUnitCompanionNavigationTests
{
    [Theory]
    [InlineData(7, 18)]
    [InlineData(8, 13)]
    [InlineData(9, 13)]
    public void EveryCanonicalLateGrade2LessonHasAStudentCompanionLink(int unit, int lastLesson)
    {
        for (var lesson = 1; lesson <= lastLesson; lesson++)
        {
            var code = $"PED:US-CCSS-MATH:G2:U{unit:D2}:L{lesson:D2}";
            var link = Grade2LateUnitCompanionNavigation.Resolve(code);
            Assert.NotNull(link);
            Assert.Equal($"Grade2Unit{unit}Activities", link.Controller);
            Assert.Equal(unit, link.UnitNumber);
            Assert.Equal(lesson, link.LessonNumber);
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("PED:US-CCSS-MATH:G1:U07:L01")]
    [InlineData("PED:US-CCSS-MATH:G2:U06:L01")]
    [InlineData("PED:US-CCSS-MATH:G2:U07:L00")]
    [InlineData("PED:US-CCSS-MATH:G2:U07:L19")]
    [InlineData("PED:US-CCSS-MATH:G2:U08:L14")]
    [InlineData("PED:US-CCSS-MATH:G2:U09:L14")]
    [InlineData("PED:US-CCSS-MATH:G2:U10:L01")]
    [InlineData("PED:US-CCSS-MATH:G2:U08:L1")]
    [InlineData("PED:US-CCSS-MATH:G2:U08:L01:EXTRA")]
    public void NoncanonicalAndOutOfRangeLessonCodesCannotCreateLinks(string? code)
    {
        Assert.Null(Grade2LateUnitCompanionNavigation.Resolve(code));
    }

    [Fact]
    public void StudentPortalActuallyWiresTheResolverAndScopedStudentRoute()
    {
        string? path = null;
        for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir is not null; dir = dir.Parent)
        {
            var candidate = Path.Combine(dir.FullName, "src", "Edulytics.Web",
                "Views", "StudentPortal", "Lesson.cshtml");
            if (File.Exists(candidate)) { path = candidate; break; }
        }
        Assert.NotNull(path);
        var view = File.ReadAllText(path);
        Assert.Contains("Grade2LateUnitCompanionNavigation.Resolve(Model.LessonCode)", view);
        Assert.Contains("asp-controller=\"@lateGrade2Companion.Controller\"", view);
        Assert.Contains("asp-route-lessonNumber=\"@lateGrade2Companion.LessonNumber\"", view);
        Assert.Contains("asp-route-curriculumAdoptionId=\"@unifiedPracticeAdoptionId.Value\"", view);
        Assert.Contains("asp-route-lessonId=\"@Model.Id\"", view);
    }
}