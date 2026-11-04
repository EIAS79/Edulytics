using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U07 L08 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit7Lesson8ContractTests
{
    [Fact] public void ContractResolvesPrimaryExactFamily()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U07:L08",out var c));
        Assert.NotNull(c);Assert.Contains("geometry.congruence.identify_criterion",c.AllowedQuestionFamilies);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("geometry.congruence.identify_criterion"));
    }
}
