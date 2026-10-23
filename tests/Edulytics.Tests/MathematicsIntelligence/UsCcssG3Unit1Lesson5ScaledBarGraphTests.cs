using Edulytics.Core.Mathematics.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Grade 3 L05 Stage C candidates; not executed until Stage D.</summary>
public sealed class UsCcssG3Unit1Lesson5ScaledBarGraphTests
{
    [Fact]
    public void LocalSourceTwoAxesHaveIdenticalCategoryCounts()
    {
        var original=UsCcssG3Unit1Lesson5ScaledBarGraph.SourceComparison;
        Assert.Equal(new[]{"Bike","Walk","Bus or Van","Car"},
            original.Select(x=>x.Category));
        Assert.Equal(new[]{12,4,6,3},original.Select(x=>x.Students));
        Assert.Equal(25,original.Sum(x=>x.Students));
        Assert.Equal(16,UsCcssG3Unit1Lesson5ScaledBarGraph.AxisCeiling(12,2));
        Assert.Equal(40,UsCcssG3Unit1Lesson5ScaledBarGraph.AxisCeiling(12,5));
    }

    [Theory]
    [InlineData("L05-SOURCE-BIKE",12)]
    [InlineData("L05-SOURCE-BUS-VAN",6)]
    [InlineData("L05-SOURCE-TOTAL",25)]
    [InlineData("L05-SOURCE-BIKE-MINUS-CAR",9)]
    public void SourceMathRightAndWrong(string id,int expected)
    {
        Assert.True(UsCcssG3Unit1Lesson5ScaledBarGraph.TryGradeSource(id,expected,out var yes));
        Assert.True(yes);
        Assert.True(UsCcssG3Unit1Lesson5ScaledBarGraph.TryGradeSource(id,expected+1,out var no));
        Assert.False(no);
    }

    [Theory]
    [InlineData("L05-EDU-CD-ART",9)]
    [InlineData("L05-EDU-CD-SCIENCE-MINUS-MUSIC",3)]
    [InlineData("L05-EDU-CD-TOTAL",20)]
    public void IndependentCoolDownScorerHonorsNumericalScope(string id,int expected)
    {
        Assert.True(UsCcssG3Unit1Lesson5ScaledBarGraph.TryGradeCoolDown(id,expected,out var yes));
        Assert.True(yes);
        Assert.True(UsCcssG3Unit1Lesson5ScaledBarGraph.TryGradeCoolDown(id,expected-1,out var no));
        Assert.False(no);
        Assert.False(UsCcssG3Unit1Lesson5ScaledBarGraph.TryGradeCoolDown(id+"-DRAW",expected,out _));
    }

    [Theory]
    [InlineData("L05-SOURCE-CD-TIGER", 10)]
    [InlineData("L05-SOURCE-CD-OTTER", 4)]
    public void OriginalSource2021ZooCooldownCorrectAndIncorrectBarHeight(string id, int expected)
    {
        Assert.True(UsCcssG3Unit1Lesson5ScaledBarGraph.TryGradeSourceCoolDown(id, expected, out var yes));
        Assert.True(yes);
        Assert.True(UsCcssG3Unit1Lesson5ScaledBarGraph.TryGradeSourceCoolDown(id, expected + 1, out var no));
        Assert.False(no);
    }

    [Fact]
    public void OriginalSourceZooGraphRetainsAllFourSourceCounts()
    {
        var zoo = UsCcssG3Unit1Lesson5ScaledBarGraph.OriginalSourceZooCoolDown;
        Assert.Equal(new [] { "Elephant", "Tiger", "Giraffe", "Otter" }, zoo.Select(x => x.Category));
        Assert.Equal(new [] { 17, 10, 14, 4 }, zoo.Select(x => x.Students));
        Assert.True(UsCcssG3Unit1Lesson5ScaledBarGraph.RequiresTeacherReview("source-zoo-drawing"));
    }

    [Fact]
    public void BothSourceAndClassBarGraphLabelsRemainOutsideThe200PixelPlot()
    {
        // Deferred Stage D visual contract, authored only and never executed here.
        DirectoryInfo? dir = new(AppContext.BaseDirectory);
        string? path = null;
        while (dir is not null)
        {
            var p = Path.Combine(dir.FullName, "src", "Edulytics.Web",
                "Views", "Grade3Lesson5Activities", "Index.cshtml");
            if (File.Exists(p)) { path = p; break; }
            dir = dir.Parent;
        }
        Assert.NotNull(path);
        var view = File.ReadAllText(path!);
        Assert.Contains("grid-template-columns:repeat(var(--label-columns)", view);
        Assert.Contains("--label-columns:@fixedData.Count", view);
        Assert.Contains("--label-columns:@Model.EnteredClassSurvey.Count", view);
        Assert.Contains("Bar graph horizontal categories", view);
        Assert.Contains("Class bar graph horizontal categories", view);
        Assert.DoesNotContain(
            "<span class=\"xlabel\">@row.Category</span>\n                        </div>", view);
    }

    [Fact]
    public void PriorClassValuesRequireHumanProvenanceAndAllowNonMultiples()
    {
        UsCcssG3Unit1Lesson4ScaledTravelSurvey.TravelCount[] values=[
            new("Car",3),new("Train",6),new("Boat",1),new("Balloon",5),
            new("Plane",4),new("Helicopter",2)];
        Assert.True(UsCcssG3Unit1Lesson5ScaledBarGraph.TryValidatePriorClassCounts(
            values,out _));
        Assert.True(UsCcssG3Unit1Lesson5ScaledBarGraph.RequiresTeacherReview(
            "prior-lesson-continuity"));
        Assert.Equal(18,UsCcssG3Unit1Lesson5ScaledBarGraph.AxisCeiling(17,2));
        Assert.Equal(40,UsCcssG3Unit1Lesson5ScaledBarGraph.AxisCeiling(21,5));
    }
}
