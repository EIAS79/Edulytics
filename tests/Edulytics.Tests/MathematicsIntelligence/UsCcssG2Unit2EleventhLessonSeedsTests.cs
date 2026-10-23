using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit2EleventhLessonSeedsTests
{
    [Fact]
    public void SourceCompareResultAndOtherThreeUnknownsAreDistinct()
    {
        var c=UsCcssG2Unit2EleventhLessonSeeds.CompareGap("26");
        Assert.True(c.Correct);Assert.True(c.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2EleventhLessonSeeds.CompareGap("58").Correct);
        Assert.True(UsCcssG2Unit2EleventhLessonSeeds.OtherThree("16","9","27").Correct);
        Assert.False(UsCcssG2Unit2EleventhLessonSeeds.OtherThree("16","28","27").Correct);
    }
    [Fact]
    public void FirstPartyExitSeparateFromTylerSeedsPublisherSource()
    {
        var r=UsCcssG2Unit2EleventhLessonSeeds.Independent("26","15","15","22");
        Assert.True(r.Correct);Assert.False(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2EleventhLessonSeeds.Independent("26","15","15","23").Correct);
        Assert.True(UsCcssG2Unit2EleventhLessonSeeds.TeacherRubric.Count>=6);
    }
}
