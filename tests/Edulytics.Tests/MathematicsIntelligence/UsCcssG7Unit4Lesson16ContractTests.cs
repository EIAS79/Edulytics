using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U04 L16 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit4Lesson16ContractTests
{
    [Fact] public void ContractResolvesPrimaryExactFamily()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U04:L16",out var c));
        Assert.NotNull(c);
        Assert.Contains("usccss.g7.u04.percent_rate_from_values",c.AllowedQuestionFamilies);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("usccss.g7.u04.percent_rate_from_values"));
    }
}
