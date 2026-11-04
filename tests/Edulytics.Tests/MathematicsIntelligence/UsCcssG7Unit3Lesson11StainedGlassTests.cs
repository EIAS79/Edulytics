using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U03 L11 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit3Lesson11StainedGlassTests
{
    const string L="PED:US-CCSS-MATH:G7:U03:L11";
    [Fact] public void ContractReusesExistingClosedFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(L,out var c));
        Assert.NotNull(c);
        foreach(var f in new[]{"supporting.geometry.circle_area_pi_coefficient","supporting.geometry.compound_area","usccss.g7.u02.l04.proportional_equations"})
        {
            Assert.Contains(f,c.AllowedQuestionFamilies);
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
        }
        Assert.Equal("READY_VERIFIED",c.Readiness);
    }
}
