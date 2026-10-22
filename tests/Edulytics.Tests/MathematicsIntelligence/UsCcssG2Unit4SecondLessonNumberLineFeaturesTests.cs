using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit4SecondLessonNumberLineFeaturesTests
{
    [Fact]
    public void ChoralFiveCountUsesExactOriginalZeroToHundredLimits()
    {
        var valid=UsCcssG2Unit4SecondLessonNumberLineFeatures.ChoralCount("5","10","50","100");
        Assert.True(valid.Correct);
        Assert.True(valid.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4SecondLessonNumberLineFeatures.ChoralCount("5","10","50","105").Correct);
    }
    [Fact]
    public void BlankLineHasElevenTicksAndDigitalPracticeDoesNotCertifySourceImages()
    {
        var source=UsCcssG2Unit4SecondLessonNumberLineFeatures.SourceBlankLine("1","5","10","11");
        Assert.True(source.Correct);
        Assert.True(source.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4SecondLessonNumberLineFeatures.SourceBlankLine("1","5","10","10").Correct);
        var numeric=UsCcssG2Unit4SecondLessonNumberLineFeatures.FirstPartyNumbers("18","6","21");
        Assert.True(numeric.Correct);
        Assert.False(numeric.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4SecondLessonNumberLineFeatures.FirstPartyNumbers("18","7","21").Correct);
        Assert.Contains(UsCcssG2Unit4SecondLessonNumberLineFeatures.TeacherRubric,
            value=>value.Contains("Andre",StringComparison.Ordinal));
    }
}
