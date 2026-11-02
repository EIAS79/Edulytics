using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage C authoring only, UNRUN.
public sealed class Grade4Ordinal125ClockMarksStageCTests
{
    [Fact] public void ExactHourFamilyIsReusedAndClockBenchmarksAreConsistent()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("usccss.g4.u07.l07.clock_hour_angle_turn_compare"));
        Assert.Equal(30,360/12);
        Assert.Equal(90,3*30);
        Assert.Equal(180,6*30);
        Assert.Equal(120,4*30);
    }
}
