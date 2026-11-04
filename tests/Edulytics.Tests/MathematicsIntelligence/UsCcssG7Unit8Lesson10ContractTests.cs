using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U08 L10 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit8Lesson10ContractTests
{
    [Fact] public void ContractResolvesPrimaryExactFamily()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U08:L10",out var c));
        Assert.NotNull(c);Assert.Contains("probability.theoretical.simple_event",c.AllowedQuestionFamilies);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("probability.theoretical.simple_event"));
    }
}
