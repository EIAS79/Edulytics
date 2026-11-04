using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U09 L06 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit9Lesson6ContractTests
{
    [Fact] public void ContractResolvesPrimaryReusedFamily()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U09:L06",out var c));
        Assert.NotNull(c);Assert.Contains("usccss.g7.u02.l03.unit_rates_reciprocal",c.AllowedQuestionFamilies);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("usccss.g7.u02.l03.unit_rates_reciprocal"));
    }
}
