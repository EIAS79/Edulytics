using Edulytics.Core.Mathematics.Curriculum;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Intentionally authored-not-run Stage C boundaries.</summary>
public sealed class UsCcssG3Unit1Lesson7GraphDifferencesTests
{
    [Fact]
    public void Local2021SourceBugCountsMatch()
    {
        Assert.Equal(new[]{62,11,19,7},UsCcssG3Unit1Lesson7GraphDifferences.Bugs.Select(x=>x.Count));
        Assert.Equal(99,UsCcssG3Unit1Lesson7GraphDifferences.Bugs.Sum(x=>x.Count));
        Assert.Equal(new[]{24,13,40,22},UsCcssG3Unit1Lesson6ChooseScale.SourceSeasons.Select(x=>x.Count));
    }
    [Theory]
    [InlineData("L07-SEASONS-TOTAL",99)]
    [InlineData("L07-SEASONS-SPRING-OR-FALL",35)]
    [InlineData("L07-SEASONS-SUMMER-MINUS-WINTER",16)]
    [InlineData("L07-SEASONS-FALL-MINUS-SPRING",9)]
    [InlineData("L07-EDU-CD-SOCCER-MINUS-TENNIS",22)]
    [InlineData("L07-EDU-CD-SWIMMING-PLUS-BASKETBALL",38)]
    [InlineData("L07-EDU-CD-TOTAL",80)]
    public void ExactNumbersPassOnlyWhenCorrect(string id,int expected)
    {
        Assert.True(UsCcssG3Unit1Lesson7GraphDifferences.TryGradeExact(id,expected,out var good));
        Assert.True(good);
        Assert.True(UsCcssG3Unit1Lesson7GraphDifferences.TryGradeExact(id,expected+1,out var wrong));
        Assert.False(wrong);
    }
    [Fact]
    public void BugGraphIsVerticallyScaledWithoutRevealingExactUnderlyingNumbers()
    {
        // Authored source-contract test: do not run until Stage D.
        Assert.NotNull(typeof(Edulytics.Web.ViewModels.LessonContent.Grade3Lesson7Answers)
            .GetProperty("PartnerQuestionExchange"));
        DirectoryInfo? dir = new(AppContext.BaseDirectory);
        string? file = null;
        while (dir is not null)
        {
            var candidate = Path.Combine(dir.FullName, "src", "Edulytics.Web",
                "Views", "Grade3Lesson7Activities", "Index.cshtml");
            if (File.Exists(candidate)) { file = candidate; break; }
            dir = dir.Parent;
        }
        Assert.NotNull(file);
        var razor = File.ReadAllText(file!);
        Assert.Contains("name=\"PartnerQuestionExchange\"", razor);
        Assert.Contains("bug-source-chart", razor);
        Assert.Contains("height:@(item.Count*300/80)px", razor);
        Assert.Contains("tick=80;tick>=0;tick-=10", razor);
        Assert.DoesNotContain("Exact underlying count @item.Count", razor);
        Assert.Contains("Exchange your two questions with another group", razor);
    }

    [Fact]
    public void BugChartEstimatesNeverAutoGradeAsExact()
    {
        Assert.True(UsCcssG3Unit1Lesson7GraphDifferences.TryCheckApproximateBugAnswer(
            "L07-BUGS-TOTAL",100,out var plausible));
        Assert.True(plausible);
        Assert.True(UsCcssG3Unit1Lesson7GraphDifferences.TryCheckApproximateBugAnswer(
            "L07-BUGS-ANTS-MINUS-SPIDERS",50,out plausible));
        Assert.True(plausible);
        Assert.False(UsCcssG3Unit1Lesson7GraphDifferences.TryGradeExact(
            "L07-BUGS-TOTAL",99,out _));
        Assert.True(UsCcssG3Unit1Lesson7GraphDifferences.RequiresTeacherReview(
            "scaled-chart-estimate"));
    }
}
