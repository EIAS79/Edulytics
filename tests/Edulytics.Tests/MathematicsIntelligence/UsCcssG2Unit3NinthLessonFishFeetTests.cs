using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3NinthLessonFishFeetTests
{
    [Fact]
    public void TeacherFloorTapeLengthsInFeetAreSourceExact()
    {
        var f=UsCcssG2Unit3NinthLessonFishFeet.SourceTapeFeet("2","4","3","2","6");
        Assert.True(f.Correct);Assert.True(f.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3NinthLessonFishFeet.SourceTapeFeet("2","4","3","2","8").Correct);
        var inches=UsCcssG2Unit3NinthLessonFishFeet.SourceTapeInchIntervals("24","48","36","30");
        Assert.True(inches.Correct);Assert.True(inches.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3NinthLessonFishFeet.SourceTapeInchIntervals("24","48","36","32").Correct);
    }
    [Fact]
    public void OneFootAndRepeatingTwelveInchTilesAreIndependentIllustrations()
    {
        var x=UsCcssG2Unit3NinthLessonFishFeet.FirstParty("12","36","30");
        Assert.True(x.Correct);Assert.False(x.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3NinthLessonFishFeet.FirstParty("10","36","30").Correct);
        Assert.True(UsCcssG2Unit3NinthLessonFishFeet.TeacherRubric.Count>=7);
    }
}
