using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Honest source numerics vs teacher-only physical block manipulation.</summary>
public sealed class UsCcssG2Unit2SecondLessonUnknownAddendsTests
{
    [Fact]
    public void ChoralCountAndActualMissingSourceNumbersAreExact()
    {
        var warm=UsCcssG2Unit2SecondLessonUnknownAddends.CheckWarmup("77","37","7");
        Assert.True(warm.Correct);
        Assert.True(warm.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2SecondLessonUnknownAddends
            .CheckWarmup("77","36","7").Correct);
        var exact=UsCcssG2Unit2SecondLessonUnknownAddends.CheckSourceUnknowns(
            "43","31","75","12");
        Assert.True(exact.Correct);
        Assert.True(exact.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2SecondLessonUnknownAddends
            .CheckSourceUnknowns("43","31","74","12").Correct);
    }
    [Fact]
    public void HanMaiInverseAndOriginalIndependentCoolDownEnforced()
    {
        Assert.True(UsCcssG2Unit2SecondLessonUnknownAddends
            .CheckTwoMethods("17+31=48","48−17=31").Correct);
        Assert.False(UsCcssG2Unit2SecondLessonUnknownAddends
            .CheckTwoMethods("17+31=48","48-31=17").Correct);
        Assert.True(UsCcssG2Unit2SecondLessonUnknownAddends
            .CheckIndependentCoolDown("44","23","22").Correct);
        Assert.False(UsCcssG2Unit2SecondLessonUnknownAddends
            .CheckIndependentCoolDown("44","23","21").Correct);
        Assert.False(UsCcssG2Unit2SecondLessonUnknownAddends
            .CheckIndependentCoolDown("44","23","NaN").Correct);
        Assert.True(UsCcssG2Unit2SecondLessonUnknownAddends.TeacherRubric.Count>=5);
    }
}
