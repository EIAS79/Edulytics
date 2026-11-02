using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G2 U02 L01 source-true graph and cube numerics, teacher-only role separation.</summary>
public sealed class UsCcssG2Unit2FirstLessonCompareActivitiesTests
{
    [Theory]
    [InlineData("37","11","21+16=37","32-21=11",true)]
    [InlineData("37","11","16+21=37","21+11=32",true)]
    [InlineData("48","11","21+16=37","32-21=11",false)]
    [InlineData("37","16","21+16=37","32-21=11",false)]
    [InlineData("37","11","21+16=37","32-16=16",false)]
    public void MovieSnackGraphChecksCorrectSourceCategoryMeaning(
        string total,string gap,string addition,string comparison,bool correct)
    {
        var r=UsCcssG2Unit2FirstLessonCompareActivities.CheckMovieSnacks(
            total,gap,addition,comparison);
        Assert.Equal(correct,r.Correct);
        Assert.True(r.TeacherReviewNeeded);
    }
    [Fact]
    public void SourceCubeTotalsAndIndependentNoRegroupCoolDownAreIndependent()
    {
        Assert.True(UsCcssG2Unit2FirstLessonCompareActivities
            .CheckBuildTrains("68","24","22+24=46").Correct);
        Assert.False(UsCcssG2Unit2FirstLessonCompareActivities
            .CheckBuildTrains("68","25","22+24=46").Correct);
        Assert.True(UsCcssG2Unit2FirstLessonCompareActivities
            .CheckIndependentCoolDown("57","43","21").Correct);
        Assert.False(UsCcssG2Unit2FirstLessonCompareActivities
            .CheckIndependentCoolDown("57","43","20").Correct);
        Assert.False(UsCcssG2Unit2FirstLessonCompareActivities
            .CheckIndependentCoolDown("57","43","NaN").Correct);
        Assert.True(UsCcssG2Unit2FirstLessonCompareActivities.TeacherRubric.Count>=4);
    }
}
