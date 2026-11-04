using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U02 L15 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit2Lesson15WaterUseTests
{
    const string L="PED:US-CCSS-MATH:G7:U02:L15";
    const string Rate="usccss.g7.u02.l03.unit_rates_reciprocal";
    const string Reciprocal="usccss.g7.u02.l05.reciprocal_proportional_equations";
    const string InfoGap="usccss.g7.u02.l09.missing_info_rate_comparison";

    [Fact]
    public void ContractReusesExistingRateReciprocalAndInformationFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(L,out var c));
        Assert.NotNull(c);
        Assert.Contains(Rate,c.AllowedQuestionFamilies);
        Assert.Contains(Reciprocal,c.AllowedQuestionFamilies);
        Assert.Contains(InfoGap,c.AllowedQuestionFamilies);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Rate));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Reciprocal));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(InfoGap));
    }

    [Fact]
    public void PreviousLessonRemainsResolvable()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G7:U02:L14",out _));
    }
}
