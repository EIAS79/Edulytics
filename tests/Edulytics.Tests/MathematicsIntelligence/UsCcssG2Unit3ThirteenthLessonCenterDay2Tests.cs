using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3ThirteenthLessonCenterDay2Tests
{
    [Fact]
    public void FourOriginalMentalExpressionsMustMatch()
    {
        var r=UsCcssG2Unit3ThirteenthLessonCenterDay2.Warmup("68","70","71","81");
        Assert.True(r.Correct);Assert.True(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3ThirteenthLessonCenterDay2.Warmup("68","70","71","71").Correct);
    }
    [Fact]
    public void TargetLimitsAbsoluteErrorAndPuzzleEquationAreExact()
    {
        Assert.True(UsCcssG2Unit3ThirteenthLessonCenterDay2.ExampleMeasurement("cm","12","15","3").Correct);
        Assert.True(UsCcssG2Unit3ThirteenthLessonCenterDay2.ExampleMeasurement("in","7","5","2").Correct);
        Assert.False(UsCcssG2Unit3ThirteenthLessonCenterDay2.ExampleMeasurement("in","11","8","3").Correct);
        Assert.False(UsCcssG2Unit3ThirteenthLessonCenterDay2.ExampleMeasurement("cm","31","34","3").Correct);
        Assert.False(UsCcssG2Unit3ThirteenthLessonCenterDay2.ExampleMeasurement("cm","12","15","-3").Correct);
        Assert.True(UsCcssG2Unit3ThirteenthLessonCenterDay2.OriginalArithmetic("25","26").Correct);
        Assert.False(UsCcssG2Unit3ThirteenthLessonCenterDay2.OriginalArithmetic("25","36").Correct);
        Assert.True(UsCcssG2Unit3ThirteenthLessonCenterDay2.IndependentExit("3","2","81").Correct);
    }
}
