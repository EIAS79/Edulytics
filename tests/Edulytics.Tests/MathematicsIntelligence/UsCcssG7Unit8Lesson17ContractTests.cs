using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U08 L17 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit8Lesson17ContractTests
{
    [Fact] public void ContractResolvesPrimaryExactFamily()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U08:L17",out var c));
        Assert.NotNull(c);Assert.Contains("statistics.center_spread.mean",c.AllowedQuestionFamilies);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("statistics.center_spread.mean"));
    }
}
