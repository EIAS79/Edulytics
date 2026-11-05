using Edulytics.Core.Mathematics.Practice;using Edulytics.Services.Mathematics;using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U04 L01 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit4Lesson1FlagsTests
{
    [Fact] public void ContractReusesScaleAndPercentFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U04:L01",out var c));
        Assert.NotNull(c);
        foreach(var f in new[]{"usccss.g7.u01.l02.scale_factor","usccss.g7.u01.l01.scaled_copy_correspondence","supporting.fdp.percent_equivalent"})
        { Assert.Contains(f,c.AllowedQuestionFamilies); Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)); }
        Assert.Equal("READY_VERIFIED",c.Readiness);
    }
}
