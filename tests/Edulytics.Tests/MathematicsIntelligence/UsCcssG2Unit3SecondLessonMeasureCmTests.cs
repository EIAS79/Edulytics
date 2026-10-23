using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3SecondLessonMeasureCmTests
{
    [Fact]
    public void SourceDragonRequiresTwentyFourIndividualCmOrTwoTensPlusFour()
    {
        var r=UsCcssG2Unit3SecondLessonMeasureCm.BeardedDragon("24","2","4","24");
        Assert.True(r.Correct);Assert.True(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3SecondLessonMeasureCm.BeardedDragon("24","2","0","24").Correct);
        Assert.False(UsCcssG2Unit3SecondLessonMeasureCm.BeardedDragon("24","3","4","24").Correct);
        Assert.False(UsCcssG2Unit3SecondLessonMeasureCm.BeardedDragon("20","2","4","24").Correct);
    }
    [Fact]
    public void FirstPartyVirtualMeasurementsAreIndependentOfPublisherWorksheet()
    {
        var r=UsCcssG2Unit3SecondLessonMeasureCm.FirstParty("2","8","15","3","6");
        Assert.True(r.Correct);Assert.False(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3SecondLessonMeasureCm.FirstParty("2","8","16","3","6").Correct);
        Assert.True(UsCcssG2Unit3SecondLessonMeasureCm.TeacherRubric.Count>=6);
    }
}
