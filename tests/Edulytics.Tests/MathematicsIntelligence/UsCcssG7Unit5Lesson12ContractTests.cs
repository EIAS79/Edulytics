using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U05 L12 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit5Lesson12ContractTests
{
    [Fact] public void ContractResolvesPrimaryExactFamily()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U05:L12",out var c));
        Assert.NotNull(c);Assert.Contains("usccss.number.signed_rate_displacement",c.AllowedQuestionFamilies);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("usccss.number.signed_rate_displacement"));
    }
}
