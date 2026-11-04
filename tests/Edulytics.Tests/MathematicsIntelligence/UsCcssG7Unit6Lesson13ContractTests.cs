using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U06 L13 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit6Lesson13ContractTests
{
    [Fact] public void ContractResolvesPrimaryExactFamily()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U06:L13",out var c));
        Assert.NotNull(c);Assert.Contains("algebra.linear.inequality.ax_plus_b_relation_c",c.AllowedQuestionFamilies);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("algebra.linear.inequality.ax_plus_b_relation_c"));
    }
}
