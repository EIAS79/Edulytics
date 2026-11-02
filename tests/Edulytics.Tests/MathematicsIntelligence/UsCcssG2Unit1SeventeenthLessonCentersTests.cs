using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit1SeventeenthLessonCentersTests
{
    [Fact]
    public void CountingRemainsHonestAboutPhysicalVsDigitalVerification()
    {
        Assert.True(UsCcssG2Unit1SeventeenthLessonCenters.CheckDigitalCollection(
            "Tens","11","2","112").Correct);
        Assert.True(UsCcssG2Unit1SeventeenthLessonCenters.CheckDigitalCollection(
            "Fives","22","2","112").Correct);
        Assert.False(UsCcssG2Unit1SeventeenthLessonCenters.CheckDigitalCollection(
            "Fives","22","7","117").Correct);
        var physical=UsCcssG2Unit1SeventeenthLessonCenters.CheckPhysicalConsistency(
            "Tens","11","6","116");
        Assert.True(physical.Correct);
        Assert.True(physical.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit1SeventeenthLessonCenters.CheckPhysicalConsistency(
            "Tens","11","9","120").Correct);
    }
    [Fact]
    public void WarmupOptionalCentersAndIndependentExitEnforceCorrectAnswers()
    {
        Assert.True(UsCcssG2Unit1SeventeenthLessonCenters.CheckWarmup(
            "37","47","57","67").Correct);
        Assert.False(UsCcssG2Unit1SeventeenthLessonCenters.CheckWarmup(
            "37","47","57","66").Correct);
        Assert.True(UsCcssG2Unit1SeventeenthLessonCenters.CheckChoicePair(
            "BehindBack","SortDisplay").Correct);
        Assert.False(UsCcssG2Unit1SeventeenthLessonCenters.CheckChoicePair(
            "HowClose","HowClose").Correct);
        foreach(var (name,expected) in new[]{
            ("BehindBack","8"),("HowClose","43"),("NumberPuzzles","40"),("SortDisplay","3")})
        {
            Assert.True(UsCcssG2Unit1SeventeenthLessonCenters.CheckChoice(name,expected).Correct);
            Assert.False(UsCcssG2Unit1SeventeenthLessonCenters.CheckChoice(name,"not-a-number").Correct);
        }
        Assert.True(UsCcssG2Unit1SeventeenthLessonCenters.CheckCoolDown(
            "117","43","3").Correct);
        Assert.False(UsCcssG2Unit1SeventeenthLessonCenters.CheckCoolDown(
            "117","44","3").Correct);
    }
}
