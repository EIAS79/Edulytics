using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit2ThirteenthLessonStoryEquationsTests
{
    [Fact]
    public void FourOriginalValuesAndWarmupRequireTrueUnknownPositions()
    {
        var warm=UsCcssG2Unit2ThirteenthLessonStoryEquations.Warmup("13");
        Assert.True(warm.Correct);Assert.True(warm.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2ThirteenthLessonStoryEquations.Warmup("17").Correct);
        var source=UsCcssG2Unit2ThirteenthLessonStoryEquations.SourceFour("28","25","70","39");
        Assert.True(source.Correct);Assert.True(source.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2ThirteenthLessonStoryEquations.SourceFour("28","25","70","53").Correct);
    }
    [Fact]
    public void FirstPartyCheckIsNumericalAndNeverClaimsTeacherCardValidation()
    {
        var v=UsCcssG2Unit2ThirteenthLessonStoryEquations.Independent("36","34","65","35");
        Assert.True(v.Correct);Assert.False(v.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2ThirteenthLessonStoryEquations.Independent("36","33","65","35").Correct);
        Assert.True(UsCcssG2Unit2ThirteenthLessonStoryEquations.TeacherRubric.Count>=6);
    }
}
