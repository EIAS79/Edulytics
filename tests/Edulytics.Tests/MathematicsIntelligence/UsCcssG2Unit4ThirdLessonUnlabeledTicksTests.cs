using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit4ThirdLessonUnlabeledTicksTests
{
    [Fact]
    public void OriginalPointNumbersNeedTeacherReviewNotAutomaticImageCertification()
    {
        var targets=UsCcssG2Unit4ThirdLessonUnlabeledTicks.SourceActivity1("24","37","48","83");
        Assert.True(targets.Correct);
        Assert.True(targets.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4ThirdLessonUnlabeledTicks.SourceActivity1("24","37","48","84").Correct);
        var offsets=UsCcssG2Unit4ThirdLessonUnlabeledTicks.SourceLandmarkOffsets("1","3","2","3");
        Assert.True(offsets.Correct);
        Assert.True(offsets.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4ThirdLessonUnlabeledTicks.SourceLandmarkOffsets("1","3","2","4").Correct);
    }
    [Fact]
    public void IndependentlyAuthoredUnitArithmeticHasNoPhysicalOrImageClaim()
    {
        var firstParty=UsCcssG2Unit4ThirdLessonUnlabeledTicks.FirstPartyNumeric("83","20","26");
        Assert.True(firstParty.Correct);
        Assert.False(firstParty.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4ThirdLessonUnlabeledTicks.FirstPartyNumeric("83","20","25").Correct);
        Assert.Contains(UsCcssG2Unit4ThirdLessonUnlabeledTicks.TeacherRubric,
            text=>text.Contains("image",StringComparison.OrdinalIgnoreCase));
    }
}
