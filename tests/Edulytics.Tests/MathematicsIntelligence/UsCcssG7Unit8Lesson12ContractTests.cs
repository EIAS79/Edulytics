using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U08 L12 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit8Lesson12ContractTests
{
    [Fact] public void ContractResolvesPrimaryExactFamily()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U08:L12",out var c));
        Assert.NotNull(c);Assert.Contains("supporting.statistics.collection_method",c.AllowedQuestionFamilies);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("supporting.statistics.collection_method"));
        Assert.Contains("statistics.center_spread.mean",c.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("statistics.center_spread.mean"));
    }
}
