using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U08 L03 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit8Lesson3ContractTests
{
    [Fact] public void ContractResolvesPrimaryExactFamily()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U08:L03",out var c));
        Assert.NotNull(c);Assert.Contains("supporting.probability.sample_space_count",c.AllowedQuestionFamilies);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("supporting.probability.sample_space_count"));
    }
}
