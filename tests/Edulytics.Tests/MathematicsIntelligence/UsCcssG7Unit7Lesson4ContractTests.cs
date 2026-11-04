using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U07 L04 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit7Lesson4ContractTests
{
    [Fact] public void ContractResolvesPrimaryExactFamily()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U07:L04",out var c));
        Assert.NotNull(c);Assert.Contains("geometry.angles.algebraic_supplementary",c.AllowedQuestionFamilies);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("geometry.angles.algebraic_supplementary"));
    }
}
